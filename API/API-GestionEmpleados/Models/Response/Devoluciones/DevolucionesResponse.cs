namespace API_GestionEmpleados.Models.Response.Devoluciones
{
    public class DevolucionesResponse
    {
        public int IdDevolucion { get; set; }
        public int IdEquipo { get; set; }
        public string? NombreEquipo { get; set; }
        public string? IdModelo { get; set; }
        public string? TipoEquipo { get; set; }
        public string? Marca { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaDevolucion { get; set; }            
    }
}
