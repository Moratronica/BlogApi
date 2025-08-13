using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlogApp.Pages.Autentication
{
    public partial class RedireccionarAlAcceso
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> estadoProveedorAutentication { get; set; }

        bool noAutorizado { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var estadoAutorizacion = await estadoProveedorAutentication;

            if(estadoAutorizacion.User == null)
            {
                var returnUrl = navigationManager.ToBaseRelativePath(navigationManager.Uri);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    navigationManager.NavigateTo("Acceder", true);
                }
                else
                {
                    navigationManager.NavigateTo($"Acceder?returnUrl={returnUrl}", true);  
                }
            }
            else
            {
                noAutorizado = false;
            }
        }
    }
}
