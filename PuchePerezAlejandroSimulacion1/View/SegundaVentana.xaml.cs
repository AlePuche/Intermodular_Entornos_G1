using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PuchePerezAlejandroSimulacion1.Model;

namespace PuchePerezAlejandroSimulacion1
{
    public partial class SegundaVentana : Window
    {
        // Cliente HTTP para realizar peticiones a la API
        private readonly HttpClient _httpClient;
        // Lista observable para almacenar los tipos de habitaciones disponibles
        public ObservableCollection<TipoHabitacion> TiposHabitacionesDisponibles { get; set; } = new ObservableCollection<TipoHabitacion>();
        // Lista observable para notificaciones
        public ObservableCollection<Notificacion> Notificaciones { get; set; } = new ObservableCollection<Notificacion>();
        // Usuario actualmente logueado en la aplicación
        public Usuario usuarioLogeado { get; set; }

        // Ruta de la imagen de la campana de notificaciones
        private string _rutaCampana;
        public string RutaCampana
        {
            get => _rutaCampana;
            set
            {
                _rutaCampana = value;
                Dispatcher.Invoke(() =>
                {
                    // Actualiza la imagen de la campana en la UI
                    CampanaIcono.Source = new BitmapImage(new Uri(_rutaCampana, UriKind.RelativeOrAbsolute));
                });
            }
        }
        // Constructor de la ventana
        public SegundaVentana(Usuario usuarioLogeado)
        {
            this.WindowState = WindowState.Maximized; // Maximiza la ventana al abrirse

            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3000") };
            DataContext = this; // Establece el contexto de datos

            VerificarNotificaciones(); // Llama a la función para verificar notificaciones
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

                    // Cambia la imagen de la campana si hay notificaciones no vistas
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
        // Muestra el popup de notificaciones y marca las notificaciones como vistas
        private async void MostrarNotificaciones(object sender, MouseButtonEventArgs e)
        {
            await CargarNotificaciones();
            NotificacionesPopup.IsOpen = true;

            await _httpClient.PostAsync("/notificaciones/marcarVistas", null);

            RutaCampana = "http://localhost:3000/images/campana.png"; // Cambia la imagen de la campana a la normal
        }
        // Alterna la visibilidad del popup de notificaciones
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
        // Carga las notificaciones desde la API
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
        // Método que abre la ventana "SegundaVentana" y cierra la actual
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sv = new SegundaVentana(usuarioLogeado); // Crea una nueva instancia de SegundaVentana
            sv.Show(); // Muestra la nueva ventana
            Close(); // Cierra la ventana actual
        }

        // Método que abre la ventana "Buscador" y cierra la actual
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Buscador buscador = new Buscador(usuarioLogeado); // Crea una nueva instancia de Buscador
            buscador.Show(); // Muestra la nueva ventana
            Close(); // Cierra la ventana actual
        }

