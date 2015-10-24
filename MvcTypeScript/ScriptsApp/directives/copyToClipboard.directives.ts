module App.Directives {


    export interface ICopyToClipboardScope {
        sqValues;
    }

    /*
     * Directive, die den String im ngModel in die Zwischenablage kopiert. 
     * https://developer.mozilla.org/en-US/docs/Web/API/Document/execCommand
     * ACHTUNG geht nur ab Chrome:42+, FF:41+, IE:9+, Opera:29+, Safari:Not Supported
     *
     * Verwendung: 
     *  <div sq-copy-to-clipboard ng-model="viewModel.Name"></div>
     */
    export class CopyToClipboard implements ng.IDirective {
        public restrict: string = "A";
        public replcae: boolean = true;
        public require = "ngModel";
        public templateUrl: string = 'ScriptsApp/directives/templates/copyToClipboard.directives.html';
        
        public scope = { }

        public controllerAs = "sqCopyPasteCtrl";
        public bindToController : ICopyToClipboardScope = {
            sqValues: "=ngModel"    //Der Wert der in die Zwischenablage kopiert werden soll.
        }

        //Achtung wir dürfen hier keine Lambda Funktion verwenden, denn sonst können wir nicht auf die bindToController Values zugreifen.
        public controller = function() {
            var inputId: string = "sqCopyToClipboardText";
            var input = $(`<input id="${inputId}" value="${this.sqValues}" style= "width: 1px; height: 1px; margin: 0; border: 0; padding: 0;" />`);

            //Scope Funktion muss ebenfalls hier definiert werden, sonst 
            this.copyToClipboard = () => {
            try {
                $(input).appendTo(document.body);
                $(`#${inputId}`, document.body).select();
                document.execCommand("copy");
            } finally {
                $(`#${inputId}`, document.body).remove();
            }
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
            this._module.directive('sqCopyToClipboard', [() => { return new CopyToClipboard(); }]);
            return this._module;
        }
        //#endregion
    }
}