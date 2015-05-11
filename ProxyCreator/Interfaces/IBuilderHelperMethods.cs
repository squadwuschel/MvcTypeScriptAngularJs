using System;
using System.Reflection;
using MvcTypeScript.ProxyCreator.Container;

namespace MvcTypeScript.ProxyCreator.Interfaces
{
    public interface IBuilderHelperMethods
    {
        /// <summary>
        /// Pr�fen ob die �bergebnene Methode das �bergenene Attribut besitzt.
        /// </summary>
        /// <param name="attribute">Der Typ des Attributs der �berpr�ft werden soll</param>
        /// <param name="method">Die Methode bei der das Attribut gesucht werden soll</param>
        bool HasAttribute(Type attribute, MethodInfo method);

        /// <summary>
        /// Ermittelt den Namen des JavaScript Modules (Service)
        /// </summary>
        string GetJavaScriptModuleName(Type controller);

        /// <summary>
        /// Ermittelt den Namen des Controller ohne die Endung "Controller",
        /// da wir diese nie mit angeben m�ssen.
        /// </summary>
        string GetClearControllerName(Type controller);

        /// <summary>
        /// Den Ersten Buchstaben der Zeichenfolge toLower umwandeln.
        /// </summary>
        string LowerFirstCharName(string lowerFirstChar);

        /// <summary>
        /// Sucht einfach nur die Parameternamen der aktuell �bergebenen Methode heraus und baut einen einfachen
        /// Kommaseperierten String mit den Parameternamen zusammen.
        /// </summary>
        string GetFunctionParameters(MethodInfo methodInfo);

        /// <summary>
        /// Sucht einfach nur die Parameternamen der aktuell �bergebenen Methode heraus und setzt noch den passenden Typ 
        /// f�r TypeScript hinter den Namen z.B.: "alter: number, name: string, ..."
        /// </summary>
        string GetFunctionParametersWithType(MethodInfo methodInfo);

        /// <summary>
        /// Anpassen eines Namespaces inkl. Type in Namespace und Interface z.B.:
        /// Aus: MyWebapp.Type.Address => MyWebapp.Type.IAddress
        /// 
        /// Wird ben�tigt f�r TypeScript Implementierungen.
        /// </summary>
        string AddInterfacePrefixToFullName(string fullNameWithNamespace);

        /// <summary>
        /// Den passenden HttpCall zusammenbauen und pr�fen ob Post oder Get verwendet werden soll
        /// Erstellt wird: post("/Home/LoadAll", data) oder get("/Home/LoadAll?userId=" + id)
        /// </summary>
        string BuildHttpCall(MethodTypeInformations methodInfo, bool hasSiteRootDefinition);
    }
}