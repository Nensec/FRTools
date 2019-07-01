using FRTools.Data.DataModels;
using System.Collections.Generic;
using System.Configuration;

namespace FRTools.Models
{
    public class ViewProfileViewModel
    {
        public User User { get; set; }
        public List<Preview> Previews { get; set; }
        public List<Skin> Skins { get; set; }

        private Privacy? _privacy;
        public Privacy Privacy
        {
            get => _privacy ?? User.Privacy;
            set => _privacy = value;
        }
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];
        public bool IsOwn { get; set; }
    }
}