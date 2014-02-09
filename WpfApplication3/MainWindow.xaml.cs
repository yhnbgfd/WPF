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
using Excel = Microsoft.Office.Interop.Excel;
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

            UpdateDataset();
        }

        public void UpdateDataset()
        {
            Properties.Settings.Default.DataGrid.ItemsSource = new Wpf.ViewModel.ViewModel_Report().Report();
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
            if (preValue != newValue)
            {
                DataGrid grid = sender as DataGrid;
                Wpf.Model.Model_Report data = (Wpf.Model.Model_Report)grid.SelectedItems[0];//这货拿的是以前的数据
                //if (new Wpf.Data.DBExtend().CheckDataIsExist(data.Dbid))
                if (data.Dbid == 0)
                {
                    Console.WriteLine("update " + data.Dbid);
                    //new Database().Update(new DBExtend().GenerateUpdateSQL(data));
                    UpdateDataset();
                }
                else
                {
                    Console.WriteLine("insert " + data.Dbid);
                    //new Database().Insert(new DBExtend().GenerateInsertSQL(data));
                    UpdateDataset();
                }
            }
        }

        private void DataGrid_Main_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //Console.WriteLine("DataGrid_Main_RowEditEnding");
        }

        private void DataGrid_Main_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
        }
    }
}
