namespace SistemaBuenCorte.BLL.Exceptions;

/// <summary>
/// Se lanza cuando los datos de un producto no cumplen las reglas de negocio
/// (nombre vacío, precio negativo, nombre duplicado, etc.).
/// Puede contener uno o varios mensajes de error a la vez.
/// </summary>
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errores { get; }

    public ValidationException(string mensaje) : base(mensaje)
    {
        Errores = new List<string> { mensaje };
    }

    public ValidationException(IEnumerable<string> errores)
        : base(string.Join(" | ", errores))
    {
        Errores = errores.ToList();
    }
}