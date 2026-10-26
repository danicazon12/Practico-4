using ApiInicial.DTOs;
using ApiInicial.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInicial.Controllers;

[ApiController]
[Route("api/v1/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ICategoriaService _categoriaService;

    public ProductosController(IProductoService productoService, ICategoriaService categoriaService)
    {
        _productoService = productoService;
        _categoriaService = categoriaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductoResponseDto>>> ObtenerTodos()
    {
        return Ok(await _productoService.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoResponseDto>> BuscarProductoPorId(int id)
    {
        var producto = await _productoService.BuscarProductoPorIdAsync(id);
        if (producto is null)
            return NotFound(new { mensaje = $"No existe el producto con id {id}" });

        return Ok(producto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoResponseDto>> CrearProducto(ProductoRequestDto request)
    {
        if (await _categoriaService.BuscarCategoriaPorIdAsync(request.CategoriaId) is null)
            return BadRequest(new { mensaje = "La categoría indicada no existe" });

        var creado = await _productoService.CrearProductoAsync(request);
        return CreatedAtAction(nameof(BuscarProductoPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoResponseDto>> ActualizarProducto(int id, ProductoRequestDto request)
    {
        if (await _categoriaService.BuscarCategoriaPorIdAsync(request.CategoriaId) is null)
            return BadRequest(new { mensaje = "La categoría indicada no existe" });

        var actualizado = await _productoService.ActualizarProducto(id, request);
        if (actualizado is null)
            return NotFound(new { mensaje = $"No existe el producto con id {id}" });

        return Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var eliminado = await _productoService.EliminarAsync(id);
        if (!eliminado)
            return NotFound(new { mensaje = $"No existe el producto con id {id}" });

        return NoContent();
    }
}
