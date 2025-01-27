using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para ListaReservas.xaml
    /// </summary>
    public partial class ListaReservas : Window
    {
        public ObservableCollection<Reserva> Reservas { get; set; }
        public Usuario usuarioLogeado { get; private set; }
        public ListaReservas(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            CargarListaReservas(); 
            DataContext = this;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                CrearReserva ventanaCrearReserva = new CrearReserva(false, reservaSeleccionada);
                ventanaCrearReserva.Show();
            }
            else
            {
                MessageBox.Show("No se pudo obtener la reserva seleccionada.");
            }
        }

        private void EditarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                CrearReserva ventanaCrearReserva = new CrearReserva(true, reservaSeleccionada);
                ventanaCrearReserva.Show();
            }
            else
            {
                MessageBox.Show("No se pudo obtener la reserva seleccionada.");
            }
        }

        private void CargarListaReservas()
        {
            Reservas = new ObservableCollection<Reserva>
            {
                new Reserva { Id = 1, IdHabitacion = 101, Cliente = new ClienteReserva { Nombre = "Juan Pérez", Email = "juan.perez@example.com" }, Precio = 200, FechaInicio = DateTime.Now, FechaSalida = DateTime.Now.AddDays(3), TipoHabitacion = "Doble", NumPersonas = 2, Extras = 30 },
                new Reserva { Id = 2, IdHabitacion = 102, Cliente = new ClienteReserva { Nombre = "María López", Email = "maria.lopez@example.com" }, Precio = 150, FechaInicio = DateTime.Now.AddDays(-2), FechaSalida = DateTime.Now.AddDays(1), TipoHabitacion = "Suite", NumPersonas = 1, Extras = 20 },
                new Reserva { Id = 3, IdHabitacion = 103, Cliente = new ClienteReserva { Nombre = "Carlos Díaz", Email = "carlos.diaz@example.com" }, Precio = 300, FechaInicio = DateTime.Now.AddDays(-5), FechaSalida = DateTime.Now.AddDays(-2), TipoHabitacion = "Triple", NumPersonas = 4, Extras = 50 },
                new Reserva { Id = 4, IdHabitacion = 104, Cliente = new ClienteReserva { Nombre = "Ana Gómez", Email = "ana.gomez@example.com" }, Precio = 250, FechaInicio = DateTime.Now.AddDays(-3), FechaSalida = DateTime.Now, TipoHabitacion = "Doble", NumPersonas = 3, Extras = 40 },
                new Reserva { Id = 5, IdHabitacion = 105, Cliente = new ClienteReserva { Nombre = "Pedro Morales", Email = "pedro.morales@example.com" }, Precio = 400, FechaInicio = DateTime.Now.AddDays(-7), FechaSalida = DateTime.Now.AddDays(-4), TipoHabitacion = "Suite", NumPersonas = 5, Extras = 60 },
                new Reserva { Id = 6, IdHabitacion = 106, Cliente = new ClienteReserva { Nombre = "Luis Martínez", Email = "luis.martinez@example.com" }, Precio = 180, FechaInicio = DateTime.Now.AddDays(2), FechaSalida = DateTime.Now.AddDays(4), TipoHabitacion = "Doble", NumPersonas = 1, Extras = 15 },
                new Reserva { Id = 7, IdHabitacion = 107, Cliente = new ClienteReserva { Nombre = "Sofía Jiménez", Email = "sofia.jimenez@example.com" }, Precio = 210, FechaInicio = DateTime.Now.AddDays(1), FechaSalida = DateTime.Now.AddDays(3), TipoHabitacion = "Suite", NumPersonas = 2, Extras = 25 },
                new Reserva { Id = 8, IdHabitacion = 108, Cliente = new ClienteReserva { Nombre = "Andrés García", Email = "andres.garcia@example.com" }, Precio = 320, FechaInicio = DateTime.Now.AddDays(-1), FechaSalida = DateTime.Now.AddDays(2), TipoHabitacion = "Triple", NumPersonas = 4, Extras = 50 },
                new Reserva { Id = 9, IdHabitacion = 109, Cliente = new ClienteReserva { Nombre = "Claudia López", Email = "claudia.lopez@example.com" }, Precio = 280, FechaInicio = DateTime.Now.AddDays(-6), FechaSalida = DateTime.Now.AddDays(-3), TipoHabitacion = "Doble", NumPersonas = 3, Extras = 35 },
                new Reserva { Id = 10, IdHabitacion = 110, Cliente = new ClienteReserva { Nombre = "Fernando Torres", Email = "fernando.torres@example.com" }, Precio = 500, FechaInicio = DateTime.Now.AddDays(-10), FechaSalida = DateTime.Now.AddDays(-7), TipoHabitacion = "Suite", NumPersonas = 6, Extras = 80 },
                new Reserva { Id = 11, IdHabitacion = 111, Cliente = new ClienteReserva { Nombre = "Isabel Ruiz", Email = "isabel.ruiz@example.com" }, Precio = 200, FechaInicio = DateTime.Now.AddDays(5), FechaSalida = DateTime.Now.AddDays(8), TipoHabitacion = "Doble", NumPersonas = 2, Extras = 30 },
                new Reserva { Id = 12, IdHabitacion = 112, Cliente = new ClienteReserva { Nombre = "Oscar Fernández", Email = "oscar.fernandez@example.com" }, Precio = 140, FechaInicio = DateTime.Now.AddDays(-3), FechaSalida = DateTime.Now, TipoHabitacion = "Suite", NumPersonas = 1, Extras = 20 },
                new Reserva { Id = 13, IdHabitacion = 113, Cliente = new ClienteReserva { Nombre = "Laura Muñoz", Email = "laura.munoz@example.com" }, Precio = 230, FechaInicio = DateTime.Now, FechaSalida = DateTime.Now.AddDays(3), TipoHabitacion = "Doble", NumPersonas = 2, Extras = 40 },
                new Reserva { Id = 14, IdHabitacion = 114, Cliente = new ClienteReserva { Nombre = "Pablo Ortega", Email = "pablo.ortega@example.com" }, Precio = 320, FechaInicio = DateTime.Now.AddDays(7), FechaSalida = DateTime.Now.AddDays(10), TipoHabitacion = "Triple", NumPersonas = 4, Extras = 50 },
                new Reserva { Id = 15, IdHabitacion = 115, Cliente = new ClienteReserva { Nombre = "Verónica Ramírez", Email = "veronica.ramirez@example.com" }, Precio = 270, FechaInicio = DateTime.Now.AddDays(-8), FechaSalida = DateTime.Now.AddDays(-5), TipoHabitacion = "Doble", NumPersonas = 3, Extras = 35 },
                new Reserva { Id = 16, IdHabitacion = 116, Cliente = new ClienteReserva { Nombre = "David Sánchez", Email = "david.sanchez@example.com" }, Precio = 120, FechaInicio = DateTime.Now.AddDays(-12), FechaSalida = DateTime.Now.AddDays(-10), TipoHabitacion = "Suite", NumPersonas = 1, Extras = 15 },
                new Reserva { Id = 17, IdHabitacion = 117, Cliente = new ClienteReserva { Nombre = "Carolina Vega", Email = "carolina.vega@example.com" }, Precio = 450, FechaInicio = DateTime.Now.AddDays(-15), FechaSalida = DateTime.Now.AddDays(-11), TipoHabitacion = "Triple", NumPersonas = 5, Extras = 70 },
                new Reserva { Id = 18, IdHabitacion = 118, Cliente = new ClienteReserva { Nombre = "Alejandro Torres", Email = "alejandro.torres@example.com" }, Precio = 190, FechaInicio = DateTime.Now.AddDays(4), FechaSalida = DateTime.Now.AddDays(6), TipoHabitacion = "Doble", NumPersonas = 2, Extras = 25 },
                new Reserva { Id = 19, IdHabitacion = 119, Cliente = new ClienteReserva { Nombre = "Gabriela Flores", Email = "gabriela.flores@example.com" }, Precio = 260, FechaInicio = DateTime.Now.AddDays(9), FechaSalida = DateTime.Now.AddDays(12), TipoHabitacion = "Suite", NumPersonas = 3, Extras = 40 },
                new Reserva { Id = 20, IdHabitacion = 120, Cliente = new ClienteReserva { Nombre = "Jorge Rojas", Email = "jorge.rojas@example.com" }, Precio = 520, FechaInicio = DateTime.Now.AddDays(-20), FechaSalida = DateTime.Now.AddDays(-17), TipoHabitacion = "Triple", NumPersonas = 6, Extras = 100 },

            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sg = new SegundaVentana(usuarioLogeado);
            sg.Show();

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
    }
}