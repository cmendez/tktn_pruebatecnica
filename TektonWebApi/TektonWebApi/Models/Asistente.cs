using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Tekton.Models
{
    public class Asistente
   {      
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdAsistente { get; set; }

      [StringLength(50, MinimumLength = 2, ErrorMessage = "El correo debe estar entre 2 a 50 caracteres.")]
      [Required(ErrorMessage = "El correo es obligatorio.")]
      [Display(Name = "Correo del asistente")]
      [DataType(DataType.EmailAddress, ErrorMessage = "No es un correo válido")]
      [Column("CorreoAsistente")]
      public string CorreoAsistente { get; set; }

      [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe estar entre 2 a 50 caracteres.")]
      [Required(ErrorMessage = "El nombre es obligatorio.")]
      [Display(Name = "Nombre de la sala")]
      [Column("NombreAsistente")]
      public string NombreAsistente { get; set; }

      [Display(Name = "Es Asistente VIP")]
      [Column("EsAsistenteVIP")]
      public bool EsAsistenteVIP { get; set; }

      [Display(Name = "Cantidad máxima de charlas")]
      [Range(-1, 999999)]
      [Column("CantidadMaxCharlas")]
      public int CantidadMaxCharlas { get; set; }

      public virtual ICollection<AsistenteCharla> Charlas { get; set; }
   }
}