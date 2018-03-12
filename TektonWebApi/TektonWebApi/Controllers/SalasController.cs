using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Tekton.Repository;
using Newtonsoft.Json;
using Tekton.DTO;
using TektonWebApi.BusinessLogic;

namespace TektonWebApi.Controllers
{
    public class SalasController : ApiController
    {
        //private static TektonContext _dbContext = new TektonContext();
        //private static TektonRepository _tektonRepository = new TektonRepository(_dbContext);
        private TektonContext _dbContext;
        private ITektonRepository _tektonRepository;

        public SalasController()
        {
            this._dbContext = new TektonContext();
            this._tektonRepository = new TektonRepository(_dbContext);
        }

        [System.Web.Http.HttpPost]
        public string AsignarCapacidad(SalaDTO salaDTO)
        {
            return TektonBusinessLogic.AsignarCapacidad(_dbContext, _tektonRepository, salaDTO);
        }
    }
}