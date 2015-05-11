using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcTypeScript.Helper;
using MvcTypeScript.Models.Todo.Container;
using MvcTypeScript.Models.Todo.Interfaces;
using Ninject;

namespace MvcTypeScript.Models.Todo.Builder
{
    public class TodoOverviewModelBuilder : ITodoOverviewModelBuilder
    {
        [Inject]
        public ITodoRepository TodoRepository { protected get; set; }

        public TodoOverviewModelBuilder()
        {
        }

        public TodoOverviewInitModel InitTodoOverviewInitModel()
        {
            TodoOverviewInitModel model = new TodoOverviewInitModel();
            model.Datum = DateTime.Now.ToLongTimeString();
            model.CurrentUser = string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? "no user found" : HttpContext.Current.User.Identity.Name;
            return model;
        }

        public TodoOverviewSearchModel InitTodoOverviewSearchModel()
        {
            TodoOverviewSearchModel model = new TodoOverviewSearchModel();
            return model;
        }

        public TodoOverviewResultModel TodoOverviewResultModel(TodoOverviewSearchModel searchModel)
        {
            TodoOverviewResultModel resultModel = new TodoOverviewResultModel();

            var entries = TodoRepository.GetItems().Where(p => p.DoDate >= searchModel.Startdatum && p.DoDate <= searchModel.Enddatum);

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                entries =
                    entries.Where(
                        p =>
                            p.Description.ToLower().Contains(searchModel.SearchText.ToLower().Trim()) ||
                            p.Creator.ToLower().Contains(searchModel.SearchText.ToLower().Trim()));
            }

            resultModel.Entries.AddRange(entries.Take(searchModel.PageSize).Select(p => new TodoEntry()
            {
                Prioritaet = p.Prioritaet,
                Description = p.Description,
                IsOpen = p.IsActive,
                Creator = p.Creator,
                DoDate = p.DoDate.ToString(),
                Id = p.Id
            }));

            return resultModel;
        }

        public void DeleteTodoEntry(int id)
        {
            TodoRepository.DeleteItem(id);
        }
    }
}