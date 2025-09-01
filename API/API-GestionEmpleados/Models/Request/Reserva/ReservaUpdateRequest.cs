namespace API_GestionEmpleados.Models.Request.Reserva
{
    public class ReservaUpdateRequest
    {
        public int IdReserva { get; set; }
        public int NuevoIdEmpleado { get; set; }
        public int NuevoIdEquipo { get; set; }
    }
}
