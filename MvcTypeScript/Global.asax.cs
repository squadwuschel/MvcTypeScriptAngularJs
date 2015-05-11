using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcTypeScript.ProxyCreator.ProxyBuilder;

namespace MvcTypeScript
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CreateTypeScriptProxy();
        }

        private void CreateTypeScriptProxy()
        {
#if DEBUG
            //Die Injections für den ProxyBuilder per Hand zusammenstellen, da dieser nur im Debug benötigt wird.
            ProxyWriter writer = new ProxyWriter("ScriptsApp/services");
            BuilderHelperMethods helper = new BuilderHelperMethods();
            MethodTypeInformationParser parser = new MethodTypeInformationParser();
            BuilderForAngularTs builderForAngularTs = new BuilderForAngularTs(helper)
            {
                HasSiteRootDefinition = false,
                LowerFirstCharInFunctionName = true
            };

            ProxyBuilder builder = new ProxyBuilder(builderForAngularTs, writer, parser);
            builder.StartBuildProcess();
#endif
        }
    }
}