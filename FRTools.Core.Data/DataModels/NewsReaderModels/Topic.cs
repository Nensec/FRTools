using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FRTools.Core.Data.DataModels.NewsReaderModels
{
    [Index(nameof(FRTopicId))]
    public class Topic
    {
        public int Id { get; set; }
        public int FRTopicId { get; set; }
        public string FRTopicName { get; set; }
        public string TopicStarter { get; set; }
        public int TopicStarterClanId { get; set; }
        public int FRClaimedReplyCount { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
