using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class DataPedido: DbContext
    {
        public DataPedido(DbContextOptions<DataPedido> options) : base(options)
        {
        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }


}
