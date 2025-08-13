using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;

namespace BlazorAPIBlog.Repositorio.IRepositorio
{
    public interface IPostRepositorio
    {
        ICollection<Post> GetPosts();
        Post GetPost(int postId);
        bool ExistePost(string tituloPost);
        bool ExistePost(int id);
        bool CrearPost(Post crearPost);
        bool ActualizarPost(Post actualizarPost);
        bool BorrarPost(Post borrarPost);
        bool Guardar();

    }
}
