using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRTools.Data.DataModels
{
    public class Post
    {
        public virtual Topic Topic { get; set; }

        public int Id { get; set; }
        [Index]
        public int FRPostId { get; set; }
        public string Content { get; set; }
        public string PostAuthor { get; set; }
        public int PostAuthorClanId { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeStamp { get; set; }
    }

}
