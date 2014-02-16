using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIndicadores.Models
{
    public class VistaProveedoresConsumidores
    {
        public Guid id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        
        [Display(Name="Provee")]
        public IEnumerable<int> provee_pk { get; set; }
        public IEnumerable<SelectListItem> provee { get; set; }

        [Display(Name="Consume")]
        public IEnumerable<int> consumidor__pk { get; set; }
        public IEnumerable<SelectListItem> consumidor { get; set; }
    }
}