using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using PuchePerezAlejandroSimulacion1.Model;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para AddHabitacion.xaml
    /// </summary>
    public partial class AddHabitacion : Window
    {
        // Propiedades para almacenar datos
        private Habitacion habitacion;
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; }
        public Usuario usuarioLogeado { get; set; }

        // Cliente HTTP para realizar solicitudes a la API
        public readonly HttpClient _httpClient;

        // Constructor para agregar una nueva habitación
        public AddHabitacion(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;

            // Configuración del cliente HTTP
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();

            // Establece el contexto de datos para el binding en XAML
            DataContext = this;
            _ = CargarTiposHabitacion();

            // Configurar la visibilidad del campo de ID
            txtIdTextBox.Visibility = Visibility.Visible;
            txtIdTextBlock.Visibility = Visibility.Collapsed;
        }

        // Constructor para editar una habitación existente
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

            // Oculta el campo de ID y muestra el texto con el ID existente
            txtIdTextBox.Visibility = Visibility.Collapsed;
            txtIdTextBlock.Visibility = Visibility.Visible;
        }

        // Carga los tipos de habitación desde la API y selecciona el tipo de la habitación actual (modo edición)
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

                    // Busca el tipo de habitación correspondiente y lo selecciona
                    foreach (var item in TiposHabitacion)
                    {
                        if (item.Tipo == habitacion.TipoHabitacion)
                        {
                            ComboBoxTipo.SelectedItem = item;
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

        // Carga los tipos de habitación desde la API
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

        // Rellena los campos con los datos de la habitación en modo edición
        private void RellenarCampos(Habitacion habitacion)
        {
            txtIdTextBlock.Text = habitacion.IdHabitacion.ToString();
            numHuespedes.Text = habitacion.NumPersonas.ToString();
            txtTamanyo.Text = habitacion.Tamanyo.ToString();

            txtDescripcion.Document.Blocks.Clear();
            txtDescripcion.Document.Blocks.Add(new Paragraph(new Run(habitacion.Descripcion)));
            foreach (ComboBoxItem item in EstadoBox.Items)
            {
                if (item.Content.ToString() == habitacion.Estado)
                {
                    EstadoBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones v = new ListaHabitaciones(usuarioLogeado);
            v.Show();
            Close();
        }

        // Restringe los campos de texto para permitir solo números
        private void TextBox_NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        // Maneja la lógica de agregar o editar una habitación
        private async void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones de campos
            string idTexto = habitacion == null ? txtIdTextBox.Text : txtIdTextBlock.Text;
            if (string.IsNullOrWhiteSpace(idTexto))
            {
                MessageBox.Show("El campo 'ID' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ComboBoxTipo.SelectedItem == null)
            {
                MessageBox.Show("El campo 'Tipo' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(numHuespedes.Text))
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

            // Crear objeto con los datos de la habitación
            var habitacionData = new
            {
                idHabitacion = int.Parse(idTexto),
                tipoHabitacion = ((TipoHabitacion)ComboBoxTipo.SelectedItem).Tipo, 
                numPersonas = int.Parse(numHuespedes.Text),
                estado = ((ComboBoxItem)EstadoBox.SelectedItem).Content.ToString(), 
                tamanyo = int.Parse(txtTamanyo.Text), 
                descripcion = new TextRange(txtDescripcion.Document.ContentStart, txtDescripcion.Document.ContentEnd).Text.Trim(),
            };

            // Llamar a la API para crear o editar la habitación
            string endpoint = habitacion == null ? "/habitaciones/create" : "/habitaciones/update";
            HttpMethod method = habitacion == null ? HttpMethod.Post : HttpMethod.Patch;

            try
            {
                // Serializa los datos de la habitación en formato JSON
                var json = JsonSerializer.Serialize(habitacionData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;

                // Dependiendo si es una nueva habitación o una edición, usa POST o PATCH

                if (method == HttpMethod.Post)
                {
                    response = await _httpClient.PostAsync(endpoint, content);
                }
                else
                {
                    response = await _httpClient.PatchAsync(endpoint, content);
                }

                // Obtiene la respuesta de la API
                string responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Si la operación fue exitosa, muestra un mensaje y regresa a la lista de habitaciones
                    MessageBox.Show("Habitación actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListaHabitaciones v = new ListaHabitaciones(usuarioLogeado);
                    v.Show();
                    Close();
                }
                else
                {
                    try
                    {
                        // Intenta interpretar la respuesta como un error en formato JSON
                        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseMessage);
                        string errorMessage = errorResponse.ContainsKey("error") ? errorResponse["error"] : "Error desconocido.";

                        // Muestra mensajes específicos para ciertos errores conocidos
                        if (errorMessage.Contains("excede la capacidad"))
                        {
                            MessageBox.Show(errorMessage, "Error de Aforo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (errorMessage.Contains("E11000 duplicate key"))
                        {
                            MessageBox.Show("El ID de la habitación ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Error: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        // Si no se puede interpretar la respuesta, muestra un error genérico
                        MessageBox.Show("Error en la operación. No se pudo interpretar la respuesta del servidor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en la conexión a la API
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        // Permite seleccionar y subir una imagen principal para la habitación
        private async void MainImagen_Click(object sender, RoutedEventArgs e)
        {
            // Verifica si la habitación existe antes de permitir subir imágenes
            if (habitacion == null || habitacion.IdHabitacion <= 0)
            {
                MessageBox.Show("No se puede subir imágenes hasta que la habitación haya sido guardada.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cuadro de diálogo para seleccionar un archivo de imagen
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Seleccionar Imagen"
            };

            // Envia la imagen al servidor
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                using var client = new HttpClient();
                using var content = new MultipartFormDataContent();

                var fileStream = File.OpenRead(filePath);
                var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "imagen", Path.GetFileName(filePath));

                var response = await client.PostAsync($"http://localhost:3000/habitaciones/images/uploadMain/{habitacion.IdHabitacion}", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
                    MessageBox.Show($"Imagen subida correctamente.\nURL: {result["imageUrl"]}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Error al subir imagen: {jsonResponse}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Permite añadir imágenes adicionales a la habitación
        private async void AddImagen_Click(object sender, RoutedEventArgs e)
        {
            if (habitacion == null || habitacion.IdHabitacion <= 0)
            {
                MessageBox.Show("No se puede subir imágenes hasta que la habitación haya sido guardada.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Seleccionar Imagen"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                using var client = new HttpClient();
                using var content = new MultipartFormDataContent();

                var fileStream = File.OpenRead(filePath);
                var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "imagen", Path.GetFileName(filePath));

                var response = await client.PostAsync($"http://localhost:3000/habitaciones/images/upload/{habitacion.IdHabitacion}", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
                    MessageBox.Show($"Imagen subida correctamente.\nURL: {result["imageUrl"]}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Error al subir imagen: {jsonResponse}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Permite borrar todas las imágenes asociadas a una habitación
        private async void BorrarImages_Click(object sender, RoutedEventArgs e)
        {
            // Verifica si la habitación existe antes de intentar eliminar imágenes
            if (habitacion == null || habitacion.IdHabitacion <= 0)
            {
                MessageBox.Show("No se puede borrar imágenes sin una habitación válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Pregunta al usuario si realmente desea eliminar todas las imágenes
            var result = MessageBox.Show("¿Seguro que deseas eliminar todas las imágenes de esta habitación?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                using var client = new HttpClient();

                // Solicitud DELETE para eliminar todas las imágenes asociadas a la habitación
                var response = await client.DeleteAsync($"http://localhost:3000/habitaciones/images/deleteAll/{habitacion.IdHabitacion}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Todas las imágenes han sido eliminadas.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Error al eliminar imágenes: {jsonResponse}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
