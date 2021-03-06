﻿using System;
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
        private Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
        public SettingsWindow()
        {
            InitializeComponent();
            InitializeToolBox();
        }

        private void InitializeToolBox()
        {
            for (int i = 0; i < 6; i++)
            {
                TextBlock tbl = FindName("TextBlock_初始金额_" + (i + 1)) as TextBlock;
                tbl.Text = Wpf.Data.DataDef.CustomerType[i];
                if (string.IsNullOrEmpty(tbl.Text))
                {
                    TextBox tb = FindName("Textbox_初始金额_" + (i + 1)) as TextBox;
                    tb.Visibility = Visibility.Collapsed;
                }
                #region 读取便签值
                TextBox tbb = FindName("TextBox_Tag" + (i + 1)) as TextBox;
                tbb.Text = xml.ReadXML("Tag" + (i + 1));
                #endregion
            }

            #region 读取初始金额
            this.Textbox_初始金额_1.Text = xml.ReadXML("预算内户");
            this.Textbox_初始金额_2.Text = xml.ReadXML("预算外户");
            this.Textbox_初始金额_3.Text = xml.ReadXML("周转金户");
            this.Textbox_初始金额_4.Text = xml.ReadXML("计生专户");
            this.Textbox_初始金额_5.Text = xml.ReadXML("政粮补贴资金专户");
            this.Textbox_初始金额_6.Text = xml.ReadXML("土地户");
            #endregion

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
            xml.WriteXML("预算内户", decimal.Parse(this.Textbox_初始金额_1.Text).ToString());
            xml.WriteXML("预算外户", decimal.Parse(this.Textbox_初始金额_2.Text).ToString());
            xml.WriteXML("周转金户", decimal.Parse(this.Textbox_初始金额_3.Text).ToString());
            xml.WriteXML("计生专户", decimal.Parse(this.Textbox_初始金额_4.Text).ToString());
            xml.WriteXML("政粮补贴资金专户", decimal.Parse(this.Textbox_初始金额_5.Text).ToString());
            xml.WriteXML("土地户", decimal.Parse(this.Textbox_初始金额_6.Text).ToString());
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

        private void Button_ChangeTag_Click(object sender, RoutedEventArgs e)
        {
            this.TextBlock_TagError.Visibility = System.Windows.Visibility.Collapsed;
            for (int i = 1; i <= 6;i++ )
            {
                TextBox tb = FindName("TextBox_Tag" + i) as TextBox;
                xml.WriteXML("Tag" + i, tb.Text.Trim());
            }
            this.TextBlock_TagMess.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_defaultTag_Click(object sender, RoutedEventArgs e)
        {
            xml.WriteXML("Tag1", "预算内户");
            xml.WriteXML("Tag2", "预算外户");
            xml.WriteXML("Tag3", "周转金户");
            xml.WriteXML("Tag4", "计生专户");
            xml.WriteXML("Tag5", "政粮补贴资金专户");
            xml.WriteXML("Tag6", "土地户");
            this.TextBlock_TagMess.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
