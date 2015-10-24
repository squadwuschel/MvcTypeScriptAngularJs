var App;
(function (App) {
    var Views;
    (function (Views) {
        var Todo;
        (function (Todo) {
            var TodoEditModalCtrl = (function () {
                function TodoEditModalCtrl($modalInstance, todoSrv, todoId, listenService) {
                    this.$modalInstance = $modalInstance;
                    this.todoSrv = todoSrv;
                    this.todoId = todoId;
                    this.listenService = listenService;
                    this.init();
                }
                TodoEditModalCtrl.prototype.init = function () {
                    var _this = this;
                    this.todoSrv.loadTodoItem(this.todoId).then(function (result) {
                        //Unser Datum noch in ein richtiges Datum verwandeln
                        result.DoDate = moment(result.DoDate).toDate();
                        _this.viewModel = result;
                    });
                };
                TodoEditModalCtrl.prototype.save = function () {
                    var _this = this;
                    //Speichern des Eintrags
                    this.todoSrv.addOrUpdateTodoItem(this.viewModel).then(function (result) {
                        //Schließen des Modals nach dem erfolgreichen Speichern.
                        _this.$modalInstance.close(_this.todoId);
                    });
                };
                TodoEditModalCtrl.prototype.cancel = function () {
                    this.$modalInstance.dismiss('cancel');
                };
                Object.defineProperty(TodoEditModalCtrl, "module", {
                    /**
                     * Stellt das aktuelle Angular Modul für den "todoEditModalCtrl" bereit.
                     */
                    get: function () {
                        if (this._module) {
                            return this._module;
                        }
                        //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
                        //und müssen seine Abhängkeiten nicht wissen.
                        this._module = angular.module('todoEditModalCtrl', [App.Services.TodoPService.module.name, "ui.bootstrap"]);
                        this._module.controller('todoEditModalCtrl', TodoEditModalCtrl);
                        return this._module;
                    },
                    enumerable: true,
                    configurable: true
                });
                TodoEditModalCtrl.$inject = ["$modalInstance", App.Services.TodoPService.module.name, "todoId", Views.Shared.TodoListenService.module.name];
                return TodoEditModalCtrl;
            })();
            Todo.TodoEditModalCtrl = TodoEditModalCtrl;
        })(Todo = Views.Todo || (Views.Todo = {}));
    })(Views = App.Views || (App.Views = {}));
})(App || (App = {}));
