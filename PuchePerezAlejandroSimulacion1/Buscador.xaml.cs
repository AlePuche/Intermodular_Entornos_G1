using System.Windows;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para Buscador.xaml
    /// </summary>
    public partial class Buscador : Window
    {
        public Buscador()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana sg = new SegundaVentana();
            sg.Show();

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Buscador buscador = new Buscador();
            buscador.Show();

            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas listaReservas = new ListaReservas();
            listaReservas.Show();

            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones listaHabitaciones = new ListaHabitaciones();
            listaHabitaciones.Show();

            Close();
        }
    }
}
