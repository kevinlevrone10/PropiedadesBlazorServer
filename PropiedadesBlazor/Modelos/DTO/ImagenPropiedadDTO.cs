using System.ComponentModel.DataAnnotations.Schema;

namespace PropiedadesBlazor.Modelos.DTO
{
    public class ImagenPropiedadDTO
    {
        public int Id { get; set; }
        public int PropiedadId { get; set; }
        public string UrlImagenPropiedad { get; set; }        
    }
}
