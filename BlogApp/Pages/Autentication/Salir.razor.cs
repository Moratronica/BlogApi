using BlogApp.Servicios.IServicio;
using Microsoft.AspNetCore.Components;

namespace BlogApp.Pages.Autentication
{
    public partial class Salir
    {
        [Inject]
        IAuthServicio authServicio { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await authServicio.Salir();
            navigationManager.NavigateTo("/");
        }
    }
}
