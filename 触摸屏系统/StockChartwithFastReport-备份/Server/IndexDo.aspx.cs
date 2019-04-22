using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;

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
                foreach(var item in list)
                {
                    if(item.pId==0)
                    {
                        result.Add(item);
                        continue;
                    }
                    Sql = @"select Val from " + item.name + " where Val<>0";
                    result.Add(item);
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

    /// <summary>
    /// 根据表名获取数据
    /// </summary>
    /// <param name="tb"></param>
    /// <returns></returns>
    [WebMethod]
    public static List<DataStructure> GetData(string tb, string start_time, string end_time)
    {
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"select DateTime,Val,Ann from " + tb + " where DateTime>=@p1 and DateTime<=@p2";
                List<DataStructure> result = sce.Database.SqlQuery<DataStructure>(Sql,
                     new SqlParameter { ParameterName = "p1", Value = DateTime.Parse(start_time) },
                    new SqlParameter { ParameterName = "p2", Value = DateTime.Parse(end_time) }).ToList();
                return result;
                /*return sce.Database.SqlQuery<DataStructure>(Sql,
                     new SqlParameter { ParameterName = "p1", Value = DateTime.Parse(start_time) },
                    new SqlParameter { ParameterName = "p2", Value = DateTime.Parse(end_time) }).ToList();*/
            }
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