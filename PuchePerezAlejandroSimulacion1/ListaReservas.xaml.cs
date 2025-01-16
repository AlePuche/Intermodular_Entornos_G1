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

        public ListaReservas()
        {
            InitializeComponent();
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
                new Reserva { Id = 1, Huespedes = 2, Huesped = "Juan Pérez", Precio = "200 €", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Doble" },
                new Reserva { Id = 2, Huespedes = 1, Huesped = "María López", Precio = "150 €", FechaInicio = DateTime.Now.AddDays(-2), FechaFin = DateTime.Now.AddDays(1), Tipo = "Suite" },
                new Reserva { Id = 3, Huespedes = 4, Huesped = "Carlos Díaz", Precio = "300 €", FechaInicio = DateTime.Now.AddDays(-5), FechaFin = DateTime.Now.AddDays(-2), Tipo = "Triple" },
                new Reserva { Id = 4, Huespedes = 3, Huesped = "Ana Gómez", Precio = "250 €", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Doble" },
                new Reserva { Id = 5, Huespedes = 5, Huesped = "Pedro Morales", Precio = "400 €", FechaInicio = DateTime.Now.AddDays(-7), FechaFin = DateTime.Now.AddDays(-4), Tipo = "Suite" },
                new Reserva { Id = 6, Huespedes = 1, Huesped = "Luis Martínez", Precio = "180 €", FechaInicio = DateTime.Now.AddDays(2), FechaFin = DateTime.Now.AddDays(4), Tipo = "Doble" },
                new Reserva { Id = 7, Huespedes = 2, Huesped = "Sofía Jiménez", Precio = "210 €", FechaInicio = DateTime.Now.AddDays(1), FechaFin = DateTime.Now.AddDays(3), Tipo = "Suite" },
                new Reserva { Id = 8, Huespedes = 4, Huesped = "Andrés García", Precio = "320 €", FechaInicio = DateTime.Now.AddDays(-1), FechaFin = DateTime.Now.AddDays(2), Tipo = "Triple" },
                new Reserva { Id = 9, Huespedes = 3, Huesped = "Claudia López", Precio = "280 €", FechaInicio = DateTime.Now.AddDays(-6), FechaFin = DateTime.Now.AddDays(-3), Tipo = "Doble" },
                new Reserva { Id = 10, Huespedes = 6, Huesped = "Fernando Torres", Precio = "500 €", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(-7), Tipo = "Suite" },
                new Reserva { Id = 11, Huespedes = 2, Huesped = "Isabel Ruiz", Precio = "200 €", FechaInicio = DateTime.Now.AddDays(5), FechaFin = DateTime.Now.AddDays(8), Tipo = "Doble" },
                new Reserva { Id = 12, Huespedes = 1, Huesped = "Oscar Fernández", Precio = "140 €", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Suite" },
                new Reserva { Id = 13, Huespedes = 2, Huesped = "Laura Muñoz", Precio = "230 €", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Doble" },
                new Reserva { Id = 14, Huespedes = 4, Huesped = "Pablo Ortega", Precio = "320 €", FechaInicio = DateTime.Now.AddDays(7), FechaFin = DateTime.Now.AddDays(10), Tipo = "Triple" },
                new Reserva { Id = 15, Huespedes = 3, Huesped = "Verónica Ramírez", Precio = "270 €", FechaInicio = DateTime.Now.AddDays(-8), FechaFin = DateTime.Now.AddDays(-5), Tipo = "Doble" },
                new Reserva { Id = 16, Huespedes = 1, Huesped = "David Sánchez", Precio = "120 €", FechaInicio = DateTime.Now.AddDays(-12), FechaFin = DateTime.Now.AddDays(-10), Tipo = "Suite" },
                new Reserva { Id = 17, Huespedes = 5, Huesped = "Carolina Vega", Precio = "450 €", FechaInicio = DateTime.Now.AddDays(-15), FechaFin = DateTime.Now.AddDays(-11), Tipo = "Triple" },
                new Reserva { Id = 18, Huespedes = 2, Huesped = "Alejandro Torres", Precio = "190 €", FechaInicio = DateTime.Now.AddDays(4), FechaFin = DateTime.Now.AddDays(6), Tipo = "Doble" },
                new Reserva { Id = 19, Huespedes = 3, Huesped = "Gabriela Flores", Precio = "260 €", FechaInicio = DateTime.Now.AddDays(9), FechaFin = DateTime.Now.AddDays(12), Tipo = "Suite" },
                new Reserva { Id = 20, Huespedes = 6, Huesped = "Jorge Rojas", Precio = "520 €", FechaInicio = DateTime.Now.AddDays(-20), FechaFin = DateTime.Now.AddDays(-17), Tipo = "Triple" },
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sg = new SegundaVentana();
            sg.Show();

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