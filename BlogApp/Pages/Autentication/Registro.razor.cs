using BlogApp.Modelos;
using BlogApp.Servicios.IServicio;
using Microsoft.AspNetCore.Components;

namespace BlogApp.Pages.Autentication
{
    public partial class Registro
    {
        private UsuarioRegistro UsuarioParaRegistro = new UsuarioRegistro();
        public bool EstaProcesando { get; set; } = false;
        public bool MostrarErroresRegistro { get; set; }
        public IEnumerable<string> Errores { get; set; }
        [Inject]
        public IAuthServicio AuthServicio { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private async Task RegistrarUsuario()
        {
            MostrarErroresRegistro = false;
            EstaProcesando = true;
            var result = await AuthServicio.RegistrarUsuario(UsuarioParaRegistro);
            EstaProcesando = false;
            if (result.RegistroCorrecto)
            {
                navigationManager.NavigateTo("/Acceder");
            }
            else
            {
                Errores = result.Errores;
                MostrarErroresRegistro = true;
            }
        }
    }
}
