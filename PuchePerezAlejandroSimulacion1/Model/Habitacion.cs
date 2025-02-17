using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    public class Habitacion
    {
        public int IdHabitacion { get; set; }
        public string TipoHabitacion { get; set; }
        public int NumPersonas { get; set; }
        public string Estado { get; set; }
        public double Tamanyo { get; set; }
        public string Descripcion { get; set; }
        public List<string> Imagenes { get; set; }
        public double Precio { get; set; }

        public Habitacion()
        {
            Imagenes = new List<string>();
        }

    }
}
