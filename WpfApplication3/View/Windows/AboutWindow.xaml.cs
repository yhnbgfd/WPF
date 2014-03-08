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

namespace Wpf.View.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
#if DEBUG
            this.Laber_Version.Content = "试用版 剩余"+Wpf.Helper.Secure.CheckLicense()+"天 ";
#endif
#if (!DEBUG)
            this.Laber_Version.Content = "正式版 ";
#endif
            this.Laber_Version.Content += Application.ResourceAssembly.GetName().Version.ToString();
        }
    }
}
