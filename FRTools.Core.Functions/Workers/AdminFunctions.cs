using System.Linq;
using FRTools.Core.Data;
using FRTools.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace FRTools.Core.Functions.Workers
{
    public class AdminFunctions
    {
        private readonly DataContext _dataContext;
        private readonly IFRItemService _fRItemService;

        public AdminFunctions(DataContext dataContext, IFRItemService fRItemService)
        {
            _dataContext = dataContext;
            _fRItemService = fRItemService;
        }

        [FunctionName(nameof(UpdateGenes))]
        public IActionResult UpdateGenes([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "/updategenes")] HttpRequest request)
        {
            var allGenes = _dataContext.FRItems.Where(x =>
                x.ItemCategory == Data.DataModels.FlightRisingModels.FRItemCategory.Trinket &&
                x.ItemType == "Specialty Item" &&
                (x.Name.StartsWith("Primary") || x.Name.StartsWith("Secondary") || x.Name.StartsWith("Tertiary"))
            ).ToList();

            foreach (var item in allGenes)            
                _fRItemService.FetchItemFromFR(item.FRId, "trinket");

            return new ContentResult { Content = "ok" };
        }
    }
}