        // Método que abre la ventana "ListaReservas" y cierra la actual
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas(usuarioLogeado); // Crea una nueva instancia de ListaReservas
            listaReservas.Show(); // Muestra la nueva ventana
            Close(); // Cierra la ventana actual
        }

        // Método que abre la ventana "ListaHabitaciones" y cierra la actual
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones(usuarioLogeado); // Crea una nueva instancia de ListaHabitaciones
            listaHabitaciones.Show(); // Muestra la nueva ventana
            Close(); // Cierra la ventana actual
        }

        //Levanta la ventana crearReserva con sus características
        private void btnReservar_Click(object sender, RoutedEventArgs e)
        {
            // Convierte el sender en un botón
            var button = sender as Button;
            int extrasInt = 0; // Contador de extras seleccionados

            // Verifica si el botón tiene una habitación asociada en su DataContext
            if (button?.DataContext is TipoHabitacion habitacionSeleccionada)
            {
                // Crea una nueva ventana para la reserva y pasa el usuario logeado
                CrearReserva crearReserva = new CrearReserva(usuarioLogeado);

                // Asigna los valores seleccionados en la interfaz a la nueva ventana
                crearReserva.txtFechaEntrada.Text = fechaEntrada.Text;
                crearReserva.txtFechaSalida.Text = fechaSalida.Text;
                crearReserva.txtHuespedes.Text = comboNumHuespedes.Text;
                crearReserva.txtTipo.Text = habitacionSeleccionada.Tipo;
                crearReserva.txtUser.Text = usuarioLogeado.Name + "  -    " + usuarioLogeado.Email;
                crearReserva.id = habitacionSeleccionada.IdHabitacion.ToString();

                // Verifica si los extras han sido seleccionados y los suma
                if (extraCama.IsChecked == true)
                {
                    crearReserva.extras += 1;
                    extrasInt++;
                }
                if (extraCuna.IsChecked == true)
                {
                    crearReserva.extras += 1;
                    extrasInt++;
                }
                if (extraDesayuno.IsChecked == true)
                {
                    crearReserva.extras += 1;
                    extrasInt++;
                }

                // Calcula el precio total de la reserva
                crearReserva.txtPrecio.Text = (nNoches() * habitacionSeleccionada.Precio) + (20 * extrasInt) + " €";

                // Guarda las fechas y el precio total en la nueva ventana de reserva
                crearReserva.entrada = fechaEntrada.SelectedDate.Value;
                crearReserva.salida = fechaSalida.SelectedDate.Value;
                crearReserva.price = ((int)(nNoches() * habitacionSeleccionada.Precio) + (20 * extrasInt));

                // Muestra la ventana de reserva
                crearReserva.Show();

                // Cierra la ventana actual
                Close();
            }
            else
            {
                // Muestra un mensaje si no se pudo obtener la habitación seleccionada
                MessageBox.Show("No se pudo obtener la habitación seleccionada.");
            }
        }

        //Calcula el numero de noches
        private int nNoches()
        {
            DateTime entrada = fechaEntrada.SelectedDate.Value;
            DateTime salida = fechaSalida.SelectedDate.Value;

            return (salida - entrada).Days;
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (fechaEntrada.SelectedDate == null || fechaSalida.SelectedDate == null || comboNumHuespedes.SelectedItem == null || fechaEntrada.SelectedDate < DateTime.Now.Date || fechaSalida.SelectedDate <= fechaEntrada.SelectedDate)
            {
                listaScroll.Visibility = Visibility.Hidden;
                MessageBox.Show("Por favor, selecciona fechas y cantidad de huéspedes válidos.");
                return;
            }

            await FiltrarHabitacionesDisponibles();
        }
        // Filtra las habitaciones disponibles según la fecha y el número de huéspedes
        // Método que filtra las habitaciones disponibles según los criterios seleccionados por el usuario
        private async Task FiltrarHabitacionesDisponibles()
        {
            try
            {
                // Obtiene el número de huéspedes seleccionados en el ComboBox
                int numHuespedes = int.Parse((comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString());

                // Verifica si la opción de "Cama Extra" está marcada
                bool extraCamaBool = extraCama.IsChecked == true;

                // Llamadas a la API para obtener la lista de habitaciones y reservas existentes
                var responseHabitaciones = await _httpClient.GetAsync("/habitaciones/getAll");
                var responseReservas = await _httpClient.GetAsync("/reservas/getAll");

                // Verifica si las respuestas de la API son exitosas
                if (!responseHabitaciones.IsSuccessStatusCode || !responseReservas.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al obtener datos del servidor.");
                    return;
                }

                // Convierte las respuestas JSON en listas de objetos Habitacion y Reserva
                var jsonHabitaciones = await responseHabitaciones.Content.ReadAsStringAsync();
                var jsonReservas = await responseReservas.Content.ReadAsStringAsync();

                var habitaciones = JsonSerializer.Deserialize<List<Habitacion>>(jsonHabitaciones,
                                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var reservas = JsonSerializer.Deserialize<List<Reserva>>(jsonReservas,
                                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtiene las fechas de entrada y salida seleccionadas por el usuario
                DateTime entrada = fechaEntrada.SelectedDate.Value;
                DateTime salida = fechaSalida.SelectedDate.Value;

                // Filtra las habitaciones disponibles según el estado, número de huéspedes y reservas activas
                var tiposDisponibles = habitaciones
                .Where(h => h.Estado != "Mantenimiento") // Excluye habitaciones en mantenimiento
                .Where(h => PuedeAlojarHuespedes(h, numHuespedes, extraCamaBool)) // Verifica la capacidad de la habitación
                .GroupBy(h => h.TipoHabitacion) // Agrupa por tipo de habitación
                .Select(g =>
                {
                    // Busca la primera habitación disponible dentro del grupo
                    var habitacionDisponible = g.FirstOrDefault(h => HabitacionDisponible(h, reservas, entrada, salida));

                    // Crea una instancia de TipoHabitacion con la información obtenida
                    return new TipoHabitacion
                    {
                        IdHabitacion = habitacionDisponible?.IdHabitacion ?? 0,
                        Tipo = g.Key, // Tipo de la habitación
                        Precio = habitacionDisponible?.Precio ?? 0, // Precio de la habitación
                        FotoUrl = habitacionDisponible != null ? "http://localhost:3000" + habitacionDisponible.Imagenes.FirstOrDefault() : "", // Imagen de la habitación
                        Disponible = habitacionDisponible != null // Indica si la habitación está disponible
                    };
                })
                .Where(t => t.Disponible) // Filtra solo las habitaciones que están disponibles
                .ToList();

                // Limpia la lista actual de habitaciones disponibles
                TiposHabitacionesDisponibles.Clear();

                // Si hay habitaciones disponibles, las añade a la lista
                if (tiposDisponibles.Any())
                {
                    foreach (var tipo in tiposDisponibles)
                    {
                        TiposHabitacionesDisponibles.Add(tipo);
                    }

                    // Muestra la lista de resultados
                    listaScroll.Visibility = Visibility.Visible;
                }
                else
                {
                    // Oculta la lista y muestra un mensaje de que no hay habitaciones disponibles
                    listaScroll.Visibility = Visibility.Hidden;
                    MessageBox.Show("No hay habitaciones disponibles para los criterios seleccionados.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores: muestra un mensaje con la excepción si ocurre algún problema
                MessageBox.Show($"Error al procesar la solicitud: {ex.Message}");
            }
        }

        // Comprueba si la habitación puede alojar a los huéspedes
        private bool PuedeAlojarHuespedes(Habitacion habitacion, int numHuespedes, bool extraCama)
        {
            return habitacion.NumPersonas >= numHuespedes || (extraCama && habitacion.NumPersonas + 1 >= numHuespedes);
        }
        // Comprueba si la habitación está disponible en las fechas seleccionadas
        private bool HabitacionDisponible(Habitacion habitacion, List<Reserva> reservas, DateTime entrada, DateTime salida)
        {
            entrada = entrada.Date;
            salida = salida.Date;

            var reservasHabitacion = reservas
                .Where(r => r.IdHabitacion == habitacion.IdHabitacion)
                .ToList();

            if (!reservasHabitacion.Any())
                return true;

            foreach (var reserva in reservasHabitacion)
            {
                DateTime inicioReserva = reserva.FechaInicio.Date;
                DateTime finReserva = reserva.FechaSalida.Date;

                if (!(salida <= inicioReserva || entrada >= finReserva))
                {
                    return false;
                }
            }

            return true;
        }
        // Cierra la sesión del usuario y regresa a la ventana de login
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
