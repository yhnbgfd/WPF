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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string preValue;

        public MainWindow()
        {
            InitializeComponent();
            Properties.Settings.Default.DataGrid = this.DataGrid_Main;
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report();
        }

        public void UpdateDataset()
        {
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report();
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
            string header = e.Column.Header.ToString();
            string key = Wpf.Data.DataDef.dict.FirstOrDefault(x => x.Value == header).Key;
            string newValue = (e.EditingElement as TextBox).Text;
            if (preValue != newValue)
            {
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
                    string sql = "insert into main.T_Report(" + key + ") values('" + newValue + "')";
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
    }
}
