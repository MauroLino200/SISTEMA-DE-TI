namespace API_GestionEmpleados.Models.Request.Contratos
{
    public class ContratoInsertRequest
    {
        public int IdEmpleado { get; set; }
        public string? TipoContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal SueldoBase { get; set; }
    }
}
