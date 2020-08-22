using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class SendUpdateDominanceMessage : BaseJob
    {
        public override async Task JobTask()
        {
            var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.DominanceTracker, "Updated")))));
            await _serviceBus.CloseAsync();
        }

        public override string RelatedEntityId { get; set; } = "DominanceUpdate";
        public override string Description { get; set; }
    }
}
