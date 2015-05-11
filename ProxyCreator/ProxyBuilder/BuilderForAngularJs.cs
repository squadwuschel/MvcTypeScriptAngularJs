using System;
using System.Linq;
using System.Text;
using MvcTypeScript.ProxyCreator.Container;
using MvcTypeScript.ProxyCreator.Interfaces;
using MvcTypeScript.ProxyCreator.Templates;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    /// <summary>
    /// JavaScript Proxy erstellen für AngularJs
    /// </summary>
    public class BuilderForAngularJs : IBuilderFor
    {
        #region Member
        /// <summary>
        /// Gibt an ob für die BasisUrl eine Variable auf Oberster window Ebene von JavaScript definiert wurde
        /// mit dem Namen "siteRoot" - hier steht die komplette Url inkl. Virtuellem Directory.
        /// Wird nur benötigt, wenn die Seite in einer Umgebung gehostet wird, in der es ein Virtuelles Dir gibt,
        /// denn sonst werden die Ajax Calls nicht die richtige Url finden und schlagen fehl. Zum Ermitteln der aktuellen
        /// Url gibt es in C# die Funktion "UrlHelper.CurrentUrlWithVirtualDirectory()" (in diesem Assembly)
        /// </summary>
        public bool HasSiteRootDefinition { get; set; }

        /// <summary>
        /// Den Ersten Buchstaben im Funktionsnamen kleinschreiben aus "InitViewModel" wird in JS "initViewModel"
        /// </summary>
        public bool LowerFirstCharInFunctionName { get; set; }

        /// <summary>
        /// Hilfsmethoden zum Erstellen der passenden Proxyklasse. z.B. UrlHelper
        /// </summary>
        private IBuilderHelperMethods BuildHelper { get; set; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Initialize
        /// </summary>
        public BuilderForAngularJs(IBuilderHelperMethods buildHelper)
        {
            if (buildHelper == null)
            {
                throw new NullReferenceException("Die BuildHelperMethods sind null.");
            }

            HasSiteRootDefinition = false;
            LowerFirstCharInFunctionName = false;
            BuildHelper = buildHelper;
        }
        #endregion

        #region Public Functions
        public void BuildProxyFile(ControllerTypeInformations informations, IProxyWriter proxyWriter)
        {
            BuildJavaScriptProxyFiles(informations, proxyWriter);
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Zusammenbauen/erstellen des JavaScripProxies
        /// </summary>
        private void BuildJavaScriptProxyFiles(ControllerTypeInformations controllerInfo, IProxyWriter proxyWriter)
        {
            //Wenn keine Methoden übergeben wurden, kann auch keine Proxyklasse erstellt werden
            if (!controllerInfo.MethodTypeInformations.Any())
            {
                return;
            }

            //Den Funktionsnamen/Modulnamen für unser Modul ermitteln, z.B. "HomePSrv"
            string javaScriptSrvName = BuildHelper.GetJavaScriptModuleName(controllerInfo.Controller);

            //Wenn angegeben, den ersten Buchstaben der funktion kleinschreiben 
            if (LowerFirstCharInFunctionName)
            {
                javaScriptSrvName = BuildHelper.LowerFirstCharName(javaScriptSrvName);
            }

            StringBuilder builder = new StringBuilder();
            //Beschriftung einfügen das es sich um ein automatisch generiertes Dokument handelt.
            builder.Append(string.Format(AngularProxyJsTemplates.AutomaticlyCreated, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), Environment.UserName));
            builder.Append(Environment.NewLine).Append(Environment.NewLine);

            //unsere Funktionsdefinition zusammenbauen und einfüge, z.B. "function HomePSrv($http, $log) { ..."
            builder.Append(string.Format(AngularProxyJsTemplates.ServerFunctionDefinition, javaScriptSrvName));
            builder.Append(Environment.NewLine).Append(Environment.NewLine);

            //Alle Methoden durchgehen und die entsprechenden Parameter ermitteln und den passenden
            //POST bzw. GET Aufruf zusammenbauen als Prototypefunktion für unsere Funktion.
            foreach (MethodTypeInformations info in controllerInfo.MethodTypeInformations)
            {
                string methodName = info.MethodInfo.Name;

                //Wenn angegeben, den ersten Buchstaben der funktion kleinschreiben 
                if (LowerFirstCharInFunctionName)
                {
                    methodName = BuildHelper.LowerFirstCharName(methodName);
                }

                string functionParameters = BuildHelper.GetFunctionParameters(info.MethodInfo);
                string methodCall = BuildHelper.BuildHttpCall(info, HasSiteRootDefinition);

                //Unseren jeweiligen Methodenaufruf zusammenbauen ob POST oder GET
                string fctCall = string.Format(AngularProxyJsTemplates.BasicSuccessServerCall,
                    javaScriptSrvName, methodName, functionParameters, methodCall);
                builder.Append(fctCall).Append(Environment.NewLine).Append(Environment.NewLine);
            }

            //Unsere ModulDefinition hinzufügen, z.B. 'angular.module("app.HomePSrv", []).service("HomePSrv", HomePSrv);'
            builder.Append(string.Format(AngularProxyJsTemplates.ModuleDefinition, javaScriptSrvName));

            string proxy = builder.ToString();
            //Die Proxy Dateien ins Dateisystem schreiben
            proxyWriter.SaveProxyContent(proxy, string.Format(@"{0}.js", javaScriptSrvName));
        }
        #endregion
    }
}
