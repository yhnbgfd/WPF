using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Wpf.Data;

namespace Wpf.Model
{
    public class Model_Report
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
        private double income;

        public double 借方发生额
        {
            get { return income; }
            set 
            { 
                income = value;
            }
        }
        private double expenses;

        public double 贷方发生额
        {
            get { return expenses; }
            set 
            { 
                expenses = value;
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
    }
}
