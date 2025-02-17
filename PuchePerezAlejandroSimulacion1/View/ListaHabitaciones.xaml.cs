using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PuchePerezAlejandroSimulacion1.Model;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para ListaHabitaciones.xaml
    /// </summary>
    public partial class ListaHabitaciones : Window
    {
        // Colecciones
        public ObservableCollection<Habitacion> Habitaciones { get; set; }
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; }
        public ObservableCollection<Notificacion> Notificaciones { get; set; } = new ObservableCollection<Notificacion>();
        
        // Usuario logeado en el sistema
        public Usuario usuarioLogeado { get; set; }

        // Cliente HTTP para conectar con la API
        public readonly HttpClient _httpClient;

        // Variable que almacena la ruta del ícono de la campana de notificaciones
        private string _rutaCampana;
        public string RutaCampana
        {
            get => _rutaCampana;
            set
            {
                _rutaCampana = value;
                // Actualiza la UI en el hilo principal
                Dispatcher.Invoke(() =>
                {
                    CampanaIcono.Source = new BitmapImage(new Uri(_rutaCampana, UriKind.RelativeOrAbsolute));
                });
            }
        }

        // Constructor de la ventana, recibe el usuario logueado
        public ListaHabitaciones(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            DataContext = this; // Asigna el contexto de datos para el data binding

            // Verifica si el usuario es administrador para mostrar los botones de gestión
            if (usuarioLogeado.Role == "Admin")
            {
                AdminButtonsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                AdminButtonsGrid.Visibility = Visibility.Collapsed;
            }

            // Configuración del cliente HTTP
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Inicializa las colecciones
            Habitaciones = new ObservableCollection<Habitacion>();
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();

            // Carga los datos desde la API
            _ = CargarListaHabitaciones();
            _ = CargarTiposHabitacion();
            VerificarNotificaciones();

        }

        // Carga los tipos de habitaciones desde la API
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

        // Carga la lista de habitaciones desde la API
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

        // Evento de clic para editar una habitación
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
            AddHabitacion ventana = new AddHabitacion(usuarioLogeado);
            ventana.Show();
            Close();
        }

        private void btnAddTipo_Click(object sender, RoutedEventArgs e)
        {
            AddTipoHabitacion ventanaAddTipo = new AddTipoHabitacion(false, usuarioLogeado);
            ventanaAddTipo.Show();
            Close();
        }

        private void btnEditTipo_Click(object sender, RoutedEventArgs e)
        {
            AddTipoHabitacion ventanaEditTipo = new AddTipoHabitacion(true, usuarioLogeado); 
            ventanaEditTipo.Show();
            Close();
        }

        // Evento para eliminar una habitación
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
                            var json = JsonSerializer.Serialize(new { idHabitacion = habitacion.IdHabitacion });
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            var response = await _httpClient.PostAsync("/habitaciones/delete", content);
                            var responseMessage = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                Habitaciones.Remove(habitacion);
                                MessageBox.Show($"Habitación {habitacion.IdHabitacion} eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                                ListaHabitaciones v = new ListaHabitaciones(usuarioLogeado);
                                v.Show();
                                Close();
                            }
                            else
                            {
                                try
                                {
                                    var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseMessage);
                                    string errorMessage = errorData.ContainsKey("message") ? errorData["message"].ToString() : "Ocurrió un error.";

                                    if (errorData.ContainsKey("reservas") && errorData["reservas"] is JsonElement reservasElement)
                                    {
                                        var reservas = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(reservasElement.ToString());
                                        errorMessage += "\n\n🔹 Reservas activas en esta habitación:\n";
                                        foreach (var res in reservas)
                                        {
                                            errorMessage += $"- ID: {res["id"]}, Desde: {res["fechaInicio"]}, Hasta: {res["fechaSalida"]}\n";
                                        }
                                    }

                                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error desconocido. Respuesta del servidor:\n{responseMessage}\n\nDetalles: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
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

        // Restringe la entrada de texto en un TextBox a solo números
        private void TextBox_NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        // Realiza una búsqueda de habitaciones aplicando los filtros ingresados en la UI
        private async void btn_BuscarHab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(IdHabitacionTextBox.Text, out int idHabitacion);
                int.TryParse(AforoMinTextBox.Text, out int aforoMin);
                int.TryParse(AforoMaxTextBox.Text, out int aforoMax);
                string tipoHabitacion = ((TipoHabitacion)ComboBoxTipo.SelectedItem)?.Tipo;
                double.TryParse(PrecioMinTextBox.Text, out double precioMin);
                double.TryParse(PrecioMaxTextBox.Text, out double precioMax);
                string estado = ((ComboBoxItem)EstadoComboBox.SelectedItem)?.Content?.ToString();
                double.TryParse(TamanyoMinTextBox.Text, out double tamanyoMin);
                double.TryParse(TamanyoMaxTextBox.Text, out double tamanyoMax);

                // Asegurar que Min no sea mayor que Max
                if (aforoMin > 0 && aforoMax > 0 && aforoMin > aforoMax)
                {
                    MessageBox.Show("El aforo mínimo no puede ser mayor que el máximo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (precioMin > 0 && precioMax > 0 && precioMin > precioMax)
                {
                    MessageBox.Show("El precio mínimo no puede ser mayor que el máximo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (tamanyoMin > 0 && tamanyoMax > 0 && tamanyoMin > tamanyoMax)
                {
                    MessageBox.Show("El tamaño mínimo no puede ser mayor que el máximo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Se crea un objeto con los filtros para enviarlo en la solicitud HTTP
                var filtros = new
                {
                    idHabitacion = idHabitacion > 0 ? idHabitacion : (int?)null,
                    tipoHabitacion = !string.IsNullOrWhiteSpace(tipoHabitacion) ? tipoHabitacion : null,
                    numPersonasMin = aforoMin > 0 ? aforoMin : (int?)null,
                    numPersonasMax = aforoMax > 0 ? aforoMax : (int?)null,
                    estado = !string.IsNullOrWhiteSpace(estado) ? estado : null,
                    precioMin = precioMin > 0 ? precioMin : (double?)null,
                    precioMax = precioMax > 0 ? precioMax : (double?)null,
                    tamanyoMin = tamanyoMin > 0 ? tamanyoMin : (double?)null,
                    tamanyoMax = tamanyoMax > 0 ? tamanyoMax : (double?)null
                };

                // Serializa el objeto a JSON y lo envía en la solicitud HTTP
                var json = JsonSerializer.Serialize(filtros);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/habitaciones/filter", content);
                var responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Deserializa la respuesta JSON en una lista de habitaciones
                    var habitacionesFiltradas = JsonSerializer.Deserialize<Habitacion[]>(responseMessage, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Limpia la lista actual y agrega las habitaciones filtradas
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

        // Restablece los valores de los filtros en la UI
        private void ResetCampos_Click(object sender, RoutedEventArgs e)
        {
            IdHabitacionTextBox.Text = "";
            AforoMinTextBox.Text = "";
            AforoMaxTextBox.Text = "";
            PrecioMinTextBox.Text = "";
            PrecioMaxTextBox.Text = "";
            TamanyoMinTextBox.Text = "";
            TamanyoMaxTextBox.Text = "";

            EstadoComboBox.SelectedIndex = -1;
            ComboBoxTipo.SelectedItem = null;
        }

        // Verifica si hay notificaciones no vistas y actualiza el ícono de la campana
        private async Task VerificarNotificaciones()
        {
            try
            {
                var response = await _httpClient.GetAsync("/notificaciones/getNoVistas");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var notificacionesNoVistas = JsonSerializer.Deserialize<Notificacion[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    RutaCampana = notificacionesNoVistas.Length > 0
                        ? "http://localhost:3000/images/campanaNotificada.png"
                        : "http://localhost:3000/images/campana.png";
                }
                else
                {
                    Console.WriteLine($"Error al obtener notificaciones no vistas: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
            }
        }

        // Muestra las notificaciones y las marca como vistas en la API
        private async void MostrarNotificaciones(object sender, MouseButtonEventArgs e)
        {
            await CargarNotificaciones();
            NotificacionesPopup.IsOpen = true;

            await _httpClient.PostAsync("/notificaciones/marcarVistas", null);

            RutaCampana = "http://localhost:3000/images/campana.png";
        }

        // Alterna la visibilidad del popup de notificaciones
        private async void AlternarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            if (NotificacionesPopup.IsOpen)
            {
                // Si el popup está abierto, lo cierra y marca las notificaciones como vistas
                await _httpClient.PostAsync("/notificaciones/marcarVistas", null);
                Notificaciones.Clear();

                NotificacionesPopup.IsOpen = false;
                FondoOscuro.Visibility = Visibility.Collapsed;

                await VerificarNotificaciones();
            }
            else
            {
                // Si está cerrado, carga las notificaciones y lo muestra
                await CargarNotificaciones();

                NotificacionesPopup.IsOpen = true;
                FondoOscuro.Visibility = Visibility.Visible;
            }
        }

        // Cierra el popup de notificaciones
        private void CerrarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            NotificacionesPopup.IsOpen = false;
            FondoOscuro.Visibility = Visibility.Collapsed;
        }

        // Carga las notificaciones no vistas desde la API
        private async Task CargarNotificaciones()
        {
            try
            {
                var response = await _httpClient.GetAsync("/notificaciones/getNoVistas");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var notificacionesNoVistas = JsonSerializer.Deserialize<Notificacion[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Notificaciones.Clear();
                    foreach (var notificacion in notificacionesNoVistas)
                    {
                        Notificaciones.Add(notificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
            }
        }

        // Evento de clic para cerrar sesión
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            usuarioLogeado = null;

            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window != loginWindow) window.Close();
            }
        }

    }
}