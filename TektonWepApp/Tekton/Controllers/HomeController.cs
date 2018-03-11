using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tekton.Repository;

namespace Tekton.Controllers
{
    public class HomeController : Controller
    {
        private TektonContext db = new TektonContext();
        private ITektonRepository tektonRepository;

        public HomeController()
        {
            this.tektonRepository = new TektonRepository(new TektonContext());
        }

        public HomeController(ITektonRepository tektonRepository)
        {
            this.tektonRepository = tektonRepository;
        }

        public ActionResult Index()
        {
            //return RedirectToAction("List","Charla");
            return RedirectToAction("List", "Charla");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
