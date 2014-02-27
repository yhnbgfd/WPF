using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Model
{
    public class Model_AddData
    {
        private string DateTime;

        public string 时间
        {
            get { return DateTime; }
            set { DateTime = value; }
        }
        private string UserName;

        public string 单位名称
        {
            get { return UserName; }
            set { UserName = value; }
        }
        private string Use;

        public string 用途
        {
            get { return Use; }
            set { Use = value; }
        }
        private decimal Income;

        public decimal 借方发生额
        {
            get { return Income; }
            set { Income = value; }
        }
        private decimal Expenses;

        public decimal 贷方发生额
        {
            get { return Expenses; }
            set { Expenses = value; }
        }
    }
}
