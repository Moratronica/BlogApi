using System.ComponentModel.DataAnnotations;

namespace BlazorAPIBlog.Modelos.DTO
{
    public class UsuarioLoginDTO
    {
        [Required (ErrorMessage = "El Usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required (ErrorMessage = "La Contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
