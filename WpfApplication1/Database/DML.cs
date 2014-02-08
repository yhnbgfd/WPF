using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    public class DML
    {
        SQLiteCommand cmd = new SQLiteCommand();

        public DML()
        {
            cmd.Connection = new DBconnect().Connect();
        }

        /// <summary>
        /// Insert DML
        /// </summary>
        public void Insert(string sql)
        {
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
        public List<List<object>> Select(string sql)
        {
            List<List<object>> lists = new List<List<object>>();
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<object> list = new List<object>();
                for (int i = 0; i < reader.FieldCount;i++ )
                {
                    list.Add(reader.GetValue(i));
                    Console.WriteLine(reader.GetValue(i));
                }
                lists.Add(list);
            }
            return lists;
        }

    }
}
