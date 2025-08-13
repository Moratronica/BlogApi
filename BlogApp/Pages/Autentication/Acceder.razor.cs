using BlogApp.Modelos;
using BlogApp.Servicios.IServicio;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace BlogApp.Pages.Autentication
{
    public partial class Acceder
    {
        private UsuarioAuth usuarioAuth = new UsuarioAuth();
        public bool EstaProcesando { get; set; } = false;
        public bool MostrarErroresAuth { get; set; }
        public string UrlRetorno { get; set; }
        public string Errores { get; set; }
        [Inject]
        public IAuthServicio AuthServicio { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        public async Task AccesoUsuario()
        {
            MostrarErroresAuth = false;
            EstaProcesando = true;
            var result = await AuthServicio.Acceder(usuarioAuth);
            EstaProcesando = false;
            if (result.IsSuccess)
            {
                var UrlAbsoluta = new Uri(navigationManager.Uri);
                var ParametrosQuery = HttpUtility.ParseQueryString(UrlAbsoluta.Query);
                UrlRetorno = ParametrosQuery["returnUrl"];

                if (string.IsNullOrEmpty(UrlRetorno))
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    navigationManager.NavigateTo("/" + UrlRetorno);
                }
                    
            }
            else
            {                
                MostrarErroresAuth = true;
                Errores = "Usuario y/o Contraseña incorrectos";
            }
        }
    }
}
