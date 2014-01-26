using System;
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
        public Guid pk { get; set; }

        [Display(Name="Correo")]
        [Required(ErrorMessage="Es necesario establecer un correo")]
        [DataType(DataType.EmailAddress, ErrorMessage="Correo no valido")]
        [StringLength(100, ErrorMessage="La cantidad de caracteres permitida fue exedida")]
        public string correo { get; set; }

        [Display(Name="Nombre")]
        [Required(ErrorMessage="Falta establecer un nombre")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string nombre { get; set; }

        [Display(Name="Apellido")]
        [Required(ErrorMessage="Falta establecer un apellido")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string apellido { get; set; }

        [Display(Name="Nombre de Usuario")]
        [Required(ErrorMessage="Falta el Nombre de Usuario")]
        [DataType(DataType.Text, ErrorMessage="No es texto permitido")]
        public string nombre_usuario { get; set; }

        [Display(Name="Contraseña")]
        [Required(ErrorMessage="Falta la contraseña")]
        [DataType(DataType.Password, ErrorMessage="Contraseña invalida")]        
        public string contrasenia { get; set; }


        [Display(Name = "IsAdmin")]
        [Required(ErrorMessage="Falta es campo")]
        public bool is_admin { get; set; }

        [Display(Name="Salt")]        
        public string salt { get; set; }

        
        public ICollection<Indicador> provee { get; set; }
        public ICollection<Area> consume { get; set; }

        public Usuario()
        {
            this.provee = new HashSet<Indicador>();
            this.consume = new HashSet<Area>();
        }
    }
}