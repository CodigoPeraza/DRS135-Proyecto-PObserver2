namespace Tema4;

//Estudiante 4
class Tarea {
    public string Titulo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
    public ObservadorUsuario Responsable { get; }

    public bool Completada => Estado == EstadoTarea.Completada;

    public Tarea(string titulo, string descripcion, ObservadorUsuario responsable) {
        Titulo = titulo;
        Descripcion = descripcion;
        Responsable = responsable;
    }
}
