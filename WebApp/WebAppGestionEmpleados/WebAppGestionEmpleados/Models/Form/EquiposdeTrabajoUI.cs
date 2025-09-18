using System.ComponentModel.DataAnnotations;
using WebAppGestionEmpleados.Models.Response;

namespace WebAppGestionEmpleados.Models.Form

{
    public class EquiposdeTrabajoUI
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Campo requerido")]
        public int IdEquipo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? NombreEquipo { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Campo requerido")]
        public int IdModelo { get; set; }

        [Display(Name = "Tipo de Equipo")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? TipoEquipo { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? Marca { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = "Campo requerido")]
        public int Cantidad { get; set; }

        [Display(Name = "Fecha Asignación")]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime FechaAsignacion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? Estado { get; set; }

        public EquiposdeTrabajoUI() { }

        // Constructor que recibe un objeto EquipodeTrabajoResponse
        public EquiposdeTrabajoUI(EquipodeTrabajoResponse r)
        {
            IdEquipo = r.IdEquipo;
            NombreEquipo = r.NombreEquipo;
            IdModelo = r.IdModelo;
            TipoEquipo = r.TipoEquipo;
            Marca = r.Marca;
            Cantidad = r.Cantidad;
            FechaAsignacion = r.FechaAsignacion;
            Estado = r.Estado;
        }
    }
}
