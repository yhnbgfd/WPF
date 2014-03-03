using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Helper
{
    static class DebugOnly
    {
        public static void Output(string str)
        {
#if DEBUG
            Console.WriteLine(str);
#endif
        }
    }
}
