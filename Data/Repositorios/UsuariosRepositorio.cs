using Data.Contratos;
using Microsoft.AspNetCore.Identity;
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
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private readonly TiendaDbContext _contexto;
        private readonly ILogger<UsuariosRepositorio> _logger;        
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private DbSet<Usuario> _dbSet;
        public UsuariosRepositorio(TiendaDbContext contexto, 
            ILogger<UsuariosRepositorio> logger,
            IPasswordHasher<Usuario> passwordHasher)
        {
            this._contexto = contexto;
            this._logger = logger;
            this._passwordHasher = passwordHasher;
            this._dbSet = _contexto.Set<Usuario>();
        }
        public async Task<bool> Actualizar(Usuario entity)
        {
            var usuarioDb = await _dbSet.FirstOrDefaultAsync(u => u.Id == entity.Id);

            if (usuarioDb == null)
            {
                _logger.LogError($"Error en {nameof(Actualizar)}: No existe el usuario con Id: {entity.Id}");
                return false;
            }

            usuarioDb.Nombre = entity.Nombre;
            usuarioDb.Apellidos = entity.Apellidos;
            usuarioDb.Email = entity.Email;            
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

        public async Task<Usuario> Agregar(Usuario entity)
        {
            entity.Estatus = UsuarioEstatus.Activo;
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
            _dbSet.Add(entity);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(Agregar)}: " + excepcion.Message);
            }
            return entity;
        }

        public async Task<bool> CambiarContrasena(Usuario usuario)
        {            
            var usuarioBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            usuarioBd.Password = _passwordHasher.HashPassword(usuarioBd, usuario.Password);
            try
            {
                return await _contexto.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(CambiarContrasena)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<bool> CambiarPerfil(Usuario usuario)
        {
            var usuarioBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            usuarioBd.PerfilId = usuario.PerfilId;
            try
            {
                return await _contexto.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(CambiarPerfil)}: " + excepcion.Message);
            }
            return false;
        }

        public async Task<bool> Eliminar(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(u => u.Id == id);
            entity.Estatus = UsuarioEstatus.Inactivo;            
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

        public async Task<Usuario> ObtenerAsync(int id)
        {
            return await _dbSet.Include(usuario => usuario.Perfil)
                                .SingleOrDefaultAsync(c => c.Id == id && c.Estatus== UsuarioEstatus.Activo);
        }

     

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _dbSet.Include(usuario => usuario.Perfil)
                                .Where(u=>u.Estatus== UsuarioEstatus.Activo)
                                .ToListAsync();
        }

        public async Task<bool> ValidarContrasena(Usuario usuario)
        {
            var usuarioBd = await _dbSet.FirstOrDefaultAsync(u => u.Id == usuario.Id);                        
            try
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuarioBd, usuarioBd.Password, usuario.Password);
                return resultado == PasswordVerificationResult.Success ? true : false;                                
            }
            catch (Exception excepcion)
            {
                _logger.LogError($"Error en {nameof(ValidarContrasena)}: " + excepcion.Message);
            }
            return false;
        }
    }
}
