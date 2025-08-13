using BlogApp.Helpers;
using BlogApp.Modelos;
using BlogApp.Servicios.IServicio;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace BlogApp.Servicios
{
    public class PostServicio : IPostServicio
    {
        private readonly HttpClient _cliente;
        public PostServicio(HttpClient cliente)
        {
            _cliente = cliente;
        }
        public async Task<Post> ActualizarPost(int idPost, Post post)
        {
            // Convertimos a Json
            var contenido = JsonConvert.SerializeObject(post);

            // Codificamos a Json de UTF8 para que no haya problemas
            var bodyContent = new StringContent(contenido, Encoding.UTF8, "application/json");

            var respuesta = await _cliente.PatchAsync(Rutas.Post + idPost, bodyContent);

            // Recoge el Json de la respuesta
            var contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.IsSuccessStatusCode)
            {
                // Convertimos el contenido Json en una lista de post
                var resultado = JsonConvert.DeserializeObject<Post>(contenidoRespuesta);

                return resultado;
            }

            // Convertimos el contenido Json en una lista de post
            var errorModel = JsonConvert.DeserializeObject<ModeloError>(contenidoRespuesta);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<Post> CrearPost(Post post)
        {
            // Convertimos a Json
            var contenido = JsonConvert.SerializeObject(post);

             // Codificamos a Json de UTF8 para que no haya problemas
            var bodyContent = new StringContent(contenido, Encoding.UTF8, "application/json");

            var respuesta = await _cliente.PostAsync(Rutas.Post, bodyContent);

            // Recoge el Json de la respuesta
            var contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.IsSuccessStatusCode)
            {
                // Convertimos el contenido Json en una lista de post
                var resultado = JsonConvert.DeserializeObject<Post>(contenidoRespuesta);

                return resultado;
            }

            // Convertimos el contenido Json en una lista de post
            var errorModel = JsonConvert.DeserializeObject<ModeloError>(contenidoRespuesta);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<bool> EliminarPost(int idPost)
        {
            // Visita la ruta haciendo una peticion get asincrona
            var respuesta = await _cliente.DeleteAsync(Rutas.Post + idPost);                       

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }

            // Recoge el Json de la respuesta
            var contenido = await respuesta.Content.ReadAsStringAsync();

            // Convertimos el contenido Json en una lista de post
            var errorModel = JsonConvert.DeserializeObject<ModeloError>(contenido);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<Post> GetPost(int idPost)
        {
            // Visita la ruta haciendo una peticion get asincrona
            var respuesta = await _cliente.GetAsync(Rutas.Post + idPost);

            // Recoge el Json de la respuesta
            var contenido = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.IsSuccessStatusCode)
            {  
                // Convertimos el contenido Json en una lista de post
                var post = JsonConvert.DeserializeObject<Post>(contenido);

                return post;
            }

            // Convertimos el contenido Json en una lista de post
            var errorModel = JsonConvert.DeserializeObject<ModeloError>(contenido);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            // Visita la ruta haciendo una peticion get asincrona
            var respuesta = await _cliente.GetAsync(Rutas.Post);

            // Recoge el Json de la respuesta
            var contenido = await respuesta.Content.ReadAsStringAsync();

            // Convertimos el contenido Json en una lista de post
            var post = JsonConvert.DeserializeObject<IEnumerable<Post>>(contenido);

            return post;
        }

        public async Task<string> SubidaImagen(MultipartFormDataContent content)
        {
            var postResult = await _cliente.PostAsync(Rutas.SubidaImagen, content);
            var postContent = await postResult.Content.ReadAsStringAsync();
            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            } 
            else
            {
                var imagenPost = Path.Combine(Rutas.Direccion, postContent);
                return imagenPost;
            }
        }
    }
}
