using System;
using System.Collections.Generic;

namespace FRTools.Web.Models
{
    public class NewsViewModel
    {
        public List<NewsTopicViewModel> Topics { get; set; }
    }

    public class NewsTopicViewModel
    {
        public int FRTopicId { get; set; }
        public string TopicName { get; set; }
        public int TopicStarterClanId { get; set; }
        public string TopicStarer { get; set; }
        public int TotalPosts { get; set; }
        public int DeletedPosts { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CurrentPage { get; set; } = 1;
        public List<NewsPostViewModel> Posts { get; set; } = new List<NewsPostViewModel>();
        public bool DeletedOnly { get; set; } = false;
    }

    public class NewsPostViewModel
    {
        public int FRPostId { get; set; }
        public int PostAuthorClanId { get; set; }
        public string PostAuthor { get; set; }
        public string RawHtmlContent { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Reports { get; set; }
        public int ExpectedFRPage { get; set; }
    }
}