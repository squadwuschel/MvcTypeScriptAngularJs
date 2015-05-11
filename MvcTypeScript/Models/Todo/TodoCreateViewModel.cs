using System;
using MvcTypeScript.Helper;
using TypeLite;

namespace MvcTypeScript.Models.Todo
{
    [TsClass]
    public class TodoCreateViewModel
    {
        public TodoCreateViewModel()
        {
            Id = 0;
            IsActive = true;
            Description = String.Empty;
            DoDate = DateTime.Today;
            Creator = String.Empty;
            Prioritaet = Prioritaet.Keine;
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public DateTime DoDate { get; set; }

        public string Creator { get; set; }

        public Prioritaet Prioritaet { get; set; }

    }
}