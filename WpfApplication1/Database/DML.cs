using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    public class DML
    {
        string sql = "";
        SQLiteCommand cmd = new SQLiteCommand();

        public DML()
        {
            cmd.Connection = new DBconnect().Connect();
        }

        /// <summary>
        /// Insert DML
        /// </summary>
        public void Insert(Wpf.Model.Model_Test insr)
        {
            sql = "INSERT INTO content(id,unitsname) VALUES("+insr.Id+",'"+insr.单位名称+"')";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update DML
        /// </summary>
        public void Update(Wpf.Model.Model_Test insr)
        {
            
        }

        /// <summary>
        /// Delete DML
        /// </summary>
        public void Delete()
        {

        }

        /// <summary>
        /// Select all DML
        /// </summary>
        /// <returns></returns>
        public string Select()
        {
            sql = "SELECT * FROM content";
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                return reader.GetString(2);
                //sb.Append("username:").Append(reader.GetString(0)).Append("\n")
                //.Append("password:").Append(reader.GetString(1));
            }
            return "asd";
        }


        public int SelectOne(string key)
        {
            


            return 0;
        }
    }
}
