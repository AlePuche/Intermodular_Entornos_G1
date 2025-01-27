using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para Buscador.xaml
    /// </summary>
    public partial class Buscador : Window
    {
        private ObservableCollection<Usuario> Usuarios { get; set; }

        public Buscador()
        {
            InitializeComponent();
            CargarListaUsuarios();
            DataContext = this;
        }

        private void CargarListaUsuarios()
        {
            Usuarios = new ObservableCollection<Usuario>
            {
                new Usuario { Name = "ana", Email = "ana@gmail.com", Password = "ananananana", Role = "Administrador", Phone = "+34 545555555", Adress = "Su Casa" },
                new Usuario { Name = "paco", Email = "sdf@gmail.com", Password = "df", Role = "Usuario", Phone = "+34 555555755", Adress = "Calle de los jazmines nº3" },
                new Usuario { Name = "juan", Email = "fdf@gmail.com", Password = "bv", Role = "Usuario", Phone = "+34 555555655", Adress = "Su Casa" },
                new Usuario { Name = "carlos", Email = "rdfcvg@gmail.com", Password = "q", Role = "Trabajador", Phone = "+34 525555555", Adress = "Su Casa" },
                new Usuario { Name = "pedro", Email = "oiu@gmail.com", Password = "a", Role = "Usuario", Phone = "+34 555555555", Adress = "Su Casa" },
                new Usuario { Name = "maria", Email = "uiiaiuuiiiai@gmail.com", Password = "z", Role = "Usuario", Phone = "+34 155555555", Adress = "Su Casa" },
                new Usuario { Name = "pepe", Email = "t@gmail.com", Password = "zx", Role = "Administrador", Phone = "+34 555565555", Adress = "Su Casa" },
                new Usuario { Name = "jose", Email = "r@gmail.com", Password = "z", Role = "Usuario", Phone = "+34 555555558", Adress = "Su Casa" },
                new Usuario { Name = "francisco", Email = "tf@gmail.com", Password = "x", Role = "Usuario", Phone = "+34 552555555", Adress = "Su Casa" },
                new Usuario { Name = "pepelu", Email = "tfffgf@gmail.com", Password = "c", Role = "Trabajador", Phone = "+34 455555555", Adress = "Su Casa" },
                new Usuario { Name = "a", Email = "ewq@gmail.com", Password = "v", Role = "Trabajador", Phone = "+34 555558555", Adress = "Su Casa" },
                new Usuario { Name = "f", Email = "qwe@gmail.com", Password = "b", Role = "Usuario", Phone = "+34 555555545", Adress = "Su Casa" },
                new Usuario { Name = "gv", Email = "umm@gmail.com", Password = "an", Role = "Usuario", Phone = "+34 555553555", Adress = "Su Casa" },
                new Usuario { Name = "tgfvd", Email = "ujnb@gmail.com", Password = "n", Role = "Usuario", Phone = "+34 511555555", Adress = "Su Casa" },
                new Usuario { Name = "rfvbgt", Email = "iuyt@gmail.com", Password = "ananananana", Role = "Trabajador", Phone = "+34 551155555", Adress = "Su Casa" },
                new Usuario { Name = "efded", Email = "poiuytre@gmail.com", Password = "g", Role = "Usuario", Phone = "+34 555567555", Adress = "Su Casa" },
                new Usuario { Name = "ffff", Email = "ñ@gmail.com", Password = "t", Role = "Usuario", Phone = "+34 555555755", Adress = "Su Casa" },
                new Usuario { Name = "sddce", Email = "cfrdc@gmail.com", Password = "re", Role = "Trabajador", Phone = "+34 555535555", Adress = "Su Casa" },
                new Usuario { Name = "Eustaquio", Email = "doneustaquio@gmail.com", Password = "doneustaquioesuncapo", Role = "Administrador", Phone = "+34 123456789", Adress = "Una mansión muy chula" },
            };
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var habitacion = (Habitacion)button.DataContext;
                AddHabitacion ventana = new AddHabitacion(habitacion);
                ventana.Show();
            }
        }

        private void ReservasButton_Click(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas();
            lr.Show();
            Close();
        }

        private void UsuariosButton_Click(object sender, RoutedEventArgs e)
        {
            Buscador b = new Buscador();
            b.Show();
            Close();
        }

        private void HabitacionesButton_Click(object sender, RoutedEventArgs e)
        {
            ListaHabitaciones lh = new ListaHabitaciones();
            lh.Show();
            Close();
        }

        private void BuscadorButton_Click(object sender, RoutedEventArgs e)
        {
            SegundaVentana s = new SegundaVentana();
            s.Show();
            Close();
        }
    }
}
