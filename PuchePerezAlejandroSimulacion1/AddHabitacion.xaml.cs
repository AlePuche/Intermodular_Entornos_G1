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

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("El campo 'ID' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTipo.Text))
            {
                MessageBox.Show("El campo 'Tipo' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (numHuespedes.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar el número de huéspedes.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(new TextRange(txtDescripcion.Document.ContentStart, txtDescripcion.Document.ContentEnd).Text.Trim()))
            {
                MessageBox.Show("El campo 'Descripción' es obligatorio.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(((TextBox)FindName("PrecioTextBox"))?.Text, out double precio) || precio <= 0)
            {
                MessageBox.Show("El campo 'Precio por noche' debe ser un número positivo.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Habitación añadida correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
