using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report();
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void DataGrid_Main_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataGrid_Main.Columns[0].Visibility = Visibility.Collapsed;
        }
    }
}
