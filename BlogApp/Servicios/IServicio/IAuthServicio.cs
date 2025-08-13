using BlogApp.Modelos;

namespace BlogApp.Servicios.IServicio
{
    public interface IAuthServicio
    {
        Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro UsuarioNuevo);
        Task<RespuestaAuth> Acceder (UsuarioAuth UsuarioAcceso);
        Task Salir();
    }
}
