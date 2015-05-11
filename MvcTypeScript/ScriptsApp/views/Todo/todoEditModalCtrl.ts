module App.Views.Todo {
    //Den NS importieren für unsere Interfaces aus .NET die vom TypeLite.tt erstellt wurden.
    import My = MvcTypeScript.Models.Todo;

    export interface ITodoEditModalCtrl {
        viewModel: My.ITodoCreateViewModel;
        listenService : Views.Shared.ITodoListenService;
        init(): void;
        save(): void;
        cancel(): void;
    }

    export class TodoEditModalCtrl implements ITodoEditModalCtrl {
        viewModel: MvcTypeScript.Models.Todo.ITodoCreateViewModel;
        listenService: Views.Shared.ITodoListenService;
        static $inject = ["$modalInstance", Services.TodoPService.module.name, "todoId", Views.Shared.TodoListenService.module.name];
        constructor(private $modalInstance: ng.ui.bootstrap.IModalServiceInstance, private todoSrv: Services.ITodoPService, public todoId: number, listenService: Views.Shared.ITodoListenService) {
            this.listenService = listenService;
            this.init();
        }    

        init() : void {
            this.todoSrv.loadTodoItem(this.todoId).then((result: My.ITodoCreateViewModel) => {
                //Unser Datum noch in ein richtiges Datum verwandeln
                result.DoDate = moment(result.DoDate).toDate();
                this.viewModel = result;
            });
        }

        save(): void {
            //Speichern des Eintrags
            this.todoSrv.addOrUpdateTodoItem(this.viewModel).then((result : My.ITodoCreateViewModel) => {
                //Schließen des Modals nach dem erfolgreichen Speichern.
                this.$modalInstance.close(this.todoId);
            });
        }

        cancel() : void {
            this.$modalInstance.dismiss('cancel');
        }

        //#region Angular Module Definition
        private static _module: ng.IModule;
        /**
         * Stellt das aktuelle Angular Modul für den "todoEditModalCtrl" bereit.
         */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
            //und müssen seine Abhängkeiten nicht wissen.
            this._module = angular.module('todoEditModalCtrl', [Services.TodoPService.module.name, "ui.bootstrap"]);
            this._module.controller('todoEditModalCtrl', TodoEditModalCtrl);
            return this._module;
        }
        //#endregion
    }
} 