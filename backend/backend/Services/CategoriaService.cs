using ApiInicial.Core;
using ApiInicial.DTOs;
using ApiInicial.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInicial.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                CantidadProductos = _context.Productos.Count(p => p.CategoriaId == c.Id),
            })
            .ToListAsync();
    }

    public async Task<CategoriaResponseDto?> BuscarCategoriaPorIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                CantidadProductos = _context.Productos.Count(p => p.CategoriaId == c.Id),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaResponseDto> CrearCategoriaAsync(CategoriaRequestDto request)
    {
        var categoria = new Categoria
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return (await BuscarCategoriaPorIdAsync(categoria.Id))!;
    }

    public async Task<CategoriaResponseDto?> ActualizarCategoria(int id, CategoriaRequestDto request)
    {
        var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null)
            return null;

        categoria.Nombre = request.Nombre;
        categoria.Descripcion = request.Descripcion;

        await _context.SaveChangesAsync();

        return await BuscarCategoriaPorIdAsync(id);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null)
            return false;

        var tieneProductos = await _context.Productos.AnyAsync(p => p.CategoriaId == id);
        if (tieneProductos)
            throw new InvalidOperationException("No se puede eliminar una categoría que tiene productos asociados.");

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }
}
