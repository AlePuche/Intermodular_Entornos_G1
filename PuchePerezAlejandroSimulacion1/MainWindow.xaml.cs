using System.Windows;
using PuchePerezAlejandroSimulacion1.Model;

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
            new Usuario { Name = "Diego Santos", Email = "d.santosbailon@edu.gva.es", Password = "Dsantos79", Role = "Admin" },// Usuario para el profesor con Rol de Admin
        };

        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            Usuario usuarioLogeado = ValidarCredenciales(email, password);

            if (usuarioLogeado != null)
            {
                SegundaVentana sv = new SegundaVentana(usuarioLogeado);
                sv.Show();
                Close();
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
                if (usuario.Email.ToLower() == email.ToLower() && usuario.Password == password)
                {
                    return usuario;
                }
            }

            return null;
        }

    }
}