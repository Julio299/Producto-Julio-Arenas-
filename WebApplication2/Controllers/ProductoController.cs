using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DataPedido _context;

        public ProductoController(DataPedido context)
        {
            _context = context;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return Ok(await _context.Productos.ToListAsync());
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return Ok("Producto registrado correctamente.");
        }

        [HttpGet("BuscarId/{id}")]
        public async Task<ActionResult<Producto>> GetProductoById(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("Producto no encontrado.");
            }
            return Ok(producto);
        }

        [HttpPut("Modificar/{id}")]
        public async Task<ActionResult> PutProducto(int id, Producto producto)
        {
            var existeProducto = await _context.Productos.FindAsync(id);
            if (existeProducto == null)
            {
                return NotFound("Producto no encontrado.");
            }

            existeProducto.Nombre = producto.Nombre;
            existeProducto.Precio = producto.Precio;
            existeProducto.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return Ok("Producto modificado correctamente.");
        }    

    [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("Producto no encontrado.");
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return Ok("Producto eliminado correctamente.");
        }
    }
}