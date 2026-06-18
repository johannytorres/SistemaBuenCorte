namespace SistemaBuenCorte.BLL.DTOs;

public class ProductoUpdateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public string TipoVenta { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal Stock { get; set; }
    public bool Activo { get; set; } = true;
}