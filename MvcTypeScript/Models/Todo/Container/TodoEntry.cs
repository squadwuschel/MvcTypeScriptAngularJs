using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcTypeScript.Helper;
using TypeLite;

namespace MvcTypeScript.Models.Todo.Container
{
    public class TodoEntry
    {
        public TodoEntry()
        {
            Id = 0;
            IsOpen = true;
            Description = String.Empty;
            DoDate = String.Empty;
            Creator = String.Empty;
            Prioritaet = Prioritaet.Keine;
        }

        public int Id { get; set; }

        public bool IsOpen { get; set; }

        public string Description { get; set; }

        public string DoDate { get; set; }

        public string Creator { get; set; }

        public Prioritaet Prioritaet { get; set; }

    }
}