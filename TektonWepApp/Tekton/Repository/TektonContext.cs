using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Tekton.Models;
using WebMatrix.WebData;

namespace Tekton.Repository
{
   public class TektonContext : DbContext
   {
      public DbSet<Sala> Salas { get; set; }
      public DbSet<Speaker> Speakers { get; set; }
      public DbSet<Asistente> Asistentes { get; set; }
      public DbSet<Charla> Charlas { get; set; }
      public DbSet<AsistenteCharla> AsistenteCharlas { get; set; }

      public TektonContext() : base("TektonContext") 
        {
          // descomentar para localhost, comentar para publicacion Azure (la BD se crea a través de script db.sql)
          //this.Configuration.ProxyCreationEnabled = false;
          //this.Configuration.LazyLoadingEnabled = false;
          //Database.SetInitializer(new TektonDBInitializer());
        }      
   }

   public class TektonDBInitializer : DropCreateDatabaseIfModelChanges<TektonContext>
   {
       protected override void Seed(TektonContext context)
       {
           var salas = new List<Sala>
            {
                new Sala { Capacidad = 10, NombreSala = "Sala 1"},
                new Sala { Capacidad = 20, NombreSala = "Sala 2"},
                new Sala { Capacidad = 30, NombreSala = "Sala 3"},
                new Sala { Capacidad = 40, NombreSala = "Sala 4"},
                new Sala { Capacidad = 50, NombreSala = "Sala 5"},
            };
           salas.ForEach(t => context.Salas.AddOrUpdate(p => p.Capacidad, t));
           context.SaveChanges();

           var speakers = new List<Speaker>
            {
                new Speaker() { NombreSpeaker = "Juan Perez"},
                new Speaker() { NombreSpeaker = "Christian Mendez"},
                new Speaker() { NombreSpeaker = "Fabiola Claro"}
            };
           speakers.ForEach(t => context.Speakers.AddOrUpdate(p => p.NombreSpeaker, t));
           context.SaveChanges();

           var asistentes = new List<Asistente>
            {
                new Asistente() { CorreoAsistente = "postulante@tektonlabs.com", NombreAsistente = "Postulante", EsAsistenteVIP = true},
                new Asistente() { CorreoAsistente = "asistente2@tektonlabs.com", NombreAsistente = "Asistente 2", EsAsistenteVIP = true},
                new Asistente() { CorreoAsistente = "asistente3@tektonlabs.com", NombreAsistente = "Asistente 3", EsAsistenteVIP = true},
                new Asistente() { CorreoAsistente = "asistente4@tektonlabs.com", NombreAsistente = "Asistente 4", EsAsistenteVIP = false, CantidadMaxCharlas = 1},
                new Asistente() { CorreoAsistente = "asistente5@tektonlabs.com", NombreAsistente = "Asistente 5", EsAsistenteVIP = false, CantidadMaxCharlas = 3},
            };
           asistentes.ForEach(t => context.Asistentes.AddOrUpdate(p => p.CorreoAsistente, t));
           context.SaveChanges();

           var charlas = new List<Charla>
            {
                new Charla() { IdSala = 1, IdSpeaker = 1, HorarioInicio = DateTime.ParseExact("09/03/2018 17:30:00", "dd/MM/yyyy HH:mm:ss",null), HorarioFin = DateTime.ParseExact("09/03/2018 18:30:00", "dd/MM/yyyy HH:mm:ss",null), CapacidadRestante = 10, NombreCharla = "Scrum Master Certification"},
                new Charla() { IdSala = 2, IdSpeaker = 2, HorarioInicio = DateTime.ParseExact("09/03/2018 18:00:00", "dd/MM/yyyy HH:mm:ss",null), HorarioFin = DateTime.ParseExact("09/03/2018 19:00:00", "dd/MM/yyyy HH:mm:ss",null), CapacidadRestante = 20, NombreCharla = "Introduction to Wep Api"},
                new Charla() { IdSala = 3, IdSpeaker = 3, HorarioInicio = DateTime.ParseExact("09/03/2018 21:00:00", "dd/MM/yyyy HH:mm:ss",null), HorarioFin = DateTime.ParseExact("09/03/2018 22:00:00", "dd/MM/yyyy HH:mm:ss",null), CapacidadRestante = 30, NombreCharla = "Charla de Inducción"}
            };
           charlas.ForEach(t => context.Charlas.AddOrUpdate(p => p.HorarioInicio, t));
           context.SaveChanges();
       }   
   }
}
