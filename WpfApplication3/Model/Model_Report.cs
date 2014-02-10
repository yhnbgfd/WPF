using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Wpf.Data;

namespace Wpf.Model
{
    public class Model_Report : INotifyPropertyChanged
    {
        private long dbid;

        public long Dbid
        {
            get { return dbid; }
            set 
            { 
                dbid = value; 
            }
        }
        
        private long id;

        public long 序号
        {
            get { return id; }
            set 
            { 
                id = value;
            }
        }
        private string datetime;

        public string 日期
        {
            get { return datetime; }
            set 
            { 
                datetime = value;
                //RowChange(this);
            }
        }
        private string unitsname;

        public string 单位名称
        {
            get { return unitsname; }
            set 
            { 
                unitsname = value;
                //RowChange(this);
            }
        }
        private string use;

        public string 用途
        {
            get { return use; }
            set 
            { 
                use = value;
                //RowChange(this);
            }
        }
        private double income;

        public double 收入
        {
            get { return income; }
            set 
            { 
                income = value;
                //RowChange(this);
            }
        }
        private double expenses;

        public double 支出
        {
            get { return expenses; }
            set 
            { 
                expenses = value;
                //RowChange(this);
            }
        }
        private double surplus;

        public double 结余
        {
            get { return surplus; }
            set 
            { 
                surplus = value; 
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                //Console.WriteLine("NotifyPropertyChanged : " + propertyName);
            }
        }

        #endregion
    }
}
