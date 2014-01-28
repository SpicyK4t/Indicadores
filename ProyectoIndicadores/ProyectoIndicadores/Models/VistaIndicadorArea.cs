using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIndicadores.Models
{
    public class VistaIndicadorArea
    {
        public int pk { get; set; }
        public string nombre { get; set; }

        [Display(Name="Areas")]
        public IEnumerable<int> aplica_a { get; set; }
        public IEnumerable<SelectListItem> areas { get; set; }
    }
}