namespace API_GestionEmpleados.Models.Request.Evaluaciones
{
    public class EvaluacionInsertRequest
    {
        public int IdEmpleado { get; set; }
        public int IdCapacitacion { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public bool Estado { get; set; }
        public int Calificacion { get; set; }
        public string? Comentarios { get; set; }
    }
}
