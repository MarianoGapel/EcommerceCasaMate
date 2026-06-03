using System;
using System.Collections.Generic;

namespace EcommerceCasaMate.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Dni { get; set; }

    public string Contrasena { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
