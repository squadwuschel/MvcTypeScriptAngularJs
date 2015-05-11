angular.module("helperFunctionFactory", [])
    .factory("helperFunctionFactory", function () {
        var scrollbarWidth = undefined;

        return {
            //Die Breite der Scrollbar des aktuellen Browsers ermitteln.
            //http://stackoverflow.com/questions/13382516/getting-scroll-bar-width-using-javascript
            getScrollbarWidth: function () {
                if (scrollbarWidth === undefined) {
                    var outer = document.createElement("div");
                    outer.style.visibility = "hidden";
                    outer.style.width = "100px";
                    outer.style.msOverflowStyle = "scrollbar"; // needed for WinJS apps

                    document.body.appendChild(outer);

                    var widthNoScroll = outer.offsetWidth;
                    // force scrollbars
                    outer.style.overflow = "scroll";

                    // add innerdiv
                    var inner = document.createElement("div");
                    inner.style.width = "100%";
                    outer.appendChild(inner);

                    var widthWithScroll = inner.offsetWidth;

                    // remove divs
                    outer.parentNode.removeChild(outer);
                    scrollbarWidth = widthNoScroll - widthWithScroll;
                }

                return scrollbarWidth +1 ;
            }
        }
    })