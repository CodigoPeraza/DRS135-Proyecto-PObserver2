namespace Tema4;

interface IObservador {
    // Se llama cada vez que el Notificador emite una notificacion.
    void Actualizar(Notificacion notificacion);
}