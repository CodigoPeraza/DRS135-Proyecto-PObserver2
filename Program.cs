using Tema4;

var notificador = new Notificador();

// Responsables de las tareas y usuarios que pueden suscribirse a los productos.
var responsables = new List<ObservadorUsuario>
{
    new ObservadorUsuario("Ana"),
    new ObservadorUsuario("Carlos"),
    new ObservadorUsuario("María")
};

var almacen = new Almacen(notificador);

// Productos de prueba: se cargan antes de suscribir los observadores generales,
// por lo que esta precarga no genera alertas.
almacen.AgregarProducto(new Producto("Producto A", "A-001", 25.50m, 20));
almacen.AgregarProducto(new Producto("Producto B", "B-001", 40.00m, 15));
almacen.AgregarProducto(new Producto("Producto C", "C-001", 12.75m, 8));

almacen.SuscribirUsuario(0, responsables[0]);
almacen.SuscribirUsuario(1, responsables[1]);
almacen.SuscribirUsuario(2, responsables[2]);

var gestor = new GestorTareas(notificador);

// Observadores generales del sistema.
notificador.Suscribir(new ObservadorConsola());
notificador.Suscribir(new ObservadorLog());
notificador.Suscribir(new ObservadorStockBajo());

// Datos de prueba de tareas.
gestor.AgregarTarea(new Tarea(
    "Diseñar la interfaz",
    "Preparar el diseño inicial del sistema.",
    responsables[0]
));

gestor.AgregarTarea(new Tarea(
    "Implementar notificaciones",
    "Integrar el patrón Observer.",
    responsables[1]
));

gestor.AgregarTarea(new Tarea(
    "Realizar pruebas",
    "Verificar el funcionamiento del sistema.",
    responsables[2]
));

bool continuar = true;

while (continuar)
{
    Console.WriteLine("\n=== SISTEMA DE GESTIÓN ===");
    Console.WriteLine("1. Listar tareas");
    Console.WriteLine("2. Agregar tarea");
    Console.WriteLine("3. Cambiar estado de tarea");
    Console.WriteLine("4. Eliminar tarea");
    Console.WriteLine("5. Productos");
    Console.WriteLine("6. Salir");
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            gestor.ListarTareas();
            break;

        case "2":
        {
            Console.Write("Título: ");
            string titulo = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("El título y la descripción son obligatorios.");
                break;
            }

            Console.WriteLine("\nResponsables:");

            for (int i = 0; i < responsables.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {responsables[i].NombreUsuario}");
            }

            Console.Write("Seleccione el responsable: ");

            if (!int.TryParse(Console.ReadLine(), out int responsableElegido) ||
                responsableElegido < 1 ||
                responsableElegido > responsables.Count)
            {
                Console.WriteLine("Responsable no válido.");
                break;
            }

            var tarea = new Tarea(
                titulo,
                descripcion,
                responsables[responsableElegido - 1]
            );

            gestor.AgregarTarea(tarea);
            Console.WriteLine("Tarea agregada correctamente.");
            break;
        }

        case "3":
        {
            gestor.ListarTareas();

            if (gestor.Cantidad == 0)
                break;

            Console.Write("Número de tarea: ");

            if (!int.TryParse(Console.ReadLine(), out int numero) ||
                numero < 1 ||
                numero > gestor.Cantidad)
            {
                Console.WriteLine("Número de tarea no válido.");
                break;
            }

            Console.WriteLine("1. Pendiente");
            Console.WriteLine("2. En progreso");
            Console.WriteLine("3. Completada");
            Console.Write("Nuevo estado: ");

            if (!int.TryParse(Console.ReadLine(), out int estado) ||
                estado < 1 ||
                estado > 3)
            {
                Console.WriteLine("Estado no válido.");
                break;
            }

            EstadoTarea nuevoEstado = estado switch
            {
                1 => EstadoTarea.Pendiente,
                2 => EstadoTarea.EnProgreso,
                _ => EstadoTarea.Completada
            };

            if (gestor.CambiarEstado(numero - 1, nuevoEstado))
                Console.WriteLine("Estado actualizado correctamente.");

            break;
        }

        case "4":
        {
            gestor.ListarTareas();

            if (gestor.Cantidad == 0)
                break;

            Console.Write("Número de tarea que desea eliminar: ");

            if (!int.TryParse(Console.ReadLine(), out int numero) ||
                numero < 1 ||
                numero > gestor.Cantidad)
            {
                Console.WriteLine("Número de tarea no válido.");
                break;
            }

            if (gestor.QuitarTarea(numero - 1))
                Console.WriteLine("Tarea eliminada correctamente.");

            break;
        }

        case "5":
            MenuProductos();
            break;

        case "6":
            continuar = false;
            Console.WriteLine("Hasta pronto.");
            break;

        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}

