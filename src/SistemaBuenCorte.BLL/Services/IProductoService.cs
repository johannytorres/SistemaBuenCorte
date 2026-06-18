using SistemaBuenCorte.BLL.DTOs;

namespace SistemaBuenCorte.BLL.Services;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> ObtenerTodosAsync(string? nombre = null, string? categoria = null);
    Task<ProductoDto> ObtenerPorIdAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCreateDto dto);
    Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto);
    Task EliminarAsync(int id);
}