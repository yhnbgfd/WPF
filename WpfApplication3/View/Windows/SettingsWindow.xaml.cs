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
            this.TextBlock_初始金额_1.Text = Wpf.Data.DataDef.CustomerType[0];
            this.TextBlock_初始金额_2.Text = Wpf.Data.DataDef.CustomerType[1];
            this.TextBlock_初始金额_3.Text = Wpf.Data.DataDef.CustomerType[2];
            this.TextBlock_初始金额_4.Text = Wpf.Data.DataDef.CustomerType[3];
            this.TextBlock_初始金额_5.Text = Wpf.Data.DataDef.CustomerType[4];
            this.TextBlock_初始金额_6.Text = Wpf.Data.DataDef.CustomerType[5];

            #region 读取初始金额
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            xml.LoadXML();
            this.Textbox_初始金额_1.Text = xml.ReadXML("预算内户");//Properties.Settings.Default.初始金额_预算内户.ToString();
            this.Textbox_初始金额_2.Text = xml.ReadXML("预算外户");//Properties.Settings.Default.初始金额_预算外户.ToString();
            this.Textbox_初始金额_3.Text = xml.ReadXML("周转金户");//Properties.Settings.Default.初始金额_周转金户.ToString();
            this.Textbox_初始金额_4.Text = xml.ReadXML("计生专户");//Properties.Settings.Default.初始金额_计生专户.ToString();
            this.Textbox_初始金额_5.Text = xml.ReadXML("政粮补贴资金专户");//Properties.Settings.Default.初始金额_政粮补贴资金专户.ToString();
            this.Textbox_初始金额_6.Text = xml.ReadXML("土地户");//Properties.Settings.Default.初始金额_土地户.ToString();
            #endregion

            string str = Application.ResourceAssembly.GetName().Version.ToString();
            this.TextBlock_正式版.Text = "版本：";
#if DEBUG
            this.TextBlock_正式版.Text += "内部版本";
#endif
#if (!DEBUG)
            this.TextBlock_正式版.Text += "正式版";
#endif
            this.TextBlock_正式版.Text += str;
        }

        private void Button_提交修改密码_Click(object sender, RoutedEventArgs e)
        {
            this.TextBlock_旧密码错误.Text = "";
            this.TextBlock_新密码不一致.Text = "";
            this.TextBlock_密码修改成功.Text = "";
            string UserName = Wpf.Data.DataDef.SignInUserName;
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
            #region 保存到config.xml
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            xml.LoadXML();
            xml.WriteXML("预算内户", decimal.Parse(this.Textbox_初始金额_1.Text).ToString());
            xml.WriteXML("预算外户", decimal.Parse(this.Textbox_初始金额_2.Text).ToString());
            xml.WriteXML("周转金户", decimal.Parse(this.Textbox_初始金额_3.Text).ToString());
            xml.WriteXML("计生专户", decimal.Parse(this.Textbox_初始金额_4.Text).ToString());
            xml.WriteXML("政粮补贴资金专户", decimal.Parse(this.Textbox_初始金额_5.Text).ToString());
            xml.WriteXML("土地户", decimal.Parse(this.Textbox_初始金额_6.Text).ToString());
            xml.SaveXML();
            #endregion

            #region 保存到数据库
            List<string> sqls = new List<string>();
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_1.Text) + "' where key=51");
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_2.Text) + "' where key=52");
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_3.Text) + "' where key=53");
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_4.Text) + "' where key=54");
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_5.Text) + "' where key=55");
            sqls.Add("Update T_Type set value='" + decimal.Parse(this.Textbox_初始金额_6.Text) + "' where key=56");
            Wpf.Data.Database.doDMLs(sqls, "Update", "Update初始金额");
            #endregion

            new Wpf.ViewModel.ViewModel_Surplus().InitSurplus();

            this.TextBlock_保存成功.Text = "保存成功。";
        }
    }
}
