using System.ComponentModel.DataAnnotations;

namespace ApiInicial.DTOs;

public class CategoriaRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres")]
    public string? Descripcion { get; set; }
}
