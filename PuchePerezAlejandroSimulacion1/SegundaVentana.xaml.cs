using System.Collections.Generic;
using System.Windows;

namespace PuchePerezAlejandroSimulacion1
{
    public partial class SegundaVentana : Window
    {
        public List<Habitacion> Habitaciones { get; set; }

        public Usuario usuarioLogeado { get; private set; }

        public SegundaVentana(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
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
            SegundaVentana sv = new SegundaVentana(usuarioLogeado);
            sv.Show();

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Buscador buscador = new Buscador(usuarioLogeado);
            buscador.Show();

            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas(usuarioLogeado);
            listaReservas.Show();

            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones(usuarioLogeado);
            listaHabitaciones.Show();

            Close();
        }

        private void btnReservar_Click(object sender, RoutedEventArgs e)
        {
            CrearReserva crearReserva = new CrearReserva();
            crearReserva.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            listaScroll.Visibility = Visibility.Visible;
        }
    }
}
