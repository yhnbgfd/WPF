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

namespace Wpf.View.Pages
{
    /// <summary>
    /// Interaction logic for Page_添加数据.xaml
    /// </summary>
    public partial class Page_添加数据 : Page
    {
        private int type = 0;
        public Page_添加数据(int type)
        {
            InitializeComponent();
            this.type = type;
        }

        private void Button_添加数据_取消_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_添加数据_保存_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
