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
        /// 重写回车=tab
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
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
            base.OnPreviewKeyDown(e);
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeToolBox();
            RefreshDisplayData();
            isInit = true;
        }

        /// <summary>
        /// 所有控件 & Settings如需初始化，则在这里面
        /// </summary>
        private void InitializeToolBox()
        {
            //保存程序目录到settings
            Properties.Settings.Default.Path = AppDomain.CurrentDomain.BaseDirectory;
            //保存当前日期到下拉框日期
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
        private void RefreshDisplayData()
        {
            //更新dataset
            try
            {
                this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex + 1, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            }
            catch (Exception ee)
            {
                new Wpf.Helper.Log().ErrorLog("ERROR : UpdateDataset" + ee);
            }

            //更新结余


            //更新“承上月结余”
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report().GetSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, this.ComboBox_Type.SelectedIndex + 1).ToString();

            //更新下方4个累计textblock
            Wpf.Data.Database.Count借方发生额累计(this.ComboBox_Type.SelectedIndex + 1, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            Wpf.Data.Database.Count贷方发生额累计(this.ComboBox_Type.SelectedIndex + 1, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            this.TextBlock_借方发生额合计.Text = Properties.Settings.Default.借方发生额合计.ToString();
            this.TextBlock_贷方发生额合计.Text = Properties.Settings.Default.贷方发生额合计.ToString();
            this.TextBlock_借方发生额累计.Text = Properties.Settings.Default.借方发生额累计.ToString();
            this.TextBlock_贷方发生额累计.Text = Properties.Settings.Default.贷方发生额累计.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Wpf.Data.Database.Disconnect();
            Properties.Settings.Default.Save();
            new Wpf.Helper.Log().SaveLog("Window Closed.");
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
                    if(key == "day" || key=="income" || key == "expenses")
                    {
                        if (key == "day" && Wpf.Helper.Date.IsStringOfDay(newValue))//如果是日，则格式化成完整时间
                        {
                            if(int.Parse(newValue) == 0 || int.Parse(newValue) > 31)
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
                        else
                        {
                            return;
                        }
                    }
                    string sql = "update main.T_Report set " + key + "='" + newValue + "' where id=" + data.Dbid;
                    Database.Update(sql);
                }
                else //insert
                {
                    if (key == "day" && Wpf.Helper.Date.IsStringOfDay(newValue))//如果是日，且输入的字符串是纯数字
                    {
                        newValue = Wpf.Helper.Date.Format(Properties.Settings.Default.下拉框_年 + "-" + Properties.Settings.Default.下拉框_月 + "-" + newValue);
                        string sql = "insert into main.T_Report(datetime,Type) values('" + newValue + "'," + (this.ComboBox_Type.SelectedIndex + 1) + ")";
                        Database.Insert(sql);
                    }
                }
                RefreshDisplayData();
                new Wpf.Data.Statistics().UpdateSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, this.ComboBox_Type.SelectedIndex + 1);
            }
        }
        private void DataGrid_Main_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
        }

        private void Button_Excel_Click(object sender, RoutedEventArgs e)
        {
            int type = this.ComboBox_Type.SelectedIndex + 1;
            new Wpf.ExcelPlus.ExcelExport().Export(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, type);
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                string year = this.ComboBox_Year.SelectedValue.ToString();
                if(!year.Equals("全部"))
                {
                    Properties.Settings.Default.下拉框_年 = int.Parse(year);
                }
                else
                {
                    Properties.Settings.Default.下拉框_年 = 0;
                }
                RefreshDisplayData();
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                string month = this.ComboBox_Month.SelectedValue.ToString();
                if (!month.Equals("全部"))
                {
                    Properties.Settings.Default.下拉框_月 = int.Parse(month);
                }
                else
                {
                    Properties.Settings.Default.下拉框_月 = 0;
                }
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);//结余
                RefreshDisplayData();
            }
        }

        private void Button_刷新_Click(object sender, RoutedEventArgs e)
        {
            RefreshDisplayData();
        }

        private void MenuItem_退出_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                catch(Exception ee )
                {
                    new Wpf.Helper.Log().SaveLog("Button_删除_Click: " + ee.ToString());
                    continue;
                }
                if(data.Dbid == 0)
                {
                    continue;
                }
                
                sql = "DELETE FROM T_Report where id="+data.Dbid;
                Wpf.Data.Database.Delete(sql);
            }
            RefreshDisplayData();
        }

        private void Button_导入Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Title = "请选择要导入的Excel文件";
            open.Filter = "office excel 2003文档|*.xls";
             if ((bool)open.ShowDialog().GetValueOrDefault())
             {
                 new Wpf.Helper.Log().SaveLog("Button_导入Excel_Click: open file: " + open.FileName);
                 new Wpf.ExcelPlus.ExcelImport().Import(open.FileName);
             }
             RefreshDisplayData();
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDisplayData();
        }

        private void MenuItem_登陆_Click(object sender, RoutedEventArgs e)
        {
            this.Popup_登陆.IsOpen = true;
        }

        private void Button_登陆_登陆_Click(object sender, RoutedEventArgs e)
        {
            string username = this.TextBox_登陆_用户名.Text;
            if (new Wpf.Helper.Secure().CheckUserNameAndPassword(username, this.PasswordBox_登陆_密码.SecurePassword))
            {
                this.Popup_登陆.IsOpen = false;
                this.Grid_遮盖.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.TextBlock_密码错误提示.Text = "账号或密码错误，请重试。";
            }
        }

        private void Button_登陆_取消_Click(object sender, RoutedEventArgs e)
        {
            this.Popup_登陆.IsOpen = false;
        }

    }
}
