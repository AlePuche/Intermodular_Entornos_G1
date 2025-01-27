using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PuchePerezAlejandroSimulacion1
{
    /// <summary>
    /// Lógica de interacción para AddHabitacion.xaml
    /// </summary>
    public partial class AddHabitacion : Window
    {
        
        private Habitacion habitacion;

        public AddHabitacion()
        {
            InitializeComponent();
        }

        public AddHabitacion(Habitacion habitacion)
        {
            InitializeComponent();
            this.habitacion = habitacion;
            
                txtAddEdit.Text = "Editar habitación";
                btnAddEdit.Content = "Editar";
                RellenarCampos(habitacion);
            
          
        }
        private void RellenarCampos(Habitacion habitacion)
        {
            txtId.Text = habitacion.Id.ToString();
            txtTipo.Text = habitacion.Tipo;
            foreach (ComboBoxItem item in numHuespedes.Items)
            {
                if (item.Content.ToString() == habitacion.Huespedes.ToString())
                {
                    numHuespedes.SelectedItem = item;
                    break;
                }
            }
            txtDescripcion.Document.Blocks.Clear();
            txtDescripcion.Document.Blocks.Add(new Paragraph(new Run(habitacion.Descripcion)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