static int LeerNumero(string mensaje, int maximo)
{
    Console.Write(mensaje);

    if (int.TryParse(Console.ReadLine(), out int numero) &&
        numero >= 1 &&
        numero <= maximo)
    {
        return numero - 1;
    }

    return -1;
}

void MenuProductos()
{
    while (true)
    {
        Console.WriteLine("\n=== SISTEMA DE PRODUCTOS ===");
        Console.WriteLine("1. Listar productos");
        Console.WriteLine("2. Agregar producto");
        Console.WriteLine("3. Modificar producto");
        Console.WriteLine("4. Eliminar producto");
        Console.WriteLine("5. Suscripciones");
        Console.WriteLine("6. Volver");
        Console.Write("Seleccione una opción: ");

        string? opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                almacen.ListarProductos();
                break;

            case "2":
                AgregarProducto();
                break;

            case "3":
                ModificarProducto();
                break;

            case "4":
                QuitarProducto();
                break;

            case "5":
                MenuSuscripcionesProductos();
                break;

            case "6":
                return;

            default:
                Console.WriteLine("Opción no válida. Intente nuevamente.");
                break;
        }
    }
}

void AgregarProducto()
{
    Console.Write("Nombre: ");
    string nombre = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Código: ");
    string codigo = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Precio: ");
    string precioTexto = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Cantidad: ");
    string cantidadTexto = Console.ReadLine()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(codigo))
    {
        Console.WriteLine("El nombre y el código son obligatorios.");
        return;
    }

    if (!decimal.TryParse(precioTexto, out decimal precio) || precio < 0)
    {
        Console.WriteLine("Precio no válido.");
        return;
    }

    if (!int.TryParse(cantidadTexto, out int cantidad) || cantidad < 0)
    {
        Console.WriteLine("Cantidad no válida.");
        return;
    }

    try
    {
        almacen.AgregarProducto(new Producto(nombre, codigo, precio, cantidad));
        Console.WriteLine("Producto agregado correctamente.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void ModificarProducto()
{
    almacen.ListarProductos();

    if (almacen.Cantidad == 0)
        return;

    int indice = LeerNumero("Número del producto a modificar: ", almacen.Cantidad);

    if (indice < 0)
    {
        Console.WriteLine("Número de producto no válido.");
        return;
    }

    Console.WriteLine("1. Nombre");
    Console.WriteLine("2. Precio");
    Console.WriteLine("3. Cantidad");
    Console.Write("Seleccione qué desea modificar: ");

    string? opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                Console.Write("Nuevo nombre: ");
                string nombre = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("El nombre es obligatorio.");
                    return;
                }

                almacen.ModificarProducto(indice, nombre: nombre);
                break;

            case "2":
                Console.Write("Nuevo precio: ");

                if (!decimal.TryParse(Console.ReadLine(), out decimal precio) || precio < 0)
                {
                    Console.WriteLine("Precio no válido.");
                    return;
                }

                almacen.ModificarProducto(indice, precio: precio);
                break;

            case "3":
                Console.Write("Nueva cantidad: ");

                if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad < 0)
                {
                    Console.WriteLine("Cantidad no válida.");
                    return;
                }

                almacen.ModificarProducto(indice, cantidad: cantidad);
                break;

            default:
                Console.WriteLine("Opción no válida.");
                return;
        }

        Console.WriteLine("Producto modificado correctamente.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void QuitarProducto()
{
    almacen.ListarProductos();

    if (almacen.Cantidad == 0)
        return;

    int indice = LeerNumero("Número del producto a eliminar: ", almacen.Cantidad);

    if (indice < 0)
    {
        Console.WriteLine("Número de producto no válido.");
        return;
    }

    if (almacen.QuitarProducto(indice))
        Console.WriteLine("Producto eliminado correctamente.");
}

void MenuSuscripcionesProductos()
{
    while (true)
    {
        Console.WriteLine("\n=== SUSCRIPCIONES DE PRODUCTOS ===");
        Console.WriteLine("1. Ver suscriptores");
        Console.WriteLine("2. Suscribir usuario");
        Console.WriteLine("3. Desuscribir usuario");
        Console.WriteLine("4. Volver");
        Console.Write("Seleccione una opción: ");

        string? opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                VerSuscriptoresProducto();
                break;

            case "2":
                CambiarSuscripcionProducto(suscribir: true);
                break;

            case "3":
                CambiarSuscripcionProducto(suscribir: false);
                break;

            case "4":
                return;

            default:
                Console.WriteLine("Opción no válida. Intente nuevamente.");
                break;
        }
    }
}

