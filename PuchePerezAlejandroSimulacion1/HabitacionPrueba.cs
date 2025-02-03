using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1
{
    public class HabitacionPrueba
    {
        public int IdHabitacion { get; set; }  // Identificador único
        public string TipoHabitacion { get; set; }  // Nombre del tipo de habitación (Ej: "Simple", "Doble", "Suite")
        public int NumPersonas { get; set; }  // Capacidad estándar de la habitación
        public string Estado { get; set; }  // "Disponible" o "Mantenimiento"
        public double Tamanyo { get; set; }  // Tamaño en m²
        public string Descripcion { get; set; }  // Información adicional
        public List<string> Imagenes { get; set; }  // Lista de URLs de imágenes
        public double Precio { get; set; }  // Precio base por noche

        public HabitacionPrueba()
        {
            Imagenes = new List<string>();
        }
    }
}
