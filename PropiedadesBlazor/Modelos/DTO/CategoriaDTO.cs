using System.ComponentModel.DataAnnotations;

namespace PropiedadesBlazor.Modelos.DTO
{
    public class CategoriaDTO
    {        
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
    }
}
