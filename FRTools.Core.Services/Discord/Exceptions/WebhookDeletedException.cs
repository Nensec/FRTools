namespace FRTools.Core.Services.Discord.Exceptions
{
    public class WebhookDeletedException : Exception
    {
        public string WebhookUrl { get; }

        public WebhookDeletedException(string webhookUrl)
        {
            WebhookUrl = webhookUrl;
        }
    }
}
