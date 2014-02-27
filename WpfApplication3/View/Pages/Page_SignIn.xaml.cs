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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.View.Pages
{
    public delegate void SignInEventHandle();

    public partial class Page_SignIn : Page
    {
        public event SignInEventHandle signIn;
        public Page_SignIn()
        {
            InitializeComponent();
            this.PasswordBox_密码.Focus();
        }

        public void OnSignIn()
        {
            if (signIn != null)
            {
                signIn();
            }
        }

        private void Button_登陆_Click(object sender, RoutedEventArgs e)
        {
            string UserName = this.TextBox_用户名.Text;
            if (Wpf.Helper.Secure.CheckUserNameAndPassword(UserName, this.PasswordBox_密码.SecurePassword))
            {
                //this.Grid_遮盖.Visibility = System.Windows.Visibility.Collapsed;
                this.PasswordBox_密码.Clear();
                Properties.Settings.Default.登陆用户名 = UserName;
                OnSignIn();
            }
            else
            {
                this.TextBlock_登陆提示.Text = "账号或密码错误，请重试。";
            }
        }
    }
}