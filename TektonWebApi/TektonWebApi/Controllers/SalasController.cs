﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Tekton.Repository;
using Newtonsoft.Json;

namespace TektonWebApi.Controllers
{
    public class SalasController : ApiController
    {
        private TektonContext db = new TektonContext();
        private ITektonRepository tektonRepository;

        public SalasController()
        {
            this.tektonRepository = new TektonRepository(new TektonContext());
        }

        public SalasController(ITektonRepository tektonRepository)
        {
            this.tektonRepository = tektonRepository;
        }

        [System.Web.Http.HttpGet]
        public string asignarCapacidad(int idSala, int capacidad)
        {
            string parsedJson = String.Empty;

            if (capacidad <= 0)
            {
                parsedJson = "Error: La capacidad no puede ser menor igual a 0";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }

            var sala = db.Salas.First(c => c.IdSala == idSala);
            if (sala != null)
            {
                try
                {
                    sala.Capacidad = capacidad;
                    tektonRepository.ActualizarSala(sala);
                    tektonRepository.Save();
                }
                catch (Exception ex)
                {
                    parsedJson = "Ocurrió un error al tratar de actualizar la sala.";
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }

                parsedJson = "La capacidad de la sala " + idSala + " fue actualizada con éxito";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            else
            {
                parsedJson = "No existe la sala con id: " + idSala;
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
        }
    }
}