using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcTypeScript.Models.Todo.Container;

namespace MvcTypeScript.Helper
{
    public class TodoRepository : ITodoRepository
    {
        private List<RepositoryTodoItem> LocalTodoRepository { get; set; }

        public TodoRepository()
        {
            if (LocalTodoRepository == null)
            {
                int i = 1;
                LocalTodoRepository = new List<RepositoryTodoItem>();
                LocalTodoRepository.Add(new RepositoryTodoItem() { Creator = "Iron Man", IsActive = true, Prioritaet = Prioritaet.Keine, Description = "Einkaufen gehen für Montag abend", DoDate = DateTime.Now.AddDays(2), Id = i++ });
                LocalTodoRepository.Add(new RepositoryTodoItem() { Creator = "Super Man ", IsActive = true, Prioritaet = Prioritaet.Hoch, Description = "Schnell zum Mond fliegen", DoDate = DateTime.Now.AddDays(1), Id = i++ });
                LocalTodoRepository.Add(new RepositoryTodoItem() { Creator = "Bat Man", IsActive = false, Prioritaet = Prioritaet.Normal, Description = "Von einer Brücke springen", DoDate = DateTime.Now.AddDays(-2), Id = i++ });
                LocalTodoRepository.Add(new RepositoryTodoItem() { Creator = "Action Man", IsActive = false, Prioritaet = Prioritaet.Dringend, Description = "Nach etwas Action suchen", DoDate = DateTime.Now.AddDays(-6), Id = i++ });
                LocalTodoRepository.Add(new RepositoryTodoItem() { Creator = "Ant Man", IsActive = true, Prioritaet = Prioritaet.Keine, Description = "Ein paar Ameisen zertreten", DoDate = DateTime.Now.AddDays(8), Id = i++ });
            }
        }

        public List<RepositoryTodoItem> GetItems()
        {
            return LocalTodoRepository;
        }

        public RepositoryTodoItem GetItem(int id)
        {
            return LocalTodoRepository.FirstOrDefault(p => p.Id == id);
        }

        public void DeleteItem(int id)
        {
            LocalTodoRepository.Remove(LocalTodoRepository.FirstOrDefault(p => p.Id == id));
        }

        public void AddOrUpdateItem(RepositoryTodoItem item)
        {
            if (item.Id == 0)
            {
                var id = LocalTodoRepository.Max(p => p.Id);
                item.Id = ++id;
                LocalTodoRepository.Add(item);    
            }
            else
            {
                var entry = LocalTodoRepository.FirstOrDefault(p => p.Id == item.Id);
                if (entry!= null)
                {
                    entry.Description = item.Description;
                    entry.Creator = item.Creator;
                    entry.DoDate = item.DoDate;
                    entry.Prioritaet = item.Prioritaet;
                    entry.IsActive = item.IsActive;
                }
            }
        }
    }
}