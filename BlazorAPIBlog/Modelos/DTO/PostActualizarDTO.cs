using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace BlazorAPIBlog.Modelos.DTO
{
    public class PostActualizarDTO
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "El titulo es obligatorio")]
        public string Titulo { get; set; }
        [Required (ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "Las etiquetas son obligatorias")]
        public string Etiquetas { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
