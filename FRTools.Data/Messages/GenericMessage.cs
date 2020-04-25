namespace FRTools.Data.Messages
{
    public class GenericMessage
    {
        public GenericMessage(MessageCategory category, string message)
        {
            Category = category;
            Message = message;
        }

        public MessageCategory Category { get; set; }
        public string Message { get; set; }
        public long? DiscordServer { get; set; }
        public string MessageType { get; private set; } = nameof(GenericMessage);
    }
}
