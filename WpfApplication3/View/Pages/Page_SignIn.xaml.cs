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
    public delegate void SignInEventHandle();

    public partial class Page_SignIn : Page
    {
        public event SignInEventHandle signIn;
        public Page_SignIn()
        {
            InitializeComponent();
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
            OnSignIn();
        }
    }
}
