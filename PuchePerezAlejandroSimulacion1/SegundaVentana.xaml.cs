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
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
                new Habitacion { Tipo = "Habitación Doble", PrecioPorNoche = "36€ / noche", FotoUrl = "Images/habitacion.png" },
           
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sv = new SegundaVentana();
            sv.Show();

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Buscador buscador = new Buscador();
            buscador.Show();

            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas();
            listaReservas.Show();

            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones();
            listaHabitaciones.Show();

            Close();
        }
    }
}
