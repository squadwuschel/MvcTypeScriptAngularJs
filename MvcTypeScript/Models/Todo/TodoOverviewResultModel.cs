using System.Collections.Generic;
using MvcTypeScript.Models.Todo.Container;
using TypeLite;

namespace MvcTypeScript.Models.Todo
{
    [TsClass]
    public class TodoOverviewResultModel
    {
        public TodoOverviewResultModel()
        {
            Entries = new List<TodoEntry>();
        }

        public List<TodoEntry> Entries { get; set; }
    }
}