using AutoMapper;
using BlazorAPIBlog.Modelos;
using BlazorAPIBlog.Modelos.DTO;

namespace BlazorAPIBlog.Mapper
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {   
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, PostCrearDTO>().ReverseMap();
            CreateMap<Post, PostActualizarDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
