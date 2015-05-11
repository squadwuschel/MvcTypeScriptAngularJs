using System;
using System.Linq;
using System.Text;
using MvcTypeScript.ProxyCreator.Container;
using MvcTypeScript.ProxyCreator.Interfaces;
using MvcTypeScript.ProxyCreator.Templates;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    /// <summary>
    /// TypeScript Proxy Erstellen für AngularJs
    /// 
    /// TODO:
    /// - Wenn als ReturnValue ein Interface übergeben wird eine Exception Werfen das hier immer der Typ angegeben werden muss
    /// 
    /// </summary>
    public class BuilderForAngularTs : IBuilderFor
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
        public BuilderForAngularTs(IBuilderHelperMethods buildHelper)
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
            string controllerName = BuildHelper.GetClearControllerName(controllerInfo.Controller).Trim() + "PService";

            //Wenn angegeben, den ersten Buchstaben der funktion kleinschreiben 
            if (LowerFirstCharInFunctionName)
            {
                javaScriptSrvName = BuildHelper.LowerFirstCharName(javaScriptSrvName);
            }

            StringBuilder builder = new StringBuilder();
            //Beschriftung einfügen das es sich um ein automatisch generiertes Dokument handelt.
            builder.Append(string.Format(AngularJsProxyTsTemplates.AutomaticlyCreated, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), Environment.UserName));
            builder.Append(Environment.NewLine).Append(Environment.NewLine);
            
            //Unser GesamtTemplate entsprechend ausfüllen. Beschreibung der Templatewerte:
            //{0}  --> Der Name der Klasse
            //{1}  --> Auflistung der Interface Funktionen für unsere Klasse
            //{2} -->  Auflistung der Service Funktionen die über das Interface eingebunden wurden
            //{3} --> der Name des Service Moduls nach "außen"

            string fct = string.Format(AngularJsProxyTsTemplates.ModuleTemplate,
                controllerName,
                BuildInterfaceDifinitions(controllerInfo),
                BuildfunctionDefinitions(controllerInfo),
                javaScriptSrvName);

            builder.Append(fct);
            builder.Append(Environment.NewLine).Append(Environment.NewLine);

            string proxy = builder.ToString();
            //Die Proxy Datei ins Dateisystem schreiben *.ts -> TypeScript
            proxyWriter.SaveProxyContent(proxy, string.Format(@"{0}.ts", javaScriptSrvName));
        }

        /// <summary>
        /// Die Definition für unsere Interface Übersicht zusammenbauen.
        /// </summary>
        private string BuildInterfaceDifinitions(ControllerTypeInformations controllerInfo)
        {
            StringBuilder interfaceDefinitions = new StringBuilder();

            //die Interface definition für unsere Klasse zusammenbauen.
            //Template:
            // {0}({1}) : ng.IPromise<{2}>;
            //Beschreibung der Templatewerte
            //{0} --> Der Name der Servicefuntkion
            //{1} --> Die Parameterliste und Typen der ServiceFunktion
            //{2} --> ReturnValue unseres Interfaces

            foreach (MethodTypeInformations info in controllerInfo.MethodTypeInformations)
            {
                string methodName = info.MethodName;
                //Wenn angegeben, den ersten Buchstaben der funktion kleinschreiben 
                if (LowerFirstCharInFunctionName)
                {
                    methodName = BuildHelper.LowerFirstCharName(methodName);
                }

                //Prüfen ob es einen Rückgabewert gibt und dann das Interface entsprechend zusammenbauen.
                if (info.ReturnType == null)
                {
                    interfaceDefinitions.Append(String.Format("{0}({1}): void;", methodName, BuildHelper.GetFunctionParametersWithType(info.MethodInfo))); 
                }
                else
                {
                    interfaceDefinitions.Append(String.Format("{0}({1}) : ng.IPromise<{2}>;", methodName,
                                                BuildHelper.GetFunctionParametersWithType(info.MethodInfo),
                                                BuildHelper.AddInterfacePrefixToFullName(info.ReturnType.FullName))); 
                }

                interfaceDefinitions.Append(Environment.NewLine);
            }

            return interfaceDefinitions.ToString();
        }

        /// <summary>
        /// Die Passenden Funktionsdefinitionen für unseren TypeScript Service zusammenbauen.
        /// </summary>
        private string BuildfunctionDefinitions(ControllerTypeInformations controllerInfo)
        {
            StringBuilder functionDefionition = new StringBuilder();

            //Das folgende Template mit Daten füllen:
            //{0}({1}) : ng.IPromise<{2}> {{
            //         return this.$http.{3}.then((response: ng.IHttpPromiseCallbackArg<{2}>) : {2} 
	        //          => {{ return response.data; }} );
            //}}
            //Template Variablen:
            //{0} --> Der Name der Service Funktion
            //{1} --> Die Parameter der Service Funktion
            //{2} --> Der RückgabeTyp der Funktion
            //{3} --> Der Get bzw. Post Call an unseren Service

            //Alle Methoden durchgehen und die entsprechenden Parameter ermitteln und den passenden
            //POST bzw. GET Aufruf zusammenbauen als Prototypefunktion für unsere Funktion.
            foreach (MethodTypeInformations info in controllerInfo.MethodTypeInformations)
            {
                string methodName = info.MethodName;
                //Wenn angegeben, den ersten Buchstaben der funktion kleinschreiben 
                if (LowerFirstCharInFunctionName)
                {
                    methodName = BuildHelper.LowerFirstCharName(methodName);
                }

                string functionParameters = BuildHelper.GetFunctionParametersWithType(info.MethodInfo);
                string methodCall = BuildHelper.BuildHttpCall(info, HasSiteRootDefinition);

                //Prüfen ob der HTTP Call auch einen Rückgabewert erwartet.
                if (info.ReturnType != null)
                {
                    //Unseren jeweiligen Methodenaufruf zusammenbauen ob POST oder GET
                    string fctCall = string.Format(AngularJsProxyTsTemplates.FunctionDefnitionWithReturnType,
                        methodName,
                        functionParameters,
                        BuildHelper.AddInterfacePrefixToFullName(info.ReturnType.FullName),
                        methodCall);
                    functionDefionition.Append(fctCall).Append(Environment.NewLine);
                }
                else
                {
                    //Es gibt keinen Rückgabewert, sondern nur ein Einfacher Call der eine Funktion auslösen soll.
                    string fctCall = string.Format(AngularJsProxyTsTemplates.FunctionDefnitionNoReturnType,
                       methodName,
                       functionParameters,
                       methodCall);
                    functionDefionition.Append(fctCall).Append(Environment.NewLine);
                }

                functionDefionition.Append(Environment.NewLine);
            }

            return functionDefionition.ToString();
        }
        #endregion
    }
}
