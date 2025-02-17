using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuchePerezAlejandroSimulacion1.Model
{
    public class ClienteReserva
    {
        public ClienteReserva(string nombre, string email)
        {
            Nombre = nombre;
            Email = email;
        }

        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}
