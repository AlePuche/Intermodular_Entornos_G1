using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    public class Reserva
    {
        public int Id { get; set; }
        public int IdHabitacion { get; set; }
        public ClienteReserva Cliente { get; set; }
        public double Precio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaSalida { get; set; }
        public string TipoHabitacion { get; set; }
        public int NumPersonas { get; set; }
        public double Extras { get; set; }
        public string FechaInicioFormatted => FechaInicio.ToString("dd/MM/yyyy");
        public string FechaFinFormatted => FechaSalida.ToString("dd/MM/yyyy");
    }
}
