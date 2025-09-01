namespace API_GestionEmpleados.Models.Response.EquiposdeTrabajo
{
    public class EquipoTrabajoResponse
    {
        public int IdEquipo { get; set; }
        public string? NombreEquipo { get; set; }
        public string? TipoEquipo { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? Estado { get; set; }
        public int IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
    }
}
