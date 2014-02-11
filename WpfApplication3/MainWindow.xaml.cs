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

namespace Wpf
{
    public partial class MainWindow : Window
    {
        string preValue;
        bool isInit = false;
        int cb_Year = 0;
        int cb_Month = 0;

        public MainWindow()
        {
            InitializeComponent();
            Properties.Settings.Default.Path = AppDomain.CurrentDomain.BaseDirectory;
            new Wpf.Helper.Log().SaveLog("Window initialize successed. @ " + AppDomain.CurrentDomain.BaseDirectory);
            ComboBoxInit();
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex, cb_Year, cb_Month);
            this.DataGrid_Main.CanUserAddRows = false;
            isInit = true;
        }

        private void ComboBoxInit()
        {
            int preYear = 3;//往前几年
            int afterYear = 5;//往后几年
            
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
            this.ComboBox_Year.SelectedIndex = 0;// preYear + 1;
            //cb_Year = new Wpf.Helper.Date().GetYear();

            this.ComboBox_Month.ItemsSource = Wpf.Data.DataDef.Month;
            this.ComboBox_Month.SelectedIndex = 0;//new Wpf.Helper.Date().GetMonth();

        }

        public void UpdateDataset()
        {
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex, cb_Year, cb_Month);
            DataGrid_Main_Loaded(null, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            new Wpf.Helper.Log().SaveLog("Window Closed.");
        }

        private void DataGrid_Main_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataGrid_Main.Columns[0].Visibility = Visibility.Collapsed;
                this.DataGrid_Main.Columns[1].IsReadOnly = true;
                this.DataGrid_Main.Columns[7].IsReadOnly = true;
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
                if (key == "datetime")//格式化日期保存
                {
                    newValue = new Wpf.Helper.Date().Format(newValue);
                    if (newValue == "Exception")
                    {
                        UpdateDataset();
                        return;
                    }
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
                    if (this.ComboBox_Type.SelectedIndex == 0)
                    {
                        new Wpf.Helper.Log().SaveLog("CellEditEnding ComboBox_Type.SelectedIndex == 0");
                        MessageBox.Show("请先选择用户类型。", "");
                        return;
                    }
                    string sql = "insert into main.T_Report(" + key + ",Type) values('" + newValue + "'," + this.ComboBox_Type.SelectedIndex + ")";
                    new Database().Insert(sql);
                    UpdateDataset();
                }
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

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                UpdateDataset();
                if (this.ComboBox_Type.SelectedIndex==0)
                {
                    this.DataGrid_Main.CanUserAddRows = false;
                }
                else
                {
                    this.DataGrid_Main.CanUserAddRows = true;
                }
            }
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                if (this.ComboBox_Year.SelectedValue.ToString() == "全部")
                {
                    cb_Year = 0;
                }
                else
                {
                    try
                    {
                        cb_Year = (int)this.ComboBox_Year.SelectedValue;
                    }
                    catch(Exception)
                    {
                        return;
                    }
                }
                UpdateDataset();
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                if (this.ComboBox_Month.SelectedValue.ToString() == "全年")
                {
                    cb_Month = 0;
                }
                else
                {
                    cb_Month = (int)this.ComboBox_Month.SelectedValue;
                }
                UpdateDataset();
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
        }

    }
}
