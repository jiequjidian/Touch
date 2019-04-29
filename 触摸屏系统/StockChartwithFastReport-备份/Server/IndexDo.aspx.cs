using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;

public static class serializerS
{//利用序列化和反序列化来进行深拷贝
    public static Menu CloneJson<Menu>(this Menu source)
    {
        if (Object.ReferenceEquals(source, null))
        {
            return default(Menu);
        }

        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        return JsonConvert.DeserializeObject<Menu>(JsonConvert.SerializeObject(source), deserializeSettings);
    }
}

/// <summary>
/// 该类主要处理web页面通过ajax请求
/// </summary>
public partial class Server_IndexDo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


  
    #region 该类功能描述
    /**
     * 该类主要处理web页面通过ajax请求
     **/
    #endregion

    /// <summary>
    /// 获取左侧菜单数据
    /// </summary>
    [WebMethod]
    public static List<Menu> GetMenu()
    {
       

        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"select* from Menu";
                
                List<Menu> list = sce.Database.SqlQuery<Menu>(Sql).ToList();
                List<Menu> result = new List<Menu>();
                int i;
                int bengNum = 0;
                
                foreach (var item in list)
                {
                    if(item.pId==0)
                    {
                        bengNum++;
                        result.Add(item);
                        continue;
                    }
                    
                    //Sql = @"select Val from " + item.name + "where Val<>0";                   
                    for (i = 0; i < 13; i++)
                    {
                        Menu itemChid = new Menu();
                        
                        itemChid = item.CloneJson<Menu>();
                        //itemChid.name += i.ToString();
                        //itemChid.id += 1;
                        itemChid.pId = i+1;
                        result.Add(itemChid);

                    }
                    //if (sce.Database.SqlQuery<double>(Sql).ToList().Count != 0)
                    //{
                    //    result.Add(item);
                    //}
                }
                
                return result;
                //return sce.Database.SqlQuery<Menu>(Sql).ToList();
                //return sce.Menu.ToList();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    //连接数据库        

    public static SqlConnection connection;
    public static SqlConnection Connection
    {
        get
        {
            if (connection == null)
            {
                //远程连接数据库命令（前提远程数据库服务器已经配置好允许远程连接）  
                //string strConn = @"Data Source=172.18.72.158;Initial Catalog=WebKuangjia;User ID=sa;Password=LIwei123;Persist Security Info=True";                    
                //连接本地数据库命令                
                string strConn = @"Data Source=.;Initial Catalog=QPCHART1;Integrated Security=True";
                connection = new SqlConnection(strConn);
                connection.Open();
            }
            else if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            else if (connection.State == ConnectionState.Broken)
            {
                connection.Close();
                connection.Open();
            }
            return connection;
        }
    }
    [WebMethod]
    //执行sql语句,返回数据库表     
    public static DataTable GetDataSet(string commandText, CommandType commandType, SqlParameter[] para)
    {
        //1.创建指令
        SqlCommand cmd = new SqlCommand();        
        cmd.Connection = Connection;
        cmd.CommandText = commandText;
        cmd.CommandType = commandType;
        try
        {
            if (para != null)
            {
                cmd.Parameters.AddRange(para);
            }
            //创建数据适配器
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable temp = new DataTable();
            //数据填充
            da.Fill(temp);
            cmd.Dispose();
            return temp;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {

        }
    }
    #region DataTablel Json  
    [WebMethod]
    public static string Dtb2Json(DataTable dtb)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        ArrayList dic = new ArrayList();
        foreach (DataRow row in dtb.Rows)
        {
            Dictionary<string, object> drow = new Dictionary<string, object>();
            foreach (DataColumn col in dtb.Columns)
            {
                drow.Add(col.ColumnName, row[col.ColumnName]);
            }
            dic.Add(drow);
        }
        return jss.Serialize(dic); //形式为"{"x":1,"y0":1,"y1":1,"y2":1},{...}"
    }
    #endregion
    /// <summary>
    /// 根据表名获取数据
    /// </summary>
    /// <param name="tb"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetData(string tb,string tag, string start_time, string end_time)
    {
        try
        {
            //using (QPCHARTEntities sce = new QPCHARTEntities())
            //{
            //    string Sql = @"select DateTime," +" from " + tb + " where DateTime>=@p1 and DateTime<=@p2";
            //    List<DataStructure> result = sce.Database.SqlQuery<DataStructure>(Sql,
            //         new SqlParameter { ParameterName = "p1", Value = DateTime.Parse(start_time) },
            //        new SqlParameter { ParameterName = "p2", Value = DateTime.Parse(end_time) }).ToList();
            //    return result;               
            //}
            DataTable tableReturn;//声明表变量 
            string sssssss = tb;
            string sql = @"select DateTime," + tag + " from " + tb + " where DateTime>=@p1 and DateTime<=@p2";
            
            SqlParameter[] para = { new SqlParameter("@p1", DateTime.Parse(start_time)), new SqlParameter("@p2", DateTime.Parse(end_time))};
           
            tableReturn = GetDataSet(sql, CommandType.Text, para);            
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
             //创建存储返回值的表              
            DataTable dt = new DataTable();
            //为表增加两个列，列名分别为x和y     
            dt.Columns.Add("x", typeof(long));
            
                string seriesName = (tb+"的"+tag);
                dt.Columns.Add(seriesName, typeof(float));

            //将存储过程中的表复制到返回表中                
            foreach (DataRow dr in tableReturn.Rows)
            {
                DataRow newdr = dt.NewRow();
                // long saaa=(long)((DateTime)dr[0] - startTime).TotalMilliseconds;
                newdr["x"] = (long)((DateTime)dr[0] - startTime).TotalMilliseconds;
              
                newdr[seriesName] = dr[1];
                dt.Rows.Add(newdr);
            }
            string result = Dtb2Json(dt).ToString();
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 更新注解值
    /// </summary>
    /// <param name="_DateTime"></param>
    /// <param name="tb"></param>
    /// <param name="Ann"></param>
    /// <returns></returns>
    [WebMethod]
    public static string SaveAnnEditData(string _DateTime,string tb,string Ann) {
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"update  " + tb + " set Ann = @p1 where DateTime = @p2";
                return sce.Database.ExecuteSqlCommand(Sql,
                    new SqlParameter { ParameterName = "p1", Value = Ann.Trim() },
                    new SqlParameter { ParameterName = "p2", Value = DateTime.Parse(_DateTime) }) > 0 ? "Success" : "";
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 删除注解
    /// </summary>
    /// <param name="_DateTime"></param>
    /// <param name="tb"></param>
    /// <returns></returns>
    [WebMethod]
    public static string DelAnn(string _DateTime, string tb)
    {
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"update  " + tb + " set Ann = NULL where DateTime = @p2";
                return sce.Database.ExecuteSqlCommand(Sql,
                    new SqlParameter { ParameterName = "p2", Value = DateTime.Parse(_DateTime) }) > 0 ? "Success" : "";
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}