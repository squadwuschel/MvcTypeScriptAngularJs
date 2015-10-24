var App;
(function (App) {
    var Views;
    (function (Views) {
        var Shared;
        (function (Shared) {
            /**
             * Ein ModalService erstellen über die man auf alle Modals in der Anwendung zugreifen kann.
             * Quelle:
             * http://www.ngroutes.com/questions/AUuAClOCa5vEqxqlK2UN/how-to-use-angular-ui-bootstrap-modals-in-typescript.html
             */
            var TodoModalService = (function () {
                function TodoModalService($modal, $q) {
                    this.$modal = $modal;
                    this.$q = $q;
                }
                /**
                 * Bearbeiten eines TodoItems, dem die passende TodoId Übergeben wird und ein Modal zum Bearbeiten öffnet.
                 * @param id Die Id des TodoItems welches Bearbeitet werden soll.
                 */
                TodoModalService.prototype.editTodoItem = function (id) {
                    var options = {
                        templateUrl: '/Todo/TodoEditModal',
                        //Controller As Syntax für Modal Controller
                        controller: App.Views.Todo.TodoEditModalCtrl.module.name + ' as todoCtrl',
                        size: 'lg',
                        resolve: {
                            todoId: function () { return id; }
                        }
                    };
                    //Das Promise zurückgeben, damit man auf den Rückgabewert des Modals in der Anwendung entsprechend reagieren kann.
                    return this.$modal.open(options).result
                        .then(function (updatedItem) {
                        return updatedItem;
                    });
                };
                Object.defineProperty(TodoModalService, "module", {
                    /**
                     * Stellt das aktuelle Angular Modul für den "todoModalService" bereit.
                     */
                    get: function () {
                        if (this._module) {
                            return this._module;
                        }
                        //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
                        //und müssen seine Abhängkeiten nicht wissen.
                        this._module = angular.module('todoModalService', ["ui.bootstrap"]);
                        this._module.service('todoModalService', TodoModalService);
                        return this._module;
                    },
                    enumerable: true,
                    configurable: true
                });
                TodoModalService.$inject = ['$modal', "$q"];
                return TodoModalService;
            })();
            Shared.TodoModalService = TodoModalService;
        })(Shared = Views.Shared || (Views.Shared = {}));
    })(Views = App.Views || (App.Views = {}));
})(App || (App = {}));
//# sourceMappingURL=todoModalService.js.map