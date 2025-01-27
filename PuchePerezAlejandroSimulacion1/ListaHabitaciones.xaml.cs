using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
                new Habitacion { Id = 104, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 4, Descripcion = "Decorada con estilo", Reservada = "Sí" },
                new Habitacion { Id = 105, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con balcón privado", Reservada = "No" },
                new Habitacion { Id = 106, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Familiar", Huespedes = 5, Descripcion = "Perfecta para familias", Reservada = "Sí" },
                new Habitacion { Id = 107, FotoUrl = "/Images/usuario.png", Tipo = "Suite Presidencial", Huespedes = 2, Descripcion = "Con jacuzzi privado", Reservada = "No" },
                new Habitacion { Id = 108, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Triple", Huespedes = 3, Descripcion = "Cómoda y espaciosa", Reservada = "Sí" },
                new Habitacion { Id = 109, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Cerca del gimnasio", Reservada = "No" },
                new Habitacion { Id = 110, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 2, Descripcion = "Con vista al jardín", Reservada = "Sí" },
                new Habitacion { Id = 111, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Económica", Huespedes = 2, Descripcion = "Ideal para viajes rápidos", Reservada = "No" },
                new Habitacion { Id = 112, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble Deluxe", Huespedes = 2, Descripcion = "Con decoración moderna", Reservada = "Sí" },
                new Habitacion { Id = 113, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Familiar", Huespedes = 4, Descripcion = "Cerca de la piscina", Reservada = "No" },
                new Habitacion { Id = 114, FotoUrl = "/Images/usuario.png", Tipo = "Suite Junior", Huespedes = 2, Descripcion = "Incluye sala de estar", Reservada = "Sí" },
                new Habitacion { Id = 115, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Con vistas a la ciudad", Reservada = "No" },
                new Habitacion { Id = 116, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Doble", Huespedes = 2, Descripcion = "Cerca del restaurante", Reservada = "Sí" },
                new Habitacion { Id = 117, FotoUrl = "/Images/usuario.png", Tipo = "Suite Presidencial", Huespedes = 3, Descripcion = "Con comedor privado", Reservada = "No" },
                new Habitacion { Id = 118, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Triple", Huespedes = 3, Descripcion = "Con acceso al spa", Reservada = "Sí" },
                new Habitacion { Id = 119, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Simple", Huespedes = 1, Descripcion = "Económica pero funcional", Reservada = "No" },
                new Habitacion { Id = 120, FotoUrl = "/Images/usuario.png", Tipo = "Habitación Familiar", Huespedes = 5, Descripcion = "Amplia y luminosa", Reservada = "Sí" }
            };  
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var habitacion = (Habitacion)button.DataContext;
                AddHabitacion ventana = new AddHabitacion(habitacion);
                ventana.Show();
            }
        }

        private void ReservasButton_Click(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas();
            lr.Show();
            Close();
        }

        private void UsuariosButton_Click(object sender, RoutedEventArgs e)
        {
            Buscador b = new Buscador();
            b.Show();
            Close();
        }

        private void HabitacionesButton_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones lh = new ListaHabitaciones();
            lh.Show();
            Close();
        }

        private void BuscadorButton_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana s = new SegundaVentana();
            s.Show();
            Close();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddHabitacion ventana = new AddHabitacion();
            ventana.Show();
        }
    }
}