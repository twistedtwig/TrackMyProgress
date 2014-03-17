using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;

namespace GoalWeb.Models
{
    public static class razorhelper
    {
//        public static Guid UserId(this IPrincipal User)
//        {
//            if (User == null) throw new ArgumentNullException("User");
//            MembershipUser user = Membership.GetUser(User.Identity.Name);
//            if (user == null) return Guid.Empty;
//
//            return (Guid)user.ProviderUserKey;
//        }


        public static MvcHtmlString LiNavActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, bool matchControllerOnly = true, string activeClass = "active")
        {
            return LiNavActionLink(helper, linkText, actionName, controllerName, new RouteValueDictionary(), new Dictionary<string, object>(), matchControllerOnly, activeClass);
        }


        public static MvcHtmlString LiNavActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, RouteValueDictionary routes, IDictionary<string, object> htmlAttributes, bool matchControllerOnly = true, string activeClass = "active")
        {
            var tagBuilder = new TagBuilder("li")
            {
                InnerHtml = helper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), htmlAttributes).ToHtmlString()
            };

            if (((string)helper.ViewContext.RouteData.Values["controller"]).Equals(controllerName, StringComparison.OrdinalIgnoreCase)
                && (matchControllerOnly || ((string)helper.ViewContext.RouteData.Values["action"]).Equals(actionName, StringComparison.OrdinalIgnoreCase)))
            {
                if (
                    !htmlAttributes.Any(
                        x =>
                        x.Key.Equals("class", StringComparison.InvariantCultureIgnoreCase) &&
                        x.Value.ToString().Contains(activeClass)))
                {
                    tagBuilder.MergeAttribute("class", activeClass);
                }
            }

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }







        public static MvcHtmlString BootStrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int numberOfCols = 2)
        {
            return BootStrapLabelFor(html, expression, null, new Dictionary<string, object>(), numberOfCols);
        }

        public static MvcHtmlString BootStrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes, int numberOfCols = 2)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            if (numberOfCols < 1 || numberOfCols > 12)
            {
                numberOfCols = 2;
            }

            var classes = new List<string> { "col-lg-" + numberOfCols, "control-label" };

            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes["class"] = string.Empty;
            }
            foreach (var c in classes)
            {
                if (
                    !htmlAttributes.Any(
                        x =>
                        x.Key.Equals("class", StringComparison.InvariantCultureIgnoreCase) &&
                        x.Value.ToString().Contains(c))
                    )
                {
                    htmlAttributes["class"] += c + " ";
                }
            }

            return html.LabelFor(expression, labelText, htmlAttributes);
        }






        public static MvcHtmlString BootStrapEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object additionalViewData = null, bool readOnly = false)
        {
            var viewData = new ViewDataDictionary();
            foreach (KeyValuePair<string, object> kvp in new RouteValueDictionary(additionalViewData))
            {
                viewData[kvp.Key] = kvp.Value.ToString().Trim();
            }

            if (!viewData.ContainsKey("class"))
            {
                viewData["class"] = string.Empty;
            }

            if (!viewData["class"].ToString().Contains("form-control"))
            {
                viewData["class"] = viewData["class"].ToString().Trim() + " form-control";
            }

            if (!viewData.ContainsKey("readonly") && readOnly)
            {
                viewData["readonly"] = "true";
            }

            var htmlString = html.EditorFor(expression).ToHtmlString();

            foreach (var item in viewData)
            {
                if (!htmlString.Contains(item.Key))
                {
                    htmlString = htmlString.Substring(0, htmlString.Length - 3) + string.Format(" {0}=\"\" />", item.Key);
                }

                if (htmlString.Contains(string.Format("{0}=\"\"", item.Key)))
                {
                    htmlString = htmlString.Replace(string.Format("{0}=\"", item.Key), string.Format("{0}=\"{1}", item.Key, item.Value));
                }
                else
                {
                    htmlString = htmlString.Replace(string.Format("{0}=\"", item.Key), string.Format("{0}=\"{1} ", item.Key, item.Value));
                }
            }

            return new MvcHtmlString(htmlString);
        }

        public static MvcHtmlString BootStrapReadonlyEditorAndLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int labelCols = 2, int editorCols = 10, string labelText = null)
        {
            return new MvcHtmlString(BootStrapEditorTempate(html, expression, labelCols, editorCols, BootStrapEditorFor(html, expression, null, true), labelText).ToHtmlString());
        }

        public static MvcHtmlString BootStrapEditorAndLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int labelCols = 2, int editorCols = 10, string labelText = null)
        {
            return new MvcHtmlString(BootStrapEditorTempate(html, expression, labelCols, editorCols, BootStrapEditorFor(html, expression), labelText).ToHtmlString());
        }

        public static MvcHtmlString BootStrapEditorAndLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object additionalViewData, int labelCols = 2, int editorCols = 10, string labelText = null)
        {
            return new MvcHtmlString(BootStrapEditorTempate(html, expression, labelCols, editorCols, BootStrapEditorFor(html, expression, additionalViewData), labelText).ToHtmlString());
        }






        public static MvcHtmlString BootStrapDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes, bool addPleaseSelect = true)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            var atts = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (!atts.ContainsKey("class"))
            {
                atts["class"] = string.Empty;
            }

            if (!atts["class"].ToString().Contains("form-control"))
            {
                atts["class"] += " form-control ";
            }

            selectList = AddPleaseSelect(selectList.ToList());
            return htmlHelper.DropDownListFor(expression, selectList, atts);
        }



        public static MvcHtmlString BootStrapDropDownListForAndLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string labelText = null, int labelCols = 2, int editorCols = 10, bool addPleaseSelect = true)
        {
            return new MvcHtmlString(BootStrapEditorTempate(html, expression, labelCols, editorCols, BootStrapDropDownListFor(html, expression, selectList, null), labelText).ToHtmlString());
        }

        public static MvcHtmlString BootStrapDropDownListForAndLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes, string labelText = null, int labelCols = 2, int editorCols = 10, bool addPleaseSelect = true)
        {
            return new MvcHtmlString(BootStrapEditorTempate(html, expression, labelCols, editorCols, BootStrapDropDownListFor(html, expression, selectList, htmlAttributes, addPleaseSelect), labelText).ToHtmlString());
        }

        public static MvcHtmlString BootStrapDropDownListForAndLabelEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Type eEnum, int labelCols = 2, int editorCols = 10, bool addPleaseSelect = true)
        {
            if (!eEnum.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return BootStrapDropDownListForAndLabelFor(html, expression, GetEnumSelectList<Enum>(eEnum), null, labelCols, editorCols);
        }

        public static MvcHtmlString BootStrapDropDownListForAndLabelEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Type eEnum, string labelText = null, int labelCols = 2, int editorCols = 10, bool addPleaseSelect = true)
        {
            if (!eEnum.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return BootStrapDropDownListForAndLabelFor(html, expression, GetEnumSelectList<Enum>(eEnum), labelText, labelCols, editorCols);
        }





        private static IEnumerable<SelectListItem> GetEnumSelectList<T>(Type eEnum)
        {
            if (!(typeof(T).Equals(eEnum.BaseType)))
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (Enum.GetValues(eEnum).Cast<int>().Select(e => new SelectListItem { Text = ((Enum)Enum.Parse(eEnum, e.ToString())).GetDescription(), Value = e.ToString() })).ToList();
        }




        private static MvcHtmlString BootStrapEditorTempate<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int labelCols, int editorCols, MvcHtmlString editor, string labelText)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttribute("class", "form-group");

            //TODO test this, might not need it.
            var ex = (MemberExpression)expression.Body;
            string name = ex.Member.Name;

            if (html.ViewContext.ViewData.ModelState.ContainsKey(name))
            {
                tagBuilder.MergeAttribute("class", "has-error");
            }



            tagBuilder.InnerHtml += BootStrapLabelFor(html, expression, labelText, null, labelCols).ToHtmlString();

            if (editorCols < 1 || editorCols > 12)
            {
                editorCols = 10;
            }

            var editorTag = new TagBuilder("div");
            editorTag.MergeAttribute("class", "col-lg-" + editorCols);
            editorTag.InnerHtml += editor;

            tagBuilder.InnerHtml += editorTag.ToString();

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }




        private static IEnumerable<SelectListItem> AddPleaseSelect(IList<SelectListItem> list)
        {
            if (list == null) return null;
            if (!list.Any()) return list;

            var text = "Please Select...";

            var first = list.First();
            if (first.Text.Equals(text)) list.AsEnumerable();

            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = text, Value = int.MinValue.ToString() });

            foreach (var i in list)
            {
                items.Add(i);
            }

            return items.AsEnumerable();
        }



        public static IHtmlString AddResource(this HtmlHelper htmlHelper, Func<object, HelperResult> template, string type)
        {
            return AddResource(htmlHelper, template(null).ToString(), type);
        }

        public static IHtmlString AddResource(this HtmlHelper htmlHelper, string text, string type)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[type] != null)
            {
                var list = (List<string>)htmlHelper.ViewContext.HttpContext.Items[type];
                if (!list.Contains(text))
                {
                    list.Add(text);
                }
            }
            else
            {
                htmlHelper.ViewContext.HttpContext.Items[type] = new List<string> { text };
            }

            return new HtmlString(String.Empty);
        }


        public static IHtmlString RenderResources(this HtmlHelper htmlHelper, string type)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[type] != null)
            {
                var resources = (List<string>)htmlHelper.ViewContext.HttpContext.Items[type];

                foreach (var resource in resources)
                {
                    if (resource != null) htmlHelper.ViewContext.Writer.Write(resource);
                }
            }

            return new HtmlString(String.Empty);
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }

                return name;
            }
            return value.ToString();
        }

    }
}