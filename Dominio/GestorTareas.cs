namespace Tema4;

/// <summary>
/// Administra el ciclo de vida de las tareas (agregar, cambiar de estado y quitar)
/// y avisa de cada cambio mediante el patron Observer.
///
/// Las notificaciones salen por dos canales:
///   1. El <see cref="INotificador"/> general del sistema (consola, log, etc.),
///      que se recibe por constructor.
///   2. Un <see cref="Notificador"/> propio por tarea, cuyos suscriptores siguen
///      unicamente ESA tarea. El responsable queda suscrito automaticamente.
///
/// Esta clase no conoce a los observadores concretos: solo trabaja con
/// <see cref="IObservador"/>, asi que se pueden agregar observadores nuevos
/// sin modificarla.
/// </summary>
class GestorTareas {
    private readonly List<Tarea> tareas = new List<Tarea>();

    // Un Notificador propio por tarea: sus suscriptores siguen el estado de ESA tarea
    private readonly Dictionary<Tarea, Notificador> notificadoresTarea = new Dictionary<Tarea, Notificador>();

    // Notificador general del sistema (consola, log)
    private readonly INotificador notificador;

    /// <summary>Cantidad de tareas registradas.</summary>
    public int Cantidad => tareas.Count;

    /// <param name="notificador">Notificador general del sistema. No puede ser null.</param>
    /// <exception cref="ArgumentNullException">Si <paramref name="notificador"/> es null.</exception>
    public GestorTareas(INotificador notificador) {
        ArgumentNullException.ThrowIfNull(notificador);
        this.notificador = notificador;
    }

    private bool IndiceValido(int indice) => indice >= 0 && indice < tareas.Count;

    // Unico punto donde se crea el notificador de una tarea (el responsable siempre queda suscrito)
    private static Notificador CrearNotificadorDeTarea(Tarea tarea) {
        Notificador notificadorTarea = new Notificador();
        notificadorTarea.Suscribir(tarea.Responsable);
        return notificadorTarea;
    }

    // Avisa al sistema general y a los suscriptores de ESA tarea
    private void Emitir(Tarea tarea, Notificacion notificacion) {
        notificador.Notificar(notificacion);
        notificadoresTarea[tarea].Notificar(notificacion);
    }

    /// <summary>
    /// Registra una tarea. El responsable queda suscrito automaticamente a ella.
    /// </summary>
    /// <param name="tarea">Tarea a registrar.</param>
    /// <param name="silenciosa">
    /// Si es true no se emite la notificacion de alta; sirve para precargar datos sin enviar avisos.
    /// </param>
    /// <exception cref="ArgumentNullException">Si la tarea o su responsable son null.</exception>
    /// <exception cref="ArgumentException">Si esa misma instancia ya esta registrada.</exception>
    public void AgregarTarea(Tarea tarea, bool silenciosa = false) {
        ArgumentNullException.ThrowIfNull(tarea);
        if (notificadoresTarea.ContainsKey(tarea))
            throw new ArgumentException("La tarea ya esta registrada en el gestor.", nameof(tarea));

        // Se crea primero el notificador: si algo falla, el gestor no queda a medias
        Notificador notificadorTarea = CrearNotificadorDeTarea(tarea);
        tareas.Add(tarea);
        notificadoresTarea[tarea] = notificadorTarea;

        if (!silenciosa) {
            Emitir(tarea, new Notificacion(
                TipoEvento.TareaAgregada,
                $"Se agrego la tarea {tarea.Titulo} (responsable: {tarea.Responsable.NombreUsuario})",
                tarea));
        }
    }

