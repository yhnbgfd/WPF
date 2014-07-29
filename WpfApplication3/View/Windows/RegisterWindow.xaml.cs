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
using Wpf.Helper;

namespace Wpf.View.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            if (Wpf.Data.Database.SelectString("select value from T_Type where key=999") == "true")
            {
                this.TextBox_注册码.Text = "已注册";
                this.TextBox_注册码.IsReadOnly = true;
                this.Button_注册.IsEnabled = false;
            }
        }

        private void Button_注册_Click(object sender, RoutedEventArgs e)
        {
            string registerNum = this.TextBox_注册码.Text.Trim();
            //每个客户改一次
            string orginCode = "StoneAnt.WPF" + DateTime.Now.ToString("yyyyMMddHHmmss") + "ASDFG";
            string validateCode = Secure.GetMD5_32(orginCode).ToUpper();
            //asdasd的16位md5
            if (registerNum.Equals(validateCode))
            {
                Wpf.Data.Database.doDML("Update T_Type set value='true' where key=999", "Update", "UpdateRegister");
                this.TextBox_注册码.Text = "";
                Clipboard.Clear();
                this.Label_信息.Content = "注册成功";
            }
            else
            {
                this.Label_信息.Content = "注册码有误";
            }
        }
    }
}
