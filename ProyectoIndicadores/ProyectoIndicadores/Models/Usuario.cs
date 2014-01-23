﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class Usuario
    {
        [Key]
        [Editable(false)]
        public Guid PK { get; set; }

        [Display(Name="Correo")]
        [Required(ErrorMessage="Es necesario establecer un correo")]
        [DataType(DataType.EmailAddress, ErrorMessage="Correo no valido")]
        [StringLength(100, ErrorMessage="La cantidad de caracteres permitida fue exedida")
        public string Correo { get; set; }

        [Display(Name="Nombre")]
        [Required(ErrorMessage="Falta establecer un nombre")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string Nombre { get; set; }

        [Display(Name="Apellido")]
        [Required(ErrorMessage="Falta establecer un apellido")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string Apellido { get; set; }

        [Display(Name="Nombre de Usuario")]
        [Required(ErrorMessage="Falta el Nombre de Usuario")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string NombreUsuario { get; set; }

        [Display(Name="Contraseña")]
        [Required(ErrorMessage="Falta la contraseña")]
        [DataType(DataType.Password, ErrorMessage="Contraseña invalida")]
        [StringLength(Int32.MaxValue, MinimumLength=6, ErrorMessage="La contraseña debe ser mayor a 6 digitos")]
        public string Contrasenia { get; set; }


        public string salt { get; set; }

    }
}