using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1
{
    public class HabitacionTipo
    {
        public int IdHabitacion { get; set; }  // ID único de la habitación
        public string Tipo { get; set; }  // Tipo de habitación (Ej: "Simple", "Doble", "Suite")
        public double Precio { get; set; }  // Precio por noche
        public string FotoUrl { get; set; }  // Imagen representativa
        public bool Disponible { get; set; }  // Si hay habitaciones disponibles de este tipo
    }

}
