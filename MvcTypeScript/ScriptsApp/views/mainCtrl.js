var App;
(function (App) {
    var Views;
    (function (Views) {
        'use strict';
        var MainAppCtrl = (function () {
            function MainAppCtrl() {
            }
            Object.defineProperty(MainAppCtrl, "module", {
                get: function () {
                    if (this._module) {
                        return this._module;
                    }
                    this._module = angular.module('mainCtrl', []);
                    this._module.controller('mainCtrl', MainAppCtrl);
                    return this._module;
                },
                enumerable: true,
                configurable: true
            });
            return MainAppCtrl;
        })();
        Views.MainAppCtrl = MainAppCtrl;
    })(Views = App.Views || (App.Views = {}));
})(App || (App = {}));
