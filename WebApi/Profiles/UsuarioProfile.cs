using AutoMapper;
using Dots;
using Models;

namespace WebApi.Profiles
{
    public class UsuarioProfile: Profile
    {
        public UsuarioProfile()
        {
            this.CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
