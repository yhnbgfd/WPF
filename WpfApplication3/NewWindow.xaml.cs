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
using System.IO;

namespace Wpf
{
    public delegate void ReflashDataEventHandle(object sender, EventArgs e);

    public partial class NewWindow : Window
    {
        public event ReflashDataEventHandle ReflashData;
        DispatcherTimer timer = new DispatcherTimer();
        Rect WorkRect = SystemParameters.WorkArea;

        public NewWindow()
        {
            InitializeComponent();
            Wpf.Helper.FileSystemCheck.CheckFolder();
            Properties.Settings.Default.MainWindow = this;
            this.Grid_Singin.Visibility = System.Windows.Visibility.Visible;
            InitializeFrame();
            //关闭弹出框事件
            Wpf.View.Pages.Page_SignIn signin = new View.Pages.Page_SignIn();
            signin.signIn += new View.Pages.SignInEventHandle(CloseSignInPage);
            this.Frame_SignIn.Content = signin;
            
            ShowTime();

            if(Properties.Settings.Default.is主窗口最大化)
            {
                MaxWindow();
            }
        }

        private void OnReflashData(object sender, EventArgs e)
        {
            if(ReflashData != null)
            {
                ReflashData(this, e);
            }
        }

        private void CloseSignInPage()
        {
            this.Grid_Singin.Visibility = System.Windows.Visibility.Collapsed;
            Wpf.Helper.Secure.SystemReg();
            if (Wpf.Data.DataDef.SignInUserName != "root")
            {
                this.MenuItem_拷贝无密码数据库.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 状态栏时间显示
        /// </summary>
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
            if (Properties.Settings.Default.is主窗口最大化 != true)
            {
                this.DragMove();
            }
        }

        private void Button_关闭_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Wpf.Data.Database.Log("SignOut", "SignOut User: " + Wpf.Data.DataDef.SignInUserName, "", "Sign");
            Wpf.Data.Database.Disconnect();
            Properties.Settings.Default.Save();
        }

        private void Button_最大化_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.is主窗口最大化 == true)
            {
                NormalSizeWindow();
            }
            else
            {
                Properties.Settings.Default.主窗口位置 = new Rect(this.Left, this.Top, this.Width, this.Height);
                MaxWindow();
            }
        }

        private void Button_最小化_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
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
            new Wpf.View.Windows.AboutWindow().ShowDialog();
        }

        private void MaxWindow()
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = WorkRect.Width;
            this.Height = WorkRect.Height;
            Properties.Settings.Default.is主窗口最大化 = true;
        }

        private void NormalSizeWindow()
        {
            this.Height = Properties.Settings.Default.主窗口位置.Height;
            this.Width = Properties.Settings.Default.主窗口位置.Width;
            this.Left = Properties.Settings.Default.主窗口位置.Left;
            this.Top = Properties.Settings.Default.主窗口位置.Top;
            Properties.Settings.Default.is主窗口最大化 = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(this.ActualHeight > WorkRect.Height || this.ActualWidth > WorkRect.Width)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                Button_最大化_Click(null,null);
            }
        }

        private void MenuItem_拷贝无密码数据库_Click(object sender, RoutedEventArgs e)
        {

            if(Wpf.Data.DataDef.SignInUserName == "root")
            {
                Wpf.Data.Database.ClearPassword();
                File.Copy("Data\\Data.db", "Data\\DataWithoutPassword.db", true);
                Wpf.Data.Database.ChangePassword(Wpf.Data.DataDef.DbPassword);
                Wpf.Data.Database.Log("CopyDB", "Successed", "", "CopyDB");
                MessageBoxResult result = MessageBox.Show("拷贝成功。");   
            }
            else
            {
                Wpf.Data.Database.Log("CopyDB", "Faild", "", "CopyDB");
                MessageBoxResult result = MessageBox.Show("权限不足。");   
            }
        }

        private void MenuItem_OpenExcelOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            Wpf.Data.Database.Log("Explorer.exe", "OpenExcelOutputFolder", "", "Explorer.exe");
            System.Diagnostics.Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "ExcelOutput");
        }

        private void MenuItem_OpenCalculators_Click(object sender, RoutedEventArgs e)
        {
            Wpf.Data.Database.Log("Calculators", "OpenCalculators", "", "Calculators");
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void MenuItem_导入Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Title = "请选择要导入的Excel文件";
            open.Filter = "Office Excel 2003文档|*.xls";
            open.RestoreDirectory = true;
            if ((bool)open.ShowDialog().GetValueOrDefault())
            {
                new Wpf.ExcelPlus.ExcelImport().Import(open.FileName);
            }
            OnReflashData(this, new EventArgs());
        }

        private void MenuItem_注册_Click(object sender, RoutedEventArgs e)
        {
            new Wpf.View.Windows.RegisterWindow().ShowDialog();
        }

        private void MenuItem_备份_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                Wpf.Data.Database.Log("Backup", date, "Backup", "Backup");
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Data\\Data.db", AppDomain.CurrentDomain.BaseDirectory + "Backup\\DataBackup_" + date + ".db", true);
                if (MessageBox.Show("数据库备份完成，是否打开备份文件夹？", "数据库备份", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "Backup");
                }
            }
            catch(Exception ee)
            {
                Wpf.Helper.Log.ErrorLog(ee.ToString());
            }
        }
    }
}
