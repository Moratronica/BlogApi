using AutoMapper;
using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;
using BlazorAPIBlog.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorAPIBlog.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _userRepo;
        protected RespuestasAPI _respuestasAPI;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _respuestasAPI = new RespuestasAPI();
            _mapper = mapper;
        }

        [AllowAnonymous] // publico no necesitas cuenta
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegisterDTO usuarioRegisterDTO)
        {
            bool validarNombreUsuaioUnico = _userRepo.IsUniqueUser(usuarioRegisterDTO.NombreUsuario);
            if (!validarNombreUsuaioUnico)
            {
                _respuestasAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessages.Add("El Nombre de Usuario ya existe"); 
                return BadRequest(_respuestasAPI);
            }


            var usuario = await _userRepo.Registro(usuarioRegisterDTO);
            if (usuario == null) {
                _respuestasAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessages.Add("Error en el Registro");
                return BadRequest(_respuestasAPI);
            }

            _respuestasAPI.StatusCode = HttpStatusCode.OK;
            _respuestasAPI.IsSuccess = true;
            return Ok(_respuestasAPI);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            
            var respuestaLogin = await _userRepo.Login(usuarioLoginDTO);
            if (respuestaLogin.usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestasAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessages.Add("El nombre de usuario o contraseña son incorrectos");
                return BadRequest(_respuestasAPI);
            }

            _respuestasAPI.StatusCode = HttpStatusCode.OK;
            _respuestasAPI.IsSuccess = true;
            _respuestasAPI.Result = respuestaLogin;
            return Ok(_respuestasAPI);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _userRepo.GetUsuarios();

            var listaUsuariosDTO = new List<UsuarioDTO>();

            foreach (var usuario in listaUsuarios)
            {
                listaUsuariosDTO.Add(_mapper.Map<UsuarioDTO>(usuario));
            }

            return Ok(listaUsuariosDTO);
        }

        [Authorize]
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _userRepo.GetUsuario(usuarioId);

            if (itemUsuario == null) return NotFound();

            var itemUsuarioDTO = _mapper.Map<UsuarioDTO>(itemUsuario);

            return Ok(itemUsuarioDTO);
        }



    }

}
