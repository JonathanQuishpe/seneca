using System;
using System.Collections.Generic;

namespace Seneca.Models.Entities;

public partial class Correo
{
    public long Id { get; set; }

    public long? UsuarioId { get; set; }

    public byte? Estado { get; set; }

    public string? Tipo { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
