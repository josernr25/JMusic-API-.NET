using AutoMapper;
using Data.Contratos;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Administrador, Vendedor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosRepositorio _productosRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductosRepositorio productosRepositorio, IMapper mapper, ILogger<ProductosController> logger)
        {
            _productosRepositorio = productosRepositorio;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Productos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            try
            {
                var producto = await _productosRepositorio.ObtenerProductosAsync();

                // Antes de retornar se mapea a Dto
                return _mapper.Map<List<ProductoDto>>(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Get) }: ${ ex.Message }");
                return BadRequest();
            }
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            try
            {
                var producto = await _productosRepositorio.ObtenerProductoAsync(id);

                if (producto == null)
                {
                    return NotFound();
                }

                // Antes de retornar se mapea al tipo Dto
                return _mapper.Map<ProductoDto>(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Get) }: ${ ex.Message }");
                return BadRequest();
            }
        }

        // POST: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto productoDto)
        {
            try
            {
                // Se mapea a tipo Producto
                var producto = _mapper.Map<Producto>(productoDto);

                // Se envia al repositorio para que se agregen los campos faltantes y almacene en DB
                var nuevoProducto = await _productosRepositorio.Agregar(producto);

                if (nuevoProducto == null)
                {
                    return BadRequest();
                }

                // Se mapea a Dto para devolver
                var nuevoProductoDto = _mapper.Map<ProductoDto>(nuevoProducto);

                return CreatedAtAction(nameof(Post), new { id = nuevoProductoDto.Id }, nuevoProductoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Post) }: ${ ex.Message }");
                return BadRequest();
            }
        }

        //// PUT: api/Productos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody] ProductoDto productoDto)
        {
            if (productoDto == null) return BadRequest();

            // Se mapea a tipo Producto
            var producto = _mapper.Map<Producto>(productoDto);

            var resultado = await _productosRepositorio.Actualizar(producto);

            if (!resultado) return BadRequest();

            return productoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Delete(int id)
        {
            try
            {
                var respuesta = await _productosRepositorio.Eliminar(id);

                if (!respuesta) return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ${ nameof(Delete) }: ${ ex.Message }");
                return BadRequest();
            }
        }
    }
}
