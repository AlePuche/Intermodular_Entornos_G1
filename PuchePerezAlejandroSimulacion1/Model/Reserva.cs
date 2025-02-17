using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    // Clase que representa una reserva en el sistema
    public class Reserva
    {
        public int Id { get; set; } // Identificador único de la reserva
        public int IdHabitacion { get; set; } // Identificador de la habitación reservada
        public ClienteReserva Cliente { get; set; } // Datos del cliente que realizó la reserva
        public double Precio { get; set; } // Precio total de la reserva
        public DateTime FechaInicio { get; set; } // Fecha de inicio de la estancia
        public DateTime FechaSalida { get; set; } // Fecha de salida de la estancia
        public string TipoHabitacion { get; set; } // Tipo de habitación reservada
        public int NumPersonas { get; set; } // Número de personas que ocuparán la habitación
        public double Extras { get; set; } // Coste de extras añadidos a la reserva

        // Propiedad de solo lectura que formatea la fecha de inicio como "dd/MM/yyyy"
        public string FechaInicioFormatted => FechaInicio.ToString("dd/MM/yyyy");

        // Propiedad de solo lectura que formatea la fecha de salida como "dd/MM/yyyy"
        public string FechaFinFormatted => FechaSalida.ToString("dd/MM/yyyy");
    }
}
