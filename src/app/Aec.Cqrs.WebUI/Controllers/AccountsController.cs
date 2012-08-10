using System;
using System.Web.Mvc;
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
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
