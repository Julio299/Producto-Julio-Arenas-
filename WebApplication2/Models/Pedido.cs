using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Pedido
    {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "La fecha del pedido es obligatoria")]
            public DateTime FechaPedido { get; set; }

            [Required(ErrorMessage = "El total es obligatorio")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
            public decimal Total { get; set; }

            [Required(ErrorMessage = "El producto es obligatorio")]
            public int ProductoId { get; set; }

            [ForeignKey(nameof(ProductoId))]
            public Producto Producto { get; set; }

            [Required(ErrorMessage = "La cantidad es obligatoria")]
            [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
            public int Cantidad { get; set; }

            public bool Cancelado { get; set; } = false;
        
    }
}




