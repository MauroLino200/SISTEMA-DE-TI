using Microsoft.AspNetCore.Mvc;
using WebAppGestionEmpleados.Service;

namespace WebAppGestionEmpleados.Controllers
{
    public class EquiposdeTrabajoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EquiposdeTrabajoService _equiposdeTrabajoService;

        public EquiposdeTrabajoController(IHttpClientFactory httpClientFactory, EquiposdeTrabajoService equiposdeTrabajoService)
        {
            _httpClientFactory = httpClientFactory;
            _equiposdeTrabajoService = equiposdeTrabajoService;
        }

        public async Task<IActionResult> ListarEquipos(int numberPage = 0)
        {
            var listado = await _equiposdeTrabajoService.GetAllAsync();

            var rowsPerPage = 5;
            var totalRecords = listado.Count();

            var paginas = (int)Math.Ceiling(totalRecords * 1.0 / rowsPerPage);
            var filtrado = listado.Skip(numberPage * rowsPerPage).Take(rowsPerPage);

            ViewBag.numberPage = numberPage;
            ViewBag.paginas = paginas;

            return View(filtrado);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPorNombre(string term)
        {
            var equipo = await _equiposdeTrabajoService.GetByNameAsync(term ?? "");
            return Json(equipo); // si no encuentra, será null
        }



        public IActionResult EquiposdeTrabajo()
        {
            return View();
        }
    }
}
