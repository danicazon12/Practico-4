using ApiInicial.DTOs;

namespace ApiInicial.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> ObtenerTodosAsync();
    Task<CategoriaResponseDto> CrearCategoriaAsync(CategoriaRequestDto categoria);
    Task<CategoriaResponseDto?> ActualizarCategoria(int id, CategoriaRequestDto categoria);
    Task<bool> EliminarAsync(int id);
    Task<CategoriaResponseDto?> BuscarCategoriaPorIdAsync(int id);
}
