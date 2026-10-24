
using Tema4;

var notificador = new Notificador();

// Observadores generales del sistema.
notificador.Suscribir(new ObservadorConsola());
notificador.Suscribir(new ObservadorLog());

// Responsables de las tareas.
var responsables = new List<ObservadorUsuario>
{
    new ObservadorUsuario("Ana"),
    new ObservadorUsuario("Carlos"),
    new ObservadorUsuario("María")
};

var gestor = new GestorTareas(notificador);

// Datos de prueba.
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
    Console.WriteLine("\n=== SISTEMA DE GESTIÓN DE TAREAS ===");
    Console.WriteLine("1. Listar tareas");
    Console.WriteLine("2. Agregar tarea");
    Console.WriteLine("3. Cambiar estado de tarea");
    Console.WriteLine("4. Eliminar tarea");
    Console.WriteLine("5. Salir");
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
            continuar = false;
            Console.WriteLine("Hasta pronto.");
            break;

        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}
