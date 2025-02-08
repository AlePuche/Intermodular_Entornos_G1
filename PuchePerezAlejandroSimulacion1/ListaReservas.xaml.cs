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
        public ObservableCollection<Notificacion> Notificaciones { get; set; } = new ObservableCollection<Notificacion>();
        public Usuario usuarioLogeado { get; set; }

        public readonly HttpClient _httpClient;

        private string _rutaCampana;
        public string RutaCampana
        {
            get => _rutaCampana;
            set
            {
                _rutaCampana = value;
                Dispatcher.Invoke(() =>
                {
                    CampanaIcono.Source = new BitmapImage(new Uri(_rutaCampana, UriKind.RelativeOrAbsolute));
                });
            }
        }
        public ListaReservas(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;

            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;

            DataContext = this;

            if (usuarioLogeado == null)
            {
                MessageBox.Show("Error: El usuario logeado es null. Se cerrará la aplicación.");
                this.Close();
                return;
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            Reservas = new ObservableCollection<Reserva>();
            CargarListaReservas();
            VerificarNotificaciones();
        }

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

        private async void MostrarNotificaciones(object sender, MouseButtonEventArgs e)
        {
            await CargarNotificaciones();
            NotificacionesPopup.IsOpen = true;

            await _httpClient.PostAsync("/notificaciones/marcarVistas", null);

            RutaCampana = "http://localhost:3000/images/campana.png";
        }

        private async void AlternarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            if (NotificacionesPopup.IsOpen)
            {
                await _httpClient.PostAsync("/notificaciones/marcarVistas", null);
                Notificaciones.Clear();

                NotificacionesPopup.IsOpen = false;
                FondoOscuro.Visibility = Visibility.Collapsed;

                await VerificarNotificaciones();
            }
            else
            {
                await CargarNotificaciones();

                NotificacionesPopup.IsOpen = true;
                FondoOscuro.Visibility = Visibility.Visible;
            }
        }

        private void CerrarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            NotificacionesPopup.IsOpen = false;
            FondoOscuro.Visibility = Visibility.Collapsed;
        }
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
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                CrearReserva ventanaCrearReserva = new CrearReserva(false, reservaSeleccionada, usuarioLogeado);
                ventanaCrearReserva.Show();
                Close();
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
                CrearReserva ventanaCrearReserva = new CrearReserva(true, reservaSeleccionada, usuarioLogeado);
                ventanaCrearReserva.txtUser.Text = usuarioLogeado.Name + "  -    " + usuarioLogeado.Email;
                ventanaCrearReserva.Show();
                Close();
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
                        var json = JsonSerializer.Serialize(new { id = reservaSeleccionada.Id });
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PostAsync("/reservas/delete", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Reserva eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                            Reservas.Remove(reservaSeleccionada);

                            ListaReservas lista = new ListaReservas(usuarioLogeado);
                            lista.Show();

                            Close();
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

        private async void BuscarReservas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtros = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(txtID.Text))
                    filtros.Add("id", int.Parse(txtID.Text));

                if (!string.IsNullOrWhiteSpace(txtHabitacion.Text))
                    filtros.Add("idHabitacion", int.Parse(txtHabitacion.Text));

                if (!string.IsNullOrWhiteSpace(txtCliente.Text))
                    filtros.Add("cliente.email", txtCliente.Text);

                if (comboNumHuespedes.SelectedItem != null && (comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString() != "")
                    filtros.Add("numPersonas", int.Parse((comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString()));

                if (StartDatePicker.SelectedDate.HasValue)
                    filtros.Add("fechaInicio", StartDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));

                if (EndDatePicker.SelectedDate.HasValue)
                    filtros.Add("fechaSalida", EndDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));

                if (comboTipoHabitacion.SelectedItem != null && (comboTipoHabitacion.SelectedItem as ComboBoxItem).Content.ToString() != "")
                    filtros.Add("tipoHabitacion", (comboTipoHabitacion.SelectedItem as ComboBoxItem).Content.ToString());

                var json = JsonSerializer.Serialize(filtros);
                Console.WriteLine($"Filtros enviados: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/reservas/filter", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var reservasFiltradas = JsonSerializer.Deserialize<List<Reserva>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Reservas.Clear();
                    foreach (var reserva in reservasFiltradas)
                    {
                        Reservas.Add(reserva);
                    }
                }
                else
                {
                    MessageBox.Show($"Error al filtrar reservas: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ResetCampos_Click(object sender, RoutedEventArgs e)
        {
            txtID.Clear();
            txtHabitacion.Clear();
            txtCliente.Clear();

            comboNumHuespedes.SelectedIndex = 0;
            comboTipoHabitacion.SelectedIndex = 0;

            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            Reservas.Clear();

            _ = CargarListaReservas();

            MessageBox.Show("Los campos han sido restablecidos.", "Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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