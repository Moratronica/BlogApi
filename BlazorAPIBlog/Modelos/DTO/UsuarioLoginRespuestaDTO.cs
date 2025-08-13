using System.ComponentModel.DataAnnotations;

namespace BlazorAPIBlog.Modelos.DTO
{
    public class UsuarioLoginRespuestaDTO
    {
        public Usuario usuario { get; set; }
        public string Token { get; set; }
    }
}
