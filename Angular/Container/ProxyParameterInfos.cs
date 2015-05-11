using System.Reflection;

namespace MvcTypeScript.ProxyCreator.Container
{
    public class ProxyParameterInfos
    {
        public ParameterInfo ParameterInfo { get; set; }
        public string Name { get; set; }
        
        /// <summary>
        /// Gibt an ob es sich um einen "Komplexen" typ handelt -
        /// also nicht um einen "einfachen" Datentyp wie "String, int, bool, ..." sondern
        /// um eine eigene Personen Klasse z.B. die später per Post übergeben werden muss.
        /// </summary>
        public bool IsComplexeType { get; set; }

        /// <summary>
        /// gibt an ob es sich um einen String handelt, denn dieser
        /// muss später UrlEncoded werden.
        /// </summary>
        public bool IsString { get; set; }
    }
}
