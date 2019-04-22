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


            Response.Write("<script language='javascript'>alert('已登录，谢谢！！！');</script>");
            
         
        }
        else
        {
          
           Response.Write("<script language='javascript'>alert('请先登录，再访问！！！');window.location='/web/index.html';</script>");
              //Response.Write("<script language='javascript'>self.location='/web/login.html';</script>");
         //   Response.Write("<script>alert('plese input name!')</script>");
       //    Response.Redirect("~/web/login.html", false); 

            //Response.Write("<script languge='javascript'>alert('请先登录，再访问！！！');");
         //   Response.Write("<script languge='javascript'>document.write<a href='/web/login.html'>登录</a>;/script>");
        }     
    }
}