using BlazorAPIBlog.Data;
using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;
using BlazorAPIBlog.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace BlazorAPIBlog.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;
        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int userId)
        {
            return _bd.Usuario.FirstOrDefault(c => c.Id == userId);
            
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuario.OrderBy(c => c.Id).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioBd = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
            return usuarioBd == null ? true : false;
        }


        public async Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDTO.Password);
            var usuario = _bd.Usuario.FirstOrDefault(
                u => u.NombreUsuario.ToLower() == usuarioLoginDTO.NombreUsuario.ToLower()
                &&
                u.Password == passwordEncriptado);

            // Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null) return new UsuarioLoginRespuestaDTO()
            {
                Token = "",
                usuario = null
            };

            // Aquí existe el usuario entonces podemos procesar el login
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    // new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDTO usuarioLoginRespuestaDTO = new UsuarioLoginRespuestaDTO()
            {
                Token = manejadorToken.WriteToken(token),
                usuario = usuario
            };

            return usuarioLoginRespuestaDTO;
        }

        public async Task<Usuario> Registro(UsuarioRegisterDTO usuarioRegisterDTO)
        {
            var passwordEncriptado = obtenermd5(usuarioRegisterDTO.Password);

            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegisterDTO.NombreUsuario,
                Nombre = usuarioRegisterDTO.Nombre,
                Email = usuarioRegisterDTO.Email,
                Password = passwordEncriptado
            };

            _bd.Usuario.Add(usuario);
            await _bd.SaveChangesAsync();
            return usuario;
        }


        // Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++) 
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}
