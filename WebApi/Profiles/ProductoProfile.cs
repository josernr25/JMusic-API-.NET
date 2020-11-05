using AutoMapper;
using Dots;
using Models;

namespace WebApi.Profiles
{
    public class ProductoProfile: Profile
    {
        public ProductoProfile()
        {
            this.CreateMap<Producto, ProductoDto>().ReverseMap();
        }
    }
}
