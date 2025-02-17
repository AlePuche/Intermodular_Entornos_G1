using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PuchePerezAlejandroSimulacion1.Model;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para AddTipoHabitacion.xaml
    /// </summary>
    public partial class AddTipoHabitacion : Window
    {
        // Tipo de habitación seleccionado en modo edición
        private TipoHabitacion tipoSeleccionado;

        // Colección de tipos de habitación para mostrar en el ComboBox
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; } = new ObservableCollection<TipoHabitacion>();

        // Cliente HTTP para realizar peticiones a la API
        private readonly HttpClient _httpClient;
        // Usuario logueado en la aplicación
        public Usuario usuarioLogeado { get; set; }
        // Indica si la ventana está en modo edición o en modo agregar
        private bool esModoEdicion;

        // Constructor principal de la ventana
        public AddTipoHabitacion(bool esEditar, Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3000") };
            esModoEdicion = esEditar;
            this.usuarioLogeado = usuarioLogeado;
            
            // Define el contexto de datos de la ventana
            DataContext = this;

            // Configura la ventana según el modo (edición o creación)
            ConfigurarModo();

            this.usuarioLogeado = usuarioLogeado;
        }

        // Configura la interfaz dependiendo si está en modo edición o creación
        private async void ConfigurarModo()
        {
            if (esModoEdicion)
            {
                txtAddEdit.Text = "Editar Tipo de Habitación";
                txtNombre.Visibility = Visibility.Collapsed;
                ComboBoxTipo.Visibility = Visibility.Visible;
                btnEliminar.Visibility = Visibility.Visible;
                await CargarTiposHabitacion(); // Carga los tipos de habitación desde la API
            }
            else
            {
                txtNombre.Visibility = Visibility.Visible;
                ComboBoxTipo.Visibility = Visibility.Collapsed;
            }
        }

        // Obtiene la lista de tipos de habitación desde la API y la carga en la UI
        private async Task CargarTiposHabitacion()
        {
            try
            {
                var response = await _httpClient.GetAsync("/tipos-habitacion/getAll");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var tipos = JsonSerializer.Deserialize<TipoHabitacion[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    TiposHabitacion.Clear();
                    foreach (var tipo in tipos)
                    {
                        TiposHabitacion.Add(tipo);
                    }

                    ComboBoxTipo.ItemsSource = TiposHabitacion; // Asigna los tipos al ComboBox
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

        // Evento al seleccionar un tipo de habitación en el ComboBox
        private void ComboBoxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTipo.SelectedItem is TipoHabitacion tipo)
            {
                tipoSeleccionado = tipo;
                txtPrecioBase.Text = tipo.Precio.ToString();
                txtCapacidadMaxima.Text = tipo.CapacidadMaxima.ToString();
                txtServicios.Text = string.Join(", ", tipo.Servicios);
            }
        }

        // Evento para guardar o actualizar un tipo de habitación
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el nombre del tipo de habitación
            string nombreTipo = esModoEdicion ? ((TipoHabitacion)ComboBoxTipo.SelectedItem)?.Tipo : txtNombre.Text;

            if (string.IsNullOrWhiteSpace(nombreTipo))
            {
                MessageBox.Show("Debe ingresar o seleccionar un nombre de tipo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtPrecioBase.Text, out double precioBase) || precioBase < 0)
            {
                MessageBox.Show("Ingrese un precio válido.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtCapacidadMaxima.Text, out int capacidadMax) || capacidadMax <= 0)
            {
                MessageBox.Show("Ingrese una capacidad máxima válida.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tipoHabitacionData = new
            {
                nombre = nombreTipo,
                precioBase,
                capacidadMaxima = capacidadMax,
                servicios = txtServicios.Text.Split(',').Select(s => s.Trim()).ToList()
            };

            string endpoint = esModoEdicion ? "/tipos-habitacion/update" : "/tipos-habitacion/create";
            HttpMethod method = esModoEdicion ? HttpMethod.Patch : HttpMethod.Post;

            try
            {
                var json = JsonSerializer.Serialize(tipoHabitacionData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (method == HttpMethod.Post)
                {
                    response = await _httpClient.PostAsync(endpoint, content);
                }
                else
                {
                    response = await _httpClient.PatchAsync(endpoint, content);
                }

                string responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tipo de habitación guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListaHabitaciones lh = new ListaHabitaciones(usuarioLogeado);
                    lh.Show();
                    Close();
                }
                else
                {
                    try
                    {
                        var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseMessage);
                        string errorMessage = errorData.ContainsKey("message") ? errorData["message"].ToString() : "Ocurrió un error.";

                        if (errorData.ContainsKey("habitaciones") && errorData["habitaciones"] is JsonElement habitacionesElement)
                        {
                            var habitaciones = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(habitacionesElement.ToString());
                            errorMessage += "\n\nHabitaciones afectadas:\n";
                            foreach (var hab in habitaciones)
                            {
                                errorMessage += $"- ID: {hab["idHabitacion"]}, Personas: {hab["numPersonas"]}\n";
                            }
                        }

                        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch
                    {
                        MessageBox.Show(responseMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento para eliminar un tipo de habitación
        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxTipo.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un tipo de habitación para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tipoSeleccionado = (TipoHabitacion)ComboBoxTipo.SelectedItem;

            var confirmResult = MessageBox.Show(
                $"¿Seguro que deseas eliminar el tipo de habitación \"{tipoSeleccionado.Tipo}\"?\n\nEsta acción es irreversible.",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmResult == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(new { nombre = tipoSeleccionado.Tipo });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/tipos-habitacion/delete", content);
                var responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tipo de habitación eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    ListaHabitaciones listaHabitaciones = new ListaHabitaciones(usuarioLogeado);
                    listaHabitaciones.Show();
                    Close();
                }
                else
                {
                    try
                    {
                        var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseMessage);
                        string errorMessage = errorData.ContainsKey("message") ? errorData["message"].ToString() : "Error desconocido.";

                        if (errorData.ContainsKey("habitaciones") && errorData["habitaciones"] is JsonElement habitacionesElement)
                        {
                            var habitaciones = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(habitacionesElement.ToString());
                            errorMessage += "\n\nHabitaciones afectadas:\n";
                            foreach (var hab in habitaciones)
                            {
                                errorMessage += $"- ID: {hab["idHabitacion"]}, Personas: {hab["numPersonas"]}\n";
                            }
                        }

                        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch
                    {
                        MessageBox.Show(responseMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones lh = new ListaHabitaciones(usuarioLogeado);
            lh.Show();
            Close();
        }

        private void TextBox_NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
    }
}

