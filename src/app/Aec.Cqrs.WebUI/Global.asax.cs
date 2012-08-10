using System.Web.Mvc;
using System.Web.Routing;
using Aec.Cqrs.Client;
using Aec.Cqrs.Client.Events;
using Aec.Cqrs.Client.Projections;
using Aec.Cqrs.WebUI.Infrastructure.Ninject;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc;

namespace Aec.Cqrs.WebUI
{
    public class MvcApplication : NinjectHttpApplication
    {
        public static IKernel NinjectKernel { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>
        /// The created kernel.
        /// </returns>
        protected override IKernel CreateKernel()
        {
            var modules = new INinjectModule[]
            {
                new NinjectMessagingModule()
            };

            NinjectKernel = new StandardKernel(modules);

            return NinjectKernel;
        }

        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            RegisterMessageRoutes();
        }

        private static void RegisterMessageRoutes()
        {
            var documentStore = NinjectKernel.Get<DocumentStorage>();
            var handler = NinjectKernel.Get<MessageHandler>();

            handler.WireToLambda<UserCreated>(created =>
            {
                var view = new RegistrationView
                {
                    SecurityID = created.Identity,
                    
                };

                //documentStore.AddEntity(created.Identity, created.)
            });
        }
    }
}