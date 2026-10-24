namespace Tema4;

class ObservadorLog : IObservador {
    private readonly string rutaArchivo;

    public ObservadorLog(string rutaArchivo = "notificaciones.log") {
        this.rutaArchivo = rutaArchivo;
    }

    public void Actualizar(Notificacion notificacion) {
        try {
            File.AppendAllText(rutaArchivo, notificacion + Environment.NewLine);
        } catch (IOException ex) {
            Console.WriteLine($"No se pudo escribir el log: {ex.Message}");
        }
    }
}
