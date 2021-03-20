using FRTools.Data.DataModels;
using FRTools.Web.Controllers;
using System.Web.Mvc;

namespace FRTools.Web.Views.Shared
{
    public class FRToolsBasePage<TModel> : WebViewPage<TModel> where TModel : class
    {
        public const string V2Layout = "~/Views/Shared/_LayoutV2.cshtml";
        public string ErrorMessage => TempData.TryGetValue("Error", out var error) ? error as string : null;
        public string WarningMessage => TempData.TryGetValue("Warning", out var error) ? error as string : null;
        public string InfoMessage => TempData.TryGetValue("Info", out var error) ? error as string : null;
        public string SuccessMessage => TempData.TryGetValue("Success", out var error) ? error as string : null;
        public bool HasMessage => ErrorMessage != null || WarningMessage != null || InfoMessage != null || SuccessMessage != null;

        public User CurrentUser { get; set; }

        protected override void InitializePage()
        {
            CurrentUser = (ViewContext.Controller as BaseController)?.LoggedInUser;
            base.InitializePage();
        }

        public override void Execute()
        {
        }
    }

    public class FRToolsBasePage : WebViewPage
    {
        public const string V2Layout = "~/Views/Shared/_LayoutV2.cshtml";
        public string ErrorMessage => TempData.TryGetValue("Error", out var error) ? error as string : null;
        public string WarningMessage => TempData.TryGetValue("Warning", out var error) ? error as string : null;
        public string InfoMessage => TempData.TryGetValue("Info", out var error) ? error as string : null;
        public string SuccessMessage => TempData.TryGetValue("Success", out var error) ? error as string : null;
        public bool HasMessage => ErrorMessage != null || WarningMessage != null || InfoMessage != null || SuccessMessage != null;

        public User CurrentUser { get; set; }

        protected override void InitializePage()
        {
            CurrentUser = (ViewContext.Controller as BaseController)?.LoggedInUser;
            base.InitializePage();
        }

        public override void Execute()
        {
        }
    }
}