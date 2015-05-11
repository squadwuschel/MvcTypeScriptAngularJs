using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcTypeScript.ProxyCreator.Container;
using MvcTypeScript.ProxyCreator.Interfaces;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    public class MethodTypeInformationParser : IMethodTypeInformationParser
    {
        public ControllerTypeInformations ParseMethodTypeInformations(Type controller)
        {
            ControllerTypeInformations controllerInfo = new ControllerTypeInformations();
            controllerInfo.Controller = controller;

            //Alle Methoden im Controller ermitteln über dem unser Attribut "AngularCreateProxy" gesetzt ist.
            //Für jede einzelne Methode müssen die passenden Werte für die MethodenInformationen ermittelt werden
            //mit denen dann später der Proxy gebaut wird.
            foreach (MethodInfo info in GetControllerMethodsWithCreateProxyAttribute(controller))
            {
                MethodTypeInformations methodTypeInformations = new MethodTypeInformations();
                methodTypeInformations.Controller = controller;
                //die Methodeninformationen ermitteln
                methodTypeInformations.MethodInfo = info;
                //Den aktuellen Methodennamen ermitteln
                methodTypeInformations.MethodName = info.Name;

                methodTypeInformations.Namespace = string.Empty;
                methodTypeInformations.MethodNameWithNamespace = string.Empty;
                if (info.DeclaringType != null)
                {
                    methodTypeInformations.Namespace = info.DeclaringType.FullName;
                    methodTypeInformations.MethodNameWithNamespace = string.Format("{0}.{1}", methodTypeInformations.Namespace, info.Name);
                }

                //Die Parameter für die aktuelle Methode ermitteln und die einzelnen Parameter auswerten.
                methodTypeInformations.ProxyParameterInfos = GetMethodParameterInfos(info);
                //In unserem Attribut kann man ebenfalls den ReturnType angeben - dieser wird hier ermittelt.
                methodTypeInformations.ReturnType = GetAngularCreateProxyReturnType(info);
                controllerInfo.MethodTypeInformations.Add(methodTypeInformations);
            }

            return controllerInfo;
        }

        /// <summary>
        /// Prüfen der Controllermethoden auf unser Custom Attribut und eine Liste mit den
        /// MethodenInfos zurückgeben bei denen das Attribut gesetzt ist.
        /// </summary>
        private List<MethodInfo> GetControllerMethodsWithCreateProxyAttribute(Type type)
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();

            //Alle Methoden durchgehen, denn nur die wo auch das Attribut gesetzt ist,
            //darf als JavaScript Funktion erstellt werden
            foreach (MethodInfo info in type.GetMethods())
            {
                if (methodInfos.Any(p => p.Name == info.Name))
                {
                    throw new Exception(string.Format("Achtung, da JavaScript keine Überladung von Methoden unterstützt, bitte eine der Methoden '{0}' umbenennen", info.Name));
                }

                //Alle die MethodenInfos ermitteln, die auch das passenden "AngularCreateProxy" Attribute haben.
                methodInfos.AddRange(from attribute in info.GetCustomAttributes(true) where attribute.GetType() == typeof(CreateProxyAttribute) select info);
            }

            return methodInfos;
        }

        private Type GetAngularCreateProxyReturnType(MethodInfo method)
        {
            CreateProxyAttribute attr = method.GetCustomAttributes(typeof (CreateProxyAttribute), false).FirstOrDefault() as CreateProxyAttribute;
            if (attr != null)
            {
                //Es ist aktuell sehr umständlich hier Interfaces zu verwenden.
                if (attr.ReturnType != null && attr.ReturnType.IsInterface)
                {
                    throw new Exception(string.Format("Bitte keine Interfaces als 'Returntype' für 'CreateProxy' verwenden: '{0}'", attr.ReturnType.Name));
                }
                return attr.ReturnType;
            }

            return null;
        }

        /// <summary>
        /// Prüfen ob in den ParameterInfos für die Funkion ein Komplexer Typ enthalten ist.
        /// d.h. eigene Objekte wie "Person" denn diese müssen per Post übergeben werden.
        /// Es wird eine Exception geworfen, sollten mehr wie ein Complexer Typ enthalten sein.
        /// </summary>
        /// <returns>Gibt eine Liste mit ProxyParameterInfos zurück um später die Parameter einfacher zusammenbauen zu können</returns>
        private List<ProxyParameterInfos> GetMethodParameterInfos(MethodInfo methodInfo)
        {
            List<ProxyParameterInfos> customInfos = new List<ProxyParameterInfos>();

            //Alle Parameter der Jeweiligen Controller Methode durchgehen und prüfen um was für eine Art Parameter es sich handelt.
            foreach (ParameterInfo info in methodInfo.GetParameters())
            {
                Type t = info.ParameterType;

                if (t.IsPrimitive || t == typeof(Decimal) || t == typeof(String) || t == typeof(DateTime) || t == typeof(Int16) ||
                    t == typeof(Int32) || t == typeof(Int64) || t == typeof(Boolean) || t == typeof(TimeSpan) ||
                    t == typeof(Decimal?) || t == typeof(DateTime?) || t == typeof(Int16?) ||
                    t == typeof(Int32?) || t == typeof(Int64?) || t == typeof(Boolean?) || t == typeof(TimeSpan?))
                {
                    customInfos.Add(new ProxyParameterInfos() { IsComplexeType = false, Name = info.Name, ParameterInfo = info, IsString = t == typeof(String) });
                }
                else
                {
                    customInfos.Add(new ProxyParameterInfos() { IsComplexeType = true, Name = info.Name, ParameterInfo = info, IsString = false });
                }
            }

            //Es darf nur ein "komplexer" Parameter pro Controller "übergeben" werden.
            if (customInfos.Count(p => p.IsComplexeType) > 1)
            {
                throw new Exception("Achtung mehr wie einen 'komplexen' Parameter entdeckt - dies wird vom Proxygenerator nicht unterstützt.");
            }

            //Wenn die Liste leer ist, gibt es keine Parameter!
            return customInfos;
        }
    }
}
