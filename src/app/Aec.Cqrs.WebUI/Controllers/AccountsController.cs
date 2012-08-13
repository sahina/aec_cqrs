using System;
using System.Web.Mvc;
using Aec.Cqrs.Client;
using Aec.Cqrs.Client.Events;
using Aec.Cqrs.WebUI.Models;

namespace Aec.Cqrs.WebUI.Controllers
{
    public class AccountsController : ApplicationController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new CreateAccountModel
            {
                AccountID = Guid.NewGuid()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var securityid = new SecurityID(Guid.NewGuid().ToString());
                var security = new SecurityInfo(securityid, model.Username, model.Password, model.Username, null);
                var regid = new RegistrationID(Guid.NewGuid());

                var cmd = new RegistrationCreated(regid, DateTime.UtcNow, security);

                Bus.SendOne(cmd);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult ResetAll()
        {
            return View();
        }
    }
}
