
# Sistema de Notificaciones — Patron Observer

Gestion de productos y tareas con notificaciones en tiempo real a observadores.

---

## Como ejecutar (VSCode)

1. Instalar el **SDK de .NET 10** y la extension **C# Dev Kit** (o **C#**).
2. Abrir esta carpeta en VSCode (`File > Open Folder`).
3. Abrir la terminal (`Ctrl + ~`) y ejecutar:
   ```bash
   dotnet run
   ```
   *(O pulsar `F5` seleccionando la configuracion de depuracion **Ejecutar PObserver**)*.

---

## Estructura del Proyecto

```text
├── Interfaces/
│   ├── IObservador.cs
│   └── INotificador.cs
├── Modelos/
│   ├── Notificacion.cs
│   ├── TipoEvento.cs
│   └── EstadoTareas.cs
├── Notificaciones/
│   └── Notificador.cs
├── Dominio/
│   ├── Producto.cs
│   ├── Almacen.cs
│   ├── Tarea.cs
│   └── GestorTareas.cs
├── Observadores/
│   ├── ObservadorConsola.cs
│   ├── ObservadorUsuario.cs
│   ├── ObservadorLog.cs
│   └── ObservadorStockBajo.cs
└── Program.cs  # Menu principal
```

---

## Descripcion de Componentes

### Ejecucion

* **`Program.cs`**: Punto de entrada de la aplicacion que despliega el menu principal.

### Interfaces

| Archivo | Descripcion |
| :--- | :--- |
| **`INotificador.cs`** | Define los metodos base (`Suscribir`, `Desuscribir`, `Notificar`) para interactuar con observadores y notificaciones. |
| **`IObservador.cs`** | Define el contrato para recibir y actualizar las notificaciones enviadas. |

### Modelos

| Archivo | Descripcion |
| :--- | :--- |
| **`Notificacion.cs`** | Clase encargada de instanciar los objetos de tipo `Notificacion` y sus metodos asociados. |
| **`TipoEvento.cs`** | Enumerado (`enum`) para clasificar el tipo de evento que se esta procesando. |
| **`EstadoTareas.cs`** | Enumerado (`enum`) que define y controla los estados posibles de una tarea. |

### Notificaciones

| Archivo | Descripcion |
| :--- | :--- |
| **`Notificador.cs`** | Implementa `INotificador`; define la logica interna para gestionar las listas de suscriptores y despachar eventos. |

### Dominio

| Archivo | Descripcion |
| :--- | :--- |
| **`Producto.cs`** | Entidad encargada de establecer la estructura de los objetos tipo Producto. |
| **`Almacen.cs`** | Gestiona el inventario de Productos, incluye el metodo `Emitir` para notificar cambios e integra la suscripcion de usuarios. |
| **`Tarea.cs`** | Entidad encargada de establecer la estructura de los objetos tipo Tarea. |
| **`GestorTareas.cs`** | Administra el ciclo de vida de las tareas, incluye el metodo `Emitir` de cambios y la gestion de suscriptores. |

### Observadores

| Archivo | Descripcion |
| :--- | :--- |
| **`ObservadorConsola.cs`** | Muestra de manera visual las alertas de notificacion directamente en la consola. |
| **`ObservadorUsuario.cs`** | Hereda de `IObservador`; filtra y notifica solo al usuario interesado en un evento especifico. |
| **`ObservadorLog.cs`** | Registra y almacena las notificaciones en un archivo de bitacora (*log*). |
| **`ObservadorStockBajo.cs`** | Hereda de `IObservador`; evalua el stock minimo y emite una alerta cuando un producto cae por debajo del umbral permitido. |
