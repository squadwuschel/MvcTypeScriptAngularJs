/*
Direktive zum einfachen Anzeigen eines Bestätigen Modals.
ACHTUNG: ui.bootstrap wird hier zwingend benötigt!

Verwendung als Attribut:
<a href="" class="btn btn-default"
   sxp-modal-apply-or-cancel 
   sxp-modal-apply-fct="ctrl.ApplyDelete()"
   sxp-modal-titel="'Einrag löschen!'"
   sxp-modal-text="'Wollen Sie den Eintrag wirklich löschen?'"
   sxp-modal-size="'lg'">
<i class="fa fa-trash-o"></i></a>
*/
angular.module('deafaultModal.directives', ['ui.bootstrap', 'input.directives'])
  .directive('sxpModalApplyOrCancel', ['$modal', function ($modal) {
        //Unser Modal initialisieren
        var modalInstanceCtrl = ["$scope", "$modalInstance", "title", "message", function ($scope, $modalInstance, title, message) {
            $scope.Locals = {};
            $scope.Locals.title = title;
            //Es kann auch ein Array aus Strings übergeben werden, wenn es sich um eine abfrage handelt die Mehrere Einträge z.b. löschen soll.
            $scope.Locals.IsMessageArray = angular.isArray(message);
            $scope.Locals.message = message;

            $scope.ok = function () {
                $modalInstance.close();
            };
            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        return {
            restrict: 'A',
            scope: {
                sxpModalApplyFct: "&",     //CallBackFunktion die aufgerufen wird, wenn OK Geklickt wurde. - PFLICHT
                sxpModalTitel: "=",     //Der Titel des Modals - Optional
                sxpModalText: "=",      //Der Anzeigetext des Modals - Optional | ACHTUNG  hier kann ein String oder auch ein Array aus Strings übergeben werden!
                sxpModalSize: "="       //Die Größe des Modals "lg", "sm" oder nichts für Default Size. - Optional
            },
            link: function (scope, element, attrs) {
                element.bind('click', function () {
                    var modalTitle = scope.sxpModalTitel,
                        modalText = scope.sxpModalText,
                        modalSize = scope.sxpModalSize;

                    //Prüfen ob ein Titel übergeben wurde.
                    if (scope.sxpModalTitel === undefined || scope.sxpModalTitel === null || scope.sxpModalTitel === "") {
                        modalTitle = "Eintrag löschen";
                    }

                    //Prüfen ob ein Anzeigetext übergeben wurde.
                    if (scope.sxpModalText === undefined || scope.sxpModalText === null || scope.sxpModalText === "") {
                        modalText = "Wollen Sie den Eintrag wirklich löschen?";
                    }

                    if (scope.sxpModalSize === undefined || scope.sxpModalSize === null || scope.sxpModalSize === "") {
                        modalSize = "";
                    }

                    var modalInstance = $modal.open({
                        templateUrl: 'ScriptsApp/directives/templates/defaultModal.directives.ApplyOrCancel.html',
                        controller: modalInstanceCtrl,
                        size: modalSize,
                         resolve: {
                             title: function () {
                                 return modalTitle;
                            },
                            message: function () {
                                return modalText;
                            }
                        }
                    });

                    //Den Klick im Modal auswerten
                    modalInstance.result.then(function () {
                        scope.sxpModalApplyFct();
                    }, function () {
                        //Modal dismissed
                    });
                });
            }
        }
    }
  ]);