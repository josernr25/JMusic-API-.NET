using AutoMapper;
using Dots;
using Models;

namespace WebApi.Profiles
{
    public class OrdenProfile: Profile
    {
        public OrdenProfile()
        {
            this.CreateMap<Orden, OrdenDto>().ReverseMap();
        }
    }
}
