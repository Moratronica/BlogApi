using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;

namespace BlazorAPIBlog.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int userId);
        bool IsUniqueUser(string usuario); 
        Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO);
        Task<Usuario> Registro(UsuarioRegisterDTO usuarioRegisterDTO);
        
    }
}
