using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class Sector
    {
        [Key]
        [Editable(false)]
        public int pk { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string descripcion { get; set; }

        public virtual ICollection<Area> areas { get; set; }

        public Sector()
        {
            this.areas = new HashSet<Area>();
        }
    }
}