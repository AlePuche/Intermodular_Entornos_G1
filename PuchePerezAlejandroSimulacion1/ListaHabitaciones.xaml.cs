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
        public Usuario usuarioLogeado { get; set; }
        public ListaHabitaciones(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            Habitaciones = new ObservableCollection<Habitacion>();
            DataContext = this;
        }

        

        private void CargarListaHabitaciones()
        {
            Habitaciones.Clear();
            var nuevasHabitaciones = new ObservableCollection<Habitacion>
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
            foreach (var habitacion in nuevasHabitaciones)
            {
                Habitaciones.Add(habitacion);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogeado.Role == "Empleado")
            {
                MessageBox.Show("No tienes permisos para editar habitaciones.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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
            ListaReservas lr = new ListaReservas(usuarioLogeado);
            lr.Show();
            Close();
        }

        private void UsuariosButton_Click(object sender, RoutedEventArgs e)
        {
            Buscador b = new Buscador(usuarioLogeado);
            b.Show();
            Close();
        }

        private void HabitacionesButton_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones lh = new ListaHabitaciones(usuarioLogeado);
            lh.Show();
            Close();
        }

        private void BuscadorButton_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana s = new SegundaVentana(usuarioLogeado);
            s.Show();
            Close();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogeado.Role == "Empleado")
            {
                MessageBox.Show("No tienes permisos para añadir habitaciones.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddHabitacion ventana = new AddHabitacion();
            ventana.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogeado.Role == "Empleado")
            {
                MessageBox.Show("No tienes permisos para eliminar habitaciones.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var button = sender as Button;
            if (button != null)
            {
                var habitacion = button.DataContext as Habitacion;
                if (habitacion != null)
                {
                    var result = MessageBox.Show($"¿Seguro que deseas eliminar la habitación {habitacion.Id}?",
                                                 "Confirmación",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        Habitaciones.Remove(habitacion);

                        MessageBox.Show($"Habitación {habitacion.Id} eliminada correctamente.",
                                        "Éxito",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                }
            }
        }

        private void btn_BuscarHab_Click(object sender, RoutedEventArgs e)
        {
            CargarListaHabitaciones();
        }
    }
}