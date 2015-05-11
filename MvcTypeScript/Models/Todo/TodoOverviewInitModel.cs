using System;
using System.Collections.Generic;
using MvcTypeScript.Helper;
using TypeLite;

namespace MvcTypeScript.Models.Todo
{
    [TsClass]
    public class TodoOverviewInitModel
    {
        public TodoOverviewInitModel()
        {
            Datum = String.Empty;
            CurrentUser = String.Empty;
            EmptyTodoEntry = new TodoCreateViewModel();
        }

        public string Datum { get; set; }

        public string CurrentUser { get; set; }

        public TodoCreateViewModel EmptyTodoEntry { get; set; }
    }
}