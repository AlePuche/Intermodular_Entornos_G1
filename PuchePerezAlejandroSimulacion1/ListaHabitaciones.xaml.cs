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
    /// Lógica de interacción para ListaHabitaciones.xaml
    /// </summary>
    public partial class ListaHabitaciones : Window
    {
        public ObservableCollection<Habitacion> Habitaciones { get; set; }
        public ListaHabitaciones()
        {
            InitializeComponent();
            CargarListaHabitaciones();
            DataContext = this;
        }
        private void CargarListaHabitaciones() 
        {
            Habitaciones = new ObservableCollection<Habitacion>
            {
                new Habitacion { Id = 102, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 103, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 104, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 105, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Amplia y cómoda", Reservada = "Sí" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas al mar", Reservada = "No" }
            };
        }

        
    }
}
