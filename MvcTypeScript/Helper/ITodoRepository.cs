using System.Collections.Generic;
using MvcTypeScript.Models.Todo.Container;

namespace MvcTypeScript.Helper
{
    public interface ITodoRepository
    {
        List<RepositoryTodoItem> GetItems();
        RepositoryTodoItem GetItem(int id);
        void AddOrUpdateItem(RepositoryTodoItem item);
        void DeleteItem(int id);
    }
}