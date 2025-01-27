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
            AddHabitacion a = new AddHabitacion();
            a.Show();
            ListaHabitaciones lh = new ListaHabitaciones();
            lh.Show();
            CrearReserva crear = new CrearReserva();
            crear.Show();
            */
            ListaReservas listaReservas = new ListaReservas();
            listaReservas.Show();
            
        }
    }
}