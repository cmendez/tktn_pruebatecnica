﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tekton.Repository;

namespace Tekton.Controllers
{
    public class HomeController : Controller
    {
        //private TektonContext _dbContext = new TektonContext();
        //private ITektonRepository tektonRepository;

        private TektonContext _dbContext; // = new TektonContext();
        //private static TektonRepository _tektonRepository = new TektonRepository(_dbContext);

        public HomeController()
        {
            //this.tektonRepository = new TektonRepository(new TektonContext()); 
            this._dbContext = new TektonContext();
        }

        //public HomeController(ITektonRepository tektonRepository)
        //{
        //    this.tektonRepository = tektonRepository;
        //}

        public ActionResult Index()
        {
            _dbContext.Charlas.Any();
            return RedirectToAction("Login", "Account");
        }
    }
}
