using System.Web.Mvc;
using Ninject;

namespace Aec.Cqrs.WebUI.Controllers
{
    public class ApplicationController : Controller
    {
        [Inject]
        public MessageSender Bus { get; set; }

        [Inject]
        public DocumentStorage DocumentStorage { get; set; }
    }
}
