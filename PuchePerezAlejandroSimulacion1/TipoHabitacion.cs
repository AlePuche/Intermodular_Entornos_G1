using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1
{
    public class TipoHabitacion
    {
        public int IdHabitacion { get; set; }
        [JsonPropertyName("nombre")]
        public string Tipo { get; set; }
        [JsonPropertyName("precioBase")]
        public double Precio { get; set; }
        public int CapacidadMaxima { get; set; }
        public List<string> Servicios { get; set; } = new List<string>();
        public string FotoUrl { get; set; }
        public bool Disponible { get; set; }
    }
}
