using Microsoft.EntityFrameworkCore;

namespace FRTools.Core.Data.DataModels.NewsReaderModels
{
    [Index(nameof(FRPostId))]
    public class Post
    {
        public virtual Topic Topic { get; set; }

        public int Id { get; set; }
        public int FRPostId { get; set; }
        public string Content { get; set; }
        public string PostAuthor { get; set; }
        public int PostAuthorClanId { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeStamp { get; set; }

        public int Reports { get; set; }
    }

}
