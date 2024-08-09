using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropiedadesBlazor.Data;
using PropiedadesBlazor.Modelos;
using PropiedadesBlazor.Modelos.DTO;
using PropiedadesBlazor.Repositorio.IRepositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PropiedadesBlazor.Repositorio
{
    public class IMagenPropiedadRepositorio : IIMagenPropiedadRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;

        public IMagenPropiedadRepositorio(ApplicationDbContext bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public async Task<int> BorrarPropiedadImagenPorIdImagen(int imagenId)
        {
            var imagen = await _bd.ImagenPropiedad.FindAsync(imagenId);
            _bd.ImagenPropiedad.Remove(imagen);
            return await _bd.SaveChangesAsync();
        }

        public async Task<int> BorrarPropiedadImagenPorIdPropiedad(int propiedadId)
        {
            var listaImagenes = await _bd.ImagenPropiedad.Where(x => x.Id == propiedadId).ToListAsync();
            _bd.ImagenPropiedad.RemoveRange(listaImagenes);
            return await _bd.SaveChangesAsync();
        }

        public async Task<int> BorrarPropiedadImagenPorUrlImagen(string imagenUrl)
        {
            var todasImagenes = await _bd.ImagenPropiedad.FirstOrDefaultAsync
                                (x => x.UrlImagenPropiedad.ToLower() == imagenUrl.ToLower());
            if (todasImagenes == null)
            {
                return 0;
            }
            _bd.ImagenPropiedad.Remove(todasImagenes);
            return await _bd.SaveChangesAsync();
        }

        public async Task<int> CrearPropiedadImagen(ImagenPropiedadDTO imagenDTO)
        {
            var imagen = _mapper.Map<ImagenPropiedadDTO, ImagenPropiedad>(imagenDTO);
            await _bd.ImagenPropiedad.AddAsync(imagen);
            return await _bd.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImagenPropiedadDTO>> GetImagenesPropiedad(int propiedadId)
        {
            return _mapper.Map<IEnumerable<ImagenPropiedad>, IEnumerable<ImagenPropiedadDTO>>(
             await _bd.ImagenPropiedad.Where(x => x.Id == propiedadId).ToListAsync());
        }
    }
}
