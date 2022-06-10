using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FRTools.Web.Infrastructure.Helpers
{
    public static class TextBoxExtentions
    {
        public static MvcHtmlString CopyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string type = null, string id = null)
        {
            return CopyTextBox(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString(), ExpressionHelper.GetExpressionText(expression), type, id);
        }

        public static MvcHtmlString CopyTextBox(this HtmlHelper htmlHelper, string text, string name, string type = null, string id = null)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var outerTag = new TagBuilder("div");
            outerTag.MergeAttribute("class", "input-group");

            var inputTag = new TagBuilder("input");
            inputTag.GenerateId(fullName);
            inputTag.MergeAttribute("type", "text");
            inputTag.MergeAttribute("readonly", "");
            inputTag.MergeAttribute("value", text);
            inputTag.MergeAttribute("class", "form-control");

            var spanTag = new TagBuilder("span");
            spanTag.MergeAttribute("class", "input-group-append");

            var buttonTag = new TagBuilder("button");
            buttonTag.MergeAttribute("class", "btn btn-success");
            buttonTag.MergeAttribute("type", "button");
            buttonTag.MergeAttribute("onclick", $"document.getElementById('{inputTag.Attributes["id"]}').select(); document.execCommand('copy'); this.innerHTML = 'Copied!'");
            buttonTag.InnerHtml = "Copy!";

            spanTag.InnerHtml = buttonTag.ToString();
            outerTag.InnerHtml = inputTag.ToString() + spanTag.ToString();

            return MvcHtmlString.Create(outerTag.ToString());
        }

        public static MvcHtmlString CopyTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return CopyTextArea(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString(), ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString CopyTextArea(this HtmlHelper htmlHelper, string text, string name)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var outerTag = new TagBuilder("div");
            outerTag.MergeAttribute("class", "input-group");

            var inputTag = new TagBuilder("textarea");
            inputTag.GenerateId(fullName);
            inputTag.InnerHtml = text;
            inputTag.MergeAttribute("readonly", "");
            inputTag.MergeAttribute("rows", "3");
            inputTag.MergeAttribute("class", "form-control");
            inputTag.MergeAttribute("style", "resize: vertical;");

            var spanTag = new TagBuilder("span");
            spanTag.MergeAttribute("class", "input-group-append");

            var buttonTag = new TagBuilder("button");
            buttonTag.MergeAttribute("class", "btn btn-success");
            buttonTag.MergeAttribute("type", "button");
            buttonTag.MergeAttribute("onclick", $"document.getElementById('{inputTag.Attributes["id"]}').select(); document.execCommand('copy'); this.innerHTML = 'Copied!'");
            buttonTag.InnerHtml = "Copy!";

            spanTag.InnerHtml = buttonTag.ToString();
            outerTag.InnerHtml = inputTag.ToString() + spanTag.ToString();

            return MvcHtmlString.Create(outerTag.ToString());
        }
    }
}