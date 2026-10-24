namespace Tema4;

class ObservadorStockBajo : IObservador {
    private readonly int limite;

    public ObservadorStockBajo(int limite = 5) {
        this.limite = limite;
    }

    public void Actualizar(Notificacion notificacion) {
        //Solo al agregar o modificar
        if (notificacion.Tipo != TipoEvento.ProductoAgregado &&
            notificacion.Tipo != TipoEvento.ProductoModificado) return;

        if (notificacion.Origen is Producto p && p.Cantidad <= limite) {
            Console.WriteLine($"[STOCK BAJO] {p.Nombre}: quedan {p.Cantidad} unidades.");
        }
    }
}
