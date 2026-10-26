using System.ComponentModel.DataAnnotations;

namespace ApiInicial.DTOs;

public class ProductoRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    public bool Estado { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
    public int CategoriaId { get; set; }
}
