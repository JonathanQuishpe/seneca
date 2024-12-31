using System.ComponentModel.DataAnnotations;

namespace Seneca.Models.Entities;

public partial class Usuario
{
    public long Id { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Nombres { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Apellidos { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Display(Name = "Fecha de nacimiento")]
    public DateOnly? FechaNacimiento { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Display(Name = "Correo electrónico")]
    public string? CorreoElectronico { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Display(Name = "Contraseña")]
    public string? Contrasenia { get; set; }
    public byte? Estado { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
