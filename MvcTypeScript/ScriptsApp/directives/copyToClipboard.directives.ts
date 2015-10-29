module App.Directives {
    /* 
    * Definition der Scope Variablen
     */
    export interface ICopyToClipboardScope {
        sqValues : string;
        sqTitle : string;
        sqIconCss : string;
        sqBtnCss: string;
        sqCallBackFct;
    }

    /*
     * Directive, die den String im ngModel in die Zwischenablage kopiert. 
     * https://developer.mozilla.org/en-US/docs/Web/API/Document/execCommand
     * ACHTUNG geht nur ab Chrome:42+, FF:41+, IE:9+, Opera:29+, Safari:Not Supported
     *
     * Verwendung: 
     *  <div sq-copy-to-clipboard ng-model="ctrl.viewModel.Name" sq-call-back-fct="ctrl.DoSomething" sq-icon-css="'fa fa-fw fa-copy'" sq-btn-css="'btn btn-default btn-xs'" sq-title="'In die Zwischenablage'"></div>
     */
    export class CopyToClipboard implements ng.IDirective {
        public restrict: string = "A";
        public replace: boolean = true;
        public require = "ngModel";
        public templateUrl: string = window.siteRoot + 'ScriptsApp/directives/templates/copyToClipboard.directives.html';
        public scope = {}

        public controller = CopyToClipboardCtrl;
        public controllerAs = "sqCopyPasteCtrl";
        public bindToController: ICopyToClipboardScope = {
            sqValues: "=ngModel",    //Der Wert der in die Zwischenablage kopiert werden soll.
            sqTitle: "=",            //Der Titel der angezeigt werden soll Optional
            sqIconCss: "=",          //CSS für das Icon
            sqBtnCss: "=",           //CSS für den Button
            sqCallBackFct: "&"        //Callback Funktion die ausgeführt wird, wenn diese gesetzt wurde.
        }

        constructor() { }

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
            this._module = angular.module('copyToClipboard.directives', [CopyToClipBoardServiceProvider.module.name]);
            this._module.directive('sqCopyToClipboard', [() => { return new CopyToClipboard(); }]);
            return this._module;
        }
        //#endregion
    }

    /*
    * Implementierung unseres CopyToClipboard Controllers.
    */
    export class CopyToClipboardCtrl implements ICopyToClipboardScope {
        public sqValues: any;
        public sqTitle: string;
        public sqIconCss: string;
        public sqBtnCss: string;
        public sqCallBackFct;

        static $inject = ['copyToClipBoardConfig'];

        /* 
        * Da wir die CSS Klassen für einen Provider setzen, hier den passenden Provider injecten und im Template dann auf dessen Config Werte zugreifen.
        */
        constructor(private copyToClipBoardConfig: ICopyToClipBoardServiceProvider) {
            this.init();
        }

        init(): void {
            //Prüfen ob ein Titel oder andere CSS Klasse übergeben wurde, sonst den Wert aus dem Provider verwenden
            if (this.sqTitle === undefined) {
                this.sqTitle = this.copyToClipBoardConfig.config.title;
            }

            if (this.sqBtnCss === undefined) {
                this.sqBtnCss = this.copyToClipBoardConfig.config.btnCssClass;
            }

            if (this.sqIconCss === undefined) {
                this.sqIconCss = this.copyToClipBoardConfig.config.iconCssClass;
            }
        }

        /*
        * Unseren ModelValue in die Zwischenablage kopieren.
        */
        public copyToClipboard(): void {
            var inputId: string = "sqCopyToClipboardText";
            
            //Unser Input erstellen inkl. des Textes den wir kopieren wollen, da die Angular Implementierung auf "this.sqValues" zugreift ist dies 
            //durch die Extra Definition des CopyToClipboard Controllers problemlos möglich. Das Input selbst ist Mindestens 1px breit und hoch, denn
            //sonst kann der Inhalt im Chrome nicht markiert werden, was zwingend notwendig ist damit der Inhalt kopiert werden kann.
            var input = $(`<input id="${inputId}" value="${this.sqValues}" style= "width: 1px; height: 1px; margin: 0; border: 0; padding: 0;" />`);

            try {
                //Unser Input dem DOM Hinzufügen
                $(input).appendTo(document.body);
                //Den Inhalt des Inputs auswählen, denn der execCommand kopiert nur die Inhalte in die Zwischenablage, die ausgewählt sind.
                $(`#${inputId}`, document.body).select();
                document.execCommand("copy");
            } finally {
                //Am Ende das Eingabefeld wieder aus dem DOM entfernen
                $(`#${inputId}`, document.body).remove();

                //Sollte eine CallBack Funktion angegeben sein, diese hier ausführen.
                if (this.sqCallBackFct !== undefined) {
                    this.sqCallBackFct();
                }
            }
        }
    }

    /*
     * Configklasse für unsere CopyPaste Direktive. Hier kann man z.B. die passenden Css Klassen ändern die gesetzt werden.
     */
    export class CopyToClipBoardServiceProvider implements ng.IServiceProvider, ICopyToClipBoardServiceProvider {
        //Die Konfigurationsdaten entsprechend setzen.
        config: ICopyToClipBoardConfig = {
            btnCssClass: "btn btn-default btn-sm",  //Bootstrap
            iconCssClass: "fa fa-fw fa-copy",       //Font Awesome
            title: "In die Zwischenablage kopieren"
        }

        $get = () => {
            return {
                config: this.config
            }
        };


        //#region Angular Module Definition
        private static _module: ng.IModule;
        /**
        * Stellt die Angular Module für CopyToClipboardProvider bereit.
        */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für unsere Direktive definieren.
            this._module = angular.module('copyToClipBoardConfig.provider', []);
            this.module.provider("copyToClipBoardConfig", () => new CopyToClipBoardServiceProvider());
            return this._module;
        }
        //#endregion

    }

    export interface ICopyToClipBoardServiceProvider {
        config: ICopyToClipBoardConfig;
    }

    export interface ICopyToClipBoardConfig {
        btnCssClass: string;
        iconCssClass: string;
        title: string;
    }
}
