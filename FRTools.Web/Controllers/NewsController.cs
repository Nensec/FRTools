﻿using FRTools.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("newsreader")]
    public class NewsController : BaseController
    {
        [Route(Name = "NewsReader")]
        public ActionResult Index() // Todo: Paging for the news topic list? 
        {
            var model = new NewsViewModel();
            model.Topics = DataContext.Topics.OrderByDescending(x => x.FRTopicId).Select(x => new NewsTopicViewModel
            {
                FRTopicId = x.FRTopicId,
                TopicName = x.FRTopicName,
                TotalPosts = x.Posts.Count(),
                DeletedPosts = x.Posts.Count(p => p.Deleted),
                TopicStarterClanId = x.TopicStarterClanId,
                TopicStarer = x.TopicStarter,
                CreatedAt = x.Posts.OrderBy(p => p.TimeStamp).FirstOrDefault().TimeStamp
            }).ToList();

            return View(model);
        }

        [Route("view/{topicId}", Name = "ReadNews")]
        public ActionResult Read(int topicId, PaginationModel pagination)
        {
            var model = new NewsTopicViewModel { FRTopicId = topicId };
            model.Pagination = new PaginationModel("ReadNews", pagination.Page, 10);
            var topic = DataContext.Topics.FirstOrDefault(x => x.FRTopicId == topicId);
            if (topic == null)
            {
                AddErrorNotification($"Could not find topic id {topicId}.");
                return RedirectToRoute("NewsReader");
            }
            var posts = topic.Posts.OrderBy(x => x.TimeStamp).ToList();
            model.Pagination.TotalItems = posts.Count();
            var firstPost = posts.First();
            model.TopicStarterClanId = topic.TopicStarterClanId;
            model.TopicStarer = topic.TopicStarter;
            model.CreatedAt = firstPost.TimeStamp;
            model.TotalPosts = posts.Count();
            model.DeletedPosts = posts.Count(x => x.Deleted);
            model.TopicName = topic.FRTopicName;
            model.Posts = posts.Skip(model.Pagination.PageSize * (model.Pagination.Page - 1)).Take(model.Pagination.PageSize).Select(x => new NewsPostViewModel
            {
                FRPostId = x.FRPostId,
                PostAuthor = x.PostAuthor,
                PostAuthorClanId = x.PostAuthorClanId,
                CreatedAt = x.TimeStamp,
                IsDeleted = x.Deleted,
                RawHtmlContent = SanitizeFRRawContent(x.Content),
                Reports = x.Reports,
                ExpectedFRPage = (int)Math.Ceiling(posts.Count(p => !p.Deleted && p.FRPostId <= x.FRPostId) / 10d)
            }).ToList();

            return View(model);
        }

        [Route("viewdeleted/{topicId}", Name = "ReadDeletedNews")]
        public ActionResult ReadDeletedOnly(int topicId, PaginationModel pagination)
        {
            var model = new NewsTopicViewModel { FRTopicId = topicId, DeletedOnly = true };
            model.Pagination = new PaginationModel("ReadDeletedNews", pagination.Page, 10);
            var topic = DataContext.Topics.FirstOrDefault(x => x.FRTopicId == topicId);
            if (topic == null)
            {
                AddErrorNotification($"Could not find topic id {topicId}.");
                return RedirectToRoute("NewsReader");
            }
            var posts = topic.Posts.OrderBy(x => x.TimeStamp).ToList();
            var firstPost = posts.First();
            model.TopicStarterClanId = topic.TopicStarterClanId;
            model.TopicStarer = topic.TopicStarter;
            model.CreatedAt = firstPost.TimeStamp;
            model.TotalPosts = posts.Count();
            model.Pagination.TotalItems = model.DeletedPosts = posts.Count(x => x.Deleted);
            model.TopicName = topic.FRTopicName;
            model.Posts = posts.Where(x => x.Deleted).Skip(model.Pagination.PageSize * (model.Pagination.Page - 1)).Take(model.Pagination.PageSize).Select(x => new NewsPostViewModel
            {
                FRPostId = x.FRPostId,
                PostAuthor = x.PostAuthor,
                PostAuthorClanId = x.PostAuthorClanId,
                CreatedAt = x.TimeStamp,
                IsDeleted = x.Deleted,
                RawHtmlContent = SanitizeFRRawContent(x.Content),
                Reports = x.Reports
            }).ToList();

            return View("Read", model);
        }

        [Route("report/{postId}", Name = "ReportPost")]
        public ActionResult Report(int postId)
        {
            var post = DataContext.Posts.FirstOrDefault(x => x.FRPostId == postId && x.Deleted);
            if (post != null)
            {
                post.Reports += 1;
                DataContext.SaveChanges();
            }
            return Content("");
        }

        private string SanitizeFRRawContent(string rawcontent)
        {
            var document = new HtmlDocument();
            document.LoadHtml(rawcontent);
            document.QuerySelectorAll("img").ToList().ForEach(x =>
            {
                var imgSrc = x.Attributes?["src"];
                if (imgSrc != null && imgSrc.Value.StartsWith("/"))
                {
                    imgSrc.Value = $"https://www1.flightrising.com{imgSrc.Value}";
                }
            });

            document.QuerySelectorAll(".bbcode_quote").ToList().ForEach(x => x.Attributes["class"].Value += " card border-info mt-3");
            document.QuerySelectorAll(".bbcode_quote_head").ToList().ForEach(x => x.Attributes["class"].Value += " card-header");
            document.QuerySelectorAll(".bbcode_quote_body").ToList().ForEach(x => x.Attributes["class"].Value += " card-body");
            document.QuerySelectorAll("script").ToList().ForEach(x => x.Remove());
            document.QuerySelectorAll(".gamedb-bbcode-icon").ToList().ForEach(x => x.Remove());
            return document.DocumentNode.OuterHtml;
        }
    }
};