using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using MvcTypeScript.ProxyCreator.Container;
using MvcTypeScript.ProxyCreator.Interfaces;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    public class BuilderHelperMethods : IBuilderHelperMethods
    {
        /// <summary>
        /// Prüfen ob die übergebnene Methode das übergenene Attribut besitzt.
        /// </summary>
        /// <param name="attribute">Der Typ des Attributs der überprüft werden soll</param>
        /// <param name="method">Die Methode bei der das Attribut gesucht werden soll</param>
        public bool HasAttribute(Type attribute, MethodInfo method)
        {
            var found = from attr in method.GetCustomAttributes(true) where attr.GetType() == attribute select attr;
            return found.Any();
        }

        /// <summary>
        /// Ermittelt den Namen des JavaScript Modules (Service)
        /// </summary>
        public string GetJavaScriptModuleName(Type controller)
        {
            string name = GetClearControllerName(controller);
            //Den Ersten Buchstaben klein schreiben!
            return string.Format("{0}PSrv", Char.ToLowerInvariant(name[0]) + name.Substring(1));
        }

        /// <summary>
        /// Ermittelt den Namen des Controller ohne die Endung "Controller",
        /// da wir diese nie mit angeben müssen.
        /// </summary>
        public string GetClearControllerName(Type controller)
        {
            string name = controller.Name;
            return name.Substring(0, name.LastIndexOf("Controller"));
        }

        /// <summary>
        /// Den Ersten Buchstaben der Zeichenfolge toLower umwandeln.
        /// </summary>
        public string LowerFirstCharName(string lowerFirstChar)
        {
            if (!string.IsNullOrEmpty(lowerFirstChar))
            {
                char[] toLower = lowerFirstChar.ToCharArray();
                toLower[0] = char.ToLower(toLower[0]);
                lowerFirstChar = new string(toLower);
            }
            return lowerFirstChar;
        }

        /// <summary>
        /// Sucht einfach nur die Parameternamen der aktuell übergebenen Methode heraus und baut einen einfachen
        /// Kommaseperierten String mit den Parameternamen zusammen.
        /// </summary>
        public string GetFunctionParameters(MethodInfo methodInfo)
        {
            StringBuilder builder = new StringBuilder();
            //Zusammenbauen der PrameterInfos für die übergebene Methode.
            foreach (ParameterInfo info in methodInfo.GetParameters())
            {
                builder.Append(info.Name).Append(",");
            }

            return builder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Anpassen eines Namespaces inkl. Type in Namespace und Interface z.B.:
        /// Aus: MyWebapp.Type.Address => MyWebapp.Type.IAddress
        /// 
        /// Wird benötigt für TypeScript Implementierungen.
        /// </summary>
        public string AddInterfacePrefixToFullName(string fullNameWithNamespace)
        {
            if (!string.IsNullOrEmpty(fullNameWithNamespace))
            {
                var entries = fullNameWithNamespace.Split('.');
                var oldStr = entries[entries.Length - 1];
                var newStr = "I" + entries[entries.Length - 1];
                return fullNameWithNamespace.Remove(fullNameWithNamespace.LastIndexOf(oldStr), oldStr.Length) + newStr;
            }

            return fullNameWithNamespace;
        }

        /// <summary>
        /// Sucht einfach nur die Parameternamen der aktuell übergebenen Methode heraus und setzt noch den passenden Typ 
        /// für TypeScript hinter den Namen z.B.: "alter: number, name: string, ..."
        /// </summary>
        public string GetFunctionParametersWithType(MethodInfo methodInfo)
        {
            StringBuilder builder = new StringBuilder();
            //Zusammenbauen der PrameterInfos für die übergebene Methode.
            foreach (ParameterInfo info in methodInfo.GetParameters())
            {
                //Die Parameterliste inkl. des Typen zurückgeben
                builder.Append(string.Format("{0}: {1}", info.Name, GetTsType(info))).Append(",");
            }

            return builder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Den passenden HttpCall zusammenbauen und prüfen ob Post oder Get verwendet werden soll
        /// Erstellt wird: post("/Home/LoadAll", data) oder get("/Home/LoadAll?userId=" + id)
        /// </summary>
        public string BuildHttpCall(MethodTypeInformations methodInfo, bool hasSiteRootDefinition)
        {
            //Wie genau das Post oder Geht aussieht, hängt von den gewünschten Parametern ab.
            //Aktuell gehen wir von einer StandardRoute aus und wenn ein "id" in den Parametern ist, dann
            //Handelt es sich z.B. um den Letzten Parameter in der StandardRoute.
            //Beispiele:
            //.post("/Home/GetAutosByHerstellerId/" + id, message)
            //.post("/ControllerName/GetAutosByHerstellerId/", message)
            //get("/Home/GetSuccessMessage")
            //get("/Home/GetSuccessMessage/" + id + "?name=" + urlencodestring + "&test=2")

            //Prüfen ob ein complexer Typ verwendet wird.
            if (methodInfo.ProxyParameterInfos.Count(p => p.IsComplexeType) == 0)
            {
                //Wenn über der Controller Action Post angegeben wurde, dann auch Post verwenden
                //obwohl kein komplexer Typ enthalten ist.
                if (HasAttribute(typeof(HttpPostAttribute), methodInfo.MethodInfo))
                {
                    return BuildPost(methodInfo, hasSiteRootDefinition);
                }

                //Kein Komplexer Typ also Get verwenden.
                return BuildGet(methodInfo, hasSiteRootDefinition);
            }

            return BuildPost(methodInfo, hasSiteRootDefinition);
        }

        #region Private Functions
        /// <summary>
        /// Für den übergebenen ParameterInfoWert den passenden "TypeScript" Typen ermitteln.
        /// </summary>
        private string GetTsType(ParameterInfo info)
        {
            //Man muss die Standard Systemtypen prüfen und den Wert zurückgeben der von TypScript entsprechend unterstützt wird.
            if (info.ParameterType == typeof(string))
            {
                return "string";
            }

            if (info.ParameterType == typeof(int) || info.ParameterType == typeof(int?) || info.ParameterType == typeof(Int16) ||
                info.ParameterType == typeof(Int16?) || info.ParameterType == typeof(Int32) || info.ParameterType == typeof(Int32?) ||
                info.ParameterType == typeof(Int64) || info.ParameterType == typeof(Int64?) || info.ParameterType == typeof(decimal) ||
                info.ParameterType == typeof(decimal?) || info.ParameterType == typeof(double) || info.ParameterType == typeof(double?))
            {
                return "number";
            }

            if (info.ParameterType == typeof(DateTime) || info.ParameterType == typeof(DateTime?))
            {
                return "Date";
            }

            if (info.ParameterType == typeof(Boolean) || info.ParameterType == typeof(Boolean?))
            {
                return "boolean";
            }

            //Bei eigenen Typen muss der Namespace noch mit angegeben werden zum Namen.
            return AddInterfacePrefixToFullName(info.ParameterType.FullName);
        }
        #endregion

        #region HTTPCall
           /// <summary>
        /// Rückgabe für Post erstellen
        /// </summary>
        /// <returns>Gibt den passenden POST Aufruf in JavaScript zurück</returns>
        private string BuildPost(MethodTypeInformations infos, bool hasSiteRootDefinition)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("post('{0}/{1}'", GetClearControllerName(infos.Controller), infos.MethodInfo.Name));

            if (hasSiteRootDefinition)
            {
                builder = new StringBuilder();
                //Die Variable 'siteRoot' muss auf Window Ebene der aktuellen Anwendung definiert sein
                //und enthält die komplette Url inkl. "/" am ende!
                builder.Append(string.Format("post(siteRoot + '{0}/{1}'", GetClearControllerName(infos.Controller), infos.MethodInfo.Name));
            }

            builder.Append(BuildUrlParameterId(infos.ProxyParameterInfos));
            builder.Append(BuildUrlParameter(infos.ProxyParameterInfos));

            //Da auch ein Post ohne komplexen Typ aufgerufen werden kann über das "HttpPost" Attribut hier prüfen
            //ob ein komplexer Typ enthalten ist.
            if (infos.ProxyParameterInfos.Any(p => p.IsComplexeType))
            {
                builder.Append(string.Format(",{0})", infos.ProxyParameterInfos.First(p => p.IsComplexeType).Name));
            }
            else
            {
                builder.Append(")");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Rückgabe für Get erstellen
        /// </summary>
        /// <returns>Gibt den passenden GET Aufruf in JavaScript zurück</returns>
        private string BuildGet(MethodTypeInformations infos, bool hasSiteRootDefinition)
        {
            StringBuilder builder = new StringBuilder();
            //Keine Komplexen Typen, einfacher Get Aufruf.
            builder.Append(string.Format("get('{0}/{1}'", GetClearControllerName(infos.Controller), infos.MethodInfo.Name));

            if (hasSiteRootDefinition)
            {
                builder = new StringBuilder();
                //Die Variable 'siteRoot' muss auf Window Ebene der aktuellen Anwendung definiert sein
                //und enthält die komplette Url inkl. "/" am ende!
                builder.Append(string.Format("get(siteRoot + '{0}/{1}'", GetClearControllerName(infos.Controller), infos.MethodInfo.Name));
            }

            builder.Append(BuildUrlParameterId(infos.ProxyParameterInfos));
            builder.Append(BuildUrlParameter(infos.ProxyParameterInfos));
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Prüfen ob eine Id enthalten ist, diese wird extra an die URL angehängt.
        /// </summary>
        private string BuildUrlParameterId(List<ProxyParameterInfos> infos)
        {
            StringBuilder builder = new StringBuilder();
            //ACHTUNG der Wert mit dem Namen "id" wird direkt an die URL angehängt und nicht als Extra Parameter verwendet
            if (infos.Any(p => p.Name.ToLower() == "id"))
            {
                builder.Append(" + '/' + ").Append(infos.FirstOrDefault(p => p.Name.ToLower() == "id").Name);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Zusammenbauen der passenden URL Parameter ACHTUNG der UrlParameterName entspricht 
        /// auch dem gleichen Namen wie der Parameter der gesetzt wird.
        /// </summary>
        /// <param name="infos">List mit den Typen die als URL Parameter angelegt werden sollen.</param>
        private string BuildUrlParameter(List<ProxyParameterInfos> infos)
        {
            StringBuilder builder = new StringBuilder();
            var allowedInfos = infos.Where(p => !p.IsComplexeType && p.Name.ToLower() != "id");
            if (allowedInfos.Any())
            {
                builder.Append("+ '?");
            }

            bool isFirst = true;

            foreach (ProxyParameterInfos info in allowedInfos)
            {
                //Prüfen ob es sich um einen String handelt der übergeben werden soll,
                //wenn ja wird dieser Url Encoded damit z.B. auch "+" Zeichen übermittelt werden
                string paramValue = info.IsString ? string.Format("encodeURIComponent({0})", info.Name) : info.Name;

                if (isFirst)
                {
                    builder.Append(string.Format("{0}='+{1}", info.Name, paramValue));
                    isFirst = false;
                }
                else
                {
                    builder.Append(string.Format("+'&{0}='+{1}", info.Name, paramValue));
                }
            }

            return builder.ToString();
        }
        #endregion

    }
}
