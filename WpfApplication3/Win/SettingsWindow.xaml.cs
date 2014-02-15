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
using System.Security;

namespace Wpf.Win
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_提交修改密码_Click(object sender, RoutedEventArgs e)
        {
            this.TextBlock_旧密码错误.Text = "";
            this.TextBlock_新密码不一致.Text = "";
            this.TextBlock_密码修改成功.Text = "";
            string UserName = Properties.Settings.Default.登陆用户名;
            SecureString OldPassword = this.PasswordBox_旧密码.SecurePassword;
            SecureString NewPassword = this.PasswordBox_新密码.SecurePassword;
            SecureString NewPasswordRepeat = this.PasswordBox_重复新密码.SecurePassword;
            if (Wpf.Helper.Secure.TranslatePassword(NewPassword).Equals(Wpf.Helper.Secure.TranslatePassword(NewPasswordRepeat)))
            {
                if (Wpf.Helper.Secure.CheckUserNameAndPassword(UserName, OldPassword))
                {
                    string sql = "UPDATE T_User set password=" + Wpf.Helper.Secure.TranslatePassword(NewPassword) + " WHERE username='" + UserName +"'";
                    Wpf.Data.Database.Update(sql);
                    this.TextBlock_密码修改成功.Text = "密码修改成功";
                }
                else
                {
                    this.TextBlock_旧密码错误.Text = "旧密码错误";
                }
            }
            else
            {
                this.TextBlock_新密码不一致.Text = "新密码不一致";
            }
        }

        private void Button_保存设置_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
