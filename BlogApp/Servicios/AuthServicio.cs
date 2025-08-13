using Blazored.LocalStorage;
using BlogApp.Helpers;
using BlogApp.Modelos;
using BlogApp.Servicios.IServicio;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace BlogApp.Servicios
{
    public class AuthServicio : IAuthServicio
    {

        private readonly HttpClient _cliente;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _estadoProveedorAuth;

        public AuthServicio(HttpClient cliente, 
            ILocalStorageService localStorageService, 
            AuthenticationStateProvider estadoProveedorAuth)
        {
            _cliente = cliente;
            _localStorageService = localStorageService;
            _estadoProveedorAuth = estadoProveedorAuth;
        }

        public async Task<RespuestaAuth> Acceder(UsuarioAuth UsuarioAcceso)
        {
            var content = JsonConvert.SerializeObject(UsuarioAcceso);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _cliente.PostAsync(Rutas.Login, bodyContent);
            var contenidoTemp = await response.Content.ReadAsStringAsync();
            var resultado = (JObject)JsonConvert.DeserializeObject(contenidoTemp);

            if(response.IsSuccessStatusCode)
            {
                var Token = resultado["result"]["token"].Value<string>();
                var Usuario = resultado["result"]["usuario"]["nombreUsuario"].Value<string>();

                await _localStorageService.SetItemAsync(Inicializar.Token_Local, Token);
                await _localStorageService.SetItemAsync(Inicializar.DatosUsuario_Local, Usuario);
                ((AuthStateProvider)_estadoProveedorAuth).NotificarUsuarioLogeado(Token);
                _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
                return new RespuestaAuth { IsSuccess = true };
            }
            else
            {
                return new RespuestaAuth { IsSuccess = false };
            }
        }

        public async Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro UsuarioNuevo)
        {
            var content = JsonConvert.SerializeObject(UsuarioNuevo);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _cliente.PostAsync(Rutas.Registro, bodyContent);
            var contenidoTemp = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaRegistro>(contenidoTemp);

            if (response.IsSuccessStatusCode)
            {                
                return new RespuestaRegistro { RegistroCorrecto = true };
            }
            else
            {
                return resultado;
            }
        }

        public async Task Salir()
        {
            // Borramos el token y los datos del usuario
            await _localStorageService.RemoveItemAsync(Inicializar.Token_Local);
            await _localStorageService.RemoveItemAsync(Inicializar.DatosUsuario_Local);

            ((AuthStateProvider)_estadoProveedorAuth).NotificarUsuarioSalir();
            _cliente.DefaultRequestHeaders.Authorization = null;
        }
    }
}
