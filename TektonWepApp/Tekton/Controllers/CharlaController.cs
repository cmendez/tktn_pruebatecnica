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
    [Authorize]
    public class CharlaController : Controller
    {
        private TektonContext _dbContext = new TektonContext();
        private ITektonRepository _tektonRepository;

        public CharlaController()
        {
            this._tektonRepository = new TektonRepository(new TektonContext());            
        }

        public CharlaController(ITektonRepository tektonRepository)
        {
            this._tektonRepository = tektonRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List", "Charla");
        }

        public ActionResult List()
        {
            List<Charla> products = new List<Charla>();

            if (_dbContext.Charlas.Any())
            {
                return View("List", _dbContext.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
            }
            else
            {
                return View("EmptyList");
            }            
        }

        //[HttpPost]
        public ActionResult Inscribir(int idCharla)
        {
            //lista de charlas en la BD
            var charlas = _dbContext.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList();

            if (charlas.Any() && User.Identity.IsAuthenticated)
            {
                try
                {
                    var asistente = _dbContext.Asistentes.First(a => a.CorreoAsistente == User.Identity.Name);
                    var charlasAsistente = _dbContext.AsistenteCharlas.Where(c => c.IdAsistente == asistente.IdAsistente).Include(s => s.Charla).ToList();
                    var charla = _dbContext.Charlas.First(c => c.IdCharla == idCharla);

                    //validar que persona no este inscrita ya en la charla
                    if (charlasAsistente.Exists(c => c.IdCharla == idCharla))
                    {
                        ViewBag.Error = "Usted ya se inscribió a la charla \"" + charla.NombreCharla + "\"";
                        return View("List", charlas);
                    }

                    //validar que asistente aun pueda inscribirse
                    if (!asistente.EsAsistenteVIP && charlasAsistente.Count >= asistente.CantidadMaxCharlas)
                    {
                        ViewBag.Error = "Ya excedió su cantidad máxima de inscripciones.";
                        return View("List", charlas);
                    }

                    //validar capacidad de charla
                    if (charla.CapacidadRestante == 0)
                    {
                        ViewBag.Error = "La charla \"" + charla.NombreCharla + "\"" + "está completa.";
                        return View("List", charlas);
                    }

                    //validar conflicto de horarios
                    foreach (var charlaAsistente in charlasAsistente)
                    {
                        if ((
                                (charla.HorarioInicio.CompareTo(charlaAsistente.Charla.HorarioInicio) >= 0)
                                &&
                                (charla.HorarioInicio.CompareTo(charlaAsistente.Charla.HorarioFin) <= 0)
                            )
                            ||
                            (
                                (charla.HorarioFin.CompareTo(charlaAsistente.Charla.HorarioInicio) >= 0)
                                &&
                                (charla.HorarioFin.CompareTo(charlaAsistente.Charla.HorarioFin) <= 0)
                            ))
                        {
                            ViewBag.Error = "Hay un conflicto de horarios con la charla \"" +
                                            charlaAsistente.Charla.NombreCharla + " a la cual usted ya está inscrito.\"";
                            return View("List", charlas);
                        }
                    }

                    //pasó todas las validaciones, registrar al asistente a la charla
                    var nuevoAsistenteCharla = new AsistenteCharla()
                                                   {
                                                       IdCharla = idCharla,
                                                       IdAsistente = asistente.IdAsistente
                                                   };

                    charla.CapacidadRestante = charla.CapacidadRestante - 1;
                    _tektonRepository.ActualizarCharla(charla);
                    _tektonRepository.RegistrarAsistenteCharla(nuevoAsistenteCharla);
                    _tektonRepository.Save();

                    ViewBag.Success = "Se ha registrado con éxito su inscripción a la charla \"" + charla.NombreCharla + "\"";
                    return View("List", _dbContext.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al intentar inscribirse.";
                    return View("List", charlas);
                }
            }

            return View("List", charlas);
        }

        protected override void Dispose(bool disposing)
        {
            _tektonRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
