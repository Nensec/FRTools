using System;
using System.Linq;
using System.Web.Routing;

namespace FRTools.Web.Models
{
    public class PaginationModel
    {
        private int _pageSize = 25;

        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => ValidPageSizes.Aggregate((x, y) => Math.Abs(x - _pageSize) < Math.Abs(y - _pageSize) ? x : y);
            set => _pageSize = value;
        }
        public string RouteUrlName { get; set; }
        public int TotalItems { get; set; }
        public int[] ValidPageSizes { get; set; } = new[] { 5, 10, 25, 50 };

        public PaginationModel()
        {
        }

        public PaginationModel(string routeUrlBase, int? page = null, int? pageSize = null)
        {
            RouteUrlName = routeUrlBase;
            Page = page ?? 1;
            PageSize = pageSize ?? 25;
        }
    }
}