void VerSuscriptoresProducto()
{
    almacen.ListarProductos();

    if (almacen.Cantidad == 0)
        return;

    int indice = LeerNumero("Número del producto: ", almacen.Cantidad);

    if (indice < 0)
    {
        Console.WriteLine("Número de producto no válido.");
        return;
    }

    IReadOnlyList<IObservador>? suscriptores = almacen.ObtenerSuscriptores(indice);

    if (suscriptores == null || suscriptores.Count == 0)
    {
        Console.WriteLine("(sin suscriptores)");
        return;
    }

    Console.WriteLine("Suscriptores:");

    foreach (IObservador observador in suscriptores)
    {
        if (observador is ObservadorUsuario usuario)
            Console.WriteLine($"- {usuario.NombreUsuario}");
    }
}

void CambiarSuscripcionProducto(bool suscribir)
{
    almacen.ListarProductos();

    if (almacen.Cantidad == 0)
        return;

    int indice = LeerNumero("Número del producto: ", almacen.Cantidad);

    if (indice < 0)
    {
        Console.WriteLine("Número de producto no válido.");
        return;
    }

    IReadOnlyList<IObservador>? actuales = almacen.ObtenerSuscriptores(indice);

    if (actuales == null)
    {
        Console.WriteLine("Ese producto no existe.");
        return;
    }

    ObservadorUsuario? elegido = ElegirUsuario();

    if (elegido == null)
        return;

    bool yaSuscrito = actuales.Contains(elegido);

    if (suscribir)
    {
        if (yaSuscrito)
        {
            Console.WriteLine($"{elegido.NombreUsuario} ya estaba suscrito.");
            return;
        }

        almacen.SuscribirUsuario(indice, elegido);
        Console.WriteLine($"{elegido.NombreUsuario} ahora está suscrito.");
    }
    else
    {
        if (!yaSuscrito)
        {
            Console.WriteLine($"{elegido.NombreUsuario} no estaba suscrito.");
            return;
        }

        almacen.DesuscribirUsuario(indice, elegido);
        Console.WriteLine($"{elegido.NombreUsuario} ya no está suscrito.");
    }
}

ObservadorUsuario? ElegirUsuario()
{
    Console.WriteLine("\nUsuarios:");

    for (int i = 0; i < responsables.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {responsables[i].NombreUsuario}");
    }

    int indice = LeerNumero("Seleccione el usuario: ", responsables.Count);

    if (indice < 0)
    {
        Console.WriteLine("Usuario no válido.");
        return null;
    }

    return responsables[indice];
}
