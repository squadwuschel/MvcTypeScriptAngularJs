namespace MvcTypeScript.Models.Todo.Interfaces
{
    public interface ITodoCreateModelBuilder
    {
        TodoCreateViewModel InitTodoCreateViewModel();
        TodoCreateViewModel AddOrUpdateTodoItem(TodoCreateViewModel viewModel);
        TodoCreateViewModel LoadTodoItem( int id);
    }
}