using System.Windows;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();          
            /*
            ListaReservas listaReservas = new ListaReservas();
            listaReservas.Show();
            */
            ListaHabitaciones lh = new ListaHabitaciones();
            lh.Show();
            AddHabitacion ah = new AddHabitacion();
            ah.Show();
        }
    }
}