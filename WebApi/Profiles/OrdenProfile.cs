using AutoMapper;
using Dtos;
using Models;

namespace WebApi.Profiles
{
    public class OrdenProfile: Profile
    {
        public OrdenProfile()
        {
            this.CreateMap<Orden, OrdenDto>()
                .ForMember(u => u.Usuario, p => p.MapFrom(m => m.Usuario.Username))
                .ReverseMap()
                .ForMember(u => u.Usuario, p => p.Ignore());
        }
    }
}
