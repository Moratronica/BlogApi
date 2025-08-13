using System.ComponentModel.DataAnnotations;

namespace BlogApp.Modelos
{
    public class RespuestaAuth
    {
        public bool IsSuccess { get; set; }
        public string Token {  get; set; }
        public Usuario Usuario { get; set; }
    }
}
