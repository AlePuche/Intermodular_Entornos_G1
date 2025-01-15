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
        // Propiedad que contiene las reservas
        public ObservableCollection<Reserva> Reservas { get; set; }

        public ListaReservas()
        {
            InitializeComponent();
            CargarListaReservas(); // Llama al método para cargar las reservas
            DataContext = this; // Configura el contexto de datos para enlazar con la vista
        }

        // Método para cargar las reservas
        private void CargarListaReservas()
        {
            Reservas = new ObservableCollection<Reserva>
            {
                new Reserva { Id = 1, Huesped = "Juan Pérez", Precio = "$200", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Vacacional", Vip = "Sí" },
                new Reserva { Id = 2, Huesped = "María López", Precio = "$150", FechaInicio = DateTime.Now.AddDays(-2), FechaFin = DateTime.Now.AddDays(1), Tipo = "Negocios", Vip = "No" },
                new Reserva { Id = 3, Huesped = "Carlos Díaz", Precio = "$300", FechaInicio = DateTime.Now.AddDays(-5), FechaFin = DateTime.Now.AddDays(-2), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 4, Huesped = "Ana Gómez", Precio = "$250", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Vacacional", Vip = "No" },
                new Reserva { Id = 5, Huesped = "Pedro Morales", Precio = "$400", FechaInicio = DateTime.Now.AddDays(-7), FechaFin = DateTime.Now.AddDays(-4), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 1, Huesped = "Juan Pérez", Precio = "$200", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Vacacional", Vip = "Sí" },
                new Reserva { Id = 2, Huesped = "María López", Precio = "$150", FechaInicio = DateTime.Now.AddDays(-2), FechaFin = DateTime.Now.AddDays(1), Tipo = "Negocios", Vip = "No" },
                new Reserva { Id = 3, Huesped = "Carlos Díaz", Precio = "$300", FechaInicio = DateTime.Now.AddDays(-5), FechaFin = DateTime.Now.AddDays(-2), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 4, Huesped = "Ana Gómez", Precio = "$250", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Vacacional", Vip = "No" },
                new Reserva { Id = 5, Huesped = "Pedro Morales", Precio = "$400", FechaInicio = DateTime.Now.AddDays(-7), FechaFin = DateTime.Now.AddDays(-4), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 1, Huesped = "Juan Pérez", Precio = "$200", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Vacacional", Vip = "Sí" },
                new Reserva { Id = 2, Huesped = "María López", Precio = "$150", FechaInicio = DateTime.Now.AddDays(-2), FechaFin = DateTime.Now.AddDays(1), Tipo = "Negocios", Vip = "No" },
                new Reserva { Id = 3, Huesped = "Carlos Díaz", Precio = "$300", FechaInicio = DateTime.Now.AddDays(-5), FechaFin = DateTime.Now.AddDays(-2), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 4, Huesped = "Ana Gómez", Precio = "$250", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Vacacional", Vip = "No" },
                new Reserva { Id = 5, Huesped = "Pedro Morales", Precio = "$400", FechaInicio = DateTime.Now.AddDays(-7), FechaFin = DateTime.Now.AddDays(-4), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 1, Huesped = "Juan Pérez", Precio = "$200", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Tipo = "Vacacional", Vip = "Sí" },
                new Reserva { Id = 2, Huesped = "María López", Precio = "$150", FechaInicio = DateTime.Now.AddDays(-2), FechaFin = DateTime.Now.AddDays(1), Tipo = "Negocios", Vip = "No" },
                new Reserva { Id = 3, Huesped = "Carlos Díaz", Precio = "$300", FechaInicio = DateTime.Now.AddDays(-5), FechaFin = DateTime.Now.AddDays(-2), Tipo = "Familiar", Vip = "Sí" },
                new Reserva { Id = 4, Huesped = "Ana Gómez", Precio = "$250", FechaInicio = DateTime.Now.AddDays(-3), FechaFin = DateTime.Now, Tipo = "Vacacional", Vip = "No" },
                new Reserva { Id = 5, Huesped = "Pedro Morales", Precio = "$400", FechaInicio = DateTime.Now.AddDays(-7), FechaFin = DateTime.Now.AddDays(-4), Tipo = "Familiar", Vip = "Sí" }
            };
        }
    }
}