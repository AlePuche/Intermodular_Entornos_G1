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
        bool edit;
        private Habitacion habitacion;

        public AddHabitacion()
        {
            InitializeComponent();
        }

        public AddHabitacion(bool edit, Habitacion habitacion)
        {
            InitializeComponent();
            this.edit = edit;
            this.habitacion = habitacion;
            if (edit)
            {
                txtAddEdit.Text = "Editar habitación";
                btnAddEdit.Content = "Editar";
                RellenarCampos(habitacion);
            }
          
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

    }
}
