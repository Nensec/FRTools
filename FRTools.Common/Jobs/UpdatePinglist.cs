using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class UpdatePinglist : IJob
    {
        private readonly List<int> _frUserList;
        private readonly string _listId;

        public string RelatedEntityId { get; set; }
        public string Description { get; set; }

        public UpdatePinglist(string listId, List<int> frUserList)
        {
            _frUserList = frUserList;
            _listId = listId;
            Description = $"Updating entries for pinglist '{_listId}'";
        }

        public async Task JobTask()
        {
            foreach (var entry in _frUserList)
                await FRHelpers.GetOrUpdateFRUser(entry);
        }
    }
}
