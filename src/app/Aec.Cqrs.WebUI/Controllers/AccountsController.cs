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
                var userid = new UserID(security.SecurityID.GetIdenfitier());

                MessageSender.SendOne(new UserCreated(securityid, userid, new TimeSpan(0, 0, 20)));

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
