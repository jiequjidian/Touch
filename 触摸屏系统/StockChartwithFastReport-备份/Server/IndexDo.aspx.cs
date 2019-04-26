﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using Newtonsoft.Json;


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

    /// <summary>
    /// 根据表名获取数据
    /// </summary>
    /// <param name="tb"></param>
    /// <returns></returns>
    [WebMethod]
    public static List<DataStructure> GetData(string tb,string tg, string start_time, string end_time)
    {
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"select DateTime,"+ tg + " from " + tb + " where DateTime>=@p1 and DateTime<=@p2";
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