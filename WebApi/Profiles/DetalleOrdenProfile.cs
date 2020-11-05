using AutoMapper;
using Dots;
using Models;

namespace WebApi.Profiles
{
    public class DetalleOrdenProfile: Profile
    {
        public DetalleOrdenProfile()
        {
            this.CreateMap<DetalleOrden, DetalleOrdenDto>().ReverseMap();
        }
    }
}
