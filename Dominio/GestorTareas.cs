
namespace Tema4;

class GestorTareas
{
    private readonly List<Tarea> tareas = new();
    private readonly INotificador notificador;

    public GestorTareas(INotificador notificador)
    {
        this.notificador = notificador;
    }

    public int Cantidad => tareas.Count;

    public IReadOnlyList<Tarea> ObtenerTareas() => tareas.AsReadOnly();

    public bool AgregarTarea(Tarea tarea)
    {
        if (tarea == null)
            return false;

        tareas.Add(tarea);

        Emitir(
            TipoEvento.TareaAgregada,
            $"Se agregó la tarea: {tarea.Titulo}",
            tarea
        );

        return true;
    }

    public bool CambiarEstado(int indice, EstadoTarea nuevoEstado)
    {
        if (indice < 0 || indice >= tareas.Count)
            return false;

        Tarea tarea = tareas[indice];

        if (tarea.Estado == nuevoEstado)
            return true;

        EstadoTarea estadoAnterior = tarea.Estado;
        tarea.Estado = nuevoEstado;

        Emitir(
            TipoEvento.TareaEstadoCambiado,
            $"La tarea '{tarea.Titulo}' cambió de {estadoAnterior} a {nuevoEstado}.",
            tarea
        );

        if (nuevoEstado == EstadoTarea.Completada)
        {
            Emitir(
                TipoEvento.TareaCompletada,
                $"La tarea '{tarea.Titulo}' se completó.",
                tarea
            );
        }

        return true;
    }

    public bool QuitarTarea(int indice)
    {
        if (indice < 0 || indice >= tareas.Count)
            return false;

        Tarea tarea = tareas[indice];

        Emitir(
            TipoEvento.TareaQuitada,
            $"Se eliminó la tarea: {tarea.Titulo}",
            tarea
        );

        tareas.RemoveAt(indice);
        return true;
    }

    public void ListarTareas()
    {
        if (tareas.Count == 0)
        {
            Console.WriteLine("No hay tareas registradas.");
            return;
        }

        for (int i = 0; i < tareas.Count; i++)
        {
            Tarea tarea = tareas[i];

            Console.WriteLine(
                $"{i + 1}. {tarea.Titulo} | " +
                $"Estado: {tarea.Estado} | " +
                $"Responsable: {tarea.Responsable.NombreUsuario}"
            );

            Console.WriteLine($"   Descripción: {tarea.Descripcion}");
        }
    }

    private void Emitir(TipoEvento tipo, string mensaje, Tarea tarea)
    {
        var notificacion = new Notificacion(tipo, mensaje, tarea);

        notificador.Notificar(notificacion);
        tarea.Responsable.Actualizar(notificacion);
    }
}
