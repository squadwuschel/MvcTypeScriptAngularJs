using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcTypeScript.Helper;
using TypeLite;

namespace MvcTypeScript.Models.Todo
{
    /// <summary>
    /// Beinhaltet alle Listen die wir in der Anwendung verwenden.
    /// </summary>
    [TsClass]
    public class TodoListenViewModel
    {
        public TodoListenViewModel()
        {
            PriorityList = new List<ListItemIntEntry>();
        }

        public List<ListItemIntEntry> PriorityList { get; set; }
    }
}