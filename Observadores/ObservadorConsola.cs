namespace Tema4;

class ObservadorConsola : IObservador {
    public void Actualizar(Notificacion notificacion) {
        ConsoleColor anterior = Console.ForegroundColor;
        Console.ForegroundColor = notificacion.Tipo switch {
            TipoEvento.ProductoQuitado or TipoEvento.TareaQuitada => ConsoleColor.Red,
            TipoEvento.ProductoModificado => ConsoleColor.Yellow,
            _ => ConsoleColor.Green
        };
        Console.WriteLine($"[ALERTA] {notificacion}");
        Console.ForegroundColor = anterior;
    }
}
