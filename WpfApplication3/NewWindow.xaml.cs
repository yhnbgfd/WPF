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
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();
            InitializeFrame();
        }

        private void InitializeFrame()
        {
            Frame frame;
            for (int i = 1; i <= 6; i++ )
            {
                frame = FindName("Frame_" + i) as Frame;
                frame.Content = new Wpf.View.Pages.Page_主内容(i);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_关闭_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Wpf.Data.Database.Disconnect();
            Properties.Settings.Default.登陆用户名 = "";
            Properties.Settings.Default.Save();
        }

        private void Button_最大化_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_最小化_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.下拉框_户型 = 1;
        }

        private void MenuItem_设置_Click(object sender, RoutedEventArgs e)
        {
            new Wpf.View.Windows.SettingsWindow().ShowDialog();
        }

        private void MenuItem_退出_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_关于_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