    /// <summary>
    /// Cambia el estado de una tarea y notifica el cambio. Emite
    /// <see cref="TipoEvento.TareaCompletada"/> si el nuevo estado es Completada y
    /// <see cref="TipoEvento.TareaEstadoCambiado"/> en cualquier otro caso.
    /// </summary>
    /// <returns>false si el indice no existe o la tarea ya tenia ese estado.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Si <paramref name="nuevoEstado"/> no es un valor definido.</exception>
    public bool CambiarEstado(int indice, EstadoTarea nuevoEstado) {
        if (!Enum.IsDefined(nuevoEstado))
            throw new ArgumentOutOfRangeException(nameof(nuevoEstado), nuevoEstado, "Estado de tarea no valido.");
        if (!IndiceValido(indice)) return false;
        Tarea tarea = tareas[indice];
        if (tarea.Estado == nuevoEstado) return false;

        EstadoTarea anterior = tarea.Estado;
        tarea.Estado = nuevoEstado;
        Emitir(tarea, new Notificacion(
            nuevoEstado == EstadoTarea.Completada ? TipoEvento.TareaCompletada : TipoEvento.TareaEstadoCambiado,
            $"La tarea {tarea.Titulo} cambio de {anterior} a {nuevoEstado}",
            tarea));
        return true;
    }

    /// <summary>
    /// Quita una tarea. El aviso se emite ANTES de eliminarla para que sus suscriptores lo reciban.
    /// </summary>
    /// <returns>false si el indice no existe.</returns>
    public bool QuitarTarea(int indice) {
        if (!IndiceValido(indice)) return false;
        Tarea tarea = tareas[indice];
        Emitir(tarea, new Notificacion(
            TipoEvento.TareaQuitada,
            $"Se quito la tarea {tarea.Titulo}", tarea));
        tareas.RemoveAt(indice);
        notificadoresTarea.Remove(tarea);
        return true;
    }

    /// <summary>Devuelve la tarea en esa posicion, o null si el indice no existe.</summary>
    public Tarea? ObtenerTarea(int indice) {
        return IndiceValido(indice) ? tareas[indice] : null;
    }

    // ---- Suscripciones por tarea ----

    /// <summary>Suscribe un observador a una tarea. Si ya estaba suscrito no se duplica.</summary>
    /// <returns>false si el indice no existe.</returns>
    /// <exception cref="ArgumentNullException">Si el observador es null (con un indice valido).</exception>
    public bool SuscribirUsuario(int indice, IObservador observador) {
        if (!IndiceValido(indice)) return false;
        notificadoresTarea[tareas[indice]].Suscribir(observador);
        return true;
    }

    /// <summary>
    /// Da de baja a un observador de una tarea.
    /// </summary>
    /// <returns>
    /// false si el indice no existe o si el observador es el responsable de la tarea
    /// (el responsable no puede desuscribirse). true en cualquier otro caso.
    /// </returns>
    public bool DesuscribirUsuario(int indice, IObservador observador) {
        if (!IndiceValido(indice)) return false;
        if (observador == tareas[indice].Responsable) return false;
        notificadoresTarea[tareas[indice]].Desuscribir(observador);
        return true;
    }

    /// <summary>Devuelve los suscriptores de una tarea, o null si el indice no existe.</summary>
    public IReadOnlyList<IObservador>? ObtenerSuscriptores(int indice) {
        return IndiceValido(indice) ? notificadoresTarea[tareas[indice]].Observadores : null;
    }

    /// <summary>
    /// Muestra por consola las tareas con su estado, responsable, numero de suscriptores y descripcion.
    /// La numeracion mostrada empieza en 1 (como la espera el menu de Program.cs); los metodos
    /// del gestor reciben indices que empiezan en 0, por lo que el menu resta 1 al numero elegido.
    /// </summary>
    public void ListarTareas() {
        if (tareas.Count == 0) {
            Console.WriteLine("(no hay tareas)");
            return;
        }
        for (int i = 0; i < tareas.Count; i++) {
            Tarea t = tareas[i];
            int suscriptores = notificadoresTarea[t].Observadores.Count;
            Console.WriteLine($"{i + 1}. [{t.Estado}] {t.Titulo} - responsable: {t.Responsable.NombreUsuario} - suscriptores: {suscriptores}");
            Console.WriteLine($"   Descripcion: {t.Descripcion}");
        }
    }
}
