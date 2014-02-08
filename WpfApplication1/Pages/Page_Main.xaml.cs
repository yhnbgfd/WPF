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

namespace Wpf.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class Page_Main : Page
    {
        public Page_Main()
        {
            InitializeComponent();
            this.DataGrid_Content1.ItemsSource = new Wpf.ViewModel.ViewModel_Test().ViewModelTest();
        }

        private void DataGrid_Content1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Wpf.Model.Model_Test info = new Wpf.Model.Model_Test();   //我自己的数据表实例类   
            info = e.Row.Item as Wpf.Model.Model_Test;        //获取该行的记录   
            string sql = "INSERT INTO content(id,unitsname) VALUES(" + info.Id + ",'" + info.单位名称 + "')";
            new Wpf.Database.DML().Insert(sql);
        }

        private void DataGrid_Content1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }
    }
}
