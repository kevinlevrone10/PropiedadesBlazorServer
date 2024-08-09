using AutoMapper;
using PropiedadesBlazor.Modelos;
using PropiedadesBlazor.Modelos.DTO;

namespace PropiedadesBlazor.Mapper
{
    public class PerfilMapa : Profile
    {
        public PerfilMapa()
        {
            CreateMap<CategoriaDTO, Categoria>();
            CreateMap<Categoria, CategoriaDTO>();
            CreateMap<Propiedad, PropiedadDTO>().ReverseMap();
            CreateMap<Categoria, DropDownCategoriaDTO>().ReverseMap();
            CreateMap<ImagenPropiedad, ImagenPropiedadDTO>().ReverseMap();
        }
    }
}
