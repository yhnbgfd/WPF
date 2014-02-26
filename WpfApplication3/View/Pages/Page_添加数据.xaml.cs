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
    public delegate void TestEventHandle();

    public partial class Page_添加数据 : Page
    {
        private int type = 0;
        public event TestEventHandle testMainPage;

        public Page_添加数据(int type)
        {
            InitializeComponent();
            this.type = type;
        }

        public void OnTestMainPage()
        {
            if (testMainPage != null)
            {
                testMainPage();
            }
        }

        void Button_添加数据_取消_Click(object sender, RoutedEventArgs e)
        {
            OnTestMainPage();
        }

        private void Button_添加数据_保存_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
