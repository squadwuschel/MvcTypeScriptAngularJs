
module App.Views.Todo {
    //Den NS importieren für unsere Interfaces aus .NET die vom TypeLite.tt erstellt wurden.
    import My = MvcTypeScript.Models.Todo;

    //Alle Variablen für unseren Controller im Interface definieren
    export interface ITodoOverviewCtrl {
        //Locals
        searchModel: My.ITodoOverviewSearchModel;
        resultModel: My.ITodoOverviewResultModel;
        initModel: My.ITodoOverviewInitModel;
        locals: LocalsModel;
        //Functions
        init(): void;
        search(): void;
        deleteEntry(id: number): void;
        toggleNewEntry(): void;
        getPrioString(prio: MvcTypeScript.Helper.Prioritaet): string;
        listenService: App.Views.Shared.ITodoListenService;
    }

    export class LocalsModel {
        isAddEntryVisible: Boolean;
        newTodoItem: My.ITodoCreateViewModel;
    }

    //Unsere TodoController Klasse erstellen
    export class TodoOverviewCtrl implements ITodoOverviewCtrl {
        searchModel: My.ITodoOverviewSearchModel;
        resultModel: My.ITodoOverviewResultModel;
        initModel: My.ITodoOverviewInitModel;
        listenService: App.Views.Shared.ITodoListenService;
        locals : LocalsModel;
        static $inject = [App.Services.TodoPService.module.name, App.Views.Shared.TodoModalService.module.name, App.Views.Shared.TodoListenService.module.name];
        constructor(private todoSrv: App.Services.ITodoPService, private modalFactory: App.Views.Shared.ITodoModalService, listenService: App.Views.Shared.ITodoListenService) {
            this.listenService = listenService;
            this.init();
        }

       /**
       * Initialisieren der wichtigsten lokalen Variablen
       */
        init(): void {
            this.locals = new LocalsModel();
            this.locals.isAddEntryVisible = false;
            this.todoSrv.initTodoOverviewInitModel().then((result: My.ITodoOverviewInitModel) => {
                //Da wir keinen extra JSON Formatter verwenden um Datumswerte zu serialisieren für einen 
                //normalen MVC Controller müssen wir den string "/Date(125345435345)/" in ein Datum umwandeln mit momentJs
                result.EmptyTodoEntry.DoDate = moment(result.EmptyTodoEntry.DoDate).toDate();

                this.initModel = result;
                this.todoSrv.initTodoOverviewSearchModel().then((resultSearchModel: My.ITodoOverviewSearchModel) => {
                    this.searchModel = resultSearchModel;
                    this.search();  
                });
            });
        }
        
       /**
       * Starten der Suche für die TodoListe
       */
        search(): void {
            this.todoSrv.todoOverviewResultModel(this.searchModel).then((result: My.ITodoOverviewResultModel) => {
                this.resultModel = result;
            });
        }

        /**
         * Ein bzw. ausblenden der Eingabefelder für neue TodoItems
         */
        toggleNewEntry(): void {
            if (!this.locals.isAddEntryVisible) {
                //eine lokale Kopie des leeren Items aus dem InitModel verwenden um die Daten entsprechend zurückzusetzen.
                //wenn ein neuer Eintrag hinzugefügt werden soll.
                this.locals.newTodoItem = angular.copy(this.initModel.EmptyTodoEntry);
            }
            this.locals.isAddEntryVisible = !this.locals.isAddEntryVisible;
        }
        
        /**
         * Neuen Eintrag anlegen oder einen bestehenden speichern
         */
        addOrUpdateEntry(item: My.ITodoCreateViewModel): void {
            this.todoSrv.addOrUpdateTodoItem(item).then((result: My.ITodoCreateViewModel) => {
                this.toggleNewEntry();
                this.search();
            });
        }

        /**
        * Den Passenden Prioritäten String abhängig vom übergebenen Enum wert zurückgeben
        * 
        * @param prio ein Enum Wert für den der passende string ermittelt wird.
        */
        getPrioString(prio: MvcTypeScript.Helper.Prioritaet): string {
            switch (prio) {
                case MvcTypeScript.Helper.Prioritaet.Normal:
                    return "Normal";
                case MvcTypeScript.Helper.Prioritaet.Dringend:
                    return "Dringend";
                case MvcTypeScript.Helper.Prioritaet.Hoch:
                    return "Hoch";
                case MvcTypeScript.Helper.Prioritaet.Keine:
                    return "Keine";
            }

            return "";
        }

        /**
         * Löschen des Todoentries mit der übergebenen Id
         * 
         * @param id die zugehörige Id zum Todoentry welcher gelöscht werden soll
         */
        deleteEntry(id: number): void {
            //Erst den Eintrag löschen und danach die Suche erneut ausführen um die Liste mit den passenden Daten zu füllen.
            this.todoSrv.deleteTodoEntry(id);
            this.search();
        }

        /**
         * Bearbeiten eines Todo Eintrags über ein Modal
         * @param {number} id Die Id des Eintrags der Bearbeitet werden soll.
         */
        editTodoEntry(id: number): void {
            //Das Passende Modal aufrufen und auf den Rückgabewert warten, wenn dieser Erfolgreich gespeichert wurde.
            this.modalFactory.editTodoItem(id).then((result : My.ITodoCreateViewModel) => {
                //Das Result bei Bedarf auswerten!
                this.search();
            });
        }

        //#region Angular Module Definition
        private static _module: ng.IModule;
        /**
         * Stellt das aktuelle Angular Modul für den "todoOverviewCtrl" bereit.
         */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
            //und müssen seine Abhängkeiten nicht wissen.
            this._module = angular.module('todoOverviewCtrl', [Services.TodoPService.module.name]);

            this._module.controller('todoOverviewCtrl', TodoOverviewCtrl);
            return this._module;
        }
        //#endregion
    }
} 