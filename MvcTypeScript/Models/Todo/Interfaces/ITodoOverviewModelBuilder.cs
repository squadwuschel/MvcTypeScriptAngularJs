namespace MvcTypeScript.Models.Todo.Interfaces
{
    public interface ITodoOverviewModelBuilder
    {
        TodoOverviewInitModel InitTodoOverviewInitModel();
        TodoOverviewSearchModel InitTodoOverviewSearchModel();
        TodoOverviewResultModel TodoOverviewResultModel(TodoOverviewSearchModel searchModel);
        void DeleteTodoEntry(int id);
    }
}