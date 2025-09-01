namespace API_GestionEmpleados.Models.Request.EquiposdeTrabajo
{
    public class EquipoTrabajoInsertRequest
    {
        public string? NombreEquipo { get; set; }
        public string? TipoEquipo { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? Estado { get; set; }
        public int IdEmpleado { get; set; }
    }
}
