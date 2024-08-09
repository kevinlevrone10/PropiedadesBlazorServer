using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropiedadesBlazor.Data; // Asegúrate de incluir el espacio de nombres para tu DbContext
using PropiedadesBlazor.Modelos.DTO;
using PropiedadesBlazor.Modelos; // Asegúrate de incluir el espacio de nombres de tus modelos
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropiedadesBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropiedadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Reemplaza ApplicationDbContext con el nombre de tu DbContext

        public PropiedadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Obtener todas las propiedades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropiedadDTO>>> GetAllPropiedades([FromQuery] int categoriaId = 0)
        {
            var propiedades = _context.Propiedad.AsQueryable();

            // Filtrar por categoría si se proporciona
            if (categoriaId > 0)
            {
                propiedades = propiedades.Where(p => p.CategoriaId == categoriaId);
            }

            var propiedadesDTO = await propiedades.Select(p => new PropiedadDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CategoriaId = p.CategoriaId,
                Descripcion = p.Descripcion,
                Precio = p.Precio
            }).ToListAsync();

            if (!propiedadesDTO.Any())
            {
                return NotFound("No se encontraron propiedades.");
            }

            return Ok(propiedadesDTO);
        }

        // 2. Obtener una propiedad por ID
        [HttpGet("{propiedadId}")]
        public async Task<ActionResult<PropiedadDTO>> GetPropiedad(int propiedadId)
        {
            var propiedad = await _context.Propiedad.FindAsync(propiedadId);
            if (propiedad == null) return NotFound("Propiedad no encontrada.");

            var propiedadDTO = new PropiedadDTO
            {
                Id = propiedad.Id,
                Nombre = propiedad.Nombre,
                CategoriaId = propiedad.CategoriaId,
                Descripcion = propiedad.Descripcion,
                Precio = propiedad.Precio
            };

            return Ok(propiedadDTO);
        }

        // 3. Obtener propiedades por rango de precio
        [HttpGet("rango-precio")]
        public async Task<ActionResult<IEnumerable<PropiedadDTO>>> GetPropiedadesPorRangoPrecio([FromQuery] double precioMinimo, [FromQuery] double precioMaximo)
        {
            var propiedades = await _context.Propiedad
                .Where(p => p.Precio >= precioMinimo && p.Precio <= precioMaximo)
                .Select(p => new PropiedadDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    CategoriaId = p.CategoriaId,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                }).ToListAsync();

            if (!propiedades.Any())
            {
                return NotFound("No se encontraron propiedades en el rango de precio especificado.");
            }

            return Ok(propiedades);
        }

        // 4. Contar propiedades
        [HttpGet("contar")]
        public async Task<ActionResult<int>> ContarPropiedades()
        {
            var totalPropiedades = await _context.Propiedad.CountAsync();
            return Ok(totalPropiedades);
        }

        // 5. Obtener propiedades por nombre
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<PropiedadDTO>>> BuscarPropiedadesPorNombre([FromQuery] string nombre)
        {
            var propiedades = await _context.Propiedad
                .Where(p => p.Nombre.Contains(nombre))
                .Select(p => new PropiedadDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    CategoriaId = p.CategoriaId,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                }).ToListAsync();

            if (!propiedades.Any())
            {
                return NotFound("No se encontraron propiedades con ese nombre.");
            }

            return Ok(propiedades);
        }

        // 6. Obtener propiedades por categoría
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<PropiedadDTO>>> GetPropiedadesPorCategoria(int categoriaId)
        {
            var propiedades = await _context.Propiedad
                .Where(p => p.CategoriaId == categoriaId)
                .Select(p => new PropiedadDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    CategoriaId = p.CategoriaId,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                }).ToListAsync();

            if (!propiedades.Any())
            {
                return NotFound("No se encontraron propiedades en esta categoría.");
            }

            return Ok(propiedades);
        }

        // 7. Obtener las propiedades más caras
        [HttpGet("mas-caras")]
        public async Task<ActionResult<IEnumerable<PropiedadDTO>>> GetPropiedadesMasCaras([FromQuery] int cantidad = 5)
        {
            var propiedades = await _context.Propiedad
                .OrderByDescending(p => p.Precio)
                .Take(cantidad)
                .Select(p => new PropiedadDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    CategoriaId = p.CategoriaId,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                }).ToListAsync();

            if (!propiedades.Any())
            {
                return NotFound("No se encontraron propiedades.");
            }

            return Ok(propiedades);
        }

        // 8. Eliminar una propiedad por ID
        [HttpDelete("{propiedadId}")]
        public async Task<ActionResult<int>> BorrarPropiedad(int propiedadId)
        {
            var propiedad = await _context.Propiedad.FindAsync(propiedadId);
            if (propiedad == null) return NotFound("Propiedad no encontrada para eliminar.");

            _context.Propiedad.Remove(propiedad);
            await _context.SaveChangesAsync();

            return Ok("Propiedad eliminada con éxito.");
        }
    }
}
