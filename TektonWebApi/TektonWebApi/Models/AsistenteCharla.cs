using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tekton.Models
{
    public class AsistenteCharla
    {
        [Key, Column(Order = 0)]
        public int IdCharla { get; set; }
        [Key, Column(Order = 1)]
        public int IdAsistente { get; set; }

        //[NotMapped]
        public virtual Charla Charla { get; set; }
        //[NotMapped]
        public virtual Asistente Asistente { get; set; }
    }
}