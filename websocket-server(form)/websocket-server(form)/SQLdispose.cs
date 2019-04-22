using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace websocket_server_form_
{
    class SQLdispose
    {
        
        private String connectString;          //数据库连接字符串
        
        private string tableName;              //表名

        ////读取数据库中的字段
        public string[] readeColumn()
        {
            connectString = System.Configuration.
                  ConfigurationManager.AppSettings["connectionString"];

            SqlConnection connection = new SqlConnection(connectString);

            //string[] ss;
            string myCommandStr = "select name      from      syscolumns      where      id=object_id(N'DataInfo') ";
            SqlCommand myCommand = new SqlCommand(myCommandStr, connection);
            var context = new DataContext(connectString);

            //string[] ss = context.ExecuteQuery<string>("select name from syscolumns where id=(select max(id) from sysobjects where xtype='u' and name='DataInfo') ").ToArray();
            string[] ss = context.ExecuteQuery<string>("select name      from      syscolumns      where      id=object_id(N'DataInfo') ").ToArray();

            return ss;
        }
              

        //向数据库增加多行记录
        public bool appendData(string column,string value)
        {
           
            string strSQL = "", strColumn = "", strValue = "";
            string[] Column = column.Split(',');
            string[] Value = value.Split(',');
            for (int i = 0; i < Column.Count(); i++)
            {
                strColumn += Column[i] + ",";
                if (Value.Count() <= i)
                    strValue += Value[Value.Count() - 1].ToString() + "','";
                else
                    strValue += Value[i].ToString() + "','";
            }
            strColumn = strColumn.Remove(strColumn.Length - 1);  //去掉结尾的","
            strValue = strValue.Remove(strValue.Length - 2);     //去掉结尾的",'"
            strSQL = "insert into " + "DataInfo" + "(";
            strSQL += strColumn + ") values('";
            strSQL += strValue + ")";
            ExecuteNoneQuery(strSQL);
            return true;
        }

        //无返回
        public void ExecuteNoneQuery(string strsql)
        {
            connectString = System.Configuration.
                  ConfigurationManager.AppSettings["connectionString"];
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {                       
                        connection.Open();
                    }

                    SqlCommand cmd = new SqlCommand(strsql, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {
                    connection.Close();
                }
            }

        }

        //有返回表
        public DataTable ExecuteWithReturn(string myCommandStr)
        {
            connectString = System.Configuration.
                ConfigurationManager.AppSettings["connectionString"];
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                DataTable dataTable = new DataTable();
               
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                   
                SqlCommand myCommand = new SqlCommand(myCommandStr, connection);
                SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                myAdapter.Fill(dataTable);
                connection.Close();
                return dataTable;              
               
            }
        }

        //将本地datatable追加到数据库中相应的表中
        public bool appendToSQL(DataTable mytable)
        {
            //1.打开数据库连接
            connectString = System.Configuration.
              ConfigurationManager.AppSettings["connectionString"];
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();



            string strSQL = "select top 14 * from DataInfo order by id desc "; //只是必须参数，在本函数中此参数没有意义
            SqlCommand myCommand = new SqlCommand(strSQL, conn);
            SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
            SqlCommandBuilder Builder = new SqlCommandBuilder(myAdapter);
            DataTable tableceshi = mytable;
            myAdapter.Update(mytable);

            return true;
        }
    }
}
