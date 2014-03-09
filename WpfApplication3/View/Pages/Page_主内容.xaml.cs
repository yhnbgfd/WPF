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
using System.Threading;
using Wpf.Helper;

namespace Wpf.View.Pages
{
    public partial class Page_主内容 : Page
    {
        private int type = 0;
        private int cb_year = Wpf.Helper.Date.GetYear();
        private int cb_month = Wpf.Helper.Date.GetMonth();
        /// <summary>
        /// DataGrid Cell编辑前的值
        /// </summary>
        private string preValue;

        public Page_主内容(int type)
        {
            InitializeComponent();
            Properties.Settings.Default.MainWindow.ReflashData += new ReflashDataEventHandle(thisReflashData);
            this.type = type;
            InitializeToolBox();
            Wpf.View.Pages.Page_添加数据 page = new Wpf.View.Pages.Page_添加数据(type);
            page.ClosePage += new TestEventHandle(CloseGrid);
            this.Frame_弹出_添加数据.Content = page;

            if(Wpf.Helper.Secure.CheckLicense() < 0 && Wpf.Data.Database.SelectString("select value from T_Type where key=999") == "false")
            {
                this.Button_添加.IsEnabled = false;
            }
        }

        private void thisReflashData(object sender, EventArgs e)
        {
            RefreshDisplayData("All");
        }

        private void CloseGrid(int count)
        {
            this.Grid_弹出_添加数据.Visibility = System.Windows.Visibility.Collapsed;
            if (count > 0)
            {
                RefreshDisplayData("All");
                this.TextBlock_通知信息.Text = "成功录入" + count + "条数据。" + DateTime.Now.ToString();
            }
        }

        private void InitializeToolBox()
        {
            //combobox
            this.ComboBox_Year.ItemsSource = Wpf.Data.DataDef.Year;//这句
            this.ComboBox_Year.SelectedIndex = Wpf.Data.DataDef.perYear + 1;
            this.ComboBox_Month.ItemsSource = Wpf.Data.DataDef.Month;//还有这句，导致程序打开很慢
            this.ComboBox_Month.SelectedIndex = Wpf.Helper.Date.GetMonth();
        }

        /// <summary>
        /// 更新页面上所有可见数据集合到这里
        /// </summary>
        private void RefreshDisplayData(string message)
        {
            this.DataGrid_Main.ItemsSource = null;
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report()
                .Report(type, cb_year, cb_month);

            //更新“承上月结余”
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report()
                .GetSurplus(cb_year, cb_month, type).ToString();

            //更新下方4个累计textblock
            this.TextBlock_借方发生额合计.Text = new Wpf.ViewModel.ViewModel_Surplus().Count借贷方发生额合计("income", type, cb_year, cb_month).ToString();
            this.TextBlock_贷方发生额合计.Text = new Wpf.ViewModel.ViewModel_Surplus().Count借贷方发生额合计("expenses", type, cb_year, cb_month).ToString();
            this.TextBlock_借方发生额累计.Text = new Wpf.ViewModel.ViewModel_Surplus().Count借贷方发生额累计("income", type, cb_year, cb_month).ToString();
            this.TextBlock_贷方发生额累计.Text = new Wpf.ViewModel.ViewModel_Surplus().Count借贷方发生额累计("expenses", type, cb_year, cb_month).ToString();
            //设置颜色
            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush green = new SolidColorBrush(Colors.Green);
            this.TextBlock_借方发生额合计.Foreground = red;
            this.TextBlock_贷方发生额合计.Foreground = green;
            this.TextBlock_借方发生额累计.Foreground = red;
            this.TextBlock_贷方发生额累计.Foreground = green;

            this.GroupBox_通知.Opacity = 100;
            this.TextBlock_通知信息.Text = "数据已保存。"+DateTime.Now.ToString();
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string year = this.ComboBox_Year.SelectedValue.ToString();
            if (year.Equals("全部"))
            {
                this.ComboBox_Month.SelectedIndex = 0;
                this.ComboBox_Month.IsEnabled = false;
                cb_year = 0;
                this.Button_Excel.IsEnabled = false;
            }
            else
            {
                this.ComboBox_Month.IsEnabled = true;
                cb_year = int.Parse(year);
                this.Button_Excel.IsEnabled = true;
            }
            RefreshDisplayData("All");
            new Wpf.ViewModel.ViewModel_Report().CheckSurplus(cb_year, cb_month);
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string month = this.ComboBox_Month.SelectedValue.ToString();
            if (!month.Equals("全部"))
            {
                cb_month = int.Parse(month);
                this.DataGrid_Main.Columns[3].IsReadOnly = false;
            }
            else
            {
                cb_month = 0;
                this.DataGrid_Main.Columns[3].IsReadOnly = true;
            }
            new Wpf.ViewModel.ViewModel_Report().CheckSurplus(cb_year, cb_month);//结余
            RefreshDisplayData("All");
        }

