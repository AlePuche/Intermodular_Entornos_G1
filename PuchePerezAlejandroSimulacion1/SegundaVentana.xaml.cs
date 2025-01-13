using System.Collections.Generic;
using System.Windows;

namespace PuchePerezAlejandroSimulacion1
{
    public partial class SegundaVentana : Window
    {
        public List<Habitacion> Habitaciones { get; set; }

        public SegundaVentana()
        {
            InitializeComponent();
            CargarHabitaciones();
            DataContext = this;
        }

        private void CargarHabitaciones()
        {
            Habitaciones = new List<Habitacion>
            {
                new Habitacion { Nombre = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Habitación Deluxe", PrecioPorNoche = "50€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Suite Presidencial", PrecioPorNoche = "120€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Habitación Económica", PrecioPorNoche = "25€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Habitación Deluxe", PrecioPorNoche = "50€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Suite Presidencial", PrecioPorNoche = "120€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Nombre = "Habitación Económica", PrecioPorNoche = "25€ / noche", FotoUrl = "Images/habitacion.png" }
            };
        }
    }
}
