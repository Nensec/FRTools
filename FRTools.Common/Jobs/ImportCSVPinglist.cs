using FRTools.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public class ImportCSVPinglist : BaseJob
    {
        private readonly string _csv;

        public override string RelatedEntityId { get; set; }
        public override string Description { get; set; }

        public ImportCSVPinglist(string listId, string csv)
        {
            RelatedEntityId = listId;
            _csv = csv;

            Description = $"Importing pinglist for pinglist '{RelatedEntityId}'";
        }

        public override async Task JobTask()
        {
            using (var ctx = new DataContext())
            {
                var list = PinglistHelpers.GetPinglist(RelatedEntityId, false, ctx);

                var usernames = _csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var username in usernames)
                {
                    try
                    {
                        if (username.All(char.IsDigit))
                            await PinglistHelpers.AddEntryToList(list, null, int.Parse(username), null, ctx);
                        else
                            await PinglistHelpers.AddEntryToList(list, username, null, null, ctx);
                    }
                    catch (Exception ex)
                    {
                        ReportError(ex.Message);
                    }
                }
                ctx.SaveChanges();
            }
        }
    }
}