        private void Button_添加_Click(object sender, RoutedEventArgs e)
        {
            this.Grid_弹出_添加数据.Visibility = System.Windows.Visibility.Visible;
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
            List<string> sqls = new List<string>();
            string sql;
            Wpf.Model.Model_Report data = new Model.Model_Report();//= (Wpf.Model.Model_Report)this.DataGrid_Main.SelectedItems[0];
            for (int i = 0; i < this.DataGrid_Main.SelectedItems.Count; i++)
            {
                try
                {
                    data = (Wpf.Model.Model_Report)this.DataGrid_Main.SelectedItems[i];
                }
                catch (Exception)
                {
                    //选中空白行这里会报错
                }
                if (data.Dbid == 0)
                {
                    return;
                }
                sql = "UPDATE T_Report set DeleteTime='" + Wpf.Helper.Date.FormatNow() + "' WHERE id=" + data.Dbid;
                sqls.Add(sql);
            }
            Wpf.Data.Database.doDMLs(sqls,"Update","DeleteReport");
            RefreshDisplayData("All");
        }

        private void Button_刷新_Click(object sender, RoutedEventArgs e)
        {
            RefreshDisplayData("All");
        }

        private void Button_Excel_Click(object sender, RoutedEventArgs e)
        {
            int type = this.type;
            new Wpf.ExcelPlus.ExcelExport().Export(cb_year, cb_month, type);
        }

        private void Button_导入Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Title = "请选择要导入的Excel文件";
            open.Filter = "Office Excel 2003文档|*.xls";
            if ((bool)open.ShowDialog().GetValueOrDefault())
            {
                new Wpf.ExcelPlus.ExcelImport().Import(open.FileName);
            }
            RefreshDisplayData("All");
        }

        private void DataGrid_Main_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Wpf.Model.Model_Report data = (Wpf.Model.Model_Report)(sender as DataGrid).SelectedItems[0];//这货拿的是以前的数据，只是为了拿dbid
            if (e.Column.Header.ToString() == "借方发生额")
            {
                if(data.贷方发生额 != 0)
                {
                    MessageBoxResult result = MessageBox.Show("借方贷方数据不允许同时存在。");
                    e.Cancel = true;
                    return;
                }
            }
            else if (e.Column.Header.ToString() == "贷方发生额")
            {
                if (data.借方发生额 != 0)
                {
                    MessageBoxResult result = MessageBox.Show("借方贷方数据不允许同时存在。");
                    e.Cancel = true;
                    return;
                }
            }
            preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
        }

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
                        newValue = Wpf.Helper.Date.Format(cb_year + "-" + cb_month + "-" + newValue);
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
                    Wpf.Data.Database.doDML(sql, "Update","修改数据");
                    RefreshDisplayData("All");//刷新导致tab键失效
                }
            }
        }

        private void Button_刷新结余_Click(object sender, RoutedEventArgs e)
        {
            this.TextBlock_通知信息.Text = "结余数据刷新中，请稍后。";
            int year = this.ComboBox_Year.SelectedIndex;
            int month = this.ComboBox_Month.SelectedIndex;
            if (MessageBox.Show("刷新过程大约持续15-30秒钟，是否继续？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                for (int i = 0; i < this.ComboBox_Year.Items.Count; i++)
                {
                    this.ComboBox_Year.SelectedIndex = i;
                    for (int j = 0; j < this.ComboBox_Month.Items.Count; j++)
                    {
                        this.ComboBox_Month.SelectedIndex = j;
                    }
                }
                this.ComboBox_Year.SelectedIndex = year;
                this.ComboBox_Month.SelectedIndex = month;
                MessageBoxResult result = MessageBox.Show("结余数据刷新成功。");  
            }
            else
            {
                this.TextBlock_通知信息.Text = "";
                return;
            }
        }
    }
}
