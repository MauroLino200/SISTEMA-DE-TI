namespace API_GestionEmpleados.Models.Request.Evaluaciones
{
    public class EvaluacionInsertRequest
    {
        public string? Curso_de_la_Evaluación { get; set; }
        public int IdEmpleado { get; set; }

        public string? NombreCompleto { get; set; }
        public int IdCapacitacion { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public bool Estado { get; set; }
        public int Calificacion { get; set; }
        public string? Comentarios { get; set; }
    }
}
