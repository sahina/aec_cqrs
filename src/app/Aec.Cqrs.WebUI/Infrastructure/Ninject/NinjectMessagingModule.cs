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
            //
            // Projections

            Bind<IDocumentStrategy>().To<FileDocumentStrategy>();

            Bind<IDocumentStore>()
                .To<FileDocumentStore>()
                .WithConstructorArgument("folderPath", "documents")
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