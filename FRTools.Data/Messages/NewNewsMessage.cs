using FRTools.Data.DataModels.NewsReaderModels;

namespace FRTools.Data.Messages
{
    public class NewNewsMessage : GenericMessage
    {
        public Topic Topic { get; }

        public override string MessageType { get; set; } = nameof(NewNewsMessage);

        public NewNewsMessage(MessageCategory source, Topic topic) : base(source, "New news")
        {
            Topic = topic;
        }
    }
}
