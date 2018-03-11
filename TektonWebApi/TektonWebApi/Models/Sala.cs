using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Tekton.Models
{
   public class Sala
   {      
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdSala { get; set; }

      [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe estar entre 2 a 50 caracteres.")]
      [Required(ErrorMessage = "El nombre es obligatorio.")]
      [Display(Name = "Nombre de la sala")]
      [Column("NombreSala")]
      public string NombreSala { get; set; }

      [Required(ErrorMessage = "La capacidad es obligatoria.")]
      [Display(Name = "Number")]
      [Range(0, 999999)]
      [Column("Capacidad")]
      public int Capacidad { get; set; }

      public virtual ICollection<Charla> Charlas { get; set; }
      //public virtual ICollection<OrderDetail> OrderDetails { get; set; }
   }
}