using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PuchePerezAlejandroSimulacion1
{
    public partial class SegundaVentana : Window
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<TipoHabitacion> TiposHabitacionesDisponibles { get; set; } = new ObservableCollection<TipoHabitacion>();

        public Usuario usuarioLogeado { get; private set; }

        public SegundaVentana(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3000") };
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sv = new SegundaVentana(usuarioLogeado);
            sv.Show();

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

        private void btnReservar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is TipoHabitacion habitacionSeleccionada)
            {
                CrearReserva crearReserva = new CrearReserva();

                crearReserva.txtFechaEntrada.Text = fechaEntrada.Text;
                crearReserva.txtFechaSalida.Text = fechaSalida.Text;
                crearReserva.txtHuespedes.Text = comboNumHuespedes.Text;
                crearReserva.txtPrecio.Text = (nNoches() * habitacionSeleccionada.Precio) + " €";
                crearReserva.txtTipo.Text = habitacionSeleccionada.Tipo;
                crearReserva.txtUser.Text = usuarioLogeado.Name + "  -    " + usuarioLogeado.Email;
                crearReserva.id = habitacionSeleccionada.IdHabitacion.ToString();
                MessageBox.Show(crearReserva.id);

                if (extraCama.IsChecked == true)
                    crearReserva.extras += 1;
                if (extraCuna.IsChecked == true)
                    crearReserva.extras += 1;
                if (extraDesayuno.IsChecked == true)
                    crearReserva.extras += 1;

                crearReserva.entrada = fechaEntrada.SelectedDate.Value;
                crearReserva.salida = fechaSalida.SelectedDate.Value;
                crearReserva.price = ((int)(nNoches() * habitacionSeleccionada.Precio));

                crearReserva.Show();
            }
            else
            {
                MessageBox.Show("No se pudo obtener la habitación seleccionada.");
            }
        }

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

        private async Task FiltrarHabitacionesDisponibles()
        {
            try
            {
                int numHuespedes = int.Parse((comboNumHuespedes.SelectedItem as ComboBoxItem).Content.ToString());
                bool extraCamaBool = extraCama.IsChecked == true;

                var responseHabitaciones = await _httpClient.GetAsync("/habitaciones/getAll");
                var responseReservas = await _httpClient.GetAsync("/reservas/getAll");

                if (!responseHabitaciones.IsSuccessStatusCode || !responseReservas.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al obtener datos del servidor.");
                    return;
                }

                var jsonHabitaciones = await responseHabitaciones.Content.ReadAsStringAsync();
                var jsonReservas = await responseReservas.Content.ReadAsStringAsync();

                var habitaciones = JsonSerializer.Deserialize<List<Habitacion>>(jsonHabitaciones, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var reservas = JsonSerializer.Deserialize<List<Reserva>>(jsonReservas, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                DateTime entrada = fechaEntrada.SelectedDate.Value;
                DateTime salida = fechaSalida.SelectedDate.Value;

                var tiposDisponibles = habitaciones
                .Where(h => PuedeAlojarHuespedes(h, numHuespedes, extraCamaBool))
                .GroupBy(h => h.TipoHabitacion)
                .Select(g =>
                {
                    var habitacionDisponible = g.FirstOrDefault(h => HabitacionDisponible(h, reservas, entrada, salida));

                    return new TipoHabitacion
                    {
                        IdHabitacion = habitacionDisponible?.IdHabitacion ?? 0,
                        Tipo = g.Key,
                        Precio = habitacionDisponible?.Precio ?? 0,
                        FotoUrl = habitacionDisponible != null ? "http://localhost:3000" + habitacionDisponible.Imagenes.FirstOrDefault() : "",
                        Disponible = habitacionDisponible != null
                    };
                })
                .Where(t => t.Disponible)
                .ToList();

                TiposHabitacionesDisponibles.Clear();

                if (tiposDisponibles.Any())
                {
                    foreach (var tipo in tiposDisponibles)
                    {
                        TiposHabitacionesDisponibles.Add(tipo);
                    }

                    listaScroll.Visibility = Visibility.Visible;
                }
                else
                {
                    listaScroll.Visibility = Visibility.Hidden;
                    MessageBox.Show("No hay habitaciones disponibles para los criterios seleccionados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private bool PuedeAlojarHuespedes(Habitacion habitacion, int numHuespedes, bool extraCama)
        {
            return habitacion.NumPersonas >= numHuespedes || (extraCama && habitacion.NumPersonas + 1 >= numHuespedes);
        }

        private bool HabitacionDisponible(Habitacion habitacion, List<Reserva> reservas, DateTime entrada, DateTime salida)
        {
            entrada = entrada.Date;  // 🔹 Eliminar la hora
            salida = salida.Date;    // 🔹 Eliminar la hora

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
    }
}
