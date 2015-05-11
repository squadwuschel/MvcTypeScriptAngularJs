using System;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    /// <summary>
    /// Dieses Attribut dient aktuell nur dem Markieren von Funktionen die in 
    /// eine JavaScript Proxy Funktion umgewandelt werden sollen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CreateProxyAttribute : Attribute
    {
        public Type ReturnType { get; set; }

        public CreateProxyAttribute()
        {
            ReturnType = null;
        }

        public CreateProxyAttribute(Type returnType)
        {
            ReturnType = returnType;
        }
    }
}
