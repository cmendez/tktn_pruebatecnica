using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Tekton.Models;
using Tekton.Repository;
using Newtonsoft.Json;
using Tekton.DTO;

namespace TektonWebApi.Controllers
{
    public class CharlasController : ApiController
    {
        private TektonContext db = new TektonContext();
        private ITektonRepository tektonRepository;

        public CharlasController()
        {
            this.tektonRepository = new TektonRepository(new TektonContext());
        }

        public CharlasController(ITektonRepository tektonRepository)
        {
            this.tektonRepository = tektonRepository;
        }

        [System.Web.Mvc.HttpPost]
        public string RegistrarCharla(CharlaDTO charlaDTO)
        {
            string jsonResponse = String.Empty;
            DateTime horarioInicioDT;
            DateTime horarioFinDT;

            try
            {
                //validar que nombre de sala sea válido
                if (String.IsNullOrEmpty(charlaDTO.NombreCharla) || charlaDTO.NombreCharla.Trim().Length > 50)
                {
                    jsonResponse = "Error: el nombre de la sala es obligatorio y debe tener entre 1 a 50 caracteres.";
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }

                //validar sala exista
                var sala = db.Salas.FirstOrDefault(c => c.IdSala == charlaDTO.IdSala);
                if (sala == null)
                {
                    jsonResponse = "Error: No existe la sala con id: " + charlaDTO.IdSala;
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }

                //validar speaker exista
                var speaker = db.Speakers.FirstOrDefault(c => c.IdSpeaker == charlaDTO.IdSpeaker);
                if (speaker == null)
                {
                    jsonResponse = "Error: No existe speaker con id: " + charlaDTO.IdSpeaker;
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }

                //validar formato de horarios
                try
                {
                    horarioInicioDT = DateTime.ParseExact(charlaDTO.HorarioInicio, "dd/MM/yyyy HH:mm", null);
                    horarioFinDT = DateTime.ParseExact(charlaDTO.HorarioFin, "dd/MM/yyyy HH:mm", null);
                }
                catch (Exception ex)
                {
                    jsonResponse = "Error: los horarios deben estar en formato dd/MM/yyyy HH:mm";
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }

                if (horarioInicioDT.CompareTo(horarioFinDT) >= 0)
                {
                    jsonResponse = "Error: El horario fin debe ser mayor que horario inicio";
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }
                                
                //validar que no haya otra charla en la misma sala y en un horario en conflicto
                var charlasSala = db.Charlas.Where(c => c.IdSala == charlaDTO.IdSala).Include(s => s.Sala).ToList();
                foreach (var charlaSala in charlasSala)
                {
                    if ((
                            (horarioInicioDT.CompareTo(charlaSala.HorarioInicio) >= 0)
                            &&
                            (horarioInicioDT.CompareTo(charlaSala.HorarioFin) <= 0)
                        )
                        ||
                        (
                            (horarioFinDT.CompareTo(charlaSala.HorarioInicio) >= 0)
                            &&
                            (horarioFinDT.CompareTo(charlaSala.HorarioFin) <= 0)
                        ))
                    {
                        jsonResponse = String.Format(@"Error en conflicto de horarios: la sala '{0}' ya se está utilizando en la charla '{1}' (Horario '{2}' - '{3}')",
                                                     charlaSala.Sala.NombreSala,
                                                     charlaSala.NombreCharla,
                                                     charlaSala.HorarioInicio.ToString("dd/MM/yyyy HH:mm"),
                                                     charlaSala.HorarioFin.ToString("dd/MM/yyyy HH:mm"));

                        return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                    }
                }

                //validar que no haya otra charla con el mismo speaker y en un horario en conflicto
                var charlasSpeaker = db.Charlas.Where(c => c.IdSpeaker == charlaDTO.IdSpeaker).Include(s => s.Speaker).ToList();
                foreach (var charlaSpeaker in charlasSpeaker)
                {
                    if ((
                            (horarioInicioDT.CompareTo(charlaSpeaker.HorarioInicio) >= 0)
                            &&
                            (horarioInicioDT.CompareTo(charlaSpeaker.HorarioFin) <= 0)
                        )
                        ||
                        (
                            (horarioFinDT.CompareTo(charlaSpeaker.HorarioInicio) >= 0)
                            &&
                            (horarioFinDT.CompareTo(charlaSpeaker.HorarioFin) <= 0)
                        ))
                    {
                        jsonResponse = String.Format(@"Error en conflicto de horarios: el speaker '{0}' ya está dictando la charla '{1}' (Horario '{2}' - '{3}')",
                                                     charlaSpeaker.Speaker.NombreSpeaker,
                                                     charlaSpeaker.NombreCharla,
                                                     charlaSpeaker.HorarioInicio.ToString("dd/MM/yyyy HH:mm"),
                                                     charlaSpeaker.HorarioFin.ToString("dd/MM/yyyy HH:mm"));

                        return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                    }
                }

                //pasó todas las validaciones -> registrar charla
                var nuevaCharla = new Charla()
                {
                    NombreCharla = charlaDTO.NombreCharla.Trim(),
                    IdSala = charlaDTO.IdSala,
                    IdSpeaker = charlaDTO.IdSpeaker,
                    HorarioInicio = horarioInicioDT,
                    HorarioFin = horarioFinDT,
                    CapacidadRestante = sala.Capacidad
                };
                tektonRepository.RegistrarCharla(nuevaCharla);
                tektonRepository.Save();

                jsonResponse = "Se registró con éxito la charla.";
                return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
            }
            catch (Exception ex)
            {
                jsonResponse = "Ocurrió un error al tratar de registrar la charla.";
                return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
            }
        }
    }
}