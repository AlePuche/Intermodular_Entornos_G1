﻿using System.Windows;

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
            SegundaVentana s = new SegundaVentana();
            s.Show();
            Buscador b = new Buscador();
            b.Show();
            AddHabitacion a = new AddHabitacion();
            a.Show();
        }
    }
}