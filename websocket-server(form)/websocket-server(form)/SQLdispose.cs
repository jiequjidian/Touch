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

        SqlConnection connection = null;     //数据库连接
       

        ////向数据库增加多行记录
        //public bool appendData(string column,string value)
        //{
        //    if (connection.State == ConnectionState.Closed)
        //    {
        //        connection.Open();
        //    }
        //    else
        //    {
        //        connection.Close();
        //        connection.Open();
        //    }
        //    string strSQL = "", strColumn = "", strValue = "";
        //    string[] Column = column.Split(',');
        //    string[] Value = value.Split(',');
        //    for (int i = 0; i < Column.Count(); i++)
        //    {
        //        strColumn += Column[i] + ",";
        //        if (Value.Count() <= i)
        //            strValue += Value[Value.Count() - 1].ToString() + "','";
        //        else
        //            strValue += Value[i].ToString() + "','";
        //    }
        //    strColumn = strColumn.Remove(strColumn.Length - 1);  //去掉结尾的","
        //    strValue = strValue.Remove(strValue.Length - 2);     //去掉结尾的",'"
        //    strSQL = "insert into " + "DataInfo" + "(";
        //    strSQL += strColumn + ") values('";
        //    strSQL += strValue + ")";
        //    ExecuteNoneQuery(strSQL);
        //    connection.Close();
        //    return true;
        //}

        ////无返回
        //public void ExecuteNoneQuery(string strsql)
        //{
        //    connectString = System.Configuration.
        //          ConfigurationManager.AppSettings["connectionString"];           
        //        try
        //        {
        //            if (connection.State == ConnectionState.Closed)
        //            {                       
        //                connection.Open();
        //            }
        //            else
        //            {
        //                connection.Close();
        //                connection.Open();
        //            }
        //            SqlCommand cmd = new SqlCommand(strsql, connection);
        //            cmd.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //        catch (Exception)
        //        {
        //            connection.Close();
        //        }
          
        //}

      /// <summary>
      /// 从数据库读取数据，并返回一张表
      /// </summary>
      /// <param name="myCommandStr">数据库操作命令</param>
      /// <returns></returns>
        public DataTable ExecuteWithReturn(string myCommandStr)
        {
            //获取数据库连接字符串
            // connectString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            connectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            DataTable dataTable = new DataTable();
            if (connection == null)
            {
                //实例化sql连接
                connection = new SqlConnection(connectString);
            }
            else
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else
                {
                    connection.Close();
                    connection.Open();
                }
            }    

                SqlCommand myCommand = new SqlCommand(myCommandStr, connection);
                SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                myAdapter.Fill(dataTable);
                myAdapter.Dispose();
                connection.Close();
                return dataTable; 
        }

        //将本地datatable追加到数据库中相应的表中
        public bool appendToSQL(DataTable mytable)
        {
            //1.打开数据库连接
            //connectString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            connectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            SqlConnection conn=null;
            if (conn==null)
            {
                conn = new SqlConnection(connectString);
            }
            conn.Open();

            string strSQL = "select top 14 * from DataInfo order by id desc "; //只是必须参数，在本函数中此参数没有意义
            SqlCommand myCommand = new SqlCommand(strSQL, conn);
            SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
            SqlCommandBuilder Builder = new SqlCommandBuilder(myAdapter);
            DataTable tableceshi = mytable;
            myAdapter.Update(mytable);
            myAdapter.Dispose();
            Builder.Dispose();
            conn.Close();
            return true;
        }
    }
}
