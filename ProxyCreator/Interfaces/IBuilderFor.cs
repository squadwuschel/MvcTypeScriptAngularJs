using MvcTypeScript.ProxyCreator.Container;

namespace MvcTypeScript.ProxyCreator.Interfaces
{
    public interface IBuilderFor
    {
        /// <summary>
        /// Gibt an ob für die BasisUrl eine Variable auf Oberster window Ebene von JavaScript definiert wurde
        /// mit dem Namen "siteRoot" - hier steht die komplette Url inkl. Virtuellem Directory.
        /// Wird nur benötigt, wenn die Seite in einer Umgebung gehostet wird, in der es ein Virtuelles Dir gibt,
        /// denn sonst werden die Ajax Calls nicht die richtige Url finden und schlagen fehl. Zum Ermitteln der aktuellen
        /// Url gibt es in C# die Funktion "UrlHelper.CurrentUrlWithVirtualDirectory()" (in diesem Assembly)
        /// </summary>
        bool HasSiteRootDefinition { get; set; }

        /// <summary>
        /// Den Ersten Buchstaben im Funktionsnamen kleinschreiben aus "InitViewModel" wird in JS "initViewModel"
        /// </summary>
        bool LowerFirstCharInFunctionName { get; set; }

        /// <summary>
        /// Erstellen des Passenden Proxyfiles.
        /// </summary>
        void BuildProxyFile(ControllerTypeInformations informations, IProxyWriter proxyWriter);
    }
}