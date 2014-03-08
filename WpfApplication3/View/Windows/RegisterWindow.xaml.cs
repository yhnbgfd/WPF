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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Button_注册_Click(object sender, RoutedEventArgs e)
        {
            string registerNum = this.TextBox_注册码.Text.Trim();
            //asdasd的16位md5
            if (registerNum == "7d9dc229d2921a40")
            {
                #region 兼容已部署版本
                if (Wpf.Data.Database.SelectCount("select count(*) from T_Type where key=997") == 0)
                {
                    Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('997','2014-03-09 01:00:00')", "Insert", "UpdateLicense");
                }
                if (Wpf.Data.Database.SelectCount("select count(*) from T_Type where key=999") == 0)
                {
                    Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('999','false')", "Insert", "UpdateLicense");
                }
                #endregion
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
