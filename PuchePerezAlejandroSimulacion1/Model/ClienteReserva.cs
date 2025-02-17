using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    // Clase que representa la información del cliente asociado a una reserva
    public class ClienteReserva
    {
        // Constructor que inicializa el nombre y el email del cliente
        public ClienteReserva(string nombre, string email)
        {
            Nombre = nombre;
            Email = email;
        }

        public string Nombre { get; set; } // Nombre del cliente
        public string Email { get; set; }  // Correo electrónico del cliente
    }
}
