using System;
using System.Collections.Generic;

namespace MvcTypeScript.ProxyCreator.Container
{
    public class ControllerTypeInformations
    {
        public ControllerTypeInformations()
        {
            MethodTypeInformations = new List<MethodTypeInformations>();
        }

        public Type Controller { get; set; }

        public List<MethodTypeInformations> MethodTypeInformations { get; set; }
    }
}
