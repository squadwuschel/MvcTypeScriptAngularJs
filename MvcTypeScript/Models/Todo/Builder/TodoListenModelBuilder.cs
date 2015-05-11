using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcTypeScript.Helper;
using MvcTypeScript.Models.Todo.Interfaces;

namespace MvcTypeScript.Models.Todo.Builder
{
    public class TodoListenModelBuilder : ITodoListenModelBuilder
    {

        public TodoListenViewModel InitTodoListenViewModel()
        {
            TodoListenViewModel model = new TodoListenViewModel();
            //Eine Liste mit den Prioritäten erstellen
            model.PriorityList.Add(new ListItemIntEntry() { Selected = false, Text = "Keine", Value = ((int)Prioritaet.Keine) });
            model.PriorityList.Add(new ListItemIntEntry() { Selected = false, Text = "Normal", Value = ((int)Prioritaet.Normal) });
            model.PriorityList.Add(new ListItemIntEntry() { Selected = false, Text = "Hoch", Value = ((int)Prioritaet.Hoch) });
            model.PriorityList.Add(new ListItemIntEntry() { Selected = false, Text = "Dringend", Value = ((int)Prioritaet.Dringend) });
            return model;
        }
    }
}