module App.Views.Shared {
    //Den NS importieren für unsere Interfaces aus .NET die vom TypeLite.tt erstellt wurden.
    import My = MvcTypeScript.Models.Todo;

    export interface ITodoListenService {
        listEntries: My.ITodoListenViewModel;
    }

    /**
     * Ein der die Inhalte der Dropdownlisten für unsere Anwendung bereitstellt.
     */
    export class TodoListenService implements ITodoListenService {
        listEntries: MvcTypeScript.Models.Todo.ITodoListenViewModel;
        static $inject = [App.Services.TodoPService.module.name];
        constructor(private todoService: App.Services.ITodoPService) {
            this.init();
        }

        init(): void {
            //Laden der Modeldaten
            this.todoService.initTodoListenViewModel()
                .then((result: My.ITodoListenViewModel) => { this.listEntries = result });
        }

        //#region Angular Module Definition
        private static _module: ng.IModule;
        /**
        * Stellt das aktuelle Angular Modul für den "todoListenService" bereit.
        */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
            //und müssen seine Abhängkeiten nicht wissen.
            this._module = angular.module('todoListenService', [App.Services.TodoPService.module.name]);
            this._module.service('todoListenService', TodoListenService);
            return this._module;
        }
        //#endregion
    }
}