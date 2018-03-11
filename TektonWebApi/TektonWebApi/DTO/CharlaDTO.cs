using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tekton.DTO
{
    public class CharlaDTO
    {
        public string NombreCharla { get; set; }
        public int IdSala { get; set; }
        public int IdSpeaker { get; set; }
        public string HorarioInicio { get; set; }
        public string HorarioFin { get; set; }        
    }
}