using FRTools.Data;
using FRTools.Data.DataModels.PinglistModels;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Common
{
    public static class PinglistHelpers
    {
        public static Pinglist GetPinglist(string listId, bool includeUsers, DataContext ctx = null)
        {
            Pinglist getList()
            {
                IQueryable<Pinglist> query = ctx.Pinglists;
                if (includeUsers)
                    query = query.Include(x => x.Entries.Select(e => e.FRUser));
                var list = query.FirstOrDefault(x => x.GeneratedId == listId);
                if (list == null)
                    return null;

                return list;
            }

            if (ctx == null)
                using (ctx = new DataContext())
                    return getList();
            else
                return getList();
        }

        public static async Task<PingListEntry> AddEntryToList(Pinglist list, string username, int? userId, string remarks, DataContext ctx)
        {
            username = username?.Trim();
            var frUser = await (username != null ? FRHelpers.GetOrUpdateFRUser(username, ctx) : FRHelpers.GetOrUpdateFRUser(userId.Value, ctx));
            if (frUser == null)
                throw new System.Exception($"Could not validate the existence of user '{username ?? userId.ToString()}.'");

            if (ctx.Pinglists.Find(list.Id).Entries.Any(x => x.FRUser.Id == frUser.Id))
                return list.Entries.FirstOrDefault(x => x.FRUser.Id == frUser.Id);

            var entry = new PingListEntry
            {
                FRUser = frUser,
                GeneratedId = CodeHelpers.GenerateId(5, list.Entries.Select(x => x.GeneratedId).ToList()),
                SecretKey = CodeHelpers.GenerateId(),
                Remarks = remarks
            };
            list.Entries.Add(entry);

            return entry;
        }
    }
}
