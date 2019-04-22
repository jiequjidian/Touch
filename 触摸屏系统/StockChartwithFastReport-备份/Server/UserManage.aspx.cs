using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;

public partial class Server_UserManage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    ///   <summary>
    ///   给一个字符串进行MD5加密
    ///   </summary>
    ///   <param   name="strText">待加密字符串</param>
    ///   <returns>加密后的字符串</returns>
    public static string md5Encode(string str)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] data = Encoding.Default.GetBytes(str);
        byte[] md5data = md5.ComputeHash(data);
        md5.Clear();
        string re_str = "";
        for (int i = 0; i < md5data.Length; i++)
        {
            re_str += md5data[i].ToString("x").PadLeft(2, '0');
        }
        return re_str;
    } 

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    [WebMethod]
    public static List<SysUser> login(string userName, string password)
    {
        List<SysUser> user = null;
        password = md5Encode(password);
        userName = userName.Trim();
        try
        {
            /**
             *@QPCHARTEntities : 与数据库链接时对应的实体名称
             * */
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"select * from SysUser where YongHu=@p1 and MiMa=@p2";
                user = sce.Database.SqlQuery<SysUser>(Sql,
                     new SqlParameter { ParameterName = "p1", Value = userName },
                    new SqlParameter { ParameterName = "p2", Value = password }).ToList();
            }
            if (user.Count <= 0) return user;
            /*
             * 写入session
             * 生命周期为10分钟
             * */
            string[] userInfo = {user[0].YongHu,user[0].MiMa,user[0].Level.ToString()};
            SessionHelper.Del("CurrLoginUser");
            SessionHelper.Adds("CurrLoginUser",userInfo,10);
        }
        catch (Exception ex)
        {
            return null;
        }
        return user;
    }

    /// <summary>
    /// 自动登录
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public static List<SysUser> autoLogin() {
        List<SysUser> user = null;
        if (loginValidation())
        {
            string[] currUserInfo = SessionHelper.Gets("CurrLoginUser");
            string userName = currUserInfo[0];
            string password = currUserInfo[1];
            try
            {
                using (QPCHARTEntities sce = new QPCHARTEntities())
                {
                    string Sql = @"select * from SysUser where YongHu=@p1 and MiMa=@p2";
                    user = sce.Database.SqlQuery<SysUser>(Sql,
                         new SqlParameter { ParameterName = "p1", Value = userName },
                        new SqlParameter { ParameterName = "p2", Value = password }).ToList();
                }
                if (user.Count <= 0) return null;
                /*
                 * 写入session
                 * 生命周期为10分钟
                 * */
                string[] userInfo = { user[0].YongHu, user[0].MiMa, user[0].Level.ToString() };
                SessionHelper.Del("CurrLoginUser");
                SessionHelper.Adds("CurrLoginUser", userInfo, 10);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        return user;
    }

    /// <summary>
    /// 校验用户是否在线
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public static bool loginValidation() {
        string[] userInfo = SessionHelper.Gets("CurrLoginUser");
        if (userInfo != null) return true;
        return false;
    }

    /// <summary>
    /// 获取当前用户的权限
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public static string getPermission() {
        if (loginValidation())
        {
            string[] userInfo = SessionHelper.Gets("CurrLoginUser");
            return  userInfo[userInfo.Length - 1];
        }
        return "logout";
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    [WebMethod]
    public static string logout()
    {
        if (loginValidation())
        {
            SessionHelper.Del("CurrLoginUser");
            return "logout";
        }
        return "";
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    [WebMethod]
    public static string reg(string userName,string password) {
        userName = userName.Trim();
        password = md5Encode(password);
        try
        {
            using (QPCHARTEntities sce = new QPCHARTEntities())
            {
                string Sql = @"select * from SysUser where YongHu =@p1";
                int count = sce.Database.SqlQuery<SysUser>(Sql,
                     new SqlParameter { ParameterName = "p1", Value = userName }).ToList().Count();
                if (count > 0) return "UserExisted";

                Sql = @"insert SysUser(YongHu,MiMa,Level) values('" + userName + "','" + password + "',0)";
                /*
                 * 写入session
                 * 生命周期为10分钟
                 * */
                string[] userInfo = { userName, password, "0" };
                SessionHelper.Del("CurrLoginUser");
                SessionHelper.Adds("CurrLoginUser", userInfo, 10);

                return sce.Database.ExecuteSqlCommand(Sql) > 0 ? userName : "";
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}