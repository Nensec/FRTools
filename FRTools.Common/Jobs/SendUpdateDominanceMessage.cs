using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class SendUpdateDominanceMessage : IJob
    {
        public async Task JobTask()
        {
            var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.DominanceTracker, "Updated")))));
            await _serviceBus.CloseAsync();
        }

        public string RelatedEntityId { get; set; }
        public string Description { get; set; }
    }
}
