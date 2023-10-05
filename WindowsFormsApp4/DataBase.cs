using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp4
{
    class DataBase
    {
        private SqlConnection Conn;
        public static string uid = "atg";
        public static string upw = "1234";
        public static string database = "TestDB";
        public static string server = "(localdb)\\MSSQLLocalDB";
        public static string testtbl = "testTbl";

        public object SqlHelper { get; private set; }

        public void Connect_DB(DataTable dt)
        {
            string connStr = ($"server = {server};DATABASE={database};UID={uid};Password= {upw}");

            Conn = new SqlConnection(connStr);

            try
            {
                string tbleT = "dbo.T_tbl";
                Conn.Open();

                using (var adapter = new SqlDataAdapter($"IF OBJECT_ID('{tbleT}') IS NOT NULL  " +
                    $"DROP TABLE {tbleT}"
                    , Conn))

                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Fill(dt);
                }

                string sql = $"Create Table {tbleT} (";
                foreach (DataColumn column in dt.Columns)
                {
                    sql += "[" + column.ColumnName + "] " + "nvarchar(50)" + ",";
                }
                sql = sql.TrimEnd(new char[] { ',' }) + ")";

                SqlCommand cmd = new SqlCommand(sql, Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();

                using (var adapter = new SqlDataAdapter($"SELECT * FROM {tbleT}", Conn))
                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Fill(dt);
                }

                using (var bulk = new SqlBulkCopy(Conn))
                {
                    bulk.DestinationTableName = tbleT;

                    bulk.WriteToServer(dt);
                }

                Conn.Close();
            }
            catch (Exception ec)
            {

            }
        }

    }

}


