using Microsoft.EntityFrameworkCore;
using SistemaBuenCorte.BLL.DTOs;
using SistemaBuenCorte.BLL.Exceptions;
using SistemaBuenCorte.DAL.Data;
using SistemaBuenCorte.DAL.Entities;

namespace SistemaBuenCorte.BLL.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync(string? nombre = null, string? categoria = null)
    {
        var query = _context.Productos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(p => p.Categoria != null && p.Categoria.Contains(categoria));

        var productos = await query
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return productos.Select(MapToDto);
    }

    public async Task<ProductoDto> ObtenerPorIdAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto is null)
            throw new NotFoundException($"No se encontró un producto con Id {id}.");

        return MapToDto(producto);
    }

    public async Task<ProductoDto> CrearAsync(ProductoCreateDto dto)
    {
        var errores = ValidarDatosBasicos(dto.Nombre, dto.Descripcion, dto.Categoria, dto.TipoVenta, dto.Precio, dto.Stock);

        var nombreDuplicado = await _context.Productos
            .AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.Trim().ToLower());

        if (nombreDuplicado)
            errores.Add("Ya existe un producto registrado con ese nombre.");

        if (errores.Count > 0)
            throw new ValidationException(errores);

        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
            Categoria = string.IsNullOrWhiteSpace(dto.Categoria) ? null : dto.Categoria.Trim(),
            TipoVenta = dto.TipoVenta.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return MapToDto(producto);
    }

    public async Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto is null)
            throw new NotFoundException($"No se encontró un producto con Id {id}.");

        var errores = ValidarDatosBasicos(dto.Nombre, dto.Descripcion, dto.Categoria, dto.TipoVenta, dto.Precio, dto.Stock);

        var nombreDuplicado = await _context.Productos
            .AnyAsync(p => p.Id != id && p.Nombre.ToLower() == dto.Nombre.Trim().ToLower());

        if (nombreDuplicado)
            errores.Add("Ya existe otro producto registrado con ese nombre.");

        if (errores.Count > 0)
            throw new ValidationException(errores);

        producto.Nombre = dto.Nombre.Trim();
        producto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        producto.Categoria = string.IsNullOrWhiteSpace(dto.Categoria) ? null : dto.Categoria.Trim();
        producto.TipoVenta = dto.TipoVenta.Trim();
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return MapToDto(producto);
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto is null)
            throw new NotFoundException($"No se encontró un producto con Id {id}.");

        // Borrado lógico: no se elimina físicamente para no perder el historial
        // (ventas pasadas que referencian este producto mediante DetalleVenta).
        producto.Activo = false;

        await _context.SaveChangesAsync();
    }

    private static List<string> ValidarDatosBasicos(
        string nombre, string? descripcion, string? categoria,
        string tipoVenta, decimal precio, decimal stock)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(nombre))
            errores.Add("El nombre del producto es obligatorio.");
        else if (nombre.Trim().Length > 100)
            errores.Add("El nombre no puede superar los 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Trim().Length > 250)
            errores.Add("La descripción no puede superar los 250 caracteres.");

        if (!string.IsNullOrWhiteSpace(categoria) && categoria.Trim().Length > 50)
            errores.Add("La categoría no puede superar los 50 caracteres.");

        if (string.IsNullOrWhiteSpace(tipoVenta))
            errores.Add("El tipo de venta es obligatorio.");
        else if (tipoVenta.Trim() != "Peso" && tipoVenta.Trim() != "Unidad")
            errores.Add("El tipo de venta debe ser 'Peso' o 'Unidad'.");

        if (precio <= 0)
            errores.Add("El precio debe ser mayor a cero.");

        if (stock < 0)
            errores.Add("El stock no puede ser negativo.");

        return errores;
    }

    private static ProductoDto MapToDto(Producto producto) => new()
    {
        Id = producto.Id,
        Nombre = producto.Nombre,
        Descripcion = producto.Descripcion,
        Categoria = producto.Categoria,
        TipoVenta = producto.TipoVenta,
        Precio = producto.Precio,
        Stock = producto.Stock,
        Activo = producto.Activo,
        FechaCreacion = producto.FechaCreacion
    };
}
