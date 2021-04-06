using BowlingLeague.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Infrastructure
{
    //what type of attribute will be put it in
    [HtmlTargetElement("div", Attributes = "page-info")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory urlInfo;
        //constructor
        public PaginationTagHelper(IUrlHelperFactory urlHelper)
        {
            urlInfo = urlHelper;
        }

        //page info 
        public PageNumInfo PageInfo { get; set; }

        //css info
        public string PageClass { get; set; }
        public string PageClassSelected { get; set; }
        public string PageClassNormal { get; set; }


        //dictionary key value pars
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> KeyValuePairs { get; set; } = new Dictionary<string, object>();

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public bool SetUpCorrectly { get; set; }


        //process method to override
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelp = urlHelp = urlInfo.GetUrlHelper(ViewContext);

            //build the div and a tag
            TagBuilder DivTag = new TagBuilder("div");

            //loop to build the pages
            for (int i = 1; i <= PageInfo.NumPages; i++)
            {


                TagBuilder aTag = new TagBuilder("a");

                KeyValuePairs["pageNum"] = i;
                aTag.Attributes["href"] = urlHelp.Action("Index", KeyValuePairs);

                aTag.InnerHtml.AppendHtml(i.ToString());

                //connect to css classes
                aTag.AddCssClass(PageClass);
                aTag.AddCssClass(i == PageInfo.CurrentPage ? PageClassSelected : PageClassNormal);

                DivTag.InnerHtml.AppendHtml(aTag);
            }


            output.Content.AppendHtml(DivTag.InnerHtml);
        }
    }
}