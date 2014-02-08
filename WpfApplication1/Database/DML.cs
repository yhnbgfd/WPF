using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

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
        public void Update(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
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
        public DataSet Select(string sql)
        {
            DataSet dataset = new DataSet();
            cmd.CommandText = sql;
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dataset);
            return dataset;
        }

    }
}
