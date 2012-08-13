using System.Web;
using Ninject;
using Ninject.Modules;

namespace Aec.Cqrs.WebUI.Infrastructure.Ninject
{
    public class NinjectMessagingModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            var folderPathDocs = HttpContext.Current.Server.MapPath("~/App_Data/Documents");
            var folderPathRecords = HttpContext.Current.Server.MapPath("~/App_Data/Records");

            //
            // Projections

            Bind<IDocumentStrategy>().To<FileJsonDocumentStrategy>();

            Bind<IDocumentStore>()
                .To<FileDocumentStore>()
                .WithConstructorArgument("folderPath", folderPathDocs)
                .WithConstructorArgument("strategy", Kernel.Get<IDocumentStrategy>());

            Bind<DocumentStorage>().ToSelf();

            //
            // Message Bus

            Bind<MemoryBusWithRouterAndHandler>().ToSelf();

            Bind<ICommandBus>()
                .ToMethod(ctx => ctx.Kernel.Get<MemoryBusWithRouterAndHandler>());

            Bind<IEventBus>()
                .ToMethod(ctx => ctx.Kernel.Get<MemoryBusWithRouterAndHandler>());

            Bind<IQueueWriter>().To<QueueWriterToBus>();


            //
            // Message Handler

            Bind<MessageHandler>().ToSelf().InSingletonScope();
        }

        #endregion
    }
}