using System;
using MvcTypeScript.ProxyCreator.Container;

namespace MvcTypeScript.ProxyCreator.Interfaces
{
    public interface IMethodTypeInformationParser
    {
        ControllerTypeInformations ParseMethodTypeInformations(Type controller);
    }
}