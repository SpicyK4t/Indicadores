using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class Indicador
    {
        [Key]
        [Editable(false)]
        public int pk { get; set; }

        [Display(Name="Nombre")]
        [Required(ErrorMessage="Campo requerido")]
        [DataType(DataType.Text)]
        public string nombre { get; set; }
        
        [Display(Name="Meta")]
        [Required(ErrorMessage="Campo requerido")]
        public float meta { get; set; }

        [Display(Name="Is")]
        public float i_s { get; set; }

        [Display(Name="Institucional")]
        public float institucional { get; set; }

        [Display(Name = "Proveedor")]
        public Guid proveedor_id { get; set; }
        [ForeignKey("proveedor_id")]
        public Usuario proveedor { get; set; }

        public ICollection<Aplica> aplica_en { get; set; }

        public Indicador()
        {
            this.aplica_en = new HashSet<Aplica>();
        }
    }
}