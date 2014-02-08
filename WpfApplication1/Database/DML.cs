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
        DataSet data = new DataSet();
        SQLiteDataAdapter SDataAdapter;

        string SelectSQL;
        string InsertSQL;

        public DML()
        {
            cmd.Connection = new DBconnect().Connect();

            InsertSQL = "insert into T_Report(id,datetime,unitsname,use,income,expenses) values(:id,:datetime,:unitsname,:use,:income,:expenses);";

        }

        /// <summary>
        /// Insert DML
        /// </summary>
        public void Insert(string sql)
        {
            //cmd.CommandText = sql;
            //cmd.ExecuteNonQuery();
            object[] rowVals = new object[6];
            rowVals[0] = 99;
            rowVals[1] = "1989-03-17 11:11:11";
            rowVals[2] = "nameTest";
            rowVals[3] = "useTest";
            rowVals[4] = 123.5;
            rowVals[5] = 32.1;

            data.Tables[0].Rows.Add(rowVals);
            SDataAdapter.InsertCommand = new SQLiteCommand(InsertSQL, cmd.Connection);
            SDataAdapter.Update(data.Tables[0]);
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
            SDataAdapter = da;
            da.Fill(dataset);
            data = dataset;

            Insert(sql);
            return dataset;
        }

    }
}
