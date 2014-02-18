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
using Wpf.Data;
using System.IO;
using Wpf.Helper;

namespace Wpf
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// DataGrid Cell编辑前的值
        /// </summary>
        private string preValue;
        private bool isInit = false;
        /// <summary>
        /// 是否在登陆界面，是则enter相应按钮事件
        /// </summary>
        private bool isInLoginWindow = true;

        /// <summary>
        /// 重写回车=tab
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (isInLoginWindow)
                {
                    Button_登陆_登陆_Click(null,null);
                    return;
                }
                // MoveFocus takes a TraveralReqest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
                e.Handled = true;
            }
            else if(e.Key == Key.F5)
            {
                RefreshDisplayData("All");
            }
            base.OnPreviewKeyDown(e);
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeToolBox();
            SystemCheck();
            RefreshDisplayData("All");
            isInit = true;
            DisableTools();
        }

        /// <summary>
        /// 系统自检、正版检查
        /// 第一次开打软件会隐藏注册，防止拷贝
        /// 数据库如果打不开，启动时就出错了。轮不到自检。
        /// </summary>
        private void SystemCheck()
        {
            if (!Properties.Settings.Default.初始化程序)//还没初始化
            {
                //可能遇到的问题：别人修改了user.config文件初始化程序=false，那么就会重新注册一遍
                Register();
            }
            Wpf.Helper.FileSystemCheck.CheckFolder();
        }
        /// <summary>
        /// 初始化、注册程序
        /// </summary>
        private void Register()
        {
            string time = DateTime.Now.ToString();
            string License = Wpf.Helper.Secure.GetMD5_32(time + " Power By StoneAnt");
            
            Properties.Settings.Default.注册时间 = time;
            Properties.Settings.Default.注册码 = License;
            Properties.Settings.Default.初始化程序 = true;
            Wpf.Data.Database.Update("UPDATE T_Type set value='" + License + "' where key=998");
            Wpf.Helper.Secure.RegistrationInformationFile();
            if (Properties.Settings.Default.正式版)
            {
                Wpf.Data.Database.ChangePassword(Wpf.Helper.Secure.GetMD5_32(License + "PowerByStoneAnt"));
            }
        }

        /// <summary>
        /// 所有控件\Settings如需初始化，则在这里面
        /// </summary>
        private void InitializeToolBox()
        {
            //保存程序目录到settings
            Properties.Settings.Default.Path = AppDomain.CurrentDomain.BaseDirectory;
            //保存当前日期到settings下拉框日期
            Properties.Settings.Default.下拉框_年 = Wpf.Helper.Date.GetYear();
            Properties.Settings.Default.下拉框_月 = Wpf.Helper.Date.GetMonth();
            //遮盖grid初始化状态：显示
            this.Grid_遮盖.Visibility = System.Windows.Visibility.Visible;
            //combobox
            this.ComboBox_Type.ItemsSource = Wpf.Data.DataDef.CustomerType;
            this.ComboBox_Type.SelectedIndex = 0;
            this.ComboBox_Year.ItemsSource = Wpf.Data.DataDef.Year;
            this.ComboBox_Year.SelectedIndex = Wpf.Data.DataDef.perYear + 1;
            this.ComboBox_Month.ItemsSource = Wpf.Data.DataDef.Month;
            this.ComboBox_Month.SelectedIndex = Wpf.Helper.Date.GetMonth();
        }

        /// <summary>
        /// 更新页面上所有可见数据集合到这里
        /// </summary>
        private void RefreshDisplayData(string message)
        {
            //更新dataset
            //if(!message.Equals("WithoutDataGrid"))
            //{
                this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report()
                    .Report(Properties.Settings.Default.下拉框_户型, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            //}

            //更新“承上月结余”
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report()
                .GetSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, Properties.Settings.Default.下拉框_户型).ToString();

            //更新下方4个累计textblock
            this.TextBlock_借方发生额合计.Text = Properties.Settings.Default.借方发生额合计.ToString("C");
            this.TextBlock_贷方发生额合计.Text = Properties.Settings.Default.贷方发生额合计.ToString("C");
            this.TextBlock_借方发生额累计.Text = Properties.Settings.Default.借方发生额累计.ToString("C");
            this.TextBlock_贷方发生额累计.Text = Properties.Settings.Default.贷方发生额累计.ToString("C");
            //设置颜色
            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush green = new SolidColorBrush(Colors.Green);
            this.TextBlock_借方发生额合计.Foreground = red;
            this.TextBlock_贷方发生额合计.Foreground = green;
            this.TextBlock_借方发生额累计.Foreground = red;
            this.TextBlock_贷方发生额累计.Foreground = green;
        }
        /// <summary>
        /// 禁用所有控件，在登陆之前
        /// </summary>
        private void DisableTools()
        {
            this.Button_Excel.IsEnabled = false;
            this.Button_导入Excel.IsEnabled = false;
            this.Button_删除.IsEnabled = false;
            this.Button_刷新.IsEnabled = false;
            this.Button_拷贝无密码数据库.IsEnabled = false;
            this.MenuItem_设置.IsEnabled = false;
            this.ComboBox_Month.IsEnabled = false;
            this.ComboBox_Type.IsEnabled = false;
            this.ComboBox_Year.IsEnabled = false;
            this.DataGrid_Main.IsEnabled = false;
        }
        /// <summary>
        /// 登陆之后启用所有控件
        /// </summary>
        private void EnableTools()
        {
            this.Button_Excel.IsEnabled = true;
            this.Button_导入Excel.IsEnabled = true;
            this.Button_删除.IsEnabled = true;
            this.Button_刷新.IsEnabled = true;
            this.Button_拷贝无密码数据库.IsEnabled = true;
            this.MenuItem_设置.IsEnabled = true;
            this.ComboBox_Month.IsEnabled = true;
            this.ComboBox_Type.IsEnabled = true;
            this.ComboBox_Year.IsEnabled = true;
            this.DataGrid_Main.IsEnabled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Wpf.Data.Database.Disconnect();
            Properties.Settings.Default.登陆用户名 = "";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// DataGrid的Cell编辑后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_Main_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text.Trim();
            if (!preValue.Equals(newValue))
            {
                string key = Wpf.Data.DataDef.dict.FirstOrDefault(x => x.Value == e.Column.Header.ToString()).Key;
                Wpf.Model.Model_Report data = (Wpf.Model.Model_Report)(sender as DataGrid).SelectedItems[0];//这货拿的是以前的数据，只是为了拿dbid
                if (data.Dbid != 0) //update
                {
                    if (key == "day")//如果是日，则格式化成完整时间
                    {
                        if (!Wpf.Helper.Date.IsStringOfDay(newValue))//不是两位数，return
                        {
                            return;
                        }
                        else if (int.Parse(newValue) == 0 || int.Parse(newValue) > 31)//是数字但不在日期范围，return
                        {
                            return;
                        }
                        newValue = Wpf.Helper.Date.Format(Properties.Settings.Default.下拉框_年 + "-" + Properties.Settings.Default.下拉框_月 + "-" + newValue);
                        key = "datetime";
                    }
                    else if (key == "income" || key == "expenses")
                    {
                        if (!Wpf.Helper.Date.IsStringOfDouble(newValue))
                        {
                            return;
                        }
                    }
                    string sql = "update main.T_Report set " + key + "='" + newValue + "' where id=" + data.Dbid;
                    Database.Update(sql);
                    //RefreshDisplayData("WithoutDataGrid");//不刷新datagrid，其他的刷新也不会发生变化
                }
                else //insert
                {
                    if (key == "day" && Wpf.Helper.Date.IsStringOfDay(newValue))//如果是日，且输入的字符串是纯数字
                    {
                        newValue = Wpf.Helper.Date.Format(Properties.Settings.Default.下拉框_年 + "-" + Properties.Settings.Default.下拉框_月 + "-" + newValue);
                        string sql = "insert into main.T_Report(datetime,Type) values('" + newValue + "'," + Properties.Settings.Default.下拉框_户型 + ")";
                        Database.Insert(sql);
                        RefreshDisplayData("All");
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        private void DataGrid_Main_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
        }

        private void Button_Excel_Click(object sender, RoutedEventArgs e)
        {
            int type = Properties.Settings.Default.下拉框_户型;
            new Wpf.ExcelPlus.ExcelExport().Export(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, type);
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if(isInit)
            {
                string year = this.ComboBox_Year.SelectedValue.ToString();
                if(year.Equals("全部"))
                {
                    this.DataGrid_Main.CanUserAddRows = false;
                    this.ComboBox_Month.SelectedIndex = 0;
                    this.ComboBox_Month.IsEnabled = false;
                    Properties.Settings.Default.下拉框_年 = 0;
                }
                else
                {
                    this.DataGrid_Main.CanUserAddRows = true;
                    this.ComboBox_Month.IsEnabled = true;
                    Properties.Settings.Default.下拉框_年 = int.Parse(year);
                }
                RefreshDisplayData("All");
                //new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                string month = this.ComboBox_Month.SelectedValue.ToString();
                if (!month.Equals("全部"))
                {
                    if (Properties.Settings.Default.下拉框_年 != 0)
                    {
                        this.DataGrid_Main.CanUserAddRows = true;
                    }
                    Properties.Settings.Default.下拉框_月 = int.Parse(month);
                }
                else
                {
                    this.DataGrid_Main.CanUserAddRows = false;
                    Properties.Settings.Default.下拉框_月 = 0;
                }
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);//结余
                RefreshDisplayData("All");
            }
        }

        private void Button_刷新_Click(object sender, RoutedEventArgs e)
        {
            RefreshDisplayData("All");
        }

        private void Button_删除_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "确认删除数据？";
            string caption = "注意";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    return;
            }

            string sql;
            Wpf.Model.Model_Report data = new Model.Model_Report();//= (Wpf.Model.Model_Report)this.DataGrid_Main.SelectedItems[0];
            for (int i = 0; i < this.DataGrid_Main.SelectedItems.Count; i++ )
            {
                try
                {
                    data = (Wpf.Model.Model_Report)this.DataGrid_Main.SelectedItems[i];
                }
                catch(Exception)
                {
                    //选中空白行这里会报错
                }
                if (data.Dbid == 0)
                {
                    return;
                }
                //sql = "DELETE FROM T_Report where id="+data.Dbid;
                //Wpf.Data.Database.Delete(sql);
                sql = "UPDATE T_Report set DeleteTime='" + Wpf.Helper.Date.FormatNow() + "' WHERE id=" + data.Dbid;
                Wpf.Data.Database.Update(sql);
            }
            RefreshDisplayData("All");
        }

        private void Button_导入Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Title = "请选择要导入的Excel文件";
            open.Filter = "Office Excel 2003文档|*.xls";
             if ((bool)open.ShowDialog().GetValueOrDefault())
             {
                 //new Wpf.Helper.Log().SaveLog("Button_导入Excel_Click: open file: " + open.FileName);
                 new Wpf.ExcelPlus.ExcelImport().Import(open.FileName);
             }
             RefreshDisplayData("All");
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.下拉框_户型 = this.ComboBox_Type.SelectedIndex + 1;
            RefreshDisplayData("All");
        }

        private void Button_登陆_登陆_Click(object sender, RoutedEventArgs e)
        {
            isInLoginWindow = false;
            string username = this.TextBox_登陆_用户名.Text;
            if (username == "root")//超级用户登录
            {
                if (Wpf.Helper.Secure.CheckUserNameAndPassword(username, this.PasswordBox_登陆_密码.SecurePassword))
                {
                    this.Button_拷贝无密码数据库.Visibility = System.Windows.Visibility.Visible;
                    EnableTools();
                }
                else
                {
                    this.TextBlock_密码错误提示.Text = "账号或密码错误，请重试。";
                    isInLoginWindow = true;
                }
            }
            else
            {
                if (Wpf.Helper.Secure.CheckUserNameAndPassword(username, this.PasswordBox_登陆_密码.SecurePassword))
                {
                    this.Grid_遮盖.Visibility = System.Windows.Visibility.Collapsed;
                    this.PasswordBox_登陆_密码.Clear();
                    Properties.Settings.Default.登陆用户名 = username;
                    EnableTools();
                }
                else
                {
                    this.TextBlock_密码错误提示.Text = "账号或密码错误，请重试。";
                    isInLoginWindow = true;
                }
            }
        }

        private void MenuItem_设置_Click(object sender, RoutedEventArgs e)
        {
            new Wpf.Win.SettingsWindow().ShowDialog();
        }

        private void Button_拷贝无密码数据库_Click(object sender, RoutedEventArgs e)
        {
            Wpf.Data.Database.ClearPassword();
            File.Copy("Data\\Data.db","Data\\DataWithoutPassword.db",true);
            Wpf.Data.Database.ChangePassword(Properties.Settings.Default.注册码 + "PowerByStoneAnt");
            this.TextBlock_密码错误提示.Text = "拷贝无密码数据库成功。";
        }

    }
}
