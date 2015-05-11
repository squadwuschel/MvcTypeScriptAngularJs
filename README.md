# MvcTypeScriptAngularJs
Beispielanwendung für AngularJs und TypeScript in ASP.NET MVC

Implementierung einer einfachen TODO Anwendung mit statischem Repository unter
der Verwendung der folgenden Technologien:

- AngularJs
- TypeScript
- TypeLite.Net4
- ASP.NET MVC

# Zusätzliches Projekt "ProxyCreator" für AngularJS Services
Automatisches Erstellen von AngularJs Services für ASP.NET MVC Controller
über ein Attribut in den entsprechenden Controller Funktionen.

     [CreateProxy(ReturnType = typeof(TodoOverviewInitModel))]
      public JsonResult InitTodoOverviewInitModel()
      {
         return Json(TodoOverviewModelBuilder.InitTodoOverviewInitModel(), JsonRequestBehavior.AllowGet);
      }