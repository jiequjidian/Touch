using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
public partial class Server_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string method = Request.QueryString["method"];
        
            if (!string.IsNullOrEmpty(method))
            {

                if (method == "gettest")
                {
                    GetData();

                }
        
        


        }
       
    }
    #region 该类功能描述
    /**
     * 该类主要处理web页面通过ajax请求
     **/
    #endregion

    /// <summary>
    /// 根据表名获取数据
    /// </summary>
    /// <param name="tb"></param>
    /// <returns></returns>

    public void GetData()
    {
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = "select top(1) * from dbo.value order by [DateTime] desc";
                List<TestStructure> result = sce.Database.SqlQuery<TestStructure>(Sql).ToList();
                object JSONObj = (Object)JsonConvert.SerializeObject(result);
                Response.Write(JSONObj);
                //  一定要加，不然前端接收失败  
                Response.End();
            }
        }
        catch (Exception ex)
        {

        }
    }





}