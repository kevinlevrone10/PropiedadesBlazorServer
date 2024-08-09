using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropiedadesBlazor.Data;
using PropiedadesBlazor.Modelos;
using PropiedadesBlazor.Modelos.DTO;
using PropiedadesBlazor.Repositorio.IRepositorio;
using System.Drawing.Printing;

namespace PropiedadesBlazor.Repositorio
{
    public class PropiedadRepositorio : IPropiedadRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;

        public PropiedadRepositorio(ApplicationDbContext bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }


        public async Task<PropiedadDTO> ActualizarPropiedad(int propiedadId, PropiedadDTO propiedadDTO)
        {
            try
            {
                if (propiedadId == propiedadDTO.Id)
                {
                    //Válido para actualizar
                    Propiedad propiedad = await _bd.Propiedad.FindAsync(propiedadId);
                    Propiedad propie = _mapper.Map<PropiedadDTO, Propiedad>(propiedadDTO, propiedad);
                    propie.FechaActualizacion = DateTime.Now;
                    var propiedadActualizada = _bd.Propiedad.Update(propie);
                    await _bd.SaveChangesAsync();
                    return _mapper.Map<Propiedad, PropiedadDTO>(propiedadActualizada.Entity);
                }
                else
                {
                    //Inválido
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<int> BorrarPropiedad(int propiedadId)
        {
            var propiedad = await _bd.Propiedad.FindAsync(propiedadId);
            if (propiedad != null)
            {
                var todasImagenes = await _bd.ImagenPropiedad.Where(x => x.Id == propiedadId).ToListAsync();
                _bd.ImagenPropiedad.RemoveRange(todasImagenes);
                _bd.Propiedad.Remove(propiedad);
                return await _bd.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> CountPropiedades()
        {
            return await _bd.Propiedad.CountAsync();
        }

        public async Task<int> CountPropiedadesPorCategoria(int categoriaId)
        {
            return await _bd.Propiedad.CountAsync(p => p.CategoriaId == categoriaId);
        }

        public async Task<PropiedadDTO> CrearPropiedad(PropiedadDTO propiedadDTO)
        {
            Propiedad propiedad = _mapper.Map<PropiedadDTO, Propiedad>(propiedadDTO);
            propiedad.FechaCreacion = DateTime.Now;
            var propiedadAgregada = await _bd.Propiedad.AddAsync(propiedad);
            await _bd.SaveChangesAsync();
            return _mapper.Map<Propiedad, PropiedadDTO>(propiedadAgregada.Entity);
        }

        public async Task<IEnumerable<PropiedadDTO>> GetAll2Propiedades()
        {
            try
            {
                // Cargar las propiedades desde la base de datos
                var propiedades = await _bd.Propiedad
                    .Include(x => x.ImagenPropiedad)
                    .Include(c => c.Categoria)
                    .ToListAsync(); // Ejecuta la consulta y convierte el resultado a una lista

                // Mapear solo las primeras 4 propiedades
                IEnumerable<PropiedadDTO> propiedadesDTO =
                            _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>(propiedades);

                return propiedadesDTO;
            }
            catch (Exception ex)
            {
                // Registrar el error para diagnóstico
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<PropiedadDTO>(); // Retorna una lista vacía en lugar de null
            }
        }

        //public async Task<IEnumerable<PropiedadDTO>> GetAll2Propiedades()
        //{
        //    //Versión 1
        //    //try
        //    //{
        //    //    IEnumerable<PropiedadDTO> propiedadesDTO =
        //    //                _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>
        //    //                (_bd.Propiedad);
        //    //    return (propiedadesDTO);
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    return null;
        //    //}

        //    //Versión 2
        //    try
        //    {
        //        IEnumerable<PropiedadDTO> propiedadesDTO =
        //                    _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>
        //                    (_bd.Propiedad.Include(x => x.ImagenPropiedad).Include(c => c.Categoria));
        //        return propiedadesDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async Task<IEnumerable<PropiedadDTO>> GetAllPropiedades(int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        var propiedades = await _bd.Propiedad
        //            .Include(x => x.ImagenPropiedad)
        //            .Include(c => c.Categoria)
        //            .Skip(pageIndex * pageSize) // Salta las propiedades de las páginas anteriores
        //            .Take(pageSize) // Toma el número de propiedades especificado
        //            .ToListAsync();

        //        return _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>(propiedades);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<IEnumerable<PropiedadDTO>> GetAllPropiedades(int categoriaId, int pageIndex, int pageSize)
        {
            try
            {
                IQueryable<Propiedad> query = _bd.Propiedad.Include(x => x.ImagenPropiedad).Include(c => c.Categoria);

                if (categoriaId > 0)
                {
                    query = query.Where(p => p.CategoriaId == categoriaId); // Filtrar por categoría si se proporciona
                }

                var propiedades = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>(propiedades);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PropiedadDTO>> GetAllPropiedades4()
        {

            try
            {
                // Cargar solo las primeras 4 propiedades desde la base de datos
                var propiedades = await _bd.Propiedad
                    .Include(x => x.ImagenPropiedad)
                    .Include(c => c.Categoria)
                    .Take(4) // Limitar la consulta a 4 propiedades
                    .ToListAsync(); // Ejecuta la consulta y convierte el resultado a una lista

                // Mapear las propiedades a DTOs
                var propiedadesDTO = _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>(propiedades);

                return propiedadesDTO;
            }
            catch (Exception ex)
            {
                // Registrar el error para diagnóstico
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<PropiedadDTO>(); // Retorna una lista vacía en lugar de null
            }


        }

        public async Task<PropiedadDTO> GetPropiedad(int propiedadId)
        {
            try
            {
                PropiedadDTO propiedadDTO =
                           _mapper.Map<Propiedad, PropiedadDTO>
                           (await _bd.Propiedad.FirstOrDefaultAsync(c => c.Id == propiedadId));
                return propiedadDTO;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<IEnumerable<PropiedadDTO>> GetPropiedadesPorCategoria(int categoriaId, int skip, int take)
        {
            return (IEnumerable<PropiedadDTO>)await _bd.Propiedad
      .Include(x => x.ImagenPropiedad) // Asegúrate de incluir las imágenes si es necesario
      .Include(c => c.Categoria) // Incluye la categoría
      .Where(p => p.CategoriaId == categoriaId)
      .Skip(skip)
      .Take(take)
      .ToListAsync(); // Cambié de Select a ToListAsync para asegurarnos de que se ejecuta la consulta


        }

        public async Task<PropiedadDTO> NombrePropiedadExiste(string nombre)
        {
            try
            {
                PropiedadDTO propiedadDTO =
                           _mapper.Map<Propiedad, PropiedadDTO>
                           (await _bd.Propiedad.FirstOrDefaultAsync(
                               c => c.Nombre.ToLower() == nombre.ToLower()));
                return propiedadDTO;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
