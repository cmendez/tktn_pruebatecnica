using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Newtonsoft.Json;
using Tekton.DTO;
using Tekton.Repository;
using Tekton.Models;

namespace TektonWebApi.BusinessLogic
{
    public static class TektonBusinessLogic
    {
        public static string AsignarCapacidad(TektonContext dbContext, ITektonRepository tektonRepository, SalaDTO salaDTO)
        {
            string parsedJson = String.Empty;

            if (salaDTO.Capacidad <= 0)
            {
                parsedJson = "Error: La capacidad no puede ser menor igual a 0";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }

            var sala = dbContext.Salas.FirstOrDefault(c => c.IdSala == salaDTO.IdSala);
            if (sala != null)
            {
                try
                {
                    sala.Capacidad = salaDTO.Capacidad;
                    tektonRepository.ActualizarSala(sala);
                    tektonRepository.Save();
                }
                catch (Exception ex)
                {
                    parsedJson = "Ocurrió un error al tratar de actualizar la sala.";
                    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                }

                parsedJson = "La capacidad de la sala " + salaDTO.IdSala + " fue actualizada con éxito";
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            else
            {
                parsedJson = "No existe la sala con id: " + salaDTO.IdSala;
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
        }

        public static string RegistrarCharla(TektonContext dbContext, ITektonRepository tektonRepository, CharlaDTO charlaDTO)
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
                var sala = dbContext.Salas.FirstOrDefault(c => c.IdSala == charlaDTO.IdSala);
                if (sala == null)
                {
                    jsonResponse = "Error: No existe la sala con id: " + charlaDTO.IdSala;
                    return JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);
                }

                //validar speaker exista
                var speaker = dbContext.Speakers.FirstOrDefault(c => c.IdSpeaker == charlaDTO.IdSpeaker);
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
                var charlasSala = dbContext.Charlas.Where(c => c.IdSala == charlaDTO.IdSala).Include(s => s.Sala).ToList();
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
                var charlasSpeaker = dbContext.Charlas.Where(c => c.IdSpeaker == charlaDTO.IdSpeaker).Include(s => s.Speaker).ToList();
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