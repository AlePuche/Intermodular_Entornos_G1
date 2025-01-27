using System.Windows;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Usuario[] usuarios = new Usuario[]
        {
            new Usuario { Name = "Alejandro", Email = "alejandro@example.com", Password = "admin123", Role = "Admin" },
            new Usuario { Name = "Alvaro", Email = "alvaro@example.com", Password = "empleado123", Role = "Empleado" },
            new Usuario { Name = "UsuarioAdmin", Email = "1", Password = "1", Role = "Admin" },// Usuario ADMIN para entrar rápido y comodamente mientras se desarrolla el proyecto
            new Usuario { Name = "UsuarioEmpleado", Email = "2", Password = "2", Role = "Empleado" },// Usuario EMPLEADO para entrar rápido y comodamente mientras se desarrolla el proyecto
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            Usuario usuarioLogeado = ValidarCredenciales(email, password);

            if (usuarioLogeado != null)
            {
                ListaHabitaciones ventanaHabitaciones = new ListaHabitaciones(usuarioLogeado);
                ventanaHabitaciones.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas. Intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Usuario ValidarCredenciales(string email, string password)
        {
            foreach (var usuario in usuarios)
            {
                if (usuario.Email == email && usuario.Password == password)
                {
                    return usuario;
                }
            }

            return null;
        }

    }
}