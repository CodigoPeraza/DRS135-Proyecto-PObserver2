namespace Tema4;

interface INotificador {
    void Suscribir(IObservador observador);
    void Desuscribir(IObservador observador);
    void Notificar(Notificacion notificacion);
}