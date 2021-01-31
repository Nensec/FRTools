using FRTools.Data.DataModels;
using FRTools.Web.Controllers;
using System.Web.Mvc;

namespace FRTools.Web.Views.Shared
{
    public class FRToolsBasePage<TModel> : WebViewPage<TModel> where TModel : class
    {
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