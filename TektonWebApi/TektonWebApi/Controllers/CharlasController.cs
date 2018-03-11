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

        [System.Web.Http.HttpPost, ValidateInput(false)]
        public string registrarCharla(int idSala, int idSpeaker, string horarioInicio, string horarioFin,                                           string nombreCharla)
        {
            string parsedJson = String.Empty;
            DateTime horarioInicioDT;
            DateTime horarioFinDT;

            try
            {
                //validar sala exista
                var sala = db.Salas.First(c => c.IdSala == idSala);
                if (sala == null)
                {
                    parsedJson = "Error: No existe la sala con id: " + idSala;
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }

                //validar speaker exista
                var speaker = db.Speakers.First(c => c.IdSpeaker == idSpeaker);
                if (speaker == null)
                {
                    parsedJson = "Error: No existe speaker con id: " + idSpeaker;
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }

                //validar formato de horarios
                try
                {
                    horarioInicioDT = DateTime.ParseExact(horarioInicio, "dd/MM/yyyy HH:mm:ss", null);
                    horarioFinDT = DateTime.ParseExact(horarioFin, "dd/MM/yyyy HH:mm:ss", null);
                }
                catch (Exception ex)
                {
                    parsedJson = "Error: los horarios deben estar en formato dd/MM/yyyy HH:mm:ss";
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }
                
                if (horarioInicioDT.CompareTo(horarioFinDT) >= 0)
                {
                    parsedJson = "Error: El horario fin debe ser mayor que horario inicio";
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }

                
                //registrar charla
                var nuevaCharla = new Charla()
                                      {
                                          NombreCharla = nombreCharla,
                                          IdSala = idSala,
                                          IdSpeaker = idSpeaker,
                                          HorarioInicio = horarioInicioDT,
                                          HorarioFin = horarioFinDT,
                                          CapacidadRestante = sala.Capacidad
                                      };
                tektonRepository.RegistrarCharla(nuevaCharla);
                tektonRepository.Save();

                parsedJson = "Se registró con éxito la charla.";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch (Exception ex)
            {
                parsedJson = "Ocurrió un error al tratar de registrar la charla.";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
        }
    }

    public class CharlaDTO
    {
        public int idSala { get; set; }
        public int idSpeaker { get; set; }
        public string horarioInicio { get; set; }
        public string horarioFin { get; set; }
        public string nombreCharla { get; set; }
    }
}