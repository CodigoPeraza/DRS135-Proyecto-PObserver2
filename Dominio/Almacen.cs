namespace Tema4;

class Almacen {
    private List<Producto> productos = new List<Producto>();

    //Un Notificador propio por cada producto
    private Dictionary<Producto, Notificador> notificadoresProducto = new Dictionary<Producto, Notificador>();

    //Notificador general del sistema (log, alertas globales)
    private readonly INotificador notificador;

    public Almacen(INotificador notificador) {
        ArgumentNullException.ThrowIfNull(notificador);
        this.notificador = notificador;
    }

    private bool IndiceValido(int indice) => indice >= 0 && indice < productos.Count;

    //Avisa al sistema general y a los suscriptores de ESE producto
    private void Emitir(Producto producto, Notificacion notificacion) {
        notificador.Notificar(notificacion);
        notificadoresProducto[producto].Notificar(notificacion);
    }

    public void AgregarProducto(Producto producto) {
        ArgumentNullException.ThrowIfNull(producto);
        if (string.IsNullOrWhiteSpace(producto.Codigo))
            throw new ArgumentException("El codigo del producto es obligatorio.", nameof(producto));
        if (producto.Cantidad < 0)
            throw new ArgumentException("La cantidad no puede ser negativa.", nameof(producto));
        if (productos.Any(p => string.Equals(p.Codigo, producto.Codigo, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"Ya existe un producto con el codigo {producto.Codigo}.", nameof(producto));

        productos.Add(producto);
        notificadoresProducto[producto] = new Notificador();
        Emitir(producto, new Notificacion(
            TipoEvento.ProductoAgregado,
            $"Se agrego {producto.Nombre} (codigo {producto.Codigo}, cantidad {producto.Cantidad})",
            producto));
    }

    public bool QuitarProducto(int indice) {
        if (!IndiceValido(indice)) return false;
        Producto producto = productos[indice];
        Emitir(producto, new Notificacion(
            TipoEvento.ProductoQuitado,
            $"Se quito {producto.Nombre}", producto));
        productos.RemoveAt(indice);
        notificadoresProducto.Remove(producto);
        return true;
    }

    public bool ModificarProducto(int indice, string? nombre = null, decimal? precio = null, int? cantidad = null) {
        if (!IndiceValido(indice)) return false;
        Producto producto = productos[indice];
        if (nombre != null) producto.Nombre = nombre;
        if (precio != null) producto.Precio = precio.Value;
        if (cantidad != null) {
            if (cantidad.Value < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.", nameof(cantidad));
            producto.Cantidad = cantidad.Value;
        }
        Emitir(producto, new Notificacion(
            TipoEvento.ProductoModificado,
            $"Se modifico {producto.Nombre}", producto));
        return true;
    }

    public Producto? ObtenerProducto(int indice) {
        return IndiceValido(indice) ? productos[indice] : null;
    }

    // ---- Suscripciones por producto ----
    public bool SuscribirUsuario(int indice, IObservador observador) {
        if (!IndiceValido(indice)) return false;
        notificadoresProducto[productos[indice]].Suscribir(observador);
        return true;
    }

    public bool DesuscribirUsuario(int indice, IObservador observador) {
        if (!IndiceValido(indice)) return false;
        notificadoresProducto[productos[indice]].Desuscribir(observador);
        return true;
    }

    public IReadOnlyList<IObservador>? ObtenerSuscriptores(int indice) {
        return IndiceValido(indice) ? notificadoresProducto[productos[indice]].Observadores : null;
    }

    public int Cantidad => productos.Count;

    public void ListarProductos() {
        if (productos.Count == 0) {
            Console.WriteLine("(no hay productos)");
            return;
        }
        for (int i = 0; i < productos.Count; i++) {
            int suscriptores = notificadoresProducto[productos[i]].Observadores.Count;
            Console.WriteLine($"{i + 1}. {productos[i].Nombre} - ${productos[i].Precio} - cant: {productos[i].Cantidad} - suscriptores: {suscriptores}");
        }
    }
}
