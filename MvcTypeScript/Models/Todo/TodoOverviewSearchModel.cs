using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace MvcTypeScript.Models.Todo
{
    [TsClass]
    public class TodoOverviewSearchModel
    {
        public TodoOverviewSearchModel()
        {
            SearchText = String.Empty;
            PageSize = 20;
            Startdatum = DateTime.Now.Date.AddDays(-10);
            Enddatum = DateTime.Now.Date.AddDays(10);
            IsActive = true;
        }

        public string SearchText { get; set; }

        public int PageSize { get; set; }

        public DateTime Startdatum { get; set; }

        public DateTime Enddatum { get; set; }

        public bool IsActive { get; set; }

    }
}