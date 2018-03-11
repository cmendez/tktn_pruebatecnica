using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Tekton.Models
{
   public class Speaker
   {      
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdSpeaker { get; set; }

      [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe estar entre 2 a 50 caracteres.")]
      [Required(ErrorMessage = "El nombre es obligatorio.")]
      [Display(Name = "Nombre del speaker")]
      [Column("NombreSpeaker")]
      public string NombreSpeaker { get; set; }
             
      public virtual ICollection<Charla> Charlas { get; set; }
   }
}