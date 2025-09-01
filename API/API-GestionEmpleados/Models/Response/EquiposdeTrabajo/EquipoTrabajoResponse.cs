namespace API_GestionEmpleados.Models.Response.EquiposdeTrabajo
{
    public class EquipoTrabajoResponse
    {
        public int IdEquipo { get; set; }
        public string? NombreEquipo { get; set; }
        public int IdModelo { get; set; }
        public string? TipoEquipo { get; set; }
        public string? Marca { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? Estado { get; set; }
    }
}
