namespace Tema4;

class ObservadorUsuario : IObservador {
    public string NombreUsuario { get; }
    private readonly HashSet<TipoEvento>? interes;

    //Si no se indican eventos, el usuario recibe todos
    public ObservadorUsuario(string nombreUsuario, params TipoEvento[] eventosDeInteres) {
        NombreUsuario = nombreUsuario;
        interes = eventosDeInteres.Length > 0 ? new HashSet<TipoEvento>(eventosDeInteres) : null;
    }

    public void Actualizar(Notificacion notificacion) {
        if (interes != null && !interes.Contains(notificacion.Tipo)) return;
        Console.WriteLine($"[Correo a {NombreUsuario}] {notificacion.Mensaje}");
    }
}
