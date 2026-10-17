namespace Tema4;

class Notificacion {
    public TipoEvento Tipo { get; }
    public string Mensaje { get; }
    public DateTime Fecha { get; }

    public object? Origen { get; }

    public Notificacion(TipoEvento tipo, string mensaje, object? origen = null) {
        Tipo = tipo;
        Mensaje = mensaje;
        Origen = origen;
        Fecha = DateTime.Now;
    }

    public override string ToString() => $"[{Fecha:HH:mm:ss}] {Tipo}: {Mensaje}";
}