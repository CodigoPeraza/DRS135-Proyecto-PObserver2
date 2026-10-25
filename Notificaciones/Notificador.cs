namespace Tema4;

class Notificador : INotificador {
    private readonly List<IObservador> observadores = new List<IObservador>();

    //Permite consultar quienes estan suscritos
    public IReadOnlyList<IObservador> Observadores => observadores;

    public void Suscribir(IObservador observador) {
        ArgumentNullException.ThrowIfNull(observador);
        if (!observadores.Contains(observador)) {
            observadores.Add(observador);
        }
    }

    public void Desuscribir(IObservador observador) {
        ArgumentNullException.ThrowIfNull(observador);
        observadores.Remove(observador);
    }

    public void Notificar(Notificacion notificacion) {
        foreach (IObservador observador in observadores.ToList()) {
            try {
                observador.Actualizar(notificacion);
            } catch (Exception ex) {
                Console.WriteLine($"Error en {observador.GetType().Name}: {ex.Message}");
            }
        }
    }
}
