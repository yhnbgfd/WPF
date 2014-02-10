using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Helper
{
    public class Date
    {
        public int GetYear()
        {
            return DateTime.Now.Year;
        }

        public int GetMonth()
        {
            return DateTime.Now.Month;
        }

        public int GetDate()
        {
            return 0;
        }
    }
}
