namespace SistemaBuenCorte.BLL.Exceptions;

/// <summary>
/// Se lanza cuando se busca un producto por un Id que no existe.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string mensaje) : base(mensaje)
    {
    }
}