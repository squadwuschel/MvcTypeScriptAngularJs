module App.Views.Todo {
    //Den NS importieren für unsere Interfaces aus .NET die vom TypeLite.tt erstellt wurden.

    //Alle Variablen für unseren Controller im Interface definieren
    export interface IEs6FeaturesCtrl {
        //Locals
        //viewModel: My.;
        //Functions
        init(): void;
    }

    class Person {
        constructor(public name: string, public age: number) {  }
    }

    class ViewModel {
        constructor(public text: string, public text2 : string) {  }
    }



    //Unsere es6Features Klasse erstellen
    export class Es6FeaturesCtrl implements IEs6FeaturesCtrl {
        private viewModel : ViewModel;
        private personList: Person[];
        static $inject = [
        ];

        constructor() {
            this.init();
        }
        
        /**
        * Initialisieren der wichtigsten lokalen Variablen
        */
        init(): void {
            this.viewModel = new ViewModel("Hallo ich bin ein Text", "Das ist ein zweiter Text");



            this.personList = [];
            this.personList.push(new Person("Maiks", 12));
            this.personList.push(new Person("Hannes", 17));
            this.personList.push(new Person("Thomas", 23));
            this.personList.push(new Person("Ilsche", 6));
            this.personList.push(new Person("Sreddy", 27));
            this.personList.push(new Person("Henries", 11));

            this.einigeArrayFunktionen();
        }

        copyToClipbaord(): void {
            $("#clp").select();
            document.execCommand("copy");
        }

        multilineStrings(): string {

            var oldStringadd = "In EcmaScript 6 werden endlich Multiline Strings" +
                    "unterstüzt ohne das man immer ein läastiges" +
                    "Plus Zeichen verwenden muss. Da" +
                    "TypeScript auch ES6 Nutzt kann man hier" +
                    "auf die gleichen Features zugreifen.";

            //Mehrzeiliger String ohne "+" Verkettung mit "back-tick" bzw. grave accent Zeichen.
            return `In EcmaScript 6 werden endlich Multiline Strings
                    unterstüzt ohne das man immer ein läastiges
                    Plus Zeichen verwenden muss. Da 
                    TypeScript auch ES6 Nutzt kann man hier 
                    auf die gleichen Features zugreifen.`;
        }

        stringFormatMitEs6(): string {
            var items = [];
            items.push("Auto");
            items.push("Fahrrad");
            items.push("Moped");

            var oldStringFormat = "Wir haben " + items.length + " Fahrzeugtypen bei uns Zuhause dazu gehört auch ein " +  items[0] + ".";

            //String Format mit ES6
            return `Wir haben ${items.length} Fahrzeugtypen bei uns Zuhause dazu gehört auch ein ${items[0]}.`;
        }

        einigeArrayFunktionen() : void {

            //EVERY
            //Ergebnis: False - Prüft ob alle Einträge dem Größer 12 Sind und nur dann wird True zurückgegeben
            var alleAelter12 = this.personList.every((element, index, array) => {
                return element.age > 12;
            });

            //Ergebnis: True - Denn alle Namen enthalten ein "S"
            var entaeltBuchstabeS = this.personList.every((element, index, array) => {
                //Wenn der Buchstabe "S" im String gefunden wird, wird ein Index größer -1 zurückgegeben!
                return element.name.toLowerCase().indexOf("s") > -1;
            });

            //SOME
            //Ergebnis: True - Prüft ob mindestens ein Eintrag dem Kriterium entspricht und gibt dann True zurück
            var mindestensEinerJuengerAls12 = this.personList.some((element, index, array) => {
                return element.age < 12;
            });
            
            //FILTER
            //Gibt eine Neue Liste mit den gefilterten Einträgen zurück
            var alleAelter18: Person[] = this.personList.filter((element, index, array) => {
                if (element.age > 18) {
                    return true;
                }
            });

            //FOREACH
            var meinePersonen: Person[] = [];
            //For Each Schleife - hat keinen Rückgabewert
            this.personList.forEach((element, index, array) => {
                if (element.age > 10 && element.age < 20) {
                    meinePersonen.push(element);
                }
            });

            //SORT
            //Sortieren der Array Einträge nach dem Alter mit Sort.
            this.personList.sort((person1, person2) => {
               return person1.age - person2.age;
            });

            //Sortieren der Array Einträge nach dem Namen
            this.personList.sort((person1, person2) => {
                return  person1.name.localeCompare(person2.name);
            });


            //Alle Aufrufe sind AUCH nur mit einem oder zwei Parametern möglich.
            var alleAelter5 = this.personList.every((element) => {
                return element.age > 5;
            });
        }

        //#region Angular Module Definition
        private static _module: ng.IModule;

        /**
         * Stellt das aktuelle Angular Modul für den "es6Features" bereit.
         */
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }

            //Hier die abhängigen Module für diesen controller definieren, damit brauchen wir von außen nur den Controller einbinden
            //und müssen seine Abhängkeiten nicht wissen.
            this._module = angular.module('es6FeaturesCtrl', []);
            this._module.controller('es6FeaturesCtrl', Es6FeaturesCtrl);
            return this._module;
        }
        //#endregion
    }
} 