using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    // Clase que representa una notificación en el sistema
    public class Notificacion
    {
        public string Mensaje { get; set; } // Contenido del mensaje de la notificación
        public string Fecha { get; set; } // Fecha en la que se generó la notificación (formateada como string)
    }
}
