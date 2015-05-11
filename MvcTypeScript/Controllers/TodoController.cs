using System.Web.Mvc;
using MvcTypeScript.Models.Todo;
using MvcTypeScript.Models.Todo.Interfaces;
using MvcTypeScript.ProxyCreator.ProxyBuilder;
using Ninject;
using TypeLite;

namespace MvcTypeScript.Controllers
{
    public class TodoController : Controller
    {
        [Inject]
        public ITodoOverviewModelBuilder TodoOverviewModelBuilder { protected get; set; }

        [Inject]
        public ITodoCreateModelBuilder TodoCreateModelBuilder { protected get; set; }

        [Inject]
        public ITodoListenModelBuilder TodoListenModelBuilder { protected get; set; }

        #region Views
        public ActionResult TodoOverview()
        {
            return View();
        }
        #endregion

        #region Ajax Calls
        [CreateProxy(ReturnType = typeof(TodoOverviewInitModel))]
        public JsonResult InitTodoOverviewInitModel()
        {
            return Json(TodoOverviewModelBuilder.InitTodoOverviewInitModel(), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy(ReturnType = typeof(TodoOverviewSearchModel))]
        public JsonResult InitTodoOverviewSearchModel()
        {
            return Json(TodoOverviewModelBuilder.InitTodoOverviewSearchModel(), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy(ReturnType = typeof(TodoOverviewResultModel))]
        public JsonResult TodoOverviewResultModel(TodoOverviewSearchModel searchModel)
        {
            return Json(TodoOverviewModelBuilder.TodoOverviewResultModel(searchModel), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy(ReturnType = typeof(TodoCreateViewModel))]
        public JsonResult InitTodoCreateViewModel()
        {
            return Json(TodoCreateModelBuilder.InitTodoCreateViewModel(), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy(ReturnType = typeof(TodoCreateViewModel))]
        public JsonResult AddOrUpdateTodoItem(TodoCreateViewModel createItem)
        {
            return Json(TodoCreateModelBuilder.AddOrUpdateTodoItem(createItem), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy()]
        public void DeleteTodoEntry(int todoItemId)
        {
            TodoOverviewModelBuilder.DeleteTodoEntry(todoItemId);
        }

        [CreateProxy(ReturnType = typeof(TodoCreateViewModel))]
        public JsonResult LoadTodoItem(int todoItemId)
        {
            return Json(TodoCreateModelBuilder.LoadTodoItem(todoItemId), JsonRequestBehavior.AllowGet);
        }

        [CreateProxy(ReturnType = typeof(TodoListenViewModel))]
        public JsonResult InitTodoListenViewModel()
        {
            return Json(TodoListenModelBuilder.InitTodoListenViewModel(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Modals
        public ActionResult TodoEditModal()
        {
            return View();
        }
        #endregion
    }
}
