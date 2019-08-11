using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using DataSenalesCaribe;
using System.Security.Policy;

namespace SenalesDelCaribe.Models
{
    public static class NewHtmlHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vHelper"></param>
        /// <param name="vData"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateMenu(this HtmlHelper vHelper)
        {
            var tmpSb = new StringBuilder();
            
            var db = new ContentDataContext(Comun.GetConnString());
            var tmpData = db.Categories.ToList().OrderBy(c => c.OrderBy);

            cRenderMenu(vHelper, tmpData, ref tmpSb);
            return MvcHtmlString.Create(tmpSb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCategories"></param>
        /// <param name="vStringB"></param>
        private static void cRenderMenu(this HtmlHelper vHelper, IEnumerable<Category> vCategories, ref StringBuilder vStringB)
        {
            
            TagBuilder tmpBuilderLI = null;
            TagBuilder tmpBuilderUL = null;
            TagBuilder tmpBuilderAncor = null;
            TagBuilder tmpBuilderSpan = null;
            
            if (vCategories != null)
            {
                foreach (var tmpDetail in vCategories)
                {
                    if (tmpDetail.IDParent == -1)
                    {
                        if (tmpDetail.IsMenu.GetValueOrDefault())
                        {
                            if (tmpBuilderUL == null)
                            {
                                //Construye el UL principal
                                tmpBuilderUL = new TagBuilder("ul");
                                tmpBuilderUL.MergeAttribute("id", "menu");
                                vStringB.AppendLine(tmpBuilderUL.ToString(TagRenderMode.StartTag));

                                //Construye el primer elemento de la lista (current page)
                                tmpBuilderLI = new TagBuilder("li");
                                tmpBuilderLI.MergeAttribute("class", "current_page_item");
                                vStringB.AppendLine(tmpBuilderLI.ToString(TagRenderMode.StartTag));
                            }
                            else
                            {
                                tmpBuilderLI = new TagBuilder("li");
                                vStringB.AppendLine(tmpBuilderLI.ToString(TagRenderMode.StartTag));
                            }

                            //Construye el anchor
                            tmpBuilderAncor = new TagBuilder("a");
                            tmpBuilderAncor.MergeAttribute("href", tmpDetail.CatURL != null && tmpDetail.CatURL != string.Empty
                                                            ? tmpDetail.CatURL : 
                                                            (tmpDetail.pCategoryType == 7 ? "#" : "/GeneralPage/Index/"+ tmpDetail.IDCategory)
                                                          );
                            vStringB.AppendLine(tmpBuilderAncor.ToString(TagRenderMode.StartTag));
                            
                            //Construye el span
                            tmpBuilderSpan = new TagBuilder("span");
                            tmpBuilderSpan.InnerHtml = tmpDetail.CatName;
                            vStringB.AppendLine(tmpBuilderSpan.ToString(TagRenderMode.Normal));

                            vStringB.AppendLine(tmpBuilderAncor.ToString(TagRenderMode.EndTag));

                            cAddMenuItem(tmpDetail.IDCategory, vCategories, ref vStringB);
                            vStringB.AppendLine(tmpBuilderLI.ToString(TagRenderMode.EndTag));
                        }
                    }
                }
            }
            if (tmpBuilderUL != null)
            {
                vStringB.AppendLine(tmpBuilderUL.ToString(TagRenderMode.EndTag));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCategoryId"></param>
        /// <param name="dtMenuItems"></param>
        /// <param name="vStringB"></param>
        private static void cAddMenuItem(int vCategoryId, IEnumerable<Category> dtMenuItems, ref StringBuilder vStringB)
        {
            TagBuilder tmpBuilderLI;
            TagBuilder tmpBuilderAncor;
            TagBuilder tmpBuilderUL = null;

            foreach (var tmpDetail in dtMenuItems)
            {
                if (tmpDetail.IDParent == vCategoryId)
                {
                    if (tmpDetail.IsMenu.GetValueOrDefault())
                    {
                        if (tmpBuilderUL == null)
                        {
                            tmpBuilderUL = new TagBuilder("ul");
                            vStringB.AppendLine(tmpBuilderUL.ToString(TagRenderMode.StartTag));
                        }
                        tmpBuilderLI = new TagBuilder("li");
                        vStringB.AppendLine(tmpBuilderLI.ToString(TagRenderMode.StartTag));

                        //Construye el anchor
                        tmpBuilderAncor = new TagBuilder("a");
                        tmpBuilderAncor.InnerHtml = tmpDetail.CatName;
                        tmpBuilderAncor.MergeAttribute("href", tmpDetail.CatURL != null && tmpDetail.CatURL != string.Empty
                                                            ? tmpDetail.CatURL : "/GeneralPage/Index/" + tmpDetail.IDCategory);
                        vStringB.AppendLine(tmpBuilderAncor.ToString(TagRenderMode.Normal));

                        cAddMenuItem(tmpDetail.IDCategory, dtMenuItems, ref vStringB);
                        vStringB.AppendLine(tmpBuilderLI.ToString(TagRenderMode.EndTag));
                    }
                }
            }
            if (tmpBuilderUL != null)
            {
                vStringB.AppendLine(tmpBuilderUL.ToString(TagRenderMode.EndTag));
            }
        }
    }
}