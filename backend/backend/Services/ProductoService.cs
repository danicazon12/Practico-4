using ApiInicial.Core;
using ApiInicial.DTOs;
using ApiInicial.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInicial.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Productos
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProductoResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                Estado = p.Estado,
                CategoriaId = p.CategoriaId,
                NombreCategoria = p.Categoria!.Nombre,
            })
            .ToListAsync();
    }

    public async Task<ProductoResponseDto?> BuscarProductoPorIdAsync(int id)
    {
        var producto = await _context.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        return producto is null ? null : ConvertirADto(producto);
    }

    public async Task<ProductoResponseDto> CrearProductoAsync(ProductoRequestDto request)
    {
        var producto = new Producto
        {
            Nombre = request.Nombre,
            Precio = request.Precio,
            Stock = request.Stock,
            Estado = request.Estado,
            CategoriaId = request.CategoriaId,
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return (await BuscarProductoPorIdAsync(producto.Id))!;
    }

    public async Task<ProductoResponseDto?> ActualizarProducto(int id, ProductoRequestDto request)
    {
        var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
        if (producto is null)
            return null;

        producto.Nombre = request.Nombre;
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.Estado = request.Estado;
        producto.CategoriaId = request.CategoriaId;

        await _context.SaveChangesAsync();

        return await BuscarProductoPorIdAsync(id);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null)
            return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductoResponseDto ConvertirADto(Producto p)
    {
        return new ProductoResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Stock = p.Stock,
            Estado = p.Estado,
            CategoriaId = p.CategoriaId,
            NombreCategoria = p.Categoria?.Nombre ?? string.Empty,
        };
    }
}
