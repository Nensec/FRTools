using FRTools.Data;
using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.NewsReader
{
    class Program
    {
        private const int _initialDelay = 30000;

        static int _delay = _initialDelay;
        static int Delay
        {
            get => _delay;
            set => _delay = value > _initialDelay ? _initialDelay : value;
        }

        static async Task Main()
        {
            var loop = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
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
                    await Task.Delay(Delay);
                }
            });

            await Task.Delay(-1);

            Console.ReadKey();
        }

        private static async Task<bool> ParseNewsForum()
        {
            bool changed = false;
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading news forum..");
                var mainNewsForum = client.DownloadString("http://www1.flightrising.com/forums/ann");

                Console.WriteLine("Parsing topic rows..");
                var topicRows = Regex.Matches(mainNewsForum, @"<tr+\s+[a-zA-Z]+=+[""a-z]+>[\s\S]+?</tr>").Cast<Match>();

                var newsTopics = topicRows.Select(x => x.Groups[0].Value).Where(x => !x.Contains("poststatus-locked")).ToList();
                Console.WriteLine($"Found {newsTopics.Count} that are not locked, parsing those..");
                foreach (var newsTopic in newsTopics)
                {
                    var newsTopicInfo = Regex.Match(newsTopic, @"<a href=""(http://www1.flightrising.com/forums/ann/(\d+))"" class=""forumlink"">([\s\S]+?)</a>");
                    var topicUrl = newsTopicInfo.Groups[1].Value;
                    var topicId = Convert.ToInt32(newsTopicInfo.Groups[2].Value);
                    var topicName = newsTopicInfo.Groups[3].Value;

                    var claimedReplies = Convert.ToInt32(Regex.Match(newsTopic, @"<div class=""activity-replies"">\s+(\d+)\s+</div>").Groups[1].Value);
                    var totalPages = Convert.ToInt32(Regex.Matches(newsTopic, $@"<a href=""http://www1.flightrising.com/forums/ann/{topicId}/(\d+)"">").Cast<Match>().Last().Groups[1].Value);

                    var lastPostInfo = Regex.Match(newsTopic, @"<td class='lastpost'>[.\s\S]+<a href=""http://www1.flightrising.com/clan-profile/(\d+)"">([a-zA-Z0-9]+)</a>[\s\S]+<br>\s+?([a-zA-Z]+ \d+, \d{4} \d\d:\d\d:\d\d)");
                    var lastPostAuthorClanId = Convert.ToInt32(lastPostInfo.Groups[1].Value);
                    var lastPostAuthor = lastPostInfo.Groups[2].Value;
                    var lastPostTimestamp = DateTime.ParseExact(lastPostInfo.Groups[3].Value, "MMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    var latestNewsAuthorInfo = Regex.Match(newsTopic, @"<a href=""http://www1.flightrising.com/clan-profile/(\d+)"">([a-zA-Z0-9]+)</a>");
                    var authorClanId = Convert.ToInt32(latestNewsAuthorInfo.Groups[1].Value);
                    var authorName = latestNewsAuthorInfo.Groups[2].Value;
                    Console.WriteLine($"Found post: \"{topicName}\" ({totalPages} pages) id: {topicId}, claimed replies: {claimedReplies}, last post: {lastPostAuthor} id: {lastPostAuthorClanId}");

                    using (var ctx = new DataContext())
                    {
                        bool parseTopic = false;
                        Console.WriteLine("Checking if topic is known..");
                        var topic = ctx.Topics.FirstOrDefault(x => x.FRTopicId == topicId);
                        if (topic == null)
                        {
                            Console.WriteLine("New topic! Adding topic to database");
                            ctx.Topics.Add(topic = new Topic
                            {
                                FRTopicId = topicId,
                                TopicStarter = authorName,
                                TopicStarterClanId = authorClanId,
                                FRClaimedReplyCount = claimedReplies
                            });
                            ctx.SaveChanges();
                            parseTopic = true;
                        }
                        else
                        {
                            Console.WriteLine("Topic known, checking for changes..");
                            topic.FRClaimedReplyCount = claimedReplies;

                            if (topic.Posts.OrderBy(x => x.TimeStamp).LastOrDefault()?.TimeStamp != lastPostTimestamp)
                            {
                                Console.WriteLine("New post was added since last check");
                                parseTopic = true;
                            }
                            else if (topic.FRClaimedReplyCount != claimedReplies)
                            {
                                Console.WriteLine("Reply count does not match up with known data, possibly deleted post!");
                                parseTopic = true;
                            }
                            else
                                Console.WriteLine("Nothing changed, skipping news post");
                        }
                        if (parseTopic)
                            changed = await ParseNewsTopic(topicId, totalPages, claimedReplies, ctx, topic) || changed;
                    }
                }
            }
            return changed;
        }

        private static async Task<bool> ParseNewsTopic(int topicId, int expectedPages, int claimedReplies, DataContext ctx, Topic topic)
        {
            bool changeFound = false;
            bool skipPaginationCheck = false;
            using (var client = new WebClient())
            {
                for (int i = expectedPages; i > 0; i--)
                {
                    Console.WriteLine($"Navigating to page {i}..");
                    var forumPage = client.DownloadString($"http://www1.flightrising.com/forums/ann/{topicId}/{i}");
                    var postsPattern = @"<div id=""post_(\d+)"".+>[.\s\S]+?(?=(?=<div id=""post_|(?=<div class=""common-pagination"">|<div id=""topic-footer"">)))";
                    var posts = Regex.Matches(forumPage, postsPattern).Cast<Match>().ToList();
                    Console.WriteLine($"Found {posts.Count} posts on this page");

                    if (!skipPaginationCheck)
                    {
                        skipPaginationCheck = true;
                        var pagination = Regex.Match(forumPage, @"<div class=""common-pagination-numbers"">[\s\S]+?</div>");
                        if (pagination.Success)
                        {
                            var actualPages = Regex.Matches(pagination.Value, @"<a href=""http://www1.flightrising.com/forums/ann/\d+/(\d+)"">\d+</a>|<strong>(\d+)</strong>");
                            var realexpectedPages = actualPages.Cast<Match>().Max(x => Convert.ToInt32(x.Groups[1].Success ? x.Groups[1].Value : x.Groups[2].Value));
                            if (realexpectedPages > expectedPages)
                            {
                                Console.WriteLine($"Forum lied to us, real page count is: {realexpectedPages}. Parsing additional pages before proceeding..");
                                for (int iex = realexpectedPages; iex > expectedPages; iex--)
                                {
                                    Console.WriteLine($"Navigating to page {iex}..");
                                    var postsEx = Regex.Matches(client.DownloadString($"http://www1.flightrising.com/forums/ann/{topicId}/{iex}"), postsPattern).Cast<Match>().ToList();
                                    Console.WriteLine($"Found {postsEx.Count} posts on this page");
                                    if (await ParseNewsTopicEx(postsEx, ctx, topic))
                                        changeFound = true;
                                }
                                Console.WriteLine($"continuing parsing page {i}..");
                            }
                        }
                    }
                    if (await ParseNewsTopicEx(posts, ctx, topic))
                        changeFound = true;
                    else if (changeFound && claimedReplies > 10 && posts.Count == 10)
                    {
                        Console.WriteLine("Posts seem to be as expected, skipping checking remaining topic..");
                        break;
                    }
                }
            }
            return changeFound;
        }

        private static async Task<bool> ParseNewsTopicEx(List<Match> posts, DataContext ctx, Topic topic)
        {
            bool changeFound = false;

            foreach (var forumPost in posts)
            {
                var postId = Convert.ToInt32(forumPost.Groups[1].Value);
                Console.Write($"Checking if post {postId} is known..");

                var post = ctx.Posts.FirstOrDefault(x => x.FRPostId == postId);
                if (post == null)
                {
                    Console.WriteLine($" New Post! Adding post to database");
                    var postAuthorInfoFrame = Regex.Match(forumPost.Groups[0].Value, @"<div class=""post-author"".+>[.\s\S]+?(?=<div class=""post-text)").Groups[0].Value;
                    var postAuthorInfo = Regex.Match(postAuthorInfoFrame, @"<a href=""http://www1.flightrising.com/clan-profile/(\d+)"".+>([a-zA-Z0-9]+)</a>");
                    var postAuthorClanId = Convert.ToInt32(postAuthorInfo.Groups[1].Value);
                    var postAuthorName = postAuthorInfo.Groups[2].Value;
                    var postInfoFrame = Regex.Match(forumPost.Groups[0].Value, @"<div class=""post-text"">[\s\S]+<div class=""post-timestamp""><strong>([a-zA-Z]+ \d+, \d{4}</strong> \d\d:\d\d:\d\d)[\s\S]+?(?=</div>[\s\n\r]+?</div>[\s\n\r]+?</div>[\s\n\r]+?</div>)</div>");
                    var postTimeStamp = DateTime.ParseExact(postInfoFrame.Groups[1].Value.Replace("</strong>", ""), "MMMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var postInfoContent = Regex.Match(postInfoFrame.Groups[0].Value, @"<div class=""post-text-content"">[.\s\S]+").Groups[0].Value;

                    if (postInfoContent.Contains(@"<div class=""post-text-signature"">"))
                        postInfoContent = postInfoContent.Substring(0, postInfoContent.IndexOf(@"<div class=""post-text-signature"">"));

                    if (postInfoContent.Contains(@"<div class=""post-text-footer"">"))
                        postInfoContent = postInfoContent.Substring(0, postInfoContent.IndexOf(@"<div class=""post-text-footer"">"));

                    topic.Posts.Add(new Post
                    {
                        FRPostId = postId,
                        PostAuthor = postAuthorName,
                        PostAuthorClanId = postAuthorClanId,
                        Content = postInfoContent,
                        TimeStamp = postTimeStamp
                    });
                    changeFound = true;
                    ctx.SaveChanges();
                }
                else
                    Console.WriteLine(" Nope.");
            }
            if (!changeFound)
            {
                var firstPostId = Convert.ToInt32(posts.First().Groups[1].Value);
                var lastPostId = Convert.ToInt32(posts.Last().Groups[1].Value);
                Console.WriteLine($"Checking if any post is deleted between first post (id: {firstPostId}) and last post (id: {lastPostId})");

                var expectedPosts = topic.Posts.Where(x => !x.Deleted && x.FRPostId >= firstPostId && x.FRPostId <= lastPostId).Select(x => new Post { Id = x.Id, FRPostId = x.FRPostId }).ToList();
                var givenPostIds = posts.Select(x => Convert.ToInt32(x.Groups[1].Value)).ToList();

                if (!expectedPosts.Select(x => x.FRPostId).SequenceEqual(givenPostIds))
                {
                    Console.WriteLine("Delete detected! Expected posts do not match up. Checking which post is deleted..");
                    foreach (var expectedPost in expectedPosts)
                        if (!givenPostIds.Contains(expectedPost.FRPostId))
                        {
                            Console.WriteLine($"Found {expectedPost.FRPostId} to be deleted!");
                            ctx.Posts.Find(expectedPost.Id).Deleted = true;
                        }
                    changeFound = true;
                    ctx.SaveChanges();
                }
            }
            await Task.Delay(100);
            return changeFound;
        }
    }

}
