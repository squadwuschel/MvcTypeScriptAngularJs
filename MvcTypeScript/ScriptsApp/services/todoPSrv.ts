//Achtung diese Datei wurde automatisch erstellt,
//bitte nehmen Sie keine Änderungen daran vor, diese werden
//beim nächsten Erstellen wieder überschrieben.
//Erstellt am 24.10.2015 um 20:14:25 von SquadWuschel.

module App.Services {

 export interface ITodoPService {
	initTodoOverviewInitModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewInitModel>;
initTodoOverviewSearchModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewSearchModel>;
todoOverviewResultModel(searchModel: MvcTypeScript.Models.Todo.ITodoOverviewSearchModel) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewResultModel>;
initTodoCreateViewModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel>;
addOrUpdateTodoItem(createItem: MvcTypeScript.Models.Todo.ITodoCreateViewModel) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel>;
deleteTodoEntry(todoItemId: number): void;
loadTodoItem(todoItemId: number) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel>;
initTodoListenViewModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoListenViewModel>;

 }

  export class TodoPService implements ITodoPService {
          static $inject = ['$http'];
          constructor(private $http: ng.IHttpService) { }

		initTodoOverviewInitModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewInitModel> {
    return this.$http.get('Todo/InitTodoOverviewInitModel').then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoOverviewInitModel>) : MvcTypeScript.Models.Todo.ITodoOverviewInitModel => { return response.data; } );
}

initTodoOverviewSearchModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewSearchModel> {
    return this.$http.get('Todo/InitTodoOverviewSearchModel').then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoOverviewSearchModel>) : MvcTypeScript.Models.Todo.ITodoOverviewSearchModel => { return response.data; } );
}

todoOverviewResultModel(searchModel: MvcTypeScript.Models.Todo.ITodoOverviewSearchModel) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoOverviewResultModel> {
    return this.$http.post('Todo/TodoOverviewResultModel',searchModel).then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoOverviewResultModel>) : MvcTypeScript.Models.Todo.ITodoOverviewResultModel => { return response.data; } );
}

initTodoCreateViewModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel> {
    return this.$http.get('Todo/InitTodoCreateViewModel').then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoCreateViewModel>) : MvcTypeScript.Models.Todo.ITodoCreateViewModel => { return response.data; } );
}

addOrUpdateTodoItem(createItem: MvcTypeScript.Models.Todo.ITodoCreateViewModel) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel> {
    return this.$http.post('Todo/AddOrUpdateTodoItem',createItem).then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoCreateViewModel>) : MvcTypeScript.Models.Todo.ITodoCreateViewModel => { return response.data; } );
}

deleteTodoEntry(todoItemId: number) : void  {
    this.$http.get('Todo/DeleteTodoEntry'+ '?todoItemId='+todoItemId);
}

loadTodoItem(todoItemId: number) : ng.IPromise<MvcTypeScript.Models.Todo.ITodoCreateViewModel> {
    return this.$http.get('Todo/LoadTodoItem'+ '?todoItemId='+todoItemId).then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoCreateViewModel>) : MvcTypeScript.Models.Todo.ITodoCreateViewModel => { return response.data; } );
}

initTodoListenViewModel() : ng.IPromise<MvcTypeScript.Models.Todo.ITodoListenViewModel> {
    return this.$http.get('Todo/InitTodoListenViewModel').then((response: ng.IHttpPromiseCallbackArg<MvcTypeScript.Models.Todo.ITodoListenViewModel>) : MvcTypeScript.Models.Todo.ITodoListenViewModel => { return response.data; } );
}




         //#region Angular Module Definition
        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('todoPSrv', []);
            this._module.service('todoPSrv', TodoPService);
            return this._module;
        }
        //#endregion
  }
}

