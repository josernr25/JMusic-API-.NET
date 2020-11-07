using AutoMapper;
using Dtos;
using Models;

namespace WebApi.Profiles
{
    public class UsuarioProfile: Profile
    {
        public UsuarioProfile()
        {
            this.CreateMap<Usuario, UsuarioRegistroDto>()
                .ForMember(u => u.Perfil, p => p.MapFrom(m => m.Perfil.Nombre))
                .ReverseMap()
                .ForMember(u => u.Perfil, p => p.Ignore());

            this.CreateMap<Usuario, UsuarioActualizacionDto>()
                .ReverseMap();

            this.CreateMap<Usuario, UsuarioListaDto>()
                .ForMember(u => u.Perfil, p => p.MapFrom(m => m.Perfil.Nombre))
                .ForMember(u => u.NombreCompleto, p => p.MapFrom(m => string.Format("{0} {1}",
                        m.Nombre, m.Apellidos)))
                .ReverseMap();

            this.CreateMap<Usuario, LoginModelDto>().ReverseMap();

            this.CreateMap<Usuario, PerfilUsuarioDto>().ReverseMap();
        }
    }
}
