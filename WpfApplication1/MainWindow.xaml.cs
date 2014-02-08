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

using Wpf.Pages;

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
            new Wpf.Database.DBInitialize().CreateDataFile();
            new Wpf.Database.DBInitialize().CreateTable();

            this.Frame_Content.Content = new Page_Main();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame_Content.Content = new Page_Main();
        }
    }
}
