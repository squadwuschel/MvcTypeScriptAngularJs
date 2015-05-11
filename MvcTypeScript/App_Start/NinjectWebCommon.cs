using MvcTypeScript.Helper;
using MvcTypeScript.Models.Todo.Builder;
using MvcTypeScript.Models.Todo.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MvcTypeScript.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MvcTypeScript.App_Start.NinjectWebCommon), "Stop")]

namespace MvcTypeScript.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            //Repository im Singleton Scope injecten
            kernel.Bind<ITodoRepository>().To<TodoRepository>().InSingletonScope();

            //ModelBuilder
            kernel.Bind<ITodoCreateModelBuilder>().To<TodoCreateModelBuilder>().InRequestScope();
            kernel.Bind<ITodoOverviewModelBuilder>().To<TodoOverviewModelBuilder>().InRequestScope();
            kernel.Bind<ITodoListenModelBuilder>().To<TodoListenModelBuilder>().InRequestScope();


        }        
    }
}
