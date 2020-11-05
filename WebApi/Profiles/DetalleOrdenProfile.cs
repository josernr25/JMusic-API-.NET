using AutoMapper;
using Dtos;
using Models;

namespace WebApi.Profiles
{
    public class DetalleOrdenProfile: Profile
    {
        public DetalleOrdenProfile()
        {
            this.CreateMap<DetalleOrden, DetalleOrdenDto>()
                .ForMember(u => u.Producto, p => p.MapFrom(u => u.Producto.Nombre))
                .ReverseMap()
                .ForMember(u => u.Producto, p => p.Ignore());
        }
    }
}
