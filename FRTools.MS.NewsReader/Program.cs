using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.DataModels.NewsReaderModels;
using FRTools.Data.Messages;
using HtmlAgilityPack;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace FRTools.MS.NewsReader
{
    class Program
    {
        private static IQueueClient _serviceBus;
        private const int _initialDelay = 30000;

        static int _delay = _initialDelay;
        static int Delay
        {
            get => _delay;
            set => _delay = value > _initialDelay ? _initialDelay : value;
        }

        static async Task Main()
        {

            while (true)
            {
                _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
                await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.NewsReader, "Started")))));

                try
                {
                    var changeDetected = await ParseNewsForum();
                    if (changeDetected)
                        Delay /= 2;
                    else if (Delay < _initialDelay)
                        Delay *= 2;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception! {ex.Message}");
                }
                Console.WriteLine($"Waiting {Delay} milliseconds before checking again..");

                await _serviceBus.CloseAsync();
                await Task.Delay(30000);
            }
        }

        static async Task<bool> ParseNewsForum()
        {
            var hasChanges = false;

            var client = new HtmlWeb();
            var mainNewsForum = client.Load("https://www1.flightrising.com/forums/ann");
            var topics = mainNewsForum.GetElementbyId("postlist").SelectNodes("tr");

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

                Console.WriteLine($"Found post: \"{topicName}\" ({totalPages} pages) id: {topicId}, claimed replies: {claimedReplies}, last post: {lastPostAuthorName} id: {lastPostAuthorId}");

                using (var ctx = new DataContext())
                {
                    Console.WriteLine("Checking if topic is known..");
                    var topic = ctx.Topics.FirstOrDefault(x => x.FRTopicId == topicId);
                    if (topic == null)
                    {
                        Console.WriteLine("New topic! Adding topic to database");
                        ctx.Topics.Add(topic = new Topic
                        {
                            FRTopicId = topicId,
                            TopicStarter = authorName,
                            TopicStarterClanId = authorId,
                            FRClaimedReplyCount = claimedReplies,
                            FRTopicName = topicName
                        });
                        ctx.SaveChanges();
                        await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewNewsMessage(MessageCategory.NewsReader, topic)))));
                    }
                    else
                    {
                        Console.WriteLine("Topic known, checking for changes..");
                        topic.FRClaimedReplyCount = claimedReplies;

                        if (topic.Posts.OrderBy(x => x.TimeStamp).LastOrDefault()?.TimeStamp != lastPostTimestamp)
                        {
                            Console.WriteLine("New post was added since last check");
                        }
                        else if (topic.FRClaimedReplyCount != claimedReplies)
                        {
                            Console.WriteLine("Reply count does not match up with known data, possibly deleted post!");
                        }
                        else if (topic.FRTopicName != topicName)
                        {
                            Console.WriteLine("Topic name changed, saving changes and moving on to next post");
                            topic.FRTopicName = topicName;
                            ctx.SaveChanges();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Nothing changed, skipping news post");
                            continue;
                        }
                    }
                    hasChanges = await ParseNewsTopic(topicId, totalPages, claimedReplies, lastPostAuthorId, lastPostTimestamp, topic, ctx) || hasChanges;
                }
            }

            return hasChanges;
        }

        static async Task<bool> ParseNewsTopic(int topicId, int expectedPages, int claimedReplies, int lastPostAuthorId, DateTime lastPostTimestamp, Topic topic, DataContext ctx)
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
                    Console.WriteLine("Latest post seems to have been deleted before saving it, skipping checking remaining topic..");
                    return true;
                }
                return false;
            }

            for (var i = expectedPages; i > 0; i--)
            {
                Console.WriteLine($"Navigating to page {i}..");
                var topicPage = client.Load($"https://www1.flightrising.com/forums/ann/{topicId}/{i}");
                var posts = topicPage.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                Console.WriteLine($"Found {posts.Count} posts on this page");

                if (!skipPaginationCheck)
                {
                    skipPaginationCheck = true;
                    var pagination = topicPage.DocumentNode.QuerySelector("#forum-content .common-pagination");
                    if (pagination != null)
                    {
                        var actualExpectedPages = Convert.ToInt32(pagination.GetAttributeValue("data-max", ""));
                        if (actualExpectedPages > expectedPages)
                        {
                            Console.WriteLine($"Forum lied to us, real page count is: {actualExpectedPages}. Parsing additional pages before proceeding..");
                            for (var iex = actualExpectedPages; iex > expectedPages; iex--)
                            {
                                Console.WriteLine($"Navigating to page {iex}..");
                                var topicPageEx = client.Load($"https://www1.flightrising.com/forums/ann/{topicId}/{iex}");
                                var postsEx = topicPageEx.DocumentNode.QuerySelectorAll("#forum-content .post").ToList();
                                Console.WriteLine($"Found {postsEx.Count} posts on this page");
                                if (onLastPage && CheckLastPostDeletion(postsEx))
                                    break;

                                if (await ParseNewsTopicPage(postsEx, topic, ctx))
                                    hasChanges = true;
                            }
                            Console.WriteLine($"Finished parsing additional pages, continuing parsing page {i}..");
                        }
                    }
                }
                if (onLastPage && CheckLastPostDeletion(posts))
                    break;

                if (await ParseNewsTopicPage(posts, topic, ctx))
                    hasChanges = true;
                else if (hasChanges && claimedReplies > 10 && posts.Count == 10)
                {
                    Console.WriteLine("Posts seem to be as expected, skipping checking remaining topic..");
                    break;
                }
            }

            return hasChanges;
        }

        static async Task<bool> ParseNewsTopicPage(List<HtmlNode> posts, Topic topic, DataContext ctx)
        {
            var hasChanges = false;

            foreach (var forumPost in posts)
            {
                var postId = Convert.ToInt32(Regex.Match(forumPost.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                Console.Write($"Checking if post {postId} is known..");

                var post = ctx.Posts.FirstOrDefault(x => x.FRPostId == postId);
                if (post == null)
                {
                    Console.WriteLine($" New Post! Adding post to database");
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
                            var unknownItems = items.Except(ctx.FRItems.Where(x => items.Contains(x.FRId)).ToList().Select(x => x.FRId));
                            var newItems = new List<FRItem>();
                            foreach (var unknownItem in unknownItems)
                            {
                                var item = await FRHelpers.FetchItem(unknownItem);
                                if (item != null)
                                {
                                    newItems.Add(item);
                                    await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.NewsReader, item)))));
                                }
                            }

                            if (FRHelpers.CheckForUnknownGenesOrRace(newItems))
                            {
                                AzurePipeLineService.TriggerRegenerateClassesPipeline();
                            }
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
                    ctx.SaveChanges();
                }
                else
                    Console.WriteLine(" Already known.");
            }
            if (!hasChanges)
            {
                var firstPostId = Convert.ToInt32(Regex.Match(posts.First().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                var lastPostId = Convert.ToInt32(Regex.Match(posts.Last().GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value);
                Console.WriteLine($"Checking if any post is deleted between first post (id: {firstPostId}) and last post (id: {lastPostId})");

                var expectedPosts = topic.Posts.Where(x => !x.Deleted && x.FRPostId >= firstPostId && x.FRPostId <= lastPostId).Select(x => new Post { Id = x.Id, FRPostId = x.FRPostId }).ToList();
                var givenPostIds = posts.Select(x => Convert.ToInt32(Regex.Match(x.GetAttributeValue("id", ""), @"post_(\d+)").Groups[1].Value)).OrderBy(x => x).ToList();

                if (!expectedPosts.Select(x => x.FRPostId).OrderBy(x => x).SequenceEqual(givenPostIds))
                {
                    Console.WriteLine("Delete detected! Expected posts do not match up. Checking which post is deleted..");
                    foreach (var expectedPost in expectedPosts)
                        if (!givenPostIds.Contains(expectedPost.FRPostId))
                        {
                            Console.WriteLine($"Found {expectedPost.FRPostId} to be deleted!");
                            ctx.Posts.Find(expectedPost.Id).Deleted = true;
                        }
                    hasChanges = true;
                    ctx.SaveChanges();
                }
            }

            await Task.Delay(100);
            return hasChanges;
        }

        static int GetUserIdFromAnchor(HtmlNode anchorNode) =>
            Convert.ToInt32(Regex.Match(anchorNode.GetAttributeValue("href", ""), @".+/clan-profile/(\d+)").Groups[1].Value);

    }
}
