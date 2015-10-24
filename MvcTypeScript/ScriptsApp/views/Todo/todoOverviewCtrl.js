var App;
(function (App) {
    var Views;
    (function (Views) {
        var Todo;
        (function (Todo) {
            var LocalsModel = (function () {
                function LocalsModel() {
                }
                return LocalsModel;
            })();
            Todo.LocalsModel = LocalsModel;
            //Unsere TodoController Klasse erstellen
            var TodoOverviewCtrl = (function () {
                function TodoOverviewCtrl(todoSrv, modalFactory, listenService) {
                    this.todoSrv = todoSrv;
                    this.modalFactory = modalFactory;
                    this.listenService = listenService;
                    this.init();
                }
                /**
                * Initialisieren der wichtigsten lokalen Variablen
                */
                TodoOverviewCtrl.prototype.init = function () {
                    var _this = this;
                    this.locals = new LocalsModel();
                    this.locals.isAddEntryVisible = false;
                    this.todoSrv.initTodoOverviewInitModel().then(function (result) {
                        //Da wir keinen extra JSON Formatter verwenden um Datumswerte zu serialisieren für einen 
                        //normalen MVC Controller müssen wir den string "/Date(125345435345)/" in ein Datum umwandeln mit momentJs
                        result.EmptyTodoEntry.DoDate = moment(result.EmptyTodoEntry.DoDate).toDate();
                        _this.initModel = result;
                        _this.todoSrv.initTodoOverviewSearchModel().then(function (resultSearchModel) {
                            _this.searchModel = resultSearchModel;
                            _this.search();
                        });
                    });
                };
                /**
                * Starten der Suche für die TodoListe
                */
                TodoOverviewCtrl.prototype.search = function () {
                    var _this = this;
                    this.todoSrv.todoOverviewResultModel(this.searchModel).then(function (result) {
                        _this.resultModel = result;
                    });
                };
                /**
                 * Ein bzw. ausblenden der Eingabefelder für neue TodoItems
                 */
                TodoOverviewCtrl.prototype.toggleNewEntry = function () {
                    if (!this.locals.isAddEntryVisible) {
                        //eine lokale Kopie des leeren Items aus dem InitModel verwenden um die Daten entsprechend zurückzusetzen.
                        //wenn ein neuer Eintrag hinzugefügt werden soll.
                        this.locals.newTodoItem = angular.copy(this.initModel.EmptyTodoEntry);
                    }
                    this.locals.isAddEntryVisible = !this.locals.isAddEntryVisible;
                };
                /**
                 * Neuen Eintrag anlegen oder einen bestehenden speichern
                 */
                TodoOverviewCtrl.prototype.addOrUpdateEntry = function (item) {
                    var _this = this;
                    this.todoSrv.addOrUpdateTodoItem(item).then(function (result) {
                        _this.toggleNewEntry();
                        _this.search();
                    });
                };
                /**
                * Den Passenden Prioritäten String abhängig vom übergebenen Enum wert zurückgeben
                *
                * @param prio ein Enum Wert für den der passende string ermittelt wird.
                */
                TodoOverviewCtrl.prototype.getPrioString = function (prio) {
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
                };
                /**
                 * Löschen des Todoentries mit der übergebenen Id
                 *
                 * @param id die zugehörige Id zum Todoentry welcher gelöscht werden soll
                 */
                TodoOverviewCtrl.prototype.deleteEntry = function (id) {
                    //Erst den Eintrag löschen und danach die Suche erneut ausführen um die Liste mit den passenden Daten zu füllen.
                    this.todoSrv.deleteTodoEntry(id);
                    this.search();
                };
                /**
                 * Bearbeiten eines Todo Eintrags über ein Modal
                 * @param {number} id Die Id des Eintrags der Bearbeitet werden soll.
                 */
                TodoOverviewCtrl.prototype.editTodoEntry = function (id) {
                    var _this = this;
                    //Das Passende Modal aufrufen und auf den Rückgabewert warten, wenn dieser Erfolgreich gespeichert wurde.
                    this.modalFactory.editTodoItem(id).then(function (result) {
                        //Das Result bei Bedarf auswerten!
                        _this.search();
                    });
                };
                Object.defineProperty(TodoOverviewCtrl, "module", {
                    /**
                     * Stellt das aktuelle Angular Modul für den "todoOverviewCtrl" bereit.
                     */
                    get: function () {
                        if (this._module) {
                            return this._module;
                        }
                        //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von Außen nur den Controller einbinden
                        //und müssen seine Abhängkeiten nicht wissen.
                        this._module = angular.module('todoOverviewCtrl', [App.Services.TodoPService.module.name]);
                        this._module.controller('todoOverviewCtrl', TodoOverviewCtrl);
                        return this._module;
                    },
                    enumerable: true,
                    configurable: true
                });
                TodoOverviewCtrl.$inject = [App.Services.TodoPService.module.name, App.Views.Shared.TodoModalService.module.name, App.Views.Shared.TodoListenService.module.name];
                return TodoOverviewCtrl;
            })();
            Todo.TodoOverviewCtrl = TodoOverviewCtrl;
        })(Todo = Views.Todo || (Views.Todo = {}));
    })(Views = App.Views || (App.Views = {}));
})(App || (App = {}));
