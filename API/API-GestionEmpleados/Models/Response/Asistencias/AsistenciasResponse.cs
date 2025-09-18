namespace API_GestionEmpleados.Models.Response.Asistencias
{
    public class AsistenciasResponse
    {
        public int IdAsistencia { get; set; }

        public int IdEmpleado { get; set; }

        public string? NombreCompleto { get; set; }
        public int IdTipoDocumento { get; set; }

        public int NumeroDocumento { get; set; }

        public int IdCargo { get; set; }

        public int IdDepartamento { get; set; }

        public DateTime HoraEntrada { get; set; }

        public DateTime HoraSalida { get; set; }

    }
}
