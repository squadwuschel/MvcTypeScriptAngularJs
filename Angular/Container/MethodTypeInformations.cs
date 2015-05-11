using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvcTypeScript.ProxyCreator.Container
{
    public class MethodTypeInformations
    {
        public Type Controller { get; set; }

        public string MethodName { get; set; }

        public string Namespace { get; set; }

        public string MethodNameWithNamespace { get; set; }

        public Type ReturnType { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public List<ProxyParameterInfos> ProxyParameterInfos { get; set; }
    }
}
