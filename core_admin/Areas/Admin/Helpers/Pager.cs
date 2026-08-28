using System.Web;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class Pager
    {
        public Pager(
            int totalItems,
            int currentPage = 1,
            int pageSize = 10,
            int maxPages = 10)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage);

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Pages = pages;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public IEnumerable<int> Pages { get; private set; }

        public string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString + "";
        }

        public string RemoveQueryStringsFromUrl(string url, List<string> keys)
        {
            if (!url.Contains("?"))
                return url;

            string[] urlParts = url.ToLower().Split('?');
            try
            {
                var querystrings = HttpUtility.ParseQueryString(urlParts[1]);
                foreach (string key in keys)
                    querystrings.Remove(key.ToLower());

                if (querystrings.Count > 0)
                    return urlParts[0]
                      + "?"
                      + string.Join("&", querystrings.AllKeys.Select(c => c.ToString() + "=" + querystrings[c.ToString()]));
                else
                    return urlParts[0];
            }
            catch (NullReferenceException)
            {
                return urlParts[0];
            }
        }

        public string CreateHtml(string currentURL = "", string pageType = "link")
        {
            if (TotalItems <= PageSize)
            {
                return "";
            }
            else
            {
                string html = "";
                if (pageType == "link")
                {
                    string URL = RemoveQueryStringByKey(currentURL, "page");
                    html += "<nav aria-label=\"Page navigation\">";
                    html += "   <ul class=\"pagination\">";
                     
                    #region ############# Link 1 (Prev link) #############
                    string _class = (CurrentPage <= 1) ? "disabled" : "";
                    string link1 = "javascript:;";
                    if (CurrentPage == 1)
                    {
                        link1 = "javascript:;";
                    }
                    else if (URL.Contains("?"))
                    {
                        link1 = URL + "&page=" + (CurrentPage - 1);
                    }
                    else
                    {
                        link1 = URL + "?page=" + (CurrentPage - 1);
                    }
                    //html += "<li class='" + _class + "'><a class=\"page-link\" href='" + link1 + "' aria-label='Previous'>Prev</a></li>";
                    html += "<li class='page-item " + _class + "'><a class=\"page-link\" href='" + link1 + "' aria-label='Previous'><span aria-hidden=\"true\">&laquo;</span></a></li>";
                    #endregion

                    #region ############# Link (page 1st ) ... ############ 
                    if (StartPage > 1)
                    {
                        #region ############# Link 2 (Prev link) #############
                        string link2 = URL + "&page=1";
                        if (URL.Contains("?"))
                        {
                            link2 = URL + "&page=1";
                        }
                        else
                        {
                            link2 = URL + "?page=1";
                        }
                        #endregion
                        html += "<li class=\"page-item\"><a class=\"page-link\" href='" + link2 + "'>1</a></li>";
                        html += "<li class=\"page-item disabled\"><span class=\"page-link\">...</span></li>";
                    }
                    #endregion

                    foreach (int i in Pages)
                    {
                        _class = (CurrentPage == i) ? "active" : "";

                        #region ############# Link 3 (Prev link) #############
                        string link3 = URL + "&page=" + i;
                        if (URL.Contains("?"))
                        {
                            link3 = URL + "&page=" + i;
                        }
                        else
                        {
                            link3 = URL + "?page=" + i;
                        }
                        #endregion

                        html += "<li class='page-item " + _class + "'><a class=\"page-link\" href='" + link3 + "'>" + i + "</a></li>";
                    }

                    #region ############# Link (page last) ... ############ 
                    if (EndPage < TotalPages)
                    {
                        html += "<li class=\"page-item disabled\"><span class=\"page-link\">...</span></li>";
                        #region ############# Link 4 (Prev link) #############
                        string link4 = URL + "&page=" + TotalPages;
                        if (URL.Contains("?"))
                        {
                            link4 = URL + "&page=" + TotalPages;
                        }
                        else
                        {
                            link4 = URL + "?page=" + TotalPages;
                        }
                        #endregion
                        html += "<li class=\"page-item\"><a class=\"page-link\" href='" + link4 + "'>" + TotalPages + "</a></li>";
                    }
                    #endregion

                    #region ############# Link 5 (Next link) #############
                    _class = (CurrentPage >= EndPage) ? "disabled" : "";
                    string link5 = URL + "&page=" + (CurrentPage + 1);
                    if (URL.Contains("?"))
                    {
                        link5 = URL + "&page=" + (CurrentPage + 1);
                    }
                    else
                    {
                        link5 = URL + "?page=" + (CurrentPage + 1);
                    }

                    if (CurrentPage >= EndPage)
                    {
                        link5 = "javascript:;";
                    }
                    //html += "<li class='page-item " + _class + "'><a class=\"page-link\" href='" + link5 + "'>Next</a></li>";
                    html += "<li class='page-item " + _class + "'><a class=\"page-link\" href='" + link5 + "'><span aria-hidden=\"true\">&raquo;</span></a></li>";
                    #endregion

                    html += "   </ul>";
                    html += "</nav> <!--EndPage=" + EndPage + "|TotalPages=" + TotalPages + "-->";
                }
                else if (pageType == "dropdown")
                {
                    html += "<select id='pager_dropdown'>";
                    for (int i = 1; i <= TotalPages; i++)
                    {
                        string selected = "";
                        if (CurrentPage == i)
                        {
                            selected = "selected";
                        }
                        html += "<option value='" + i + "' " + selected + " >" + i + "</option>";
                    }
                    html += "</select>";
                }

                return html;
            }

        }
    }
}
