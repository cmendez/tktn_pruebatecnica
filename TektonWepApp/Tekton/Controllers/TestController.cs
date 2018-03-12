using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tekton.Models;
using Tekton.Repository;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using Tekton.Filters;
using WebMatrix.WebData;

namespace Tekton.Controllers
{
    //[Authorize]
    public class TestController : Controller
    {
        private TektonContext _dbContext = new TektonContext();
        private ITektonRepository _tektonRepository;

        public TestController()
        {
            this._tektonRepository = new TektonRepository(new TektonContext());            
        }

        public TestController(ITektonRepository tektonRepository)
        {
            this._tektonRepository = tektonRepository;
        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.Data = _dbContext.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList()[0].NombreCharla;
            }
            catch (Exception ex)
            {
                ViewBag.Data = ex.ToString();                
            }
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _tektonRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
