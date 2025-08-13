using System.ComponentModel.DataAnnotations;

namespace BlogApp.Modelos
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
