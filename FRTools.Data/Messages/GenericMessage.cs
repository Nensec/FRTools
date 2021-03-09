namespace FRTools.Data.Messages
{
    public class GenericMessage
    {
        public GenericMessage(MessageCategory source, string message)
        {
            Source = source;
            Message = message;
        }

        public virtual MessageCategory Source { get; set; }
        public virtual string Message { get; set; }
        public virtual long? DiscordServer { get; set; }
        public virtual string MessageType { get; } = nameof(GenericMessage);
    }
}
