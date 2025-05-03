using System.ComponentModel.DataAnnotations;

public class UsuarioUpdateDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "ID de rol inválido")]
    public int? IdRol { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ID de tipo documento inválido")]
    public int? IdTipoDocumento { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Número de documento debe tener entre 3 y 50 caracteres")]
    public string? NumeroDocumento { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nombre debe tener entre 2 y 100 caracteres")]
    public string? NombreUsuario { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Apellido debe tener entre 2 y 100 caracteres")]
    public string? ApellidoUsuario { get; set; }

    [RegularExpression(@"^[0-9]{6,20}$", ErrorMessage = "Teléfono debe contener solo números (6-20 dígitos)")]
    public string? TelUsuario { get; set; }

    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    [StringLength(100, ErrorMessage = "Correo no puede exceder 100 caracteres")]
    public string? CorreoUsuario { get; set; }

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número")]
    public string? ContraseñaUsuario { get; set; }

    public bool? Activo { get; set; }
}