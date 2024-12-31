namespace Seneca.Models.Entities;

public partial class Log
{
    public long Id { get; set; }

    public long? UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
