angular.module("table.fixedHeader.directives", ["helperFunctionFactory"])
    .directive("sxpFixedHeader", [
        "helperFunctionFactory", "$interval", function (helperFunctionFactory, $interval) {
            return {
                restrict: 'A',
                transclude: true,
                replace: false,
                controllerAs: 'fixedHeaderCtrl',
                bindToController: true,
                scope: {
                    listEntries: "="
                },
                templateUrl: 'ScriptsApp/directives/templates/table.fixedHeader.directives.sxpFixedHeader.html',
                controller: function ($element, $scope) {
                    //TODO Prüfen
                    //- Header Events noch gehen wegen Clone
                    //- Resize des Browsers die Zahlen neu berechnen für die Breite der Spalten
                    //- Table Selector auf First Element found eingrenzen

                    var that = this;
                    var wasAdded = false;

                    $scope.$watch(angular.bind(this, function (name) {return this.listEntries;}), function (newVal, oldVal) {
                        if (newVal !== undefined) {
                            var stop = $interval(function () {
                                //Warten bis die Daten der Tabelle alle geladen sind.
                               if ($element.find(".fixed-content-div table tbody tr").length === that.listEntries.length && !wasAdded) {
                                   that.TableWidth = $element.find("table").width() + helperFunctionFactory.getScrollbarWidth();
                                   that.HeaderHeight = jQuery($element.find("thead")[0]).height();
                                   that.CompleteHeight = "400px";
                                   that.ContentHeight = "270px";
                                   var originalTable = $element.find("table").clone();
                                   //1. Die Originaltabelle Clonen für die Headeranzeige.
                                   var headerTable = jQuery(originalTable.clone());
                                   //In der Headertabelle noch ein zusätzliches Th einfügen für den Scrollbalken.
                                   var headertableTempTh = jQuery("<th>").css({ 'margin': 0, 'padding': 0, 'border': 0, 'width': helperFunctionFactory.getScrollbarWidth() });
                                   headerTable.find("thead tr").append(headertableTempTh);
                                   $element.find(".fixed-header-div").append(headerTable);
                                   wasAdded = true;
                                   $interval.cancel(stop);
                                   stop = undefined;
                                }
                            }, 25);
                        };
                    });
                }
            }
        }]);







