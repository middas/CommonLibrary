using CommonLibrary.Native;
using System;
using System.Text;
using System.Web;

namespace CommonLibrary.Pager
{
    public static class PagedListHelper
    {
        public static HtmlString GetPagerHtml<T>(IPagedList<T> list, Func<int, string> urlGenerator)
        {
            return GetPagerHtml(list, urlGenerator, PagerRenderOptions.Default);
        }

        public static HtmlString GetPagerHtml<T>(IPagedList<T> list, Func<int, string> urlGenerator, PagerRenderOptions options)
        {
            HtmlString result;

            if (options == null)
            {
                options = PagerRenderOptions.Default;
            }

            if (options.AlwaysShow || list.TotalPages > 1)
            {
                StringBuilder sb = new StringBuilder();

                if (options.Ellipsis != null && options.Ellipsis.ShowEllipsis)
                {
                    if (options.Ellipsis.TotalPagesToShow < 1)
                    {
                        throw new ArgumentException("Cannot show less than 1 page");
                    }

                    int startPage = 1;
                    int endPage = list.TotalPages;

                    if (options.Ellipsis.TotalPagesToShow < list.TotalPages)
                    {
                        startPage = System.Math.Max(1, list.CurrentPage - (int)(System.Math.Floor(options.Ellipsis.TotalPagesToShow / 2D)));
                        endPage = System.Math.Min(list.TotalPages, list.CurrentPage + (int)(System.Math.Floor(options.Ellipsis.TotalPagesToShow / 2D)));

                        while ((endPage - startPage) + 1 < options.Ellipsis.TotalPagesToShow)
                        {
                            startPage = System.Math.Max(1, startPage - 1);
                            endPage = System.Math.Min(list.TotalPages, endPage + 1);
                        }
                    }

                    if (startPage > 1)
                    {
                        sb.Append("...").Append(WriteDelimiter(options));
                    }

                    sb.Append(WriteNumberLinks(urlGenerator, startPage, endPage, list.CurrentPage, options));

                    if (endPage < list.TotalPages)
                    {
                        sb.Append("...").Append(WriteDelimiter(options));
                    }
                }
                else
                {
                    sb.Append(WriteNumberLinks(urlGenerator, 1, list.TotalPages, list.CurrentPage, options));
                }

                if (options.ShowNextPreviousLinks)
                {
                    sb.Insert(0, WriteDelimiter(options));
                    sb.Insert(0, WriteLink(list.IsFirstPage ? null : urlGenerator(list.CurrentPage - 1), list.IsFirstPage ? options.DisabledItemTag : options.LinkedItemTag, options.PreviousHtml, list.IsFirstPage ? options.DisabledItemClass : options.LinkedItemClass, null));
                    sb.Append(WriteLink(list.IsLastPage ? null : urlGenerator(list.CurrentPage + 1), list.IsLastPage ? options.DisabledItemTag : options.LinkedItemTag, options.NextHtml, list.IsLastPage ? options.DisabledItemClass : options.LinkedItemClass, null));
                    sb.Append(WriteDelimiter(options));
                }

                if (options.ShowFirstLastLinks)
                {
                    sb.Insert(0, WriteDelimiter(options));
                    sb.Insert(0, WriteLink(list.IsFirstPage ? null : urlGenerator(1), list.IsFirstPage ? options.DisabledItemTag : options.LinkedItemTag, options.FirstHtml, list.IsFirstPage ? options.DisabledItemClass : options.LinkedItemClass, null));
                    sb.Append(WriteLink(list.IsLastPage ? null : urlGenerator(list.TotalPages), list.IsLastPage ? options.DisabledItemTag : options.LinkedItemTag, options.LastHtml, list.IsLastPage ? options.DisabledItemClass : options.LinkedItemClass, null));
                }

                sb.Insert(0, "<" + options.PagerTag + " class=\"" + options.PagerClass + "\">");
                sb.Append("</" + options.PagerTag + ">");

                result = new HtmlString(sb.ToString());
            }
            else
            {
                result = new HtmlString("");
            }

            return result;
        }

        private static string WriteDelimiter(PagerRenderOptions options)
        {
            string result = "";

            if (!string.IsNullOrEmpty(options.Delimiter))
            {
                result = options.Delimiter;
            }

            return result;
        }

        private static string WriteLink(string url, string tag, string text, string @class, string urlClass)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(url))
            {
                url = "javascript:return false;";
            }

            sb.Append("<a href=\"").Append(url).Append("\">");
            if (!urlClass.IsNullOrEmptyTrim())
            {
                sb.Insert(sb.Length - 1, " class=\"" + urlClass + "\"");
            }
            sb.Append(text).Append("</a>");

            string tagStr = "<" + tag;
            if (!@class.IsNullOrEmptyTrim())
            {
                tagStr += " class=\"" + @class + "\"";
            }
            tagStr += ">";

            sb.Insert(0, tagStr).Append("</").Append(tag).Append(">");

            return sb.ToString();
        }

        private static string WriteNumberLinks(Func<int, string> urlGenerator, int startPage, int endPage, int currentPage, PagerRenderOptions options)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = startPage; i <= endPage; i++)
            {
                sb.Append(WriteLink(i == currentPage ? null : urlGenerator(i), i == currentPage ? options.DisabledItemTag : options.LinkedItemTag, i.ToString(), i == currentPage ? options.DisabledItemClass : options.LinkedItemClass, null));
                sb.Append(WriteDelimiter(options));
            }

            return sb.ToString();
        }
    }
}