using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para AddHabitacion.xaml
    /// </summary>
    public partial class AddHabitacion : Window
    {
        
        private Habitacion habitacion;
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; }
        public Usuario usuarioLogeado { get; set; }
        public readonly HttpClient _httpClient;

        public AddHabitacion(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();
            DataContext = this;
            _ = CargarTiposHabitacion();
        }

        public AddHabitacion(Habitacion habitacion, Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();
            this.habitacion = habitacion;

            DataContext = this;

            txtAddEdit.Text = "Editar habitación";
            btnAddEdit.Content = "Editar";

            RellenarCampos(habitacion);
            _ = CargarTiposHabitacionYSeleccionar(habitacion);
        }

        private async Task CargarTiposHabitacionYSeleccionar(Habitacion habitacion)
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

                    foreach (var item in TiposHabitacion)
                    {
                        if (item.Tipo == habitacion.TipoHabitacion)
                        {
                            txtTipo.SelectedItem = item;
                            break;
                        }
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


        private void RellenarCampos(Habitacion habitacion)
        {
            txtId.Text = habitacion.IdHabitacion.ToString();

            foreach (ComboBoxItem item in numHuespedes.Items)
            {
                if (item.Content.ToString() == habitacion.NumPersonas.ToString())
                {
                    numHuespedes.SelectedItem = item;
                    break;
                }
            }
            txtDescripcion.Document.Blocks.Clear();
            txtDescripcion.Document.Blocks.Add(new Paragraph(new Run(habitacion.Descripcion)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones v = new ListaHabitaciones(usuarioLogeado);
            v.Show();
            Close();
        }

        private void TextBox_NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); // 🔹 Solo permite números del 0-9
        }

        private async void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("El campo 'ID' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtTipo.SelectedItem == null)
            {
                MessageBox.Show("El campo 'Tipo' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (numHuespedes.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar el número de huéspedes.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(new TextRange(txtDescripcion.Document.ContentStart, txtDescripcion.Document.ContentEnd).Text.Trim()))
            {
                MessageBox.Show("El campo 'Descripción' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTamanyo.Text))
            {
                MessageBox.Show("El campo 'Tamaño (m2)' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 🔥 Recopilar datos para enviar a la API
            var habitacionData = new
            {
                idHabitacion = int.Parse(txtId.Text),
                tipoHabitacion = ((TipoHabitacion)txtTipo.SelectedItem).Tipo, // 🔹 Enviamos el nombre, NO el objeto
                numPersonas = int.Parse(((ComboBoxItem)numHuespedes.SelectedItem).Content.ToString()),
                estado = ((ComboBoxItem)EstadoBox.SelectedItem).Content.ToString(), // Puedes modificarlo en la UI si lo deseas
                tamanyo = int.Parse(txtTamanyo.Text), // Agrega un TextBox en la UI si quieres modificarlo
                descripcion = new TextRange(txtDescripcion.Document.ContentStart, txtDescripcion.Document.ContentEnd).Text.Trim(),
                //imagenes = new List<string>() 
            };

            string endpoint = habitacion == null ? "/habitaciones/create" : "/habitaciones/update";
            HttpMethod method = habitacion == null ? HttpMethod.Post : HttpMethod.Patch;

            try
            {
                var json = JsonSerializer.Serialize(habitacionData);
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
                    MessageBox.Show("Habitación actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListaHabitaciones v = new ListaHabitaciones(usuarioLogeado);
                    v.Show();
                    Close();
                }
                else
                {
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseMessage);
                        string errorMessage = errorResponse.ContainsKey("error") ? errorResponse["error"] : "Error desconocido.";

                        // Si es un error de ID duplicado, mostramos un mensaje más claro
                        if (errorMessage.Contains("E11000 duplicate key"))
                        {
                            errorMessage = "El ID de la habitación ya existe.";
                        }

                        MessageBox.Show($"Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch
                    {
                        MessageBox.Show("Error en la operación. No se pudo interpretar la respuesta del servidor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
