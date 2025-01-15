using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1
{
    public class Reserva
    {
        public int Id { get; set; }
        public string Huesped { get; set; }
        public string Precio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Tipo { get; set; }
        public string Vip { get; set; } // "Sí" o "No"

        public string FechaInicioFormatted => FechaInicio.ToString("dd/MM/yyyy");
        public string FechaFinFormatted => FechaFin.ToString("dd/MM/yyyy");
    }
}
