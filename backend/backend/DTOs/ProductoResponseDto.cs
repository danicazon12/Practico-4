namespace ApiInicial.DTOs;

public class ProductoResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Estado { get; set; }
    public int CategoriaId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
}
