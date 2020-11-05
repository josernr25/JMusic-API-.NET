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

    public class ProductosRepositorio : IProductosRepositorio
    {
        private readonly TiendaDbContext _contexto;
        private readonly ILogger _logger;

        public ProductosRepositorio(TiendaDbContext contexto, ILogger<ProductosRepositorio> logger)
        {
            _contexto = contexto;
            _logger = logger;
        }

        public async Task<bool> Actualizar(Producto producto)
        {
            var productoDb = await ObtenerProductoAsync(producto.Id);

            productoDb.Nombre = producto.Nombre;
            productoDb.Precio = producto.Precio;

            //_contexto.Productos.Attach(producto);
            //_contexto.Entry(producto).State = EntityState.Modified;
            try
            {
                return await _contexto.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Actualizar) }: ${ ex.Message }");
            }
            return false;
        }

        public async Task<Producto> Agregar(Producto producto)
        {
            producto.Estatus = ProductoEstatus.Activo;
            producto.FechaRegistro = DateTime.UtcNow;

            _contexto.Productos.Add(producto);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Agregar) }: ${ ex.Message }");
            }

            return producto;
        }

        public async Task<bool> Eliminar(int id)
        {
            //Se realiza una eliminación suave, solamente inactivamos el producto

            var producto = await _contexto.Productos
                                .SingleOrDefaultAsync(c => c.Id == id);

            producto.Estatus = ProductoEstatus.Inactivo;
            _contexto.Productos.Attach(producto);
            _contexto.Entry(producto).State = EntityState.Modified;

            try
            {
                return (await _contexto.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Eliminar) }: ${ ex.Message }");
            }
            return false;

        }

        public async Task<Producto> ObtenerProductoAsync(int id)
        {
            return await _contexto.Productos
                               .SingleOrDefaultAsync(c => c.Id == id && c.Estatus == ProductoEstatus.Activo);
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            return await _contexto.Productos
                .Where(u => u.Estatus == ProductoEstatus.Activo)
                .OrderBy(u => u.Nombre).ToListAsync();
        }
    }

}
