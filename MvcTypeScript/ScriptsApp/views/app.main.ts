module App {
    export class MainApp {
        static createApp(angular: ng.IAngularStatic) {
            //Alle Module definieren die wir verwenden.
            angular.module("app.main", [
                //Fremdanbieter Module
                "ui.router",
                "ui.bootstrap",
                "mgcrea.ngStrap.datepicker",
                //Eigene Module einbinden
                "deafaultModal.directives",
                //Module die mit TypeScript geschrieben wurden einbinden
                Views.MainAppCtrl.module.name,
                Views.Todo.TodoOverviewCtrl.module.name,
                Views.Todo.TodoEditModalCtrl.module.name,
                Views.Shared.TodoModalService.module.name,
                Views.Shared.TodoListenService.module.name
            ]).config([
                "$stateProvider", "$urlRouterProvider","$locationProvider", ($stateProvider : ng.ui.IStateProvider, $urlRouterProvider : ng.ui.IUrlRouterProvider, $locationProvider: ng.ILocationProvider) => {
                    return new Config.RouteConfig($stateProvider, $urlRouterProvider, $locationProvider);
                }
            ]);
        }

        //Alternative Methode zum initialisieren
        //private static _module: ng.IModule;
        //public static createModule(ng: ng.IAngularStatic): ng.IModule {
        //    if (this._module) {
        //        return this._module;
        //    }
        //    this._module = ng.module('mainApp', [MainAppCtrl.module.name]);
        //    return this._module;  
        //}
    }
}

//Unsere Anwendung intial aufrufen/starten
App.MainApp.createApp(angular);

//Stackoverflow Post zur Frage wie man eine App am besten initialisiert
//http://stackoverflow.com/questions/29545246/how-to-initialize-angularjs-app-with-typescript

//Wenn wir die TypeScript Compilierung verwenden in der alle Ts Scripte in eine Datei compiliert werden, dann muss
//eine "_references.ts" Datei im Root des Projektes liegen und enthält alle Abhängigkeiten inkl. 
//der passenden Reihenfolge damit TS weiß welche Datei von welcher abhängt und alle in der richtigen Reihenfolge compiliert werden
//und damit am Ende auch alle Abhängigkeiten gefunden werden. - ACHTUNG ich hatte hier einige Probleme mit der richtigen Reihenfolge, daher
//verwende ich die Bundles von .NET
