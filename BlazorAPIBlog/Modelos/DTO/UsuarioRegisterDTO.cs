using System.ComponentModel.DataAnnotations;

namespace BlazorAPIBlog.Modelos
{
    public class UsuarioRegisterDTO
    {
        [Required (ErrorMessage = "El Usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required (ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Correo es obligatorio")]
        public string Email { get; set; }

        [Required (ErrorMessage = "La Contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
