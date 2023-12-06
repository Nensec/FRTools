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
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class NewsTrackerFunction : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly IFRItemService _itemService;

        public NewsTrackerFunction(DataContext dataContext, IFRItemService itemService)
        {
            _dataContext = dataContext;
            _itemService = itemService;
        }

        [FunctionName(nameof(NewsTracker))]
        public async Task NewsTracker([TimerTrigger("0 */5 * * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            var mainNewsForum = await Common.Helpers.LoadHtmlPage("https://www1.flightrising.com/forums/ann");
            var topics = mainNewsForum.GetElementbyId("postlist").SelectNodes("tr");

            log.LogInformation($"Found {topics.Count} topics");
            foreach (var newsTopic in topics)
            {
                var topicInfo = newsTopic.SelectSingleNode("td[1]");
                var topicId = Convert.ToInt32(Regex.Match(topicInfo.SelectSingleNode("a").GetAttributeValue("href", ""), @".+/ann/(\d+)").Groups[1].Value);
                var topicName = topicInfo.SelectSingleNode("a").InnerText;

                var authorInfo = topicInfo.SelectSingleNode("span/a");
                var authorId = GetUserIdFromAnchor(authorInfo);
                var authorName = authorInfo.InnerText;

                var totalPages = Convert.ToInt32(Regex.Match(topicInfo.SelectSingleNode(@"div[2]/div/a[last()]").GetAttributeValue("href", ""), $@".+/{topicId}/(\d+)").Groups[1].Value);
                var claimedReplies = Convert.ToInt32(FRHelpers.CleanupFRHtmlText(newsTopic.SelectSingleNode(@"td[2]/div[1]").InnerText));

                var lastPost = newsTopic.SelectSingleNode("td[3]");
                var lastPostAuthorInfo = lastPost.SelectSingleNode("a");
                var lastPostAuthorId = GetUserIdFromAnchor(lastPostAuthorInfo);
                var lastPostAuthorName = lastPostAuthorInfo.InnerText;
                var lastPostTimestamp = DateTime.ParseExact(FRHelpers.CleanupFRHtmlText(lastPost.ChildNodes.Last().InnerText), "MMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                log.LogInformation($"Found post: \"{topicName}\" ({totalPages} pages) id: {topicId}, claimed replies: {claimedReplies}, last post: {lastPostAuthorName} id: {lastPostAuthorId}");

                log.LogInformation("Checking if topic is known");
                var topic = _dataContext.Topics.FirstOrDefault(x => x.FRTopicId == topicId);
                if (topic == null)
                {
                    log.LogInformation("New topic! Adding topic to database");
                    _dataContext.Topics.Add(topic = new Topic
                    {
                        FRTopicId = topicId,
                        TopicStarter = authorName,
                        TopicStarterClanId = authorId,
                        FRClaimedReplyCount = claimedReplies,
                        FRTopicName = topicName
                    });
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    log.LogInformation("Topic known, checking for changes..");
                    topic.FRClaimedReplyCount = claimedReplies;

                    if (topic.Posts.OrderBy(x => x.TimeStamp).LastOrDefault()?.TimeStamp != lastPostTimestamp)
                    {
                        log.LogInformation("New post was added since last check");
                    }
                    else if (topic.FRClaimedReplyCount != claimedReplies)
                    {
                        log.LogInformation("Reply count does not match up with known data, possibly deleted post!");
                    }
                    else if (topic.FRTopicName != topicName)
                    {
                        log.LogInformation("Topic name changed, saving changes and moving on to next post");
                        topic.FRTopicName = topicName;
                        await _dataContext.SaveChangesAsync();
                        continue;
                    }
                    else
                    {
                        log.LogInformation("Nothing changed, skipping news post");
                        continue;
                    }
                }
                await ParseNewsTopic(topicId, totalPages, claimedReplies, lastPostAuthorId, lastPostTimestamp, topic, log);
            }
        }

        private async Task ParseNewsTopic(int topicId, int expectedPages, int claimedReplies, int lastPostAuthorId, DateTime lastPostTimestamp, Topic topic, ILogger log)
        {
            var hasChanges = false;
            var skipPaginationCheck = false;
            var onLastPage = true;

            var client = new HtmlWeb();

            bool CheckLastPostDeletion(List<HtmlNode> posts)
            {
                onLastPage = false;
                if (!posts.Any(x =>
                    lastPostTimestamp == DateTime.ParseExact(x.SelectSingleNode("div[2]/div[2]/div[1]/div[1]").InnerText.Replace("</string>", ""), "MMMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture) &&
                    lastPostAuthorId == GetUserIdFromAnchor(x.SelectSingleNode("div[2]/div[1]/div[2]/a"))))
                {
                    log.LogInformation("Latest post seems to have been deleted before saving it, skipping checking remaining topic..");
                    return true;
                }
                return false;
            }

            for (var i = expectedPages; i > 0; i--)
            {
                log.LogInformation($"Navigating to page {i}..");
                var topicPage = client.Load($"https://www1.flightrising.com/forums/ann/{topicId}/{i}");
                var posts = topicPage.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                log.LogInformation($"Found {posts.Count} posts on this page");

                if (!skipPaginationCheck)
                {
                    skipPaginationCheck = true;
                    var pagination = topicPage.DocumentNode.QuerySelector("#forum-content .common-pagination");
                    if (pagination != null)
                    {
                        var actualExpectedPages = Convert.ToInt32(pagination.GetAttributeValue("data-max", ""));
                        if (actualExpectedPages > expectedPages)
                        {
                            log.LogInformation($"Forum lied to us, real page count is: {actualExpectedPages}. Parsing additional pages before proceeding..");
                            for (var iex = actualExpectedPages; iex > expectedPages; iex--)
                            {
                                log.LogInformation($"Navigating to page {iex}..");
                                var topicPageEx = client.Load($"https://www1.flightrising.com/forums/ann/{topicId}/{iex}");
                                var postsEx = topicPageEx.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                                log.LogInformation($"Found {postsEx.Count} posts on this page");
                                if (onLastPage && CheckLastPostDeletion(postsEx))
                                    break;

                                if (await ParseNewsTopicPage(postsEx, topic, log))
                                    hasChanges = true;
                            }
                            log.LogInformation($"Finished parsing additional pages, continuing parsing page {i}..");
                        }
                    }
                }
                if (onLastPage && CheckLastPostDeletion(posts))
                    break;

                if (!await ParseNewsTopicPage(posts, topic, log) && hasChanges && claimedReplies > 10 && posts.Count == 10)
                {
                    log.LogInformation("Posts seem to be as expected, skipping checking remaining topic..");
                    break;
                }
            }
        }

        private async Task<bool> ParseNewsTopicPage(List<HtmlNode> posts, Topic topic, ILogger log)
        {
            var hasChanges = false;

            foreach (var forumPost in posts)
            {
                var postId = Convert.ToInt32(Regex.Match(forumPost.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                log.LogInformation($"Checking if post {postId} is known..");

                var post = _dataContext.Posts.FirstOrDefault(x => x.FRPostId == postId);
                if (post == null)
                {
                    log.LogInformation($" New Post! Adding post to database");
                    var postAuthorId = Convert.ToInt32(GetUserIdFromAnchor(forumPost.SelectSingleNode("div[2]/div[1]/div[2]/a")));
                    var postAuthorName = forumPost.SelectSingleNode("div[2]/div[1]/div[2]/a").InnerText;
                    var postTimeStamp = DateTime.ParseExact(forumPost.SelectSingleNode("div[2]/div[2]/div[1]/div[1]").InnerText.Replace(" </strong>", ""), "MMMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var postContent = forumPost.SelectSingleNode("div[2]/div[2]/div[2]");
                    var postContentHtml = FRHelpers.CleanupFRHtmlText(forumPost.SelectSingleNode("div[2]/div[2]/div[2]").InnerHtml);

                    if (postId == topic.FRTopicId)
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

                            //if (FRHelpers.CheckForUnknownGenesOrBreed(newItems))
                            //{
                            //    AzurePipeLineService.TriggerRegenerateClassesPipeline();
                            //}
                        }
                    }

                    topic.Posts.Add(new Post
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
                    log.LogInformation(" Already known.");
            }

            if (!hasChanges)
            {
                var firstPostId = Convert.ToInt32(Regex.Match(posts.First().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                var lastPostId = Convert.ToInt32(Regex.Match(posts.Last().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                log.LogInformation($"Checking if any post is deleted between first post (id: {firstPostId}) and last post (id: {lastPostId})");

                var expectedPosts = topic.Posts.Where(x => !x.Deleted && x.FRPostId >= firstPostId && x.FRPostId <= lastPostId).Select(x => new Post { Id = x.Id, FRPostId = x.FRPostId }).ToList();
                var givenPostIds = posts.Select(x => Convert.ToInt32(Regex.Match(x.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value)).OrderBy(x => x).ToList();

                if (!expectedPosts.Select(x => x.FRPostId).OrderBy(x => x).SequenceEqual(givenPostIds))
                {
                    log.LogInformation("Delete detected! Expected posts do not match up. Checking which post is deleted..");
                    foreach (var expectedPost in expectedPosts)
                        if (!givenPostIds.Contains(expectedPost.FRPostId))
                        {
                            log.LogInformation($"Found {expectedPost.FRPostId} to be deleted!");
                            _dataContext.Posts.Find(expectedPost.Id).Deleted = true;
                        }
                    hasChanges = true;
                    await _dataContext.SaveChangesAsync();
                }
            }

            await Task.Delay(100);
            return hasChanges;
        }

        static int GetUserIdFromAnchor(HtmlNode anchorNode) =>
            Convert.ToInt32(Regex.Match(anchorNode.GetAttributeValue("href", ""), @".+/clan-profile/(\d+)").Groups[1].Value);
    }
}
