﻿using System;
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
using System.Data;

namespace Wpf.View.Pages
{
    public delegate void TestEventHandle(bool commit);

    public partial class Page_添加数据 : Page
    {
        private int type = 0;
        public event TestEventHandle ClosePage;
        private DataSet ds = new DataSet();

        public Page_添加数据(int type)
        {
            InitializeComponent();
            this.type = type;
            InitializeDataGrid();
        }
        /// <summary>
        /// 初始化DataGrid
        /// </summary>
        private void InitializeDataGrid()
        {
            this.DataGrid_添加数据.ItemsSource = new Wpf.ViewModel.ViewModel_AddData().InitData();
        }

        public void OnClosePage()
        {
            if (ClosePage != null)
            {
                ClosePage(false);
            }
        }
        public void OnCommitPage()
        {
            if (ClosePage != null)
            {
                ClosePage(true);
            }
        }

        private void Button_添加数据_取消_Click(object sender, RoutedEventArgs e)
        {
            OnClosePage();
        }

        private void Button_添加数据_保存_Click(object sender, RoutedEventArgs e)
        {
            //List<Wpf.Model.Model_AddData> ds = DataGrid_添加数据.ItemsSource as List<Wpf.Model.Model_AddData>;
            //for (int i = 0; i < ds.Count; i++)
            //{
            //    Console.WriteLine(ds[i].时间);
            //}
            if (new Wpf.ViewModel.ViewModel_AddData().InsertData(DataGrid_添加数据.ItemsSource as List<Wpf.Model.Model_AddData>, type))
            {
                DataGrid_添加数据.ItemsSource = new Wpf.ViewModel.ViewModel_AddData().InitData();
                OnCommitPage();
            }
            else
            {
                Console.WriteLine("InsertData False");
            }
        }

        private void DatePickerInDataGrid_CalendarClosed(object sender, RoutedEventArgs e)
        {
            this.DataGrid_添加数据.BeginEdit();//解决DatePicker不触发CellEditEnding的bug
        }

    }
}
