using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
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
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; }
        public Usuario usuarioLogeado { get; set; }
        public readonly HttpClient _httpClient;
        public ListaHabitaciones(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            DataContext = this;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            Habitaciones = new ObservableCollection<Habitacion>();
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();
            
            _ = CargarListaHabitaciones();
            _ = CargarTiposHabitacion();
            
        }

        private async Task CargarTiposHabitacion()
        {
            try
            {
                var response = await _httpClient.GetAsync("/tipos-habitacion/getAll");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var tipos = JsonSerializer.Deserialize<TipoHabitacion[]>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    foreach (var tipo in tipos)
                    {
                        TiposHabitacion.Add(tipo);
                    }
                }
                else
                {
                    MessageBox.Show("Error al obtener los tipos de habitación.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
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
                AddHabitacion ventana = new AddHabitacion(habitacion, usuarioLogeado);
                ventana.Show();
                this.Close();
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
            AddHabitacion ventana = new AddHabitacion(usuarioLogeado);
            ventana.Show();
            Close();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
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
                        try
                        {
                            // 🔥 Configurar datos de la petición
                            var json = JsonSerializer.Serialize(new { idHabitacion = habitacion.IdHabitacion });
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            // 🔥 Enviar solicitud a la API
                            var response = await _httpClient.PostAsync("/habitaciones/delete", content);
                            var responseMessage = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                // 🔥 Eliminar la habitación de la lista si la API lo confirma
                                Habitaciones.Remove(habitacion);
                                MessageBox.Show($"Habitación {habitacion.IdHabitacion} eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show($"Error al eliminar la habitación: {responseMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private async void btn_BuscarHab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 🔥 Recoger los valores de los filtros en la UI
                int.TryParse(IdHabitacionTextBox.Text, out int idHabitacion);
                int.TryParse(((ComboBoxItem)AforoComboBox.SelectedItem)?.Content?.ToString(), out int numPersonasMax);
                string tipoHabitacion = ((TipoHabitacion)txtTipo.SelectedItem)?.Tipo;
                double.TryParse(PrecioMaxTextBox.Text, out double precioMax);
                string estado = ((ComboBoxItem)EstadoComboBox.SelectedItem)?.Content?.ToString();
                double.TryParse(TamanyoMaxTextBox.Text, out double tamanyoMax);

                // 🔹 Construir el objeto con los filtros
                var filtros = new
                {
                    idHabitacion = idHabitacion > 0 ? idHabitacion : (int?)null,
                    tipoHabitacion = !string.IsNullOrWhiteSpace(tipoHabitacion) ? tipoHabitacion : null,
                    numPersonasMax = numPersonasMax > 0 ? numPersonasMax : (int?)null,
                    estado = !string.IsNullOrWhiteSpace(estado) ? estado : null,
                    precioMax = precioMax > 0 ? precioMax : (double?)null,
                    tamanyoMax = tamanyoMax > 0 ? tamanyoMax : (double?)null
                };

                var json = JsonSerializer.Serialize(filtros);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔥 Enviar petición a la API
                var response = await _httpClient.PostAsync("/habitaciones/filter", content);
                var responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var habitacionesFiltradas = JsonSerializer.Deserialize<Habitacion[]>(responseMessage, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // 🔥 Limpiar y actualizar la lista de habitaciones
                    Habitaciones.Clear();
                    foreach (var habitacion in habitacionesFiltradas)
                    {
                        Habitaciones.Add(habitacion);
                    }

                    if (Habitaciones.Count == 0)
                    {
                        MessageBox.Show("No se encontraron habitaciones con esos filtros.", "Sin Resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Error al filtrar habitaciones: {responseMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}