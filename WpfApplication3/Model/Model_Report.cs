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
        
        private int id;

        public int 序号
        {
            get { return id; }
            set 
            { 
                id = value;
            }
        }
        /*
        private string datetime;

        public string 日期
        {
            get { return datetime; }
            set 
            { 
                datetime = value;
            }
        }*/
        private int month;

        public int 月
        {
            get { return month; }
            set { month = value; }
        }

        private int day;

        public int 日
        {
            get { return day; }
            set { day = value; }
        }

        private string unitsname;

        public string 单位名称
        {
            get { return unitsname; }
            set 
            { 
                unitsname = value;
            }
        }
        private string use;

        public string 用途
        {
            get { return use; }
            set 
            { 
                use = value;
            }
        }
        private decimal income;

        public decimal 借方发生额
        {
            get { return income; }
            set 
            { 
                income = value;
            }
        }
        private decimal expenses;

        public decimal 贷方发生额
        {
            get { return expenses; }
            set 
            { 
                expenses = value;
            }
        }
        private decimal surplus;

        public decimal 结余
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

        /// <summary>
        /// cell内容改变事件
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
