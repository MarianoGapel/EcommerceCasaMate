using System;
using System.Collections.Generic;

namespace EcommerceCasaMate.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Total { get; set; }

    public string DireccionEntrega { get; set; } = null!;

    public string MetodoPago { get; set; } = null!;

    public string? Estado { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
