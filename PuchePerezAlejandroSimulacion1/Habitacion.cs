using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1
{
    public class Habitacion
    {
        public int Id { get; set; }
        public string FotoUrl { get; set; }
        public string Tipo { get; set; }
        public int Huespedes { get; set; }
        public string Descripcion { get; set; }
        public string Reservada { get; set; } // "Sí" o "No"
        public string PrecioPorNoche { get; set; }
        
    }
}
