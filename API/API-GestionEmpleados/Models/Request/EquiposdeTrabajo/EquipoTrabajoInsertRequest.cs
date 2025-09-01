namespace API_GestionEmpleados.Models.Request.EquiposdeTrabajo
{
    public class EquipoTrabajoInsertRequest
    {
        public int IdModelo { get; set; }
        public string? NombreEquipo { get; set; }
        public string? TipoEquipo { get; set; }
        public string? Marca { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? Estado { get; set; }
    }
}
