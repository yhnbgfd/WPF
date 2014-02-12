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
        string preValue;
        bool isInit = false;
        int cb_Year = 0;
        int cb_Month = 0;

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
            this.Grid_遮盖.Visibility = System.Windows.Visibility.Visible;
            Properties.Settings.Default.Path = AppDomain.CurrentDomain.BaseDirectory;
            new Wpf.Helper.Log().SaveLog("Window initialize successed. @ " + AppDomain.CurrentDomain.BaseDirectory);
            ComboBoxInit();
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex+1, cb_Year, cb_Month);
            Set累计();
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report().GetSurplus(cb_Year, cb_Month, this.ComboBox_Type.SelectedIndex + 1).ToString();
            isInit = true;
        }

        private void ComboBoxInit()
        {
            int preYear = 3;//往前几年
            int afterYear = 5;//往后几年
            cb_Year = new Wpf.Helper.Date().GetYear();
            cb_Month = new Wpf.Helper.Date().GetMonth();

            int nowYear = new Wpf.Helper.Date().GetYear();
            List<object> YearSource = new List<object>();
            YearSource.Add("全部");
            for (int i = nowYear - preYear; i < nowYear + afterYear; i++)
            {
                YearSource.Add(i);
            }
            this.ComboBox_Type.ItemsSource = Wpf.Data.DataDef.CustomerType;
            this.ComboBox_Type.SelectedIndex = 0;

            this.ComboBox_Year.ItemsSource = YearSource;
            this.ComboBox_Year.SelectedIndex = preYear+1;

            this.ComboBox_Month.ItemsSource = Wpf.Data.DataDef.Month;
            this.ComboBox_Month.SelectedIndex = new Wpf.Helper.Date().GetMonth();
            
            

        }

        public void UpdateDataset()
        {
            try
            {
                List<Model.Model_Report> data = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex + 1, cb_Year, cb_Month);
                this.DataGrid_Main.ItemsSource = data;
                Set_DataGrid_Column_Type();
            }
            catch(Exception ee)
            {
                new Wpf.Helper.Log().SaveLog("ERROR : UpdateDataset" + ee);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            new Wpf.Helper.Log().SaveLog("Window Closed.");
        }

        private void Set_DataGrid_Column_Type()
        {
            try
            {
                this.DataGrid_Main.Columns[0].Visibility = Visibility.Collapsed;
                this.DataGrid_Main.Columns[1].IsReadOnly = true;//序号
                this.DataGrid_Main.Columns[2].IsReadOnly = true;//月
                this.DataGrid_Main.Columns[8].IsReadOnly = true;//结余
            }
            catch (Exception ee)
            {
                new Wpf.Helper.Log().SaveLog(ee.ToString());
            }
        }

        private void DataGrid_Main_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text.Trim();
            if (!preValue.Equals(newValue))
            {
                string header = e.Column.Header.ToString();
                string key = Wpf.Data.DataDef.dict.FirstOrDefault(x => x.Value == header).Key;
                if (key == "month")//格式化日期保存
                {
                    newValue = new Wpf.Helper.Date().Format(this.ComboBox_Year.Text+"-"+newValue+"-01");
                    key = "datetime";
                }
                else if(key == "day")
                {
                    newValue = new Wpf.Helper.Date().Format(this.ComboBox_Year.Text + "-" + this.ComboBox_Month.Text + "-"+newValue);
                    key = "datetime";
                }
                DataGrid grid = sender as DataGrid;
                Wpf.Model.Model_Report data = (Wpf.Model.Model_Report)grid.SelectedItems[0];//这货拿的是以前的数据
                
                if (data.Dbid != 0) //update
                {
                    string sql = "update main.T_Report set " + key + "='" + newValue + "' where id=" + data.Dbid;
                    new Database().Update(sql);
                    UpdateDataset();
                }
                else //insert
                {
                    if(key != "datetime")
                    {
                        return;
                    }
                    string sql = "insert into main.T_Report(" + key + ",Type) values('" + newValue + "'," + (this.ComboBox_Type.SelectedIndex + 1) + ")";
                    new Database().Insert(sql);
                    UpdateDataset();
                }
                Set累计();
                new Wpf.Data.Statistics().UpdateSurplus(cb_Year,cb_Month,this.ComboBox_Type.SelectedIndex+1);
            }
        }

        private void DataGrid_Main_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
        }

        private void Button_Excel_Click(object sender, RoutedEventArgs e)
        {
            new Wpf.ExcelPlus.ExcelInit();
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                if (this.ComboBox_Year.SelectedIndex == 0)
                {
                    cb_Year = 0;
                }
                else
                {
                    cb_Year = (int)this.ComboBox_Year.SelectedValue;
                }
                UpdateDataset();
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(cb_Year, cb_Month);
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                if (this.ComboBox_Month.SelectedIndex == 0)//全部
                {
                    cb_Month = 0;
                }
                else
                {
                    cb_Month = (int)this.ComboBox_Month.SelectedValue;
                }
                this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report().GetSurplus(cb_Year, cb_Month, this.ComboBox_Type.SelectedIndex + 1).ToString();
                UpdateDataset();
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(cb_Year, cb_Month);//结余
                Set累计();
            }
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataset();
        }

        private void Button_刷新_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataset();
        }

        private void MenuItem_退出_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_设置_Click(object sender, RoutedEventArgs e)
        {

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
                new Wpf.Data.Database().Delete(sql);
            }
            UpdateDataset();
            Set累计();
        }

        private void Button_导入Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Title = "请选择要导入的Excel文件";
            open.Filter = "office excel 2003文档|*.xls";
             if ((bool)open.ShowDialog().GetValueOrDefault())
             {
                 new Wpf.Helper.Log().SaveLog("Button_导入Excel_Click: open file: " + open.FileName);
                 new Wpf.ExcelPlus.Test().excelTest(open.FileName);
             }
        }

        private void Button_修改结余_Click(object sender, RoutedEventArgs e)
        {
            string value = this.TextBox_承上月结余.Text.Trim();
            string sql = "UPDATE T_Surplus set surplus="+int.Parse(value)+" where year="+cb_Year+" and month="+cb_Month+" and type="+this.ComboBox_Type.SelectedIndex+1;
            new Database().Update(sql);
            UpdateDataset();
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Wpf.Data.DataDef.isDBconnect)
            {
                return;
            }
            UpdateDataset();
            Set累计();
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report().GetSurplus(cb_Year, cb_Month, this.ComboBox_Type.SelectedIndex + 1).ToString();
        }

        /// <summary>
        /// 设置底部 借方发生额累计 & 贷方发生额累计
        /// </summary>
        private void Set累计()
        {
            this.TextBlock_借方发生额累计_月.Text = Properties.Settings.Default.借方发生额累计.ToString();
            this.TextBlock_贷方发生额累计_月.Text = Properties.Settings.Default.贷方发生额累计.ToString();
            this.TextBlock_借方发生额累计.Text = new Wpf.Data.Database().Count借方发生额累计(this.ComboBox_Type.SelectedIndex + 1,cb_Year,cb_Month).ToString();
            this.TextBlock_贷方发生额累计.Text = new Wpf.Data.Database().Count贷方发生额累计(this.ComboBox_Type.SelectedIndex + 1,cb_Year,cb_Month).ToString();
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

        private void DataGrid_Main_Loaded(object sender, RoutedEventArgs e)
        {
            Set_DataGrid_Column_Type();
        }

    }
}
