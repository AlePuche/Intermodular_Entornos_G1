using Newtonsoft.Json;
using PuchePerezAlejandroSimulacion1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    /// Lógica de interacción para CrearReserva.xaml
    /// </summary>
    public partial class CrearReserva : Window
    {
        bool editable; // Indica si la reserva está en modo edición
        Reserva reservaEdit; // Reserva a editar
        public int extras = 0; // Contador de extras seleccionados
        public DateTime entrada; // Fecha de entrada
        public DateTime salida; // Fecha de salida
        public int price; // Precio total de la reserva
        public Usuario usuarioLogeado; // Usuario actualmente logeado
        public string id; // ID de la habitación

        private readonly HttpClient _httpClient;// Cliente HTTP para realizar peticiones a la API

        // Constructor para editar o visualizar una reserva existente.
        public CrearReserva(bool editable, Reserva reserva, Usuario usuario)
        {
            this.WindowState = WindowState.Maximized;

            InitializeComponent();
            this.editable = editable;
            this.reservaEdit = reserva;
            this.usuarioLogeado = usuario;

            if (editable)// Si está en modo edición
            {
                // Inicializa el cliente HTTP
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:3000")
                };
                reservaButton.Visibility = Visibility.Visible;
                reservaButton.Content = "Editar Reserva";
                lblTitulo.Text = "Editar Reserva";

                txtTipo.Text = reservaEdit.TipoHabitacion;

                txtFechaEntradaEdit.Text = reservaEdit.FechaInicioFormatted.ToString();

                txtFechaSalidaEdit.Text = reservaEdit.FechaFinFormatted.ToString();

                txtHuespedes.Visibility = Visibility.Collapsed;
                huespedesEdit.Visibility = Visibility.Visible;
                huespedesEdit.Text = reservaEdit.NumPersonas.ToString();

                txtPrecio.Visibility = Visibility.Collapsed;
                txtPrecioEdit.Visibility = Visibility.Visible;
                txtPrecioEdit.Text = reservaEdit.Precio.ToString();

                txtClienteLbl.Visibility = Visibility.Collapsed;
                txtCliente.Visibility = Visibility.Visible;
                txtCliente.Text = reservaEdit.Cliente.Nombre;

                txtEmailInfo.Visibility = Visibility.Collapsed;
                txtEmail.Visibility = Visibility.Visible;
                txtEmail.Text = reservaEdit.Cliente.Email;
            }
            else // Si solo se muestra la información de la reserva
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:3000")
                };
                reservaButton.Visibility = Visibility.Collapsed;
                lblTitulo.Text = "Info Reserva";

                txtTipo.Text = "Habitación " + reservaEdit.TipoHabitacion;

                txtFechaEntradaEdit.Visibility = Visibility.Collapsed;
                txtFechaEntrada.Visibility = Visibility.Visible;
                txtFechaEntrada.Text = reservaEdit.FechaInicioFormatted.ToString();

                txtFechaSalidaEdit.Visibility = Visibility.Collapsed;
                txtFechaSalida.Visibility = Visibility.Visible;
                txtFechaSalida.Text = reservaEdit.FechaFinFormatted.ToString();

                huespedesEdit.Visibility = Visibility.Collapsed;
                txtHuespedes.Visibility = Visibility.Visible;
                txtHuespedes.Text = reservaEdit.NumPersonas.ToString();

                txtPrecioEdit.Visibility = Visibility.Collapsed;
                txtPrecio.Visibility = Visibility.Visible;
                txtPrecio.Text = reservaEdit.Precio.ToString();

                txtCliente.Visibility = Visibility.Collapsed;
                txtClienteLbl.Visibility = Visibility.Visible;
                txtClienteLbl.Text = reservaEdit.Cliente.Nombre;

                txtEmail.Visibility = Visibility.Collapsed;
                txtEmailInfo.Visibility = Visibility.Visible;
                txtEmailInfo.Text = reservaEdit.Cliente.Email;
            }
        }
        // Constructor para crear una nueva reserva.
        public CrearReserva(Usuario usuario)
        {
            this.WindowState = WindowState.Maximized;
            this.usuarioLogeado = usuario;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            InitializeComponent();
        }
        // Cierra la ventana y vuelve a la lista de reservas.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas(usuarioLogeado);
            listaReservas.Show();
            Close();
        }
        // Maneja la creación o edición de una reserva.
        private async void clickCrearReserva(object sender, RoutedEventArgs e)
        {
            if (editable)
            {
                if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("ERROR: Hay que completar el cliente y su email.");
                    return;
                }

                Reserva reserva = new Reserva
                {
                    Id = reservaEdit.Id,
                    IdHabitacion = reservaEdit.IdHabitacion,
                    Cliente = new ClienteReserva(txtCliente.Text.Trim(), txtEmail.Text.Trim()),
                    Precio = double.TryParse(txtPrecioEdit.Text.Replace("€", "").Trim(), out double precio) ? precio : reservaEdit.Precio,
                    FechaInicio = DateTime.TryParse(txtFechaEntradaEdit.Text, out DateTime fechaInicio) ? fechaInicio : reservaEdit.FechaInicio,
                    FechaSalida = DateTime.TryParse(txtFechaSalidaEdit.Text, out DateTime fechaSalida) ? fechaSalida : reservaEdit.FechaSalida,
                    TipoHabitacion = txtTipo.Text.Trim(),
                    NumPersonas = int.TryParse(huespedesEdit.Text, out int numPersonas) ? numPersonas : reservaEdit.NumPersonas,
                    Extras = extras
                };

                await EditarReserva(reserva);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("ERROR: Hay que completar el cliente y su email.");
                    return;
                }

                if (!EsEmailValido(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("ERROR: El email introducido no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                Reserva reserva = new Reserva
                {
                    IdHabitacion = int.Parse(id),
                    Cliente = new ClienteReserva(txtCliente.Text.Trim(), txtEmail.Text.Trim()),
                    Precio = double.Parse(txtPrecio.Text.Replace("€", "").Trim()),
                    FechaInicio = entrada,
                    FechaSalida = salida,
                    TipoHabitacion = txtTipo.Text.Trim(),
                    NumPersonas = int.Parse(txtHuespedes.Text),
                    Extras = extras
                };

                await crearReserva(reserva);
            }
        }
        // Crea una nueva reserva enviando una solicitud a la API.
        private async Task crearReserva(Reserva nuevaReserva)
        {
            try
            {
                var reservaJson = new
                {
                    idHabitacion = nuevaReserva.IdHabitacion,
                    cliente = new
                    {
                        nombre = nuevaReserva.Cliente.Nombre,
                        email = nuevaReserva.Cliente.Email
                    },
                    precio = nuevaReserva.Precio,
                    fechaInicio = nuevaReserva.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    fechaSalida = nuevaReserva.FechaSalida.ToString("yyyy-MM-ddTHH:mm:ss"),
                    tipoHabitacion = nuevaReserva.TipoHabitacion,
                    numPersonas = nuevaReserva.NumPersonas,
                    extras = nuevaReserva.Extras
                };

                var json = JsonConvert.SerializeObject(reservaJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/reservas/create", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Reserva creada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    SegundaVentana segundaVentana = new SegundaVentana(usuarioLogeado);
                    segundaVentana.Show();

                    Close();
                }
                else
                {
                    MessageBox.Show($"Error al crear la reserva: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    SegundaVentana segundaVentana = new SegundaVentana(usuarioLogeado);
                    segundaVentana.Show();

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Edita una reserva existente enviando una solicitud a la API.
        private async Task EditarReserva(Reserva reservaActualizada)
        {
            try
            {
                var reservaJson = new
                {
                    id = reservaActualizada.Id,
                    idHabitacion = reservaActualizada.IdHabitacion,
                    cliente = new
                    {
                        nombre = reservaActualizada.Cliente.Nombre,
                        email = reservaActualizada.Cliente.Email
                    },
                    precio = reservaActualizada.Precio,
                    fechaInicio = reservaActualizada.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    fechaSalida = reservaActualizada.FechaSalida.ToString("yyyy-MM-ddTHH:mm:ss"),
                    numPersonas = reservaActualizada.NumPersonas,
                    extras = reservaActualizada.Extras
                };

                var json = JsonConvert.SerializeObject(reservaJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PatchAsync("/reservas/update", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Reserva editada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListaReservas listaReservas = new ListaReservas(usuarioLogeado);
                    listaReservas.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error al editar la reserva: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ListaReservas listaReservas = new ListaReservas(usuarioLogeado);
                    listaReservas.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Verifica si un email tiene un formato válido.
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
