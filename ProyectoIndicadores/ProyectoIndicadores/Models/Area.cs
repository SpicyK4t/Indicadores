using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class Area
    {
        [Key]
        [Editable(false)]
        public int pk { get; set; }

        [Display(Name="Nombre")]
        [Required(ErrorMessage="Campo requerido")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage="Cantidad de carácteres exedidos")]
        public string nombre { get; set; }

        [Display(Name="Descripción")]
        [DataType(DataType.MultilineText)]
        public string descripcion { get; set; }

        [Display(Name="Sector")]
        public int? sector_id { get; set; }
        [ForeignKey("sector_id")]
        public virtual Sector sector { get; set; }

        public virtual ICollection<Aplica> aplica_en { get; set; }
        public virtual ICollection<Usuario> consumidores { get; set; }

        public Area()
        {
            this.aplica_en = new HashSet<Aplica>();
            this.consumidores = new HashSet<Usuario>();
        }

    }
}