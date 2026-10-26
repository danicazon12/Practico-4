using ApiInicial.DTOs;
using ApiInicial.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInicial.Controllers;

[ApiController]
[Route("api/v1/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoriaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoriaResponseDto>>> ObtenerTodos()
    {
        return Ok(await _categoriaService.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaResponseDto>> BuscarCategoriaPorId(int id)
    {
        var categoria = await _categoriaService.BuscarCategoriaPorIdAsync(id);
        if (categoria is null)
            return NotFound(new { mensaje = $"No existe la categoría con id {id}" });

        return Ok(categoria);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoriaResponseDto>> CrearCategoria(CategoriaRequestDto request)
    {
        var creada = await _categoriaService.CrearCategoriaAsync(request);
        return CreatedAtAction(nameof(BuscarCategoriaPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaResponseDto>> ActualizarCategoria(int id, CategoriaRequestDto request)
    {
        var actualizada = await _categoriaService.ActualizarCategoria(id, request);
        if (actualizada is null)
            return NotFound(new { mensaje = $"No existe la categoría con id {id}" });

        return Ok(actualizada);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        try
        {
            var eliminada = await _categoriaService.EliminarAsync(id);
            if (!eliminada)
                return NotFound(new { mensaje = $"No existe la categoría con id {id}" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
