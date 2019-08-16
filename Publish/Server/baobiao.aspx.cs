using System;
using System.Data;
using System.Configuration;
using System.Security;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastReport;
using FastReport.Data;
using FastReport.Editor;
using FastReport.Web;
using FastReport.Utils;

public partial class Server_baobiao : System.Web.UI.Page
{
    protected void doReport(string reportpath)
    {
        WebReport1.Width = 1290;
        WebReport1.Height = 600;
        WebReport1.ReportFile = @reportpath;
        WebReport1.Prepare();
    }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["CurrLoginUser"] != null)
                {
                    WebReport1.ReportFile = @".\\Report\\赵重路日运行报表.frx";
                    WebReport1.Prepare();
                }
                else
                {
                    Response.Write("<script>alert('请登录!');window.location.href='../web/login.html'</script>");
                }
               
            }

        }
        protected void TopMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            //MultiViewBody.ActiveViewIndex = Convert.ToInt16(e.Item.Value) - 1;
        }

        protected void LeftMenu_Init(object sender, EventArgs e)
        {
          
        }

        protected void LeftMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            string path = @".\\Report\\" + e.Item.Value+".frx";
            doReport(path);
        }

        protected void WebReport1_StartReport(object sender, EventArgs e)
        {
            Report FReport = (sender as WebReport).Report;
            //Label1.Text = FReport.ReportInfo.Description;
            //Label1.Style.Add("padding", "5px");
            RegisterData(FReport);
        }

        private void RegisterData(Report FReport)
        {
           
        }


    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
         string path = @".\\Report\\" + e.Item.Value + ".frx";
         doReport(path);
    }
}