using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PuchePerezAlejandroSimulacion1.Model;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3000") };
    }

    public async Task<bool> CreateReservaAsync(Reserva nuevaReserva)
    {
        try
        {
            var json = JsonConvert.SerializeObject(nuevaReserva);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/reservas", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Reserva creada correctamente.");
                return true;
            }

            Console.WriteLine($"Error al crear reserva: {response.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear reserva: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteReservaAsync(int id)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { id });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_httpClient.BaseAddress + "reservas"),
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Reserva eliminada correctamente.");
                return true;
            }

            Console.WriteLine($"Error al eliminar reserva: {response.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar reserva: {ex.Message}");
            return false;
        }
    }
}