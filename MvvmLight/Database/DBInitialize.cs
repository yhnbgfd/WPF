using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MvvmLight.Database
{
    class DBInitialize
    {
        string datasource = Properties.Settings.Default.DataSource;

        public void CreateDataFile()
        {
            SQLiteConnection.CreateFile(datasource);
            //dbconn.DisConnect();
        }
    }
}
