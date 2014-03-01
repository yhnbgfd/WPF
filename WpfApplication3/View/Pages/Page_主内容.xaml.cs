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

namespace Wpf.View.Pages
{
    public partial class Page_主内容 : Page
    {
        private int type = 0;
        /// <summary>
        /// DataGrid Cell编辑前的值
        /// </summary>
        private string preValue;

        public Page_主内容(int type)
        {
            InitializeComponent();
            this.type = type;
            InitializeToolBox();
            Wpf.View.Pages.Page_添加数据 page = new Wpf.View.Pages.Page_添加数据(type);
            page.ClosePage += new TestEventHandle(CloseGrid);
            this.Frame_弹出_添加数据.Content = page;
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
            //保存程序目录到settings
            Properties.Settings.Default.Path = AppDomain.CurrentDomain.BaseDirectory;
            //保存当前日期到settings下拉框日期
            Properties.Settings.Default.下拉框_年 = Wpf.Helper.Date.GetYear();
            Properties.Settings.Default.下拉框_月 = Wpf.Helper.Date.GetMonth();
            //combobox
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
            this.DataGrid_Main.ItemsSource = null;
            this.DataGrid_Main.ItemsSource = new Wpf.ViewModel.ViewModel_Report()
                .Report(type, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            //}

            //更新“承上月结余”
            this.TextBox_承上月结余.Text = new Wpf.ViewModel.ViewModel_Report()
                .GetSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, type).ToString();

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

            this.GroupBox_通知.Opacity = 100;
            this.TextBlock_通知信息.Text = "数据已保存。"+DateTime.Now.ToString();
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (isInit)
            {
                string year = this.ComboBox_Year.SelectedValue.ToString();
                if (year.Equals("全部"))
                {
                    this.ComboBox_Month.SelectedIndex = 0;
                    this.ComboBox_Month.IsEnabled = false;
                    Properties.Settings.Default.下拉框_年 = 0;
                }
                else
                {
                    this.ComboBox_Month.IsEnabled = true;
                    Properties.Settings.Default.下拉框_年 = int.Parse(year);
                }
                RefreshDisplayData("All");
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            }
        }

        private void ComboBox_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (isInit)
            {
                string month = this.ComboBox_Month.SelectedValue.ToString();
                if (!month.Equals("全部"))
                {
                    Properties.Settings.Default.下拉框_月 = int.Parse(month);
                    this.DataGrid_Main.Columns[3].IsReadOnly = false;
                }
                else
                {
                    Properties.Settings.Default.下拉框_月 = 0;
                    this.DataGrid_Main.Columns[3].IsReadOnly = true;
                }
                new Wpf.ViewModel.ViewModel_Report().CheckSurplus(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);//结余
                RefreshDisplayData("All");
            }
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
            new Wpf.ExcelPlus.ExcelExport().Export(Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月, type);
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
                    //Wpf.Data.Database.Update(sql);
                    Wpf.Data.Database.doDML(sql, "Update","修改数据");
                    RefreshDisplayData("All");//刷新导致tab键失效
                }
                //else //insert
                //{
                //    if (key == "day" && Wpf.Helper.Date.IsStringOfDay(newValue))//如果是日，且输入的字符串是纯数字
                //    {
                //        newValue = Wpf.Helper.Date.Format(Properties.Settings.Default.下拉框_年 + "-" + Properties.Settings.Default.下拉框_月 + "-" + newValue);
                //        string sql = "insert into main.T_Report(datetime,Type) values('" + newValue + "'," + type + ")";
                //        Wpf.Data.Database.Insert(sql);
                //        RefreshDisplayData("All");
                //    }
                //    else
                //    {
                //        return;
                //    }
                //}
            }
        }
    }
}
