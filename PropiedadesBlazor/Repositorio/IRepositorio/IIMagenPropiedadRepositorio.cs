using PropiedadesBlazor.Modelos.DTO;

namespace PropiedadesBlazor.Repositorio.IRepositorio
{
    public interface IIMagenPropiedadRepositorio
    {
        public Task<int> CrearPropiedadImagen(ImagenPropiedadDTO imagenDTO);
        public Task<int> BorrarPropiedadImagenPorIdImagen(int imagenId);
        public Task<int> BorrarPropiedadImagenPorIdPropiedad(int propiedadId);
        public Task<int> BorrarPropiedadImagenPorUrlImagen(string imagenUrl);
        public Task<IEnumerable<ImagenPropiedadDTO>> GetImagenesPropiedad(int propiedadId);
    }
}
