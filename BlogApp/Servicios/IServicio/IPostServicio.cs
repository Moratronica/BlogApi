using BlogApp.Modelos;

namespace BlogApp.Servicios.IServicio
{
    public interface IPostServicio
    {
        public Task<IEnumerable<Post>> GetPosts();
        public Task<Post> GetPost(int idPost);
        public Task<Post> CrearPost(Post post);
        public Task<Post> ActualizarPost(int idPost, Post post);
        public Task<bool> EliminarPost(int idPost);
        public Task<string> SubidaImagen(MultipartFormDataContent content);
    }
}

