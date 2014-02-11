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
        int cb_Year = 2014;
        int cb_Month = 0;

        public MainWindow()
        {
            InitializeComponent();
            ComboBoxInit();
            Properties.Settings.Default.DataGrid = this.DataGrid_Main;
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex);
            isInit = true;
        }

        private void ComboBoxInit()
        {
            int nowYear = new Wpf.Helper.Date().GetYear();
            List<int> YearSource = new List<int>();
            for (int i = nowYear - 10; i < nowYear + 10; i++)
            {
                YearSource.Add(i);
            }
            this.ComboBox_Type.ItemsSource = Wpf.Data.DataDef.CustomerType;
            this.ComboBox_Type.SelectedIndex = 0;
            this.DataGrid_Main.CanUserAddRows = false;

            this.ComboBox_Year.ItemsSource = YearSource;
            this.ComboBox_Year.SelectedIndex = 10;

            this.ComboBox_Month.ItemsSource = Wpf.Data.DataDef.Month;
            this.ComboBox_Month.SelectedIndex = new Wpf.Helper.Date().GetMonth();
        }

        public void UpdateDataset()
        {
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex);
            DataGrid_Main_Loaded(null, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void DataGrid_Main_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataGrid_Main.Columns[0].Visibility = Visibility.Collapsed;
            this.DataGrid_Main.Columns[1].IsReadOnly = true;
            this.DataGrid_Main.Columns[7].IsReadOnly = true;
        }

        private void DataGrid_Main_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text;
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
                }
                else //insert
                {
                    string sql = "insert into main.T_Report(" + key + ",Type) values('" + newValue + "'," + this.ComboBox_Type.SelectedIndex + ")";
                    new Database().Insert(sql);
                }
            }
            else
            {
                e.Cancel = true;
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

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report((int)this.ComboBox_Year.SelectedValue, 0);
            UpdateDataset();
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(this.ComboBox_Type.SelectedIndex);
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
                cb_Year = int.Parse(this.ComboBox_Year.Text);
                Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(cb_Year, cb_Month);
                UpdateDataset();
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isInit)
            {
                cb_Month = int.Parse(this.ComboBox_Month.Text);
                Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report(cb_Year, cb_Month);
                UpdateDataset();
            }
        }
    }
}
