using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly DataPedido _context;

        public PedidoController(DataPedido context)
        {
            _context = context;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos.Include(p => p.Producto).ToListAsync();
            return Ok(pedidos);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostPedido(Pedido pedido)
        {
            var producto = await _context.Productos.FindAsync(pedido.ProductoId);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado.");
            }

            if (producto.Stock < pedido.Cantidad)
            {
                return BadRequest("Stock insuficiente para el producto.");
            }

            producto.Stock -= pedido.Cantidad;

            _context.Entry(producto).State = EntityState.Modified;

            pedido.Producto = null;
            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();
            return Ok("Pedido registrado correctamente.");
        }


        [HttpPut("Modificar/{id}")]
        public async Task<ActionResult> PutPedido(int id, Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.Include(p => p.Producto).FirstOrDefaultAsync(p => p.Id == id);

            if (pedidoExistente == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            if (pedidoExistente.Cancelado)
            {
                return BadRequest("No se puede modificar un pedido cancelado.");
            }

            var producto = await _context.Productos.FindAsync(pedido.ProductoId);
            if (producto == null)
            {
                return BadRequest("Producto no encontrado.");
            }

            if (pedido.Cantidad > producto.Stock + pedidoExistente.Cantidad)
            {
                return BadRequest("Stock insuficiente para la nueva cantidad del pedido.");
            }

            producto.Stock += pedidoExistente.Cantidad;
            producto.Stock -= pedido.Cantidad;

            pedidoExistente.FechaPedido = pedido.FechaPedido;
            pedidoExistente.Total = pedido.Total;
            pedidoExistente.ProductoId = pedido.ProductoId;
            pedidoExistente.Cantidad = pedido.Cantidad;

            await _context.SaveChangesAsync();
            return Ok("Pedido modificado correctamente.");
        }

        [HttpPut("Cancelar/{id}")]
        public async Task<ActionResult> CancelarPedido(int id)
        {
            var pedido = await _context.Pedidos.Include(p => p.Producto).FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            if (pedido.Cancelado)
            {
                return BadRequest("El pedido ya está cancelado.");
            }

            pedido.Cancelado = true;
            pedido.Producto.Stock += pedido.Cantidad;
            await _context.SaveChangesAsync();

            return Ok("Pedido cancelado correctamente.");
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            if (pedido.Cancelado == false)
            {
                var producto = await _context.Productos.FindAsync(pedido.ProductoId);
                if (producto != null)
                {
                    producto.Stock += pedido.Cantidad;
                }
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return Ok("Pedido eliminado correctamente.");
        }
    }
}