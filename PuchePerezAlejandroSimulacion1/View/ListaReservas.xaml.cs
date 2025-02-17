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
using Org.BouncyCastle.Pqc.Crypto.Lms;
using PuchePerezAlejandroSimulacion1.Model;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para ListaReservas.xaml
    /// </summary>
    public partial class ListaReservas : Window
    {
        // Colección observable que almacena las reservas disponibles
        public ObservableCollection<Reserva> Reservas { get; set; }

        // Lista de notificaciones obtenidas del servidor
        public ObservableCollection<Notificacion> Notificaciones { get; set; } = new ObservableCollection<Notificacion>();

        // Lista de tipos de habitaciones disponibles en el hotel
        public ObservableCollection<string> TiposHabitacionesDisponibles { get; set; } = new ObservableCollection<string>();

        // Usuario actualmente logeado en la aplicación
        public Usuario usuarioLogeado { get; set; }

        // Cliente HTTP para realizar solicitudes al servidor
        public readonly HttpClient _httpClient;

        // Ruta del icono de la campana de notificaciones
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
        // Constructor de la ventana de lista de reservas.
        public ListaReservas(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized;

            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;

            DataContext = this;

            // Verifica si el usuario está logeado
            if (usuarioLogeado == null)
            {
                MessageBox.Show("Error: El usuario logeado es null. Se cerrará la aplicación.");
                this.Close();
                return;
            }

            // Inicializa el cliente HTTP con la dirección base del servidor
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            Reservas = new ObservableCollection<Reserva>();

            // Cargar datos iniciales
            CargarListaReservas();
            VerificarNotificaciones();
            _ = CargarTiposDeHabitacion();
        }

        // Obtiene la lista de tipos de habitaciones disponibles desde el servidor.
        private async Task CargarTiposDeHabitacion()
        {
            try
            {
                var response = await _httpClient.GetAsync("/habitaciones/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var habitaciones = JsonSerializer.Deserialize<List<Habitacion>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var tiposUnicos = habitaciones.Select(h => h.TipoHabitacion).Distinct().ToList();

                    TiposHabitacionesDisponibles.Clear();
                    TiposHabitacionesDisponibles.Add("");

                    foreach (var tipo in tiposUnicos)
                    {
                        TiposHabitacionesDisponibles.Add(tipo);
                    }
                }
                else
                {
                    MessageBox.Show("Error al obtener los tipos de habitaciones.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Obtiene las notificaciones no vistas y actualiza el icono de la campana.
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

        private async void AlternarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            // Verifica si el popup de notificaciones ya está abierto
            if (NotificacionesPopup.IsOpen)
            {
                // Marca todas las notificaciones como vistas enviando una petición a la API
                await _httpClient.PostAsync("/notificaciones/marcarVistas", null);

                // Limpia la lista de notificaciones en la interfaz
                Notificaciones.Clear();

                // Cierra el popup de notificaciones y oculta el fondo oscuro
                NotificacionesPopup.IsOpen = false;
                FondoOscuro.Visibility = Visibility.Collapsed;

                // Vuelve a verificar si hay nuevas notificaciones no vistas
                await VerificarNotificaciones();
            }
            else
            {
                // Carga las notificaciones no vistas desde la API
                await CargarNotificaciones();

                // Abre el popup de notificaciones y muestra el fondo oscuro
                NotificacionesPopup.IsOpen = true;
                FondoOscuro.Visibility = Visibility.Visible;
            }
        }

        private void CerrarPopupNotificaciones(object sender, MouseButtonEventArgs e)
        {
            // Cierra el popup de notificaciones y oculta el fondo oscuro
            NotificacionesPopup.IsOpen = false;
            FondoOscuro.Visibility = Visibility.Collapsed;
        }

        private async Task CargarNotificaciones()
        {
            try
            {
                // Realiza una petición a la API para obtener las notificaciones no vistas
                var response = await _httpClient.GetAsync("/notificaciones/getNoVistas");

                // Verifica si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Deserializa la respuesta en una lista de notificaciones
                    var json = await response.Content.ReadAsStringAsync();
                    var notificacionesNoVistas = JsonSerializer.Deserialize<Notificacion[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Limpia la lista de notificaciones actual en la interfaz
                    Notificaciones.Clear();

                    // Agrega las nuevas notificaciones a la lista para mostrarlas en la interfaz
                    foreach (var notificacion in notificacionesNoVistas)
                    {
                        Notificaciones.Add(notificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja errores de conexión mostrando un mensaje en la consola
                Console.WriteLine($"Error al conectar con la API: {ex.Message}");
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el botón que ha sido presionado
            var button = sender as Button;

            // Verifica si el botón tiene una reserva asociada
            if (button?.DataContext is Reserva reservaSeleccionada)
            {
                // Crea una nueva ventana de reserva en modo solo lectura (no editable)
                CrearReserva ventanaCrearReserva = new CrearReserva(false, reservaSeleccionada, usuarioLogeado);

                // Muestra la ventana de la reserva seleccionada
                ventanaCrearReserva.Show();

                // Cierra la ventana actual
                Close();
            }
            else
            {
                // Muestra un mensaje de error si no se pudo obtener la reserva
                MessageBox.Show("No se pudo obtener la reserva seleccionada.");
            }
        }

        // Permite abrir la ventana de edición de una reserva existente.
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

        // Carga la lista de reservas desde la API.
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

        // Maneja el evento de clic en un botón para abrir la ventana principal (SegundaVentana).
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Crea una nueva instancia de la ventana principal y le pasa el usuario logeado
            SegundaVentana sg = new SegundaVentana(usuarioLogeado);

            // Muestra la nueva ventana
            sg.Show();

            // Cierra la ventana actual
            Close();
        }

        // Maneja el evento de clic en un botón para abrir la ventana de búsqueda (Buscador).
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Crea una nueva instancia de la ventana de búsqueda y le pasa el usuario logeado
            Buscador buscador = new Buscador(usuarioLogeado);

            // Muestra la nueva ventana
            buscador.Show();

            // Cierra la ventana actual
            Close();
        }

        // Maneja el evento de clic en un botón para abrir la ventana de lista de reservas.
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Crea una nueva instancia de la ventana de lista de reservas y le pasa el usuario logeado
            ListaReservas listaReservas = new ListaReservas(usuarioLogeado);

            // Muestra la nueva ventana
            listaReservas.Show();

            // Cierra la ventana actual
            Close();
        }

        // Maneja el evento de clic en un botón para abrir la ventana de lista de habitaciones.
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Crea una nueva instancia de la ventana de lista de habitaciones y le pasa el usuario logeado
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones(usuarioLogeado);

            // Muestra la nueva ventana
            listaHabitaciones.Show();

            // Cierra la ventana actual
            Close();
        }


        // Permite eliminar una reserva después de confirmar la acción con el usuario.
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
                // Crea un diccionario para almacenar los filtros que se enviarán a la API.
                var filtros = new Dictionary<string, object>();

                // Si el campo de ID no está vacío, se añade al diccionario como un entero.
                if (!string.IsNullOrWhiteSpace(txtID.Text))
                    filtros.Add("id", int.Parse(txtID.Text));

                // Si el campo de ID de habitación no está vacío, se añade al diccionario como un entero.
                if (!string.IsNullOrWhiteSpace(txtHabitacion.Text))
                    filtros.Add("idHabitacion", int.Parse(txtHabitacion.Text));

                // Si el campo de cliente no está vacío, se añade al diccionario como una cadena de texto.
                if (!string.IsNullOrWhiteSpace(txtCliente.Text))
                    filtros.Add("cliente.email", txtCliente.Text);

                // Si se ha seleccionado un número de huéspedes, se añade al diccionario como un entero.
                if (comboNumHuespedes.SelectedItem != null && (comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString() != "")
                    filtros.Add("numPersonas", int.Parse((comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString()));

                // Si hay una fecha de inicio seleccionada, se añade al diccionario en formato "YYYY-MM-DD".
                if (StartDatePicker.SelectedDate.HasValue)
                    filtros.Add("fechaInicio", StartDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));

                // Si hay una fecha de salida seleccionada, se añade al diccionario en formato "YYYY-MM-DD".
                if (EndDatePicker.SelectedDate.HasValue)
                    filtros.Add("fechaSalida", EndDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));

                // Si se ha seleccionado un tipo de habitación, se añade al diccionario.
                if (comboTipoHabitacion.SelectedItem != null && comboTipoHabitacion.SelectedItem.ToString() != "")
                {
                    filtros.Add("tipoHabitacion", comboTipoHabitacion.SelectedItem.ToString());
                }

                // Serializa los filtros en formato JSON.
                var json = JsonSerializer.Serialize(filtros);
                Console.WriteLine($"Filtros enviados: {json}");

                // Envía la solicitud POST con los filtros a la API.
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/reservas/filter", content);

                // Si la solicitud es exitosa, procesa la respuesta.
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializa la respuesta en una lista de reservas.
                    var reservasFiltradas = JsonSerializer.Deserialize<List<Reserva>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Limpia la lista actual de reservas y agrega las nuevas reservas filtradas.
                    Reservas.Clear();
                    foreach (var reserva in reservasFiltradas)
                    {
                        Reservas.Add(reserva);
                    }
                }
                else
                {
                    // Muestra un mensaje de error si la solicitud no fue exitosa.
                    MessageBox.Show($"Error al filtrar reservas: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Captura cualquier error de conexión con la API y lo muestra en un mensaje de error.
                MessageBox.Show($"Error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetCampos_Click(object sender, RoutedEventArgs e)
        {
            // Borra el contenido de los campos de texto.
            txtID.Clear();
            txtHabitacion.Clear();
            txtCliente.Clear();

            // Restablece los valores seleccionados en los ComboBox.
            comboNumHuespedes.SelectedIndex = 0;
            comboTipoHabitacion.SelectedIndex = 0;

            // Reinicia las fechas seleccionadas en los DatePicker.
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            // Limpia la lista de reservas actuales.
            Reservas.Clear();

            // Recarga la lista de reservas desde la API.
            _ = CargarListaReservas();

            // Muestra un mensaje indicando que los campos han sido restablecidos.
            MessageBox.Show("Los campos han sido restablecidos.", "Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Cierra la sesión y regresa a la pantalla de inicio de sesión.
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