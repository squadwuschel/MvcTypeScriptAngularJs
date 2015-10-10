var App;
(function (App) {
    var Views;
    (function (Views) {
        var Shared;
        (function (Shared) {
            /**
             * Ein der die Inhalte der Dropdownlisten für unsere Anwendung bereitstellt.
             */
            var TodoListenService = (function () {
                function TodoListenService(todoService) {
                    this.todoService = todoService;
                    this.init();
                }
                TodoListenService.prototype.init = function () {
                    var _this = this;
                    //Laden der Modeldaten
                    this.todoService.initTodoListenViewModel()
                        .then(function (result) { _this.listEntries = result; });
                };
                Object.defineProperty(TodoListenService, "module", {
                    /**
                    * Stellt das aktuelle Angular Modul für den "todoListenService" bereit.
                    */
                    get: function () {
                        if (this._module) {
                            return this._module;
                        }
                        //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
                        //und müssen seine Abhängkeiten nicht wissen.
                        this._module = angular.module('todoListenService', [App.Services.TodoPService.module.name]);
                        this._module.service('todoListenService', TodoListenService);
                        return this._module;
                    },
                    enumerable: true,
                    configurable: true
                });
                TodoListenService.$inject = [App.Services.TodoPService.module.name];
                return TodoListenService;
            })();
            Shared.TodoListenService = TodoListenService;
        })(Shared = Views.Shared || (Views.Shared = {}));
    })(Views = App.Views || (App.Views = {}));
})(App || (App = {}));
