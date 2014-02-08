using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Wpf.Model
{
    public class Model_Test : INotifyPropertyChanged
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }
        private string datatime;

        public string 日期
        {
            get { return datatime; }
            set 
            { 
                datatime = value;
                //NotifyPropertyChanged(datatime);
            }
        }
        private string unitsname;

        public string 单位名称
        {
            get { return unitsname; }
            set 
            { 
                unitsname = value;
                //NotifyPropertyChanged(unitsname);
            }
        }
        private string used;

        public string 用途
        {
            get { return used; }
            set { used = value; }
        }
        private double income;

        public double 收入
        {
            get { return income; }
            set { income = value; }
        }
        private double expenses;

        public double 支出
        {
            get { return expenses; }
            set { expenses = value; }
        }

        private double CashSurplus;

        public double 结余
        {
            get { return CashSurplus; }
            set { CashSurplus = value; }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        /// <summary>
        /// cell内容改变事件
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Console.WriteLine("NotifyPropertyChanged : " + propertyName);
            }
        }

        #endregion
    }
}
