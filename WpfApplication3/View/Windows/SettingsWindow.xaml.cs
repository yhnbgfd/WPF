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


namespace Wpf.View.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            InitializeToolBox();
        }

        private void InitializeToolBox()
        {
            //this.TextBox_起始年.Text = Properties.Settings.Default.起始年.ToString();
            //this.TextBox_结束年.Text = Properties.Settings.Default.结束年.ToString();

            this.TextBlock_初始金额_1.Text = Wpf.Data.DataDef.CustomerType[0];
            this.TextBlock_初始金额_2.Text = Wpf.Data.DataDef.CustomerType[1];
            this.TextBlock_初始金额_3.Text = Wpf.Data.DataDef.CustomerType[2];
            this.TextBlock_初始金额_4.Text = Wpf.Data.DataDef.CustomerType[3];
            this.TextBlock_初始金额_5.Text = Wpf.Data.DataDef.CustomerType[4];
            this.TextBlock_初始金额_6.Text = Wpf.Data.DataDef.CustomerType[5];
            this.Textbox_初始金额_1.Text = Properties.Settings.Default.初始金额_预算内户.ToString();
            this.Textbox_初始金额_2.Text = Properties.Settings.Default.初始金额_预算外户.ToString();
            this.Textbox_初始金额_3.Text = Properties.Settings.Default.初始金额_周转金户.ToString();
            this.Textbox_初始金额_4.Text = Properties.Settings.Default.初始金额_计生专户.ToString();
            this.Textbox_初始金额_5.Text = Properties.Settings.Default.初始金额_政粮补贴资金专户.ToString();
            this.Textbox_初始金额_6.Text = Properties.Settings.Default.初始金额_土地户.ToString();

            string str = Application.ResourceAssembly.GetName().Version.ToString();
            this.TextBlock_正式版.Text = "版本：";
            this.TextBlock_正式版.Text += (Properties.Settings.Default.正式版)?"正式版":"内部版本";
            this.TextBlock_正式版.Text += str;
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
                if (Wpf.Helper.Secure.TranslatePassword(NewPassword) == "" || Wpf.Helper.Secure.TranslatePassword(NewPasswordRepeat) == "")
                {
                    this.TextBlock_新密码不一致.Text = "密码不能为空";
                }
                else if (Wpf.Helper.Secure.CheckUserNameAndPassword(UserName, OldPassword))
                {
                    string sql = "UPDATE T_User set password='" + Wpf.Helper.Secure.TranslatePassword(NewPassword) + "' WHERE username='" + UserName +"'";
                    //Wpf.Data.Database.Update(sql);
                    Wpf.Data.Database.doDML(sql,"Update","ChangePassword");
                    this.TextBlock_密码修改成功.Text = "密码修改成功";
                    this.PasswordBox_旧密码.Clear();
                    this.PasswordBox_新密码.Clear();
                    this.PasswordBox_重复新密码.Clear();
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
            //Properties.Settings.Default.起始年 = int.Parse(this.TextBox_起始年.Text);
            //Properties.Settings.Default.结束年 = int.Parse(this.TextBox_结束年.Text);
            //保存初始金额
            Properties.Settings.Default.初始金额_预算内户 = decimal.Parse(this.Textbox_初始金额_1.Text);
            Properties.Settings.Default.初始金额_预算外户 = decimal.Parse(this.Textbox_初始金额_2.Text);
            Properties.Settings.Default.初始金额_周转金户 = decimal.Parse(this.Textbox_初始金额_3.Text);
            Properties.Settings.Default.初始金额_计生专户 = decimal.Parse(this.Textbox_初始金额_4.Text);
            Properties.Settings.Default.初始金额_政粮补贴资金专户 = decimal.Parse(this.Textbox_初始金额_5.Text);
            Properties.Settings.Default.初始金额_土地户 = decimal.Parse(this.Textbox_初始金额_6.Text);
            new Wpf.ViewModel.ViewModel_Surplus().InitSurplus();
            Wpf.Helper.Secure.Save初始金额();
            this.TextBlock_保存成功.Text = "保存成功。";
        }
    }
}
