module App.Directives {

    interface ICopyToClipboardScope extends ng.IScope {
        //sqDragData: any;
        sqValues: ng.INgModelController;
        copyToClipbaord() : void;
    }

    /*
     * Beschreibung 
     * 
     * Optionale Attribute: 
     * 
     * Verwendung: 
     *  TODO 
     */
    export class CopyToClipboard implements ng.IDirective {
        public restrict : string = "A";
        public replcae: boolean = true;
        public require = "ngModel";
        public templateUrl: string = 'ScriptsApp/directives/templates/copyToClipboard.directives.html';

        public scope = {
            sqValues : "=ngModel"
        }

        public controller = ($scope: ICopyToClipboardScope) => {
            var inputId: string = "sqCopyToClipboardText";
            var input = $(`<input id="${inputId}" value="${$scope.sqValues}" style= "width: 1px; height: 1px; margin: 0; border: 0; padding: 0;" />`);
           
            $scope.copyToClipbaord = () => {
                $(input).appendTo(document.body);

                $(`#${inputId}`, document.body).select();
                document.execCommand("copy");
                $(`#${inputId}`, document.body).remove();
            }

        }

        //#region Angular Module Definition
        private static _module: ng.IModule;

        /**
        * Stellt die Angular Module für CopyToClipboard bereit.
        */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für unsere Direktive definieren.
            this._module = angular.module('copyToClipboard.directives', []);
            this._module.directive('copyToClipboard', [() => { return new CopyToClipboard(); }]);
            return this._module;
        }
        //#endregion
    }
}