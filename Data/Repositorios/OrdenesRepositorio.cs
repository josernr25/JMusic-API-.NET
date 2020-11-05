using Data.Contratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositorios
{
    public class OrdenesRepositorio : IOrdenesRepositorio
    {
        private readonly TiendaDbContext _contexto;
        private readonly ILogger<OrdenesRepositorio> _logger;
        private DbSet<Orden> _dbSet;

        public OrdenesRepositorio(TiendaDbContext contexto, ILogger<OrdenesRepositorio> logger)
        {
            this._contexto = contexto;
            this._logger = logger;
            this._dbSet = _contexto.Set<Orden>();
        }

        public async Task<bool> Actualizar(Orden entity)
        {
            _dbSet.Attach(entity);
            _contexto.Entry(entity).State = EntityState.Modified;
            try
            {
                return await _contexto.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Actualizar)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<Orden> Agregar(Orden entity)
        {
            entity.EstatusOrden = OrdenEstatus.Activo;
            entity.FechaRegistro = DateTime.UtcNow;            
            _dbSet.Add(entity);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Agregar)}: " + excepcion.Message);
                return null;
            }
            return entity;
        }

        public async Task<bool> Eliminar(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(u => u.Id == id);
            entity.EstatusOrden = OrdenEstatus.Inactivo;            
            try
            {
                return (await _contexto.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Eliminar)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<Orden> ObtenerAsync(int id)
        {
            return await _dbSet.Include(orden=>orden.Usuario)                    
                                .SingleOrDefaultAsync(c => c.Id == id 
                                && c.EstatusOrden == OrdenEstatus.Activo);
        }

        public async Task<Orden> ObtenerConDetallesAsync(int id)
        {
            return await _dbSet.Include(orden => orden.Usuario)
                                .Include(orden=>orden.DetalleOrden)
                                    .ThenInclude(detalleOrden => detalleOrden.Producto)
                                .SingleOrDefaultAsync(c => c.Id == id 
                                && c.EstatusOrden == OrdenEstatus.Activo);
        }

        public async Task<IEnumerable<Orden>> ObtenerTodosAsync()
        {
            return await _dbSet.Where(u=>u.EstatusOrden== OrdenEstatus.Activo)
                                .Include(orden => orden.Usuario)                                                               
                                .ToListAsync();
        }

        public async Task<IEnumerable<Orden>> ObtenerTodosConDetallesAsync()
        {
            return await _dbSet.Where(u => u.EstatusOrden == OrdenEstatus.Activo)
                                .Include(orden => orden.Usuario)
                                .Include(orden => orden.DetalleOrden)
                                    .ThenInclude(detalleOrden => detalleOrden.Producto)
                                .ToListAsync();
        }

    }
}
