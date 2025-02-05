using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para AddHabitacion.xaml
    /// </summary>
    public partial class AddHabitacion : Window
    {
        
        private Habitacion habitacion;
        public ObservableCollection<TipoHabitacion> TiposHabitacion { get; set; }
        public readonly HttpClient _httpClient;

        public AddHabitacion()
        {   
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            TiposHabitacion = new ObservableCollection<TipoHabitacion>();
            DataContext = this;
            CargarTiposHabitacion();

        }

        public AddHabitacion(Habitacion habitacion)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            this.habitacion = habitacion;
            txtAddEdit.Text = "Editar habitación";
            btnAddEdit.Content = "Editar";
            CargarTiposHabitacion();
            RellenarCampos(habitacion);
        }

        private async void CargarTiposHabitacion()
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
            txtTipo.Text = habitacion.TipoHabitacion;
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
            Close();
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
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

            if (!double.TryParse(((TextBox)FindName("PrecioTextBox"))?.Text, out double precio) || precio <= 0)
            {
                MessageBox.Show("El campo 'Precio por noche' debe ser un número positivo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Habitación añadida correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
