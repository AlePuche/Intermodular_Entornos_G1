using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
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
        public readonly HttpClient _httpClient;
        public ListaHabitaciones(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            DataContext = this;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            Habitaciones = new ObservableCollection<Habitacion>();
            CargarListaHabitaciones();
            
        }

        private async Task CargarListaHabitaciones()
        {
            try
            {
                var response = await _httpClient.GetAsync("/habitaciones/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var habitaciones = JsonSerializer.Deserialize<Habitacion[]>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    foreach (var habitacion in habitaciones)
                    {
                        Habitaciones.Add(habitacion);
                    }
                }
                else
                {
                    Console.WriteLine($"Error al obtener habitaciones: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
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
                    var result = MessageBox.Show($"¿Seguro que deseas eliminar la habitación {habitacion.IdHabitacion}?",
                                                 "Confirmación",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        Habitaciones.Remove(habitacion);

                        MessageBox.Show($"Habitación {habitacion.IdHabitacion} eliminada correctamente.",
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