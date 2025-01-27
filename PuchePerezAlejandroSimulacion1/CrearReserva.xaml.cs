using System;
using System.Collections.Generic;
using System.Linq;
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
        public CrearReserva(bool editable, Reserva reserva)
        {
            InitializeComponent();
            this.editable = editable;
            this.reservaEdit = reserva;

            if (editable)
            {
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
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
