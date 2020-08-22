using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class UpdatePinglist : BaseJob
    {
        private readonly List<int> _frUserList;

        public override string RelatedEntityId { get; set; }
        public override string Description { get; set; }

        public UpdatePinglist(string listId, List<int> frUserList)
        {
            _frUserList = frUserList;
            RelatedEntityId = listId;
            Description = $"Updating entries for pinglist '{RelatedEntityId}'";
        }

        public override async Task JobTask()
        {
            foreach (var entry in _frUserList)
                try
                {
                    await FRHelpers.GetOrUpdateFRUser(entry);
                }
                catch (Exception ex)
                {
                    ReportError(ex.Message);
                }
        }
    }
}
