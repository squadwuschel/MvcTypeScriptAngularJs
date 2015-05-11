using MvcTypeScript.Helper;
using MvcTypeScript.Models.Todo.Container;
using MvcTypeScript.Models.Todo.Interfaces;
using Ninject;

namespace MvcTypeScript.Models.Todo.Builder
{
    public class TodoCreateModelBuilder : ITodoCreateModelBuilder
    {
        [Inject]
        public ITodoRepository TodoRepository { protected get; set; }

        public TodoCreateModelBuilder()
        {
            
        }

        public TodoCreateViewModel InitTodoCreateViewModel()
        {
            TodoCreateViewModel model = new TodoCreateViewModel();
            return model;
        }

        public TodoCreateViewModel LoadTodoItem( int id)
        {
            var item = TodoRepository.GetItem(id);
            TodoCreateViewModel model = new TodoCreateViewModel()
            {
                Creator = item.Creator,
                Description = item.Description,
                DoDate = item.DoDate,
                Id = item.Id,
                IsActive = item.IsActive,
                Prioritaet = item.Prioritaet
            };

            return model;
        }

        public TodoCreateViewModel AddOrUpdateTodoItem(TodoCreateViewModel viewModel)
        {
            RepositoryTodoItem repository = TodoRepository.GetItem(viewModel.Id);
            if (repository == null)
            {
                repository = new RepositoryTodoItem();
            }

            repository.Creator = viewModel.Creator;
            repository.Description = viewModel.Description;
            repository.IsActive = viewModel.IsActive;
            repository.Prioritaet = viewModel.Prioritaet;
            repository.DoDate = viewModel.DoDate;
            TodoRepository.AddOrUpdateItem(repository);
            viewModel.Id = repository.Id; 

            return viewModel;
        }
    }
}