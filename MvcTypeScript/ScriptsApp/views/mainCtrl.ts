module App.Views {
    'use strict';

    export interface IMainAppCtrl {
    }

    export class MainAppCtrl implements IMainAppCtrl {
        constructor() {}

        //#region Angular Module Definition
        //Beispiel für Module Implementation: https://github.com/Brocco/ts-star-wars
        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('mainCtrl', []);
            this._module.controller('mainCtrl', MainAppCtrl);
            return this._module;
        }
        //#endregion
    }
}

