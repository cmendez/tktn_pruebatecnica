using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tekton.Models
{
   public class Charla
   {
       [Key, Column(Order = 0)]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int IdCharla { get; set; }

       [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe estar entre 2 a 50 caracteres.")]
       [Required(ErrorMessage = "El nombre es obligatorio.")]
       [Display(Name = "Charla")]
       [Column("NombreCharla")]
       public string NombreCharla { get; set; }

       //[Key, Column(Order = 1)]
       public int IdSala { get; set; }

       //[Key, Column(Order = 2)]
       public int IdSpeaker { get; set; }

       [Required(ErrorMessage = "El horario de inicio es obligatorio.")]
       [Column("HorarioInicio")]
       public DateTime HorarioInicio { get; set; }

       [Required(ErrorMessage = "El horario de fin es obligatorio.")]
       [Column("HorarioFin")]
       public DateTime HorarioFin { get; set; }

       [Required(ErrorMessage = "La capacidad restante es obligatoria.")]
       [Display(Name = "Capacidad Restante")]
       [Range(0, 999999)]
       [Column("CapacidadRestante")]
       public int CapacidadRestante { get; set; }

       public virtual Sala Sala { get; set; }
       public virtual Speaker Speaker { get; set; }
       public virtual ICollection<AsistenteCharla> Asistentes { get; set; }
   }
}