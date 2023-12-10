namespace FRTools.Core.Data.DataModels.PinglistModels
{
    public class PinglistCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Pinglist> Pinglists { get; set; } = new HashSet<Pinglist>();
    }
}
