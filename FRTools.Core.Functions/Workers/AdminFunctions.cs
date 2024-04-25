using System.Linq;
using FRTools.Core.Data;
using FRTools.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class AdminFunctions : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly IFRItemService _fRItemService;
        private readonly ILogger<AdminFunctions> _logger;

        public AdminFunctions(DataContext dataContext, IFRItemService fRItemService, ILogger<AdminFunctions> logger)
        {
            _dataContext = dataContext;
            _fRItemService = fRItemService;
            _logger = logger;
        }

        [Function(nameof(UpdateGenes))]
        public IActionResult UpdateGenes([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "updategenes")] HttpRequest request)
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
