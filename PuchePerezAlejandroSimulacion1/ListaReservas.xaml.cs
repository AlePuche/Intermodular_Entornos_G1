using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para ListaReservas.xaml
    /// </summary>
    public partial class ListaReservas : Window
    {
        public ObservableCollection<Reserva> Reservas { get; set; }
        public Usuario usuarioLogeado { get; private set; }

        public readonly HttpClient _httpClient;
        public ListaReservas(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
     
            DataContext = this;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000") 
            };

            Reservas = new ObservableCollection<Reserva>();
            CargarListaReservas();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                CrearReserva ventanaCrearReserva = new CrearReserva(false, reservaSeleccionada);
                ventanaCrearReserva.Show();
            }
            else
            {
                MessageBox.Show("No se pudo obtener la reserva seleccionada.");
            }
        }

        private void EditarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                CrearReserva ventanaCrearReserva = new CrearReserva(true, reservaSeleccionada);
                ventanaCrearReserva.Show();
            }
            else
            {
                MessageBox.Show("No se pudo obtener la reserva seleccionada.");
            }
        }
        
        private async Task CargarListaReservas()
        {
            try
            {
                var response = await _httpClient.GetAsync("/reservas/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var reservas = JsonSerializer.Deserialize<Reserva[]>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    foreach (var reserva in reservas)
                    {
                        Reservas.Add(reserva);
                    }
                }
                else
                {
                    Console.WriteLine($"Error al obtener reservas: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sg = new SegundaVentana(usuarioLogeado);
            sg.Show();

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Buscador buscador = new Buscador(usuarioLogeado);
            buscador.Show();

            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas(usuarioLogeado);
            listaReservas.Show();

            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones(usuarioLogeado);
            listaHabitaciones.Show();

            Close();
        }

        private async void eliminarReserva(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                var result = MessageBox.Show($"¿Estás seguro de que deseas eliminar la reserva con ID {reservaSeleccionada.Id}?",
                                             "Confirmar eliminación",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Crear el JSON con el ID
                        var json = JsonSerializer.Serialize(new { id = reservaSeleccionada.Id });
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        // Enviar solicitud POST al servidor
                        var response = await _httpClient.PostAsync("/reservas/delete", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Reserva eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Eliminar la reserva de la ObservableCollection
                            Reservas.Remove(reservaSeleccionada);
                        }
                        else
                        {
                            MessageBox.Show($"Error al eliminar la reserva: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudo obtener la reserva seleccionada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}