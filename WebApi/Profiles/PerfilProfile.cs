using AutoMapper;
using Dtos;
using Models;

namespace WebApi.Profiles
{
    public class PerfilProfile: Profile
    {
        public PerfilProfile()
        {
            this.CreateMap<Perfil, PerfilDto>().ReverseMap();
        }
    }
}
