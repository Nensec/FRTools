using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class UpdatePinglist : IJob
    {
        private readonly List<int> _frUserList;

        public string RelatedEntityId { get; set; }
        public string Description { get; set; }

        public UpdatePinglist(string listId, List<int> frUserList)
        {
            _frUserList = frUserList;
            RelatedEntityId = listId;
            Description = $"Updating entries for pinglist '{RelatedEntityId}'";
        }

        public async Task JobTask()
        {
            foreach (var entry in _frUserList)
                await FRHelpers.GetOrUpdateFRUser(entry);
        }
    }
}
