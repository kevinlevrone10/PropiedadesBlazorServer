using PropiedadesBlazor.Modelos.DTO;

namespace PropiedadesBlazor.Repositorio.IRepositorio
{
    public interface IPropiedadRepositorio
    {


      public Task<IEnumerable<PropiedadDTO>> GetAllPropiedades(int categoriaId, int pageIndex, int pageSize);

        public Task<IEnumerable<PropiedadDTO>> GetAll2Propiedades();


        public  Task<int> CountPropiedades(); // Método para contar todas las propiedades
      public Task<IEnumerable<PropiedadDTO>> GetPropiedadesPorCategoria(int categoriaId, int pageIndex, int pageSize);
      public  Task<int> CountPropiedadesPorCategoria(int categoriaId); // Método para contar propiedades por categoría


        


        public Task<IEnumerable<PropiedadDTO>> GetAllPropiedades4();
        public Task<PropiedadDTO> GetPropiedad(int propiedadId);
        public Task<PropiedadDTO> CrearPropiedad(PropiedadDTO propiedadDTO);

        public Task<PropiedadDTO> ActualizarPropiedad(int propiedadId, PropiedadDTO propiedadDTO);

        public Task<int> BorrarPropiedad(int propiedadId);

        public Task<PropiedadDTO> NombrePropiedadExiste(string nombre);       

    }
}
