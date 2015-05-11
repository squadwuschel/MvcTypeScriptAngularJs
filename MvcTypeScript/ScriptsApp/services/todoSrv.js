var App;
(function (App) {
    var Services;
    (function (Services) {
        var TodoService = (function () {
            //Konstruktor in dem wir unseren Service "Injecten" und mit private als locale Variable unter "this" bereitstellen
            function TodoService($http) {
                this.$http = $http;
            }
            TodoService.prototype.initTodoOverviewInitModel = function () {
                return this.$http.get('/Todo/InitTodoOverviewInitModel').then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.initTodoOverviewSearchModel = function () {
                return this.$http.get('/Todo/InitTodoOverviewSearchModel').then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.todoOverviewResultModel = function (searchModel) {
                return this.$http.put('/Todo/TodoOverviewResultModel', searchModel).then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.initTodoCreateViewModel = function () {
                return this.$http.get('/Todo/InitTodoCreateViewModel').then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.addOrUpdateTodoItem = function (createItem) {
                return this.$http.put('/Todo/AddOrUpdateTodoItem', createItem).then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.loadTodoItem = function (todoItemId) {
                return this.$http.get('/Todo/LoadTodoItem?todoItemId=' + todoItemId.toString()).then(function (response) {
                    return response.data;
                });
            };
            TodoService.prototype.initTodoListenViewModel = function () {
                return this.$http.get('/Todo/InitTodoListenViewModel').then(function (response) {
                    return response.data;
                });
            };
            /**
             * Löschen des TodoItems mit der übergebenen Id
             *
             * @param todoItemId die Id welche aus der Liste der TodoItems gelöscht werden soll
             */
            TodoService.prototype.deleteTodoEntry = function (todoItemId) {
                return this.$http.get('Todo/DeleteTodoEntry?todoItemId=' + todoItemId.toString()).then(function (response) {
                    return true;
                });
            };
            Object.defineProperty(TodoService, "module", {
                get: function () {
                    if (this._module) {
                        return this._module;
                    }
                    this._module = angular.module('todoSrv', []);
                    this._module.service('todoSrv', TodoService);
                    return this._module;
                },
                enumerable: true,
                configurable: true
            });
            //Unseren Service Injecten und für Minification vorbereiten
            TodoService.$inject = ['$http'];
            return TodoService;
        })();
        Services.TodoService = TodoService;
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=todoSrv.js.map