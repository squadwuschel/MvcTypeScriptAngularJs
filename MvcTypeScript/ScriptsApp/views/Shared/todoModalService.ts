module App.Views.Shared {
    //Den NS importieren für unsere Interfaces aus .NET die vom TypeLite.tt erstellt wurden.
    import My = MvcTypeScript.Models.Todo;

    export interface ITodoModalService {
        editTodoItem(id: Number) : ng.IPromise<My.ITodoCreateViewModel>;
    }

    /**
     * Ein ModalService erstellen über die man auf alle Modals in der Anwendung zugreifen kann.
     * Quelle:
     * http://www.ngroutes.com/questions/AUuAClOCa5vEqxqlK2UN/how-to-use-angular-ui-bootstrap-modals-in-typescript.html
     */
    export class TodoModalService implements ITodoModalService {
        static $inject = ['$modal', "$q"];
        constructor(private $modal: ng.ui.bootstrap.IModalService, private $q: ng.IQService) { }
      
        /**
         * Bearbeiten eines TodoItems, dem die passende TodoId Übergeben wird und ein Modal zum Bearbeiten öffnet.
         * @param id Die Id des TodoItems welches Bearbeitet werden soll.
         */
        public editTodoItem(id: Number): ng.IPromise<My.ITodoCreateViewModel> {
            var options: ng.ui.bootstrap.IModalSettings = {
                templateUrl: '/Todo/TodoEditModal',
                //Controller As Syntax für Modal Controller
                controller: App.Views.Todo.TodoEditModalCtrl.module.name + ' as todoCtrl',
                size: 'lg',
                resolve: {
                    todoId: () => id
                }
            };

            //Das Promise zurückgeben, damit man auf den Rückgabewert des Modals in der Anwendung entsprechend reagieren kann.
            return this.$modal.open(options).result
                .then((updatedItem: My.ITodoCreateViewModel) => {
                return updatedItem;
            });
        }

        //#region Angular Module Definition
        private static _module: ng.IModule;
        /**
         * Stellt das aktuelle Angular Modul für den "todoModalService" bereit.
         */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
            //und müssen seine Abhängkeiten nicht wissen.
            this._module = angular.module('todoModalService', ["ui.bootstrap"]);
            this._module.service('todoModalService', TodoModalService);
            return this._module;
        }
        //#endregion
    }
} 