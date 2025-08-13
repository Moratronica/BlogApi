using BlazorAPIBlog.Data;
using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace BlazorAPIBlog.Repositorio
{
    public class PostRepositorio : IPostRepositorio
    {
        private readonly ApplicationDbContext _bd;
        public PostRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarPost(Post actualizarPost)
        {
            actualizarPost.FechaActualizacion = DateTime.Now;
            var imagenDesdeBd = _bd.Post.AsNoTracking().FirstOrDefault(c => c.Id == actualizarPost.Id);

            if (actualizarPost.RutaImagen == null)
            {
                actualizarPost.RutaImagen = imagenDesdeBd.RutaImagen;
            }

            _bd.Post.Update(actualizarPost);
            return Guardar();
        }

        public bool BorrarPost(Post borrarPost)
        {
            _bd.Post.Remove(borrarPost);
            return Guardar();
        }

        public bool CrearPost(Post crearPost)
        {
            crearPost.FechaCreacion = DateTime.Now;
            _bd.Post.Add(crearPost);
            return Guardar();
        }

        public bool ExistePost(string tituloPost)
        {
            bool valor = _bd.Post.Any(c => c.Titulo.ToLower().Trim() == tituloPost.ToLower().Trim());
            return valor;
        }

        public bool ExistePost(int id)
        {
            bool valor = _bd.Post.Any(c => c.Id == id);
            return valor;
        }

        public Post GetPost(int postId)
        {
            return _bd.Post.FirstOrDefault(c => c.Id == postId);
        }

        public ICollection<Post> GetPosts()
        {
            return _bd.Post.OrderBy(c => c.Id).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false; 
        }
    }
}
