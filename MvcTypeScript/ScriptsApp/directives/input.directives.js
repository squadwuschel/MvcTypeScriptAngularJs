angular.module('input.directives', [])
/*
Erlaubt nur das Eingeben von Strings ohne Leerzeichen und nur Valide Zeichen ohne Umlaute und sonstiges
mit ng-trim wird auch bei der Eingabe eines Leerzeichens die Direktive aufgerufen, was sonst nicht geschieht.

<input type="text" ng-trim="false" sxp-no-space>

*/
    .directive('sxpNoSpace', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (value) {
                    //Ist notwendig für 'ng-required' 
                    if (value === undefined) return '';

                    var transformedInput = value.replace(' ', '');
                    if (transformedInput != value) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }
                    return transformedInput;
                });
            }
        };
        /*
        Erlaubt nur das Eingeben von Strings ohne Leerzeichen und nur Valide Zeichen ohne Umlaute und sonstiges
        mit ng-trim wird auch bei der Eingabe eines Leerzeichens die Direktive aufgerufen, was sonst nicht geschieht.

        <input type="text" ng-trim="false" sxp-camel-case>

        */
    }).directive('sxpCamelCase', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (value) {
                    //Ist notwendig für 'ng-required' 
                    if (value === undefined) return '';

                    //findet ein Zeichen, das nicht alphanumerisch und auch kein Unterstrich ist
                    //(typisch zum Suchen nach illegalen Zeichen bei programmiersprachengerechten selbstvergebenen Namen).
                    var transformedInput = value.replace(/\W/g, '');
                    //Auch die leerzeichen entfernen
                    transformedInput = transformedInput.replace(' ', '');
                    if (transformedInput != value) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }
                    return transformedInput;
                });
            }
        };
    })
/*
Führt die übergebene Funktion aus wenn "ENTER" gedrückt wird
Beispiel

<input type="button" ng-click="save()" sxp-enter="save()" sxp-dont-unbind value="speichern" />

*/
    .directive('sxpEnter', ["$document", function ($document) {
        return {
            scope: {
                sxpEnter: "&",
                sxpDontUnbind: "=" //Wenn das Attribut vorhanden ist, dann wird das Event nicht wieder entfernt.
                //Das Entfernen des Events ist nötig, wenn die Elemente immer wieder neu erstellt werden 
                //z.B. bei einem Modal muss das event wieder entfernt werden. - OPTIONAL
            },
            link: function (scope, element, attrs) {
                var enterWatcher = function (event) {
                    if (event.which === 13) {
                        scope.sxpEnter();
                        scope.$apply();
                        event.preventDefault();

                        //Wenn das Attribut vorhanden ist, dann nicht das Event entfernen
                        if (attrs.sxpDontUnbind === undefined || attrs.sxpDontUnbind === null) {
                            $document.unbind("keydown keypress", enterWatcher);
                        }
                    }
                };
                $document.bind("keydown keypress", enterWatcher);
            }
        }
    } ])
/*
Setzt den Focus automatisch auf das Element in dem sxpFocus definiert wurde, das Timeout ist notwendig, da der focus sonst nicht gesetzt wird.
Beispiel:

<input type="text" sxp-focus ng-Model="ViewModel.Name">
*/
    .directive('sxpFocus', ["$timeout", function ($timeout) {
        return {
            link: function (scope, element, attrs, model) {
                $timeout(function () {
                    element[0].focus();
                }, 400);
            }
        };
    } ]);
