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
using System.Windows.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        ///// <summary>
        ///// 重写回车=tab
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        // MoveFocus takes a TraveralReqest as its argument.
        //        TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
        //        // Gets the element with keyboard focus.
        //        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
        //        // Change keyboard focus.
        //        if (elementWithFocus != null)
        //        {
        //            elementWithFocus.MoveFocus(request);
        //        }
        //        e.Handled = true;
        //    }
        //    base.OnPreviewKeyDown(e);
        //}
        DispatcherTimer timer = new DispatcherTimer();

        public NewWindow()
        {
            InitializeComponent();
            this.Grid_Singin.Visibility = System.Windows.Visibility.Visible;
            InitializeFrame();

            Wpf.View.Pages.Page_SignIn signin = new View.Pages.Page_SignIn();
            signin.signIn += new View.Pages.SignInEventHandle(CloseSignInPage);
            this.Frame_SignIn.Content = signin;
            ShowTime();
        }

        private void CloseSignInPage()
        {
            this.Grid_Singin.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShowTime()
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(0.1);   //设置刷新的间隔时间
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            this.StatusBarItem_时间.Content = DateTime.Now.ToString("yyyy年MM月dd日 dddd HH:mm:ss");
        }

        private void InitializeFrame()
        {
            Frame frame;
            for (int i = 1; i <= 6; i++ )
            {
                frame = FindName("Frame_" + i) as Frame;
                frame.Content = new Wpf.View.Pages.Page_主内容(i);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_关闭_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Wpf.Data.Database.Disconnect();
            Properties.Settings.Default.登陆用户名 = "";
            Properties.Settings.Default.Save();
        }

        private void Button_最大化_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_最小化_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.下拉框_户型 = 1;
        }

        private void MenuItem_设置_Click(object sender, RoutedEventArgs e)
        {
            new Wpf.View.Windows.SettingsWindow().ShowDialog();
        }

        private void MenuItem_退出_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_关于_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
