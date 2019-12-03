using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRTools.Data.DataModels.NewsReaderModels
{
    public class Topic
    {
        public int Id { get; set; }
        [Index]
        public int FRTopicId { get; set; }
        public string FRTopicName { get; set; }
        public string TopicStarter { get; set; }
        public int TopicStarterClanId { get; set; }
        public int FRClaimedReplyCount { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
