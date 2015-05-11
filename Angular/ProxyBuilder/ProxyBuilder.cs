using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcTypeScript.ProxyCreator.Container;
using MvcTypeScript.ProxyCreator.Interfaces;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    /// <summary>
    /// Erstellen eines Proxies für unsere Controller Funktionen die mit einem passenden Attribut "CreateProxy" versehen sind.
    /// </summary>
    public class ProxyBuilder
    {
        #region Member
        /// <summary>
        /// Kümmert sich um das Speichern unserer Proxy Daten.
        /// </summary>
        private IProxyWriter ProxyWriter { get; set; }

        /// <summary>
        /// Parser zum Ermitteln der Methodeninformationen mit denen der Proxy erstellt werden kann
        /// </summary>
        private IMethodTypeInformationParser MethodTypeInformationParser { get; set; }

        private IBuilderFor BuilderFor { get; set; }
        #endregion

        #region Konstruktor

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="builderFor">Der Passende Builder für z.B. einen AngularJs Proxy oder jQueryTs Proxy</param>
        /// <param name="proxyWriter">Der ProxyWriter, welcher sich um das Speichern unserer JavaScript Dateien kümmert.</param>
        /// <param name="methodTypeInformationParser">Enthält alle Methodeninformationen für den aktuellen Controller</param>
        public ProxyBuilder(IBuilderFor builderFor, IProxyWriter proxyWriter, IMethodTypeInformationParser methodTypeInformationParser)
        {
            if (proxyWriter == null)
            {
                throw new NullReferenceException("Der Proxywriter ist Null");
            }

            if (methodTypeInformationParser == null)
            {
                throw new NullReferenceException("Der MethodTypeInformationParser ist Null");
            }
            
            if (builderFor == null)
            {
                throw new NullReferenceException("Der BuilderFor ist Null");
            }

            ProxyWriter = proxyWriter;
            MethodTypeInformationParser = methodTypeInformationParser;
            BuilderFor = builderFor;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Erstellen der passenden Proxy Klassen für die Controller
        /// </summary>
        /// <param name="controller">Liste mit Controllern für die Proxy Klassen erstellt werden sollen</param>
        public void StartBuildProcess(List<Type> controller)
        {
            //Alle übergebene Typen durchgehen
            foreach (Type type in controller)
            {
                //Prüfen ob der Controller direkt von Controller ableitet oder eine andere Basisklasse hat, die von Controller ableitet.
                if (type.IsSubclassOf(typeof(Controller)) || type == typeof(Controller))
                {
                    //Alle Methoden ermitteln für die ein Proxy erstellt werden soll.
                    ControllerTypeInformations controllerInfo = MethodTypeInformationParser.ParseMethodTypeInformations(type);
                    //Für den jeweiligen Controller dann den JavaScriptProxy erstellen.
                    BuilderFor.BuildProxyFile(controllerInfo, ProxyWriter);
                }
            }
        }

        /// <summary>
        /// Erstellen der passenden Proxy Klassen. Es werden automatisch alle Controller der
        /// Webanwendung herausgesucht und für die jeweiligen Funktionen mit dem passenden 
        /// Attribut werden die Proxy Funktionen erstellt.
        /// </summary>
        public void StartBuildProcess()
        {
            List<Type> controller = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)))
                .ToList();

            StartBuildProcess(controller);
        }
        #endregion
    }
}
