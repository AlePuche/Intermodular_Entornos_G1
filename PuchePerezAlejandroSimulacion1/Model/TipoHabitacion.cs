using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    // Representa un tipo de habitación dentro del sistema
    public class TipoHabitacion
    {
        // Identificador único del tipo de habitación
        public int IdHabitacion { get; set; }
        // Nombre del tipo de habitación, se serializa como "nombre" al convertirlo en JSON
        [JsonPropertyName("nombre")]
        public string Tipo { get; set; }
        // Precio base del tipo de habitación por noche en euros (€), se serializa como "precioBase" en JSON
        [JsonPropertyName("precioBase")]
        public double Precio { get; set; }
        // Capacidad máxima de huéspedes permitida en este tipo de habitación
        public int CapacidadMaxima { get; set; }
        // Lista de servicios incluidos en la habitación (Ejemplo: WiFi, TV, MiniBar, etc.)
        public List<string> Servicios { get; set; } = new List<string>();
        // URL o ruta de la imagen representativa del tipo de habitación
        public string FotoUrl { get; set; }
        // Indica si este tipo de habitación está disponible para reservas
        public bool Disponible { get; set; }
    }
}
