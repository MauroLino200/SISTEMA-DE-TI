using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;

namespace API_GestionEmpleados.Models.Response.Reserva
{
    public class ReservaResponse 
    {
        public int IdReserva { get; set; }
        public int IdEquipo { get; set; }
        public string? NombreEquipo { get; set; }
        public int IdModelo { get; set; }
        public string? TipoEquipo { get; set; }
        public string? Marca { get; set; }
        public int IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public int IdCargo { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime FechaReserva { get; set; }= DateTime.Now;

    }
}
