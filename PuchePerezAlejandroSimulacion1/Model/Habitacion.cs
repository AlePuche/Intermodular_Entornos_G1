using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    // Representa una habitación dentro del sistema
    public class Habitacion
    {
        // Identificador único de la habitación
        public int IdHabitacion { get; set; }
        // Tipo de habitación (ejemplo: Suite, Doble, Sencilla, etc.)
        public string TipoHabitacion { get; set; }
        // Número máximo de personas que pueden alojarse en la habitación
        public int NumPersonas { get; set; }
        // Estado actual de la habitación (Disponible, Mantenimiento)
        public string Estado { get; set; }
        // Tamaño de la habitación en metros cuadrados (m²)
        public double Tamanyo { get; set; }
        // Descripción de la habitación con detalles adicionales
        public string Descripcion { get; set; }
        // Lista de URLs o rutas de las imágenes asociadas a la habitación
        public List<string> Imagenes { get; set; }
        // Precio base de la habitación por noche en euros (€)
        public double Precio { get; set; }

        // Constructor de la clase que inicializa la lista de imágenes
        public Habitacion()
        {
            Imagenes = new List<string>();
        }

    }
}
