using Newtonsoft.Json;
using WebAppGestionEmpleados.Models.Response;
using static System.Net.WebRequestMethods;

namespace WebAppGestionEmpleados.Service
{
    public class EquiposdeTrabajoService
    {
        private readonly HttpClient _http;
        public EquiposdeTrabajoService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("MyWebApi");
        }

        public async Task<List<EquipodeTrabajoResponse>> GetAllAsync()
        {
            var res = await _http.GetAsync("/api/EquipodeTrabajo");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<EquipodeTrabajoResponse>>(await res.Content.ReadAsStringAsync()) ?? new()
                : new();
        }


        public async Task<EquipodeTrabajoResponse?> GetByIdAsync(int id)
        {
            var res = await _http.GetAsync($"/api/EquipodeTrabajo/id/{id}");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<EquipodeTrabajoResponse>(await res.Content.ReadAsStringAsync())
                : null;
        }

        public async Task<EquipodeTrabajoResponse?> GetByNameAsync(string nom_equipo)
        {
            var res = await _http.GetAsync($"/api/EquipodeTrabajo/name/{nom_equipo}");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<EquipodeTrabajoResponse>(await res.Content.ReadAsStringAsync())
                : null;
        }


        public async Task<EquipodeTrabajoResponse?> GetByTypeAsync(string tipo_equipo)
        {
            var res = await _http.GetAsync($"/api/EquipodeTrabajo/type/{tipo_equipo}");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<EquipodeTrabajoResponse>(await res.Content.ReadAsStringAsync())
                : null;
        }

        public async Task<EquipodeTrabajoResponse?> GetByStatusAsync(string estado)
        {
            var res = await _http.GetAsync($"/api/EquipodeTrabajo/status/{estado}");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<EquipodeTrabajoResponse>(await res.Content.ReadAsStringAsync())
                : null;
        }

        public async Task<EquipodeTrabajoResponse?> GetByDateAsync(DateTime fecha)
        {
            var res = await _http.GetAsync($"/api/EquipodeTrabajo/date/{fecha}");
            return res.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<EquipodeTrabajoResponse>(await res.Content.ReadAsStringAsync())
                : null;
        }







    }
}
