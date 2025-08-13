using AutoMapper;
using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;
using BlazorAPIBlog.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAPIBlog.Controllers
{
    [Route("api/posts")] // navegar a este endpoint
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepositorio _postRepo;
        private readonly IMapper _mapper;

        public PostsController(IPostRepositorio postRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _mapper = mapper;
        }


        [AllowAnonymous] // Para que no necesites cuenta
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPosts()
        {
            var listaPosts = _postRepo.GetPosts();

            var listaPostsDTO = new List<PostDTO>();

            // pasamos de post a postDTO
            foreach (var post in listaPosts)
            {
                listaPostsDTO.Add(_mapper.Map<PostDTO>(post));
            }
            return Ok(listaPostsDTO);
        }

        [AllowAnonymous] // Para que no necesites cuenta
        [HttpGet("{postId:int}", Name = "GetPost")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPost(int postId)
        {
            var itemPost = _postRepo.GetPost(postId);

            if (itemPost == null) return NotFound();

            var itemPostDTO = _mapper.Map<PostDTO>(itemPost);

            return Ok(itemPostDTO);
        }


        [Authorize] // necesitas tener cuenta
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPost([FromBody]PostCrearDTO crearPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPostDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (_postRepo.ExistePost(crearPostDTO.Titulo))
            {
                ModelState.AddModelError("", "El post ya existe");
                return StatusCode(404, ModelState);
            }

            var itemPost = _mapper.Map<Post>(crearPostDTO);

            if (!_postRepo.CrearPost(itemPost))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{itemPost.Titulo}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPost", new { postId = itemPost.Id }, itemPost);
        }


        [Authorize]
        [HttpPatch("{postId:int}", Name = "ActualizarPatchPost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPost(int postId, [FromBody] PostActualizarDTO actualizarPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarPostDTO == null || postId != actualizarPostDTO.Id)
            {
                return BadRequest(ModelState);
            }

            Post postAactualizar = _postRepo.GetPost(postId);

            string tituloAnterior = postAactualizar.Titulo;

            if (_postRepo.ExistePost(actualizarPostDTO.Titulo) && tituloAnterior != actualizarPostDTO.Titulo)
            {
                ModelState.AddModelError("", "El post ya existe");
                return StatusCode(404, ModelState);
            }
            
            _mapper.Map(actualizarPostDTO, postAactualizar);

            if (!_postRepo.ActualizarPost(postAactualizar))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{postAactualizar.Titulo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [Authorize]
        [HttpDelete("{postId:int}", Name = "BorrarPost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public IActionResult BorrarPost(int postId)
        {
            if (!_postRepo.ExistePost(postId))
            {
                return NotFound();
            }

            var post = _postRepo.GetPost(postId);

            if(!_postRepo.BorrarPost(post))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {post.Titulo}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
