namespace Tema4;

class Producto {
    // Variables con todo y el get y set
    public string Nombre { get; set; } = "";
    public string Codigo { get; set; } = "";
    public decimal Precio { get; set; } = 0;
    public int Cantidad { get; set; } = 0 ;

    // Constructores
    public Producto(string nombre, string codigo, decimal precio, int cantidad) {
        Nombre = nombre;
        Codigo = codigo;
        Precio = precio;
        Cantidad = cantidad;
    }

    public Producto() {}
}