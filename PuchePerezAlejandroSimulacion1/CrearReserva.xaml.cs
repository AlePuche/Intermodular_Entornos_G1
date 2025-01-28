using Newtonsoft.Json;
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
        bool editable;
        Reserva reservaEdit;
        public int extras = 0;
        public DateTime entrada;
        public DateTime salida;
        public int price;

        private readonly HttpClient _httpClient;

        public CrearReserva(bool editable, Reserva reserva)
        {
            InitializeComponent();
            this.editable = editable;
            this.reservaEdit = reserva;

            if (editable)
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:3000")
                };
                reservaButton.Visibility = Visibility.Visible;
                reservaButton.Content = "Editar Reserva";
                lblTitulo.Text = "Editar Reserva";

                txtTipo.Text = "Habitación " + reservaEdit.TipoHabitacion;

                txtFechaEntrada.Visibility = Visibility.Collapsed;
                txtFechaEntradaEdit.Visibility = Visibility.Visible;
                txtFechaEntradaEdit.Text = reservaEdit.FechaInicioFormatted.ToString();

                txtFechaSalida.Visibility = Visibility.Collapsed;
                txtFechaSalidaEdit.Visibility = Visibility.Visible;
                txtFechaSalidaEdit.Text = reservaEdit.FechaFinFormatted.ToString();

                txtHuespedes.Visibility = Visibility.Collapsed;
                txtHuespedesEdit.Visibility = Visibility.Visible;
                txtHuespedesEdit.Text = reservaEdit.NumPersonas.ToString();

                txtPrecio.Visibility = Visibility.Collapsed;
                txtPrecioEdit.Visibility = Visibility.Visible;
                txtPrecioEdit.Text = reservaEdit.Precio.ToString();

                txtClienteLbl.Visibility = Visibility.Collapsed;
                txtCliente.Visibility = Visibility.Visible;
                txtCliente.Text = reservaEdit.Cliente.Nombre;
            }
            else
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

                txtHuespedesEdit.Visibility = Visibility.Collapsed;
                txtHuespedes.Visibility = Visibility.Visible;
                txtHuespedes.Text = reservaEdit.NumPersonas.ToString();

                txtPrecioEdit.Visibility = Visibility.Collapsed;
                txtPrecio.Visibility = Visibility.Visible;
                txtPrecio.Text = reservaEdit.Precio.ToString();

                txtCliente.Visibility = Visibility.Collapsed;
                txtClienteLbl.Visibility = Visibility.Visible;
                txtClienteLbl.Text = reservaEdit.Cliente.Nombre;
            }
        }
        public CrearReserva()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void clickCrearReserva(object sender, RoutedEventArgs e)
        {
            Reserva nuevaReserva = null;
            if (txtCliente.Text.Trim() == "" && txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("ERROR: Hay que completar el cliente y su email");
            }
            else
            {
                nuevaReserva = new Reserva
                {
                    Extras = this.extras,
                    Cliente = new ClienteReserva(txtCliente.Text, txtEmail.Text),
                    FechaInicio = entrada,
                    FechaSalida = salida,
                    TipoHabitacion = txtTipo.Text,
                    IdHabitacion = 101,
                    NumPersonas = Int32.Parse(txtHuespedes.Text),
                    Precio = this.price,
                };

                crearReserva(nuevaReserva);
            }
        }

        private async void crearReserva(Reserva nuevaReserva) {
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
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
