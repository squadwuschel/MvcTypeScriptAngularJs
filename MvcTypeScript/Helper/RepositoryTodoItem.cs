using System;

namespace MvcTypeScript.Helper
{
    public class RepositoryTodoItem
    {
        public RepositoryTodoItem()
        {
            Description = String.Empty;
            Id = 0;
            IsActive = false;
            DoDate = DateTime.Now;
            Creator = String.Empty;
            Prioritaet = Prioritaet.Normal;
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public DateTime DoDate { get; set; }

        public string Creator { get; set; }

        public Prioritaet Prioritaet { get; set; }
    }
}