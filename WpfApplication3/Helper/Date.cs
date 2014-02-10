using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Helper
{
    public class Date
    {
        public void GetDate()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }

        public int GetYear()
        {
            return DateTime.Now.Year;
        }
    }
}
