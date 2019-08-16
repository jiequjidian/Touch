using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Server_verifyLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CurrLoginUser"]!=null)            　//判断管理员是否登录
        {


          // Response.Write("<script language='javascript'>alert('已登录，谢谢！！！');</script>");
            
         
        }
        else
        {

             Response.Write("<script language='javascript'>alert('请先登录，再访问！！！');window.location.href='../web/login.html';</script>");
           // Response.Write("<script language='javascript'>var loginHtml = '<form >';loginHtml += '<div><label >请先登录</label></div></form>';layer.confirm(loginHtml, {area: '400px',btn: '确认',title:''}, function() {window.location.href='/web/login.html'});</script>");

           
        }     
    }
}