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

namespace Tekton.Controllers
{
    public class CharlaController : Controller
    {
        private TektonContext db = new TektonContext();
        private ITektonRepository tektonRepository;

        public CharlaController()
        {
            this.tektonRepository = new TektonRepository(new TektonContext());
        }

        public CharlaController(ITektonRepository tektonRepository)
        {
            this.tektonRepository = tektonRepository;
        }

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult List()
        {
            List<Charla> products = new List<Charla>();
            
            if (db.Charlas.Any())
            {
                return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
            }
            else
            {
                return View("EmptyList");
            }            
        }

        //[ValidateAntiForgeryToken]
        public ActionResult Inscribir(int idCharla)
        {
            if(User.Identity.IsAuthenticated)
            {
                try
                {
                    var asistente = db.Asistentes.First(a => a.CorreoAsistente ==
                                                        User.Identity.Name);

                    var charlasAsistente = db.AsistenteCharlas.Where(c => c.IdAsistente ==
                                                                    asistente.IdAsistente).Include(s =>
                                                                        s.Charla).ToList();

                    var charla = db.Charlas.First(c => c.IdCharla == idCharla);

                    //validar que persona no este inscrita ya en la charla
                    if (charlasAsistente.Exists(c => c.IdCharla == idCharla))
                    {
                        ViewBag.Error = "Usted ya se inscribió a la charla \"" + charla.NombreCharla + "\"";
                        return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                    }

                    //validar que asistente aun pueda inscribirse
                    if (!asistente.EsAsistenteVIP && charlasAsistente.Count >= asistente.CantidadMaxCharlas)
                    {
                        ViewBag.Error = "Ya excedió su cantidad máxima de inscripciones.";
                        return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                    }

                    //validar capacidad de sala
                    if (charla.CapacidadRestante == 0)
                    {
                        ViewBag.Error = "La charla \"" + charla.NombreCharla + "\"" + "está completa.";
                        return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
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
                            )
                            )
                        {
                            ViewBag.Error = "Hay un conflicto de horarios con la charla \"" +
                                            charlaAsistente.Charla.NombreCharla + "\"";
                            return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                        }
                    }

                    //pasó todas las validaciones, registrar al asistente a la charla
                    var nuevoAsistenteCharla = new AsistenteCharla()
                                                   {
                                                       IdCharla = idCharla,
                                                       IdAsistente = asistente.IdAsistente
                                                   };

                    charla.CapacidadRestante = charla.CapacidadRestante - 1;
                    tektonRepository.ActualizarCharla(charla);
                    tektonRepository.RegistrarAsistenteCharla(nuevoAsistenteCharla);
                    tektonRepository.Save();

                    ViewBag.Success = "Se ha registrado con éxito su inscripción a la charla \"" +
                                            charla.NombreCharla + "\"";
                    return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al intentar inscribirse.";
                    return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
                }
            }

            return View("List", db.Charlas.Include(s => s.Sala).Include(s => s.Speaker).ToList());
        }

        //
        // GET: /Product/Details/5

        //public ViewResult Details(int id)
        //{
        //    Product product = tektonRepository.GetProductByID(id);
        //    return View(product);
        //}

        ////
        //// GET: /Product/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //public ActionResult CreateVM()
        //{
        //    ViewBag.GetStorageTypesList = TektonSelectLists.GetStorageTypesList();
        //    return View("CreateVM");
        //}

        ////
        //// POST: /Product/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Product product)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            tektonRepository.InsertProduct(product);
        //            tektonRepository.Save();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (DataException)
        //    {
        //        ModelState.AddModelError(string.Empty, "Unable to save changes.");
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateVM(ProductViewModel productVM)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            Product p = Mapper.Map<Product>(productVM);

        //            if (productVM.StorageTypeSelected.Equals(TektonEnum.StorageType.MemoryStorage.GetHashCode().ToString()))
        //            {
        //                TektonMemoryStorage.SaveProductInMemoryStorage(p);
        //            }
        //            else if (productVM.StorageTypeSelected.Equals(TektonEnum.StorageType.PersistentStorage.GetHashCode().ToString()))
        //            {
        //                tektonRepository.InsertProduct(p);
        //                tektonRepository.Save();
        //            }

        //            return View("Success");
        //        }
        //    }
        //    catch (DataException)
        //    {
        //        ModelState.AddModelError(string.Empty, "Unable to save changes.");
        //    }
        //    return View(productVM);
        //}

        ////
        //// GET: /Product/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    Product product = tektonRepository.GetProductByID(id);
        //    return View(product);
        //}

        ////
        //// POST: /Product/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Product product)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            tektonRepository.UpdateProduct(product);
        //            tektonRepository.Save();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (DataException)
        //    {
        //        ModelState.AddModelError(string.Empty, "Unable to save changes.");
        //    }
        //    return View(product);
        //}

        ////
        //// GET: /Product/Delete/5

        //public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        //{
        //    if (saveChangesError.GetValueOrDefault())
        //    {
        //        ViewBag.ErrorMessage = "Delete failed.";
        //    }
        //    Product product = tektonRepository.GetProductByID(id);
        //    return View(product);
        //}

        ////
        //// POST: /Product/Delete/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        Product product = tektonRepository.GetProductByID(id);
        //        tektonRepository.DeleteProduct(id);
        //        tektonRepository.Save();
        //    }
        //    catch (DataException)
        //    {
        //        return RedirectToAction("Delete", new { id = id, saveChangesError = true });
        //    }
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            tektonRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
