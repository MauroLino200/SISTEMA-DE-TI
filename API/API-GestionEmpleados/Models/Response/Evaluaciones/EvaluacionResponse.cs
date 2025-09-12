namespace API_GestionEmpleados.Models.Response.Evaluaciones
{
    public class EvaluacionResponse
    {
        public int IdEvaluacion { get; set; }

        public string? Curso_de_la_Evaluación { get; set; }

        public int IdEmpleado { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Cargo { get; set; }

        public string? NombreDepartamento { get; set; }
        public string? Ubicacion { get; set; }
        public int Calificacion { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string? Comentarios { get; set; }
    }
}
