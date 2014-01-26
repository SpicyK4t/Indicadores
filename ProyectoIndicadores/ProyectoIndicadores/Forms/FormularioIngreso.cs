using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Forms
{
    public class FormularioIngreso
    {
        [Display(Name="Usuario")]
        [Required(ErrorMessage="Falta el Usuario")]
        [DataType(DataType.Text)]
        [StringLength(40, MinimumLength=5, ErrorMessage="Cantidad de carácteres no valida")]
        public string usuario     { get; set; }

        [Display(Name="Contraseña")]
        [Required(ErrorMessage="Falta la contraseña")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength=6, ErrorMessage="La contraseña debe ser mayor a 6 digitos")]
        public string contrasenia { get; set; }
    }
}