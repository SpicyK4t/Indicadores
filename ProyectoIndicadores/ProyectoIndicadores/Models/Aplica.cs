using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class Aplica
    {
        [Key]
        [Editable(false)]
        public int pk { get; set; }

        [Required]
        [Display(Name = "Area")]
        public Area area { get; set; }

        [Required]
        [Display(Name = "Indicador")]
        public int indicador_id { get; set; }
        [ForeignKey("indicador_id")]
        public virtual Indicador indicador { get; set; }

        [Display(Name = "Valor")]
        public float? valor { get; set; }
    }
}