//Achtung diese Datei wurde automatisch erstellt,
//bitte nehmen Sie keine Änderungen daran vor, diese werden
//beim nächsten Erstellen wieder überschrieben.
//Erstellt am 24.10.2015 um 22:37:03 von SquadWuschel.
var App;
(function (App) {
    var Services;
    (function (Services) {
        var TodoPService = (function () {
            function TodoPService($http) {
                this.$http = $http;
            }
            TodoPService.prototype.initTodoOverviewInitModel = function () {
                return this.$http.get('Todo/InitTodoOverviewInitModel').then(function (response) { return response.data; });
            };
            TodoPService.prototype.initTodoOverviewSearchModel = function () {
                return this.$http.get('Todo/InitTodoOverviewSearchModel').then(function (response) { return response.data; });
            };
            TodoPService.prototype.todoOverviewResultModel = function (searchModel) {
                return this.$http.post('Todo/TodoOverviewResultModel', searchModel).then(function (response) { return response.data; });
            };
            TodoPService.prototype.initTodoCreateViewModel = function () {
                return this.$http.get('Todo/InitTodoCreateViewModel').then(function (response) { return response.data; });
            };
            TodoPService.prototype.addOrUpdateTodoItem = function (createItem) {
                return this.$http.post('Todo/AddOrUpdateTodoItem', createItem).then(function (response) { return response.data; });
            };
            TodoPService.prototype.deleteTodoEntry = function (todoItemId) {
                this.$http.get('Todo/DeleteTodoEntry' + '?todoItemId=' + todoItemId);
            };
            TodoPService.prototype.loadTodoItem = function (todoItemId) {
                return this.$http.get('Todo/LoadTodoItem' + '?todoItemId=' + todoItemId).then(function (response) { return response.data; });
            };
            TodoPService.prototype.initTodoListenViewModel = function () {
                return this.$http.get('Todo/InitTodoListenViewModel').then(function (response) { return response.data; });
            };
            Object.defineProperty(TodoPService, "module", {
                get: function () {
                    if (this._module) {
                        return this._module;
                    }
                    this._module = angular.module('todoPSrv', []);
                    this._module.service('todoPSrv', TodoPService);
                    return this._module;
                },
                enumerable: true,
                configurable: true
            });
            TodoPService.$inject = ['$http'];
            return TodoPService;
        })();
        Services.TodoPService = TodoPService;
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
