using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Tekton.DTO
{
   public class SalaDTO
   {      
      public int IdSala { get; set; }
      public int Capacidad { get; set; }
   }
}