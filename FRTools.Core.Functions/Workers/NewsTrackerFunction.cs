using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Data.DataModels.NewsReaderModels;
using FRTools.Core.Helpers;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class NewsTrackerFunction : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly IFRItemService _itemService;
        private readonly IAzurePipelineService _pipelineService;
        private ILogger _logger;

        public NewsTrackerFunction(DataContext dataContext, IFRItemService itemService, IAzurePipelineService pipelineService)
        {
            _dataContext = dataContext;
            _itemService = itemService;
            _pipelineService = pipelineService;
        }

        [FunctionName(nameof(NewsTracker))]
        public async Task NewsTracker([TimerTrigger("0 */5 * * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            _logger = log;
            var mainNewsForum = await Common.Helpers.LoadHtmlPage("https://www1.flightrising.com/forums/ann");
            var topics = mainNewsForum.GetElementbyId("postlist").SelectNodes("tr");

            _logger.LogInformation($"Found {topics.Count} topics");

            var topicInfos = new List<TopicInfo>();

            foreach(var topic in topics)
            {
                topicInfos.Add(await GetHomeTopicInfo(topic));
            }

            foreach (var newsTopic in topicInfos.Where(x => x.RequireParse))
            {
                await ParseNewsTopic(newsTopic);
            }
        }

        private async Task<TopicInfo> GetHomeTopicInfo(HtmlNode newsTopic)
        {
            var topicInfo = new TopicInfo();

            var topic = newsTopic.SelectSingleNode("td[1]");
            topicInfo.FRId = Convert.ToInt32(Regex.Match(topic.SelectSingleNode("a").GetAttributeValue("href", ""), @".+/ann/(\d+)").Groups[1].Value);
            topicInfo.Name = topic.SelectSingleNode("a").InnerText;
            topicInfo.TotalPages = Convert.ToInt32(Regex.Match(topic.SelectSingleNode(@"div[2]/div/a[last()]").GetAttributeValue("href", ""), $@".+/{topicInfo.FRId}/(\d+)").Groups[1].Value);
            topicInfo.ClaimedReplies = Convert.ToInt32(FRHelpers.CleanupFRHtmlText(newsTopic.SelectSingleNode(@"td[2]/div[1]").InnerText));

            var authorInfo = topic.SelectSingleNode("span/a");
            topicInfo.FRAuthorId = GetUserIdFromAnchor(authorInfo);
            topicInfo.AuthorName = authorInfo.InnerText;

            var lastPost = newsTopic.SelectSingleNode("td[3]");
            var lastPostAuthorInfo = lastPost.SelectSingleNode("a");
            topicInfo.LastPostFRAuthorId = GetUserIdFromAnchor(lastPostAuthorInfo);
            topicInfo.LastPostAuthorName = lastPostAuthorInfo.InnerText;
            topicInfo.LastPostTimestamp = DateTime.ParseExact(FRHelpers.CleanupFRHtmlText(lastPost.ChildNodes.Last().InnerText), "MMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            _logger.LogInformation($"Found post: \"{topicInfo.Name}\" ({topicInfo.TotalPages} pages) id: {topicInfo.FRId}, claimed replies: {topicInfo.ClaimedReplies}, last post: {topicInfo.LastPostAuthorName} id: {topicInfo.LastPostFRAuthorId}");

            _logger.LogInformation("Checking if topic is known");
            var dbTopic = await _dataContext.Topics.FirstOrDefaultAsync(x => x.FRTopicId == topicInfo.FRId);
            if (dbTopic == null)
            {
                _logger.LogInformation("New topic! Adding topic to database");
                _dataContext.Topics.Add(dbTopic = new Topic
                {
                    FRTopicId = topicInfo.FRId,
                    FRTopicName = topicInfo.Name,
                    TopicStarter = topicInfo.AuthorName,
                    TopicStarterClanId = topicInfo.FRAuthorId,
                    FRClaimedReplyCount = topicInfo.ClaimedReplies,
                });
                topicInfo.RequireParse = true;
            }
            else
            {
                _logger.LogInformation("Topic known, checking for changes..");
                dbTopic.FRClaimedReplyCount = topicInfo.ClaimedReplies;

                if (dbTopic.Posts.OrderBy(x => x.TimeStamp).LastOrDefault()?.TimeStamp != topicInfo.LastPostTimestamp)
                {
                    _logger.LogInformation("New post was added since last check");
                    topicInfo.RequireParse = true;
                }
                else if (dbTopic.FRClaimedReplyCount != topicInfo.ClaimedReplies)
                {
                    _logger.LogInformation("Reply count does not match up with known data, possibly deleted post?");
                    topicInfo.RequireParse = true;
                }
                else if (dbTopic.FRTopicName != topicInfo.Name)
                {
                    _logger.LogInformation("Topic name changed, saving changes and moving on to next post");
                    dbTopic.FRTopicName = topicInfo.Name;
                }
                else
                {
                    _logger.LogInformation("Nothing changed, skipping news post");
                }
            }
            await _dataContext.SaveChangesAsync();

            return topicInfo;
        }

        private async Task ParseNewsTopic(TopicInfo topicInfo)
        {
            var hasChanges = false;
            var skipPaginationCheck = false;
            var onLastPage = true;

            var client = new HtmlWeb();

            bool CheckLastPostDeletion(List<HtmlNode> posts)
            {
                onLastPage = false;
                if (!posts.Any(x =>
                    topicInfo.LastPostTimestamp == DateTime.ParseExact(x.SelectSingleNode("div[2]/div[2]/div[1]/div[1]").InnerText.Replace("</string>", ""), "MMMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture) &&
                    topicInfo.LastPostFRAuthorId == GetUserIdFromAnchor(x.SelectSingleNode("div[2]/div[1]/div[2]/a"))))
                {
                    _logger.LogInformation("Latest post seems to have been deleted before saving it, skipping checking remaining topic..");
                    return true;
                }
                return false;
            }

            for (var i = topicInfo.TotalPages; i > 0; i--)
            {
                _logger.LogInformation($"Navigating to page {i}..");
                var topicPage = client.Load($"https://www1.flightrising.com/forums/ann/{topicInfo.FRId}/{i}");
                var posts = topicPage.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                _logger.LogInformation($"Found {posts.Count} posts on this page");

                if (!skipPaginationCheck)
                {
                    skipPaginationCheck = true;
                    var pagination = topicPage.DocumentNode.QuerySelector("#forum-content .common-pagination");
                    if (pagination != null)
                    {
                        var actualExpectedPages = Convert.ToInt32(pagination.GetAttributeValue("data-max", ""));
                        if (actualExpectedPages > topicInfo.TotalPages)
                        {
                            _logger.LogInformation($"Forum lied to us, real page count is: {actualExpectedPages}. Parsing additional pages before proceeding..");
                            for (var iex = actualExpectedPages; iex > topicInfo.TotalPages; iex--)
                            {
                                _logger.LogInformation($"Navigating to page {iex}..");
                                var topicPageEx = client.Load($"https://www1.flightrising.com/forums/ann/{topicInfo.FRId}/{iex}");
                                var postsEx = topicPageEx.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                                _logger.LogInformation($"Found {postsEx.Count} posts on this page");
                                if (onLastPage && CheckLastPostDeletion(postsEx))
                                    break;

                                if (await ParseNewsTopicPage(postsEx, topicInfo))
                                    hasChanges = true;
                            }
                            _logger.LogInformation($"Finished parsing additional pages, continuing parsing page {i}..");
                        }
                    }
                }
                if (onLastPage && CheckLastPostDeletion(posts))
                    break;

                if (await ParseNewsTopicPage(posts, topicInfo))
                    hasChanges = true;
                else if (hasChanges && topicInfo.ClaimedReplies > 10 && posts.Count == 10)
                {
                    _logger.LogInformation("Posts seem to be as expected, skipping checking remaining topic..");
                    break;
                }
            }
        }

        private async Task<bool> ParseNewsTopicPage(List<HtmlNode> posts, TopicInfo topicInfo)
        {
            var hasChanges = false;

            foreach (var forumPost in posts)
            {
                var postId = Convert.ToInt32(Regex.Match(forumPost.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                _logger.LogInformation($"Checking if post {postId} is known..");

                var post = _dataContext.Posts.FirstOrDefault(x => x.FRPostId == postId);
                if (post == null)
                {
                    _logger.LogInformation($" New Post! Adding post to database");
                    var postAuthorId = Convert.ToInt32(GetUserIdFromAnchor(forumPost.SelectSingleNode("div[2]/div[1]/div[2]/a")));
                    var postAuthorName = forumPost.SelectSingleNode("div[2]/div[1]/div[2]/a").InnerText;
                    var postTimeStamp = DateTime.ParseExact(forumPost.SelectSingleNode("div[2]/div[2]/div[1]/div[1]").InnerText.Replace(" </strong>", ""), "MMMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var postContent = forumPost.SelectSingleNode("div[2]/div[2]/div[2]");
                    var postContentHtml = FRHelpers.CleanupFRHtmlText(forumPost.SelectSingleNode("div[2]/div[2]/div[2]").InnerHtml);

                    if (postId == topicInfo.FRId)
                    {
                        var itemsInContent = postContent.QuerySelectorAll(".bbcode-item-icon")
                            .Select(x => x.GetAttributeValue("data-itemid", null))
                            .Where(x => x != null)
                            .Select(x => Convert.ToInt32(x));

                        var skinsInContent = postContent.QuerySelectorAll(".skin-bbcode")
                            .Select(x => x.GetAttributeValue("skin-id", null))
                            .Where(x => x != null)
                            .Select(x => Convert.ToInt32(x));

                        var items = itemsInContent.Concat(skinsInContent).ToList();

                        if (items.Any())
                        {
                            var unknownItems = items.Except(_dataContext.FRItems.Where(x => items.Contains(x.FRId)).ToList().Select(x => x.FRId));
                            var newItems = new List<FRItem>();
                            foreach (var unknownItem in unknownItems)
                            {
                                var item = await _itemService.FetchItemFromFR(unknownItem);
                                if (item != null)
                                {
                                    newItems.Add(item);
                                }
                            }

                            if (FRHelpers.CheckForUnknownGenesOrBreed(newItems))
                            {
                                if (!DEBUG)
                                    await _pipelineService.TriggerPipeline(Environment.GetEnvironmentVariable("AzureDevOpsPipeline"));
                            }
                        }
                    }
                    (await _dataContext.Topics.FirstOrDefaultAsync(x => x.FRTopicId == topicInfo.FRId)).Posts.Add(new Post
                    {
                        FRPostId = postId,
                        PostAuthor = postAuthorName,
                        PostAuthorClanId = postAuthorId,
                        Content = postContentHtml,
                        TimeStamp = postTimeStamp
                    });
                    hasChanges = true;
                    await _dataContext.SaveChangesAsync();
                }
                else
                    _logger.LogInformation(" Already known.");
            }

            if (!hasChanges)
            {
                var firstPostId = Convert.ToInt32(Regex.Match(posts.First().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                var lastPostId = Convert.ToInt32(Regex.Match(posts.Last().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                _logger.LogInformation($"Checking if any post is deleted between first post (id: {firstPostId}) and last post (id: {lastPostId})");

                var expectedPosts = (await _dataContext.Topics.FirstOrDefaultAsync(x => x.FRTopicId == topicInfo.FRId)).Posts.Where(x => !x.Deleted && x.FRPostId >= firstPostId && x.FRPostId <= lastPostId).Select(x => new Post { Id = x.Id, FRPostId = x.FRPostId }).ToList();
                var givenPostIds = posts.Select(x => Convert.ToInt32(Regex.Match(x.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value)).OrderBy(x => x).ToList();

                if (!expectedPosts.Select(x => x.FRPostId).OrderBy(x => x).SequenceEqual(givenPostIds))
                {
                    _logger.LogInformation("Delete detected! Expected posts do not match up. Checking which post is deleted..");
                    foreach (var expectedPost in expectedPosts)
                        if (!givenPostIds.Contains(expectedPost.FRPostId))
                        {
                            _logger.LogInformation($"Found {expectedPost.FRPostId} to be deleted!");
                            _dataContext.Posts.Find(expectedPost.Id).Deleted = true;
                        }
                    hasChanges = true;
                    await _dataContext.SaveChangesAsync();
                }
            }
            return hasChanges;
        }

        static int GetUserIdFromAnchor(HtmlNode anchorNode) =>
            Convert.ToInt32(Regex.Match(anchorNode.GetAttributeValue("href", ""), @".+/clan-profile/(\d+)").Groups[1].Value);

        private class TopicInfo
        {
            public int FRId { get; set; }
            public string Name { get; set; }
            public int FRAuthorId { get; set; }
            public string AuthorName { get; set; }
            public bool RequireParse { get; set; }
            public int TotalPages { get; internal set; }
            public int ClaimedReplies { get; internal set; }
            public int LastPostFRAuthorId { get; internal set; }
            public string LastPostAuthorName { get; internal set; }
            public DateTime LastPostTimestamp { get; internal set; }
        }
    }
}
