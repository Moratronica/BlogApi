using System.ComponentModel.DataAnnotations;

namespace BlogApp.Modelos
{
    public class UsuarioAuth
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
