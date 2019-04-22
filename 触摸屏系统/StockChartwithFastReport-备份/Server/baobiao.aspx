<%@ Page Language="C#" AutoEventWireup="true" CodeFile="baobiao.aspx.cs" Inherits="Server_baobiao" %>
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel="shortcut icon" href="../assets/logo/chart-logo.ico" type="image/x-icon" />
    <title>报表&bull;Report</title>
     <link href="../assets/bootstrap-3.3.4-dist/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link href="../assets/bootstrap-3.3.4-dist/css/bootstrap-theme.min.css" rel="stylesheet"
        type="text/css" />
    <link href="../assets/bootstrap-3.3.4-dist/css/buttons.css" rel="stylesheet" type="text/css" />
    <link href="../web-css/bootstrap-extends.css" rel="stylesheet" type="text/css" />
    <script src="../assets/jQuery/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="../assets/bootstrap-3.3.4-dist/js/bootstrap.min.js" type="text/javascript"></script>
      <script src="../assets/layer-v1.9.2/layer/layer.js" type="text/javascript"></script>
    <script src="../assets/layer-v1.9.2/layer/extend/layer.ext.js" type="text/javascript"></script>
    <meta name="description" content="The description of my page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!--<script src="../web-js/test.js"></script>-->
<script type="text/javascript">
    var layerLoading = null;
    var loading = function () { layerLoading = layer.load(1, { shade: [0.5, '#fff'] }); };
    var loadingClose = function () { layer.close(layerLoading); };
    var isEmpty = function (e) { if (e == null || e == undefined || e == "" || e.length <= 0) return true; return false; };
    var layerAlert = function (msg) { layer.alert(msg, { icon: 3, title: layerTitle, skin: 'layer-ext-moon' }); };

    Date.prototype.format = function (fmt) { //author: meizz  
        var Week = ['日', '一', '二', '三', '四', '五', '六'];
        var o = {
            "M+": this.getMonth() + 1,                      //月份   
            "d+": this.getDate(),                           //日
            "w+": Week[this.getDay()],                      //星期  
            "h+": this.getHours(),                          //小时   
            "m+": this.getMinutes(),                        //分   
            "s+": this.getSeconds(),                        //秒   
            "q+": Math.floor((this.getMonth() + 3) / 3),    //季度   
            "S": this.getMilliseconds()                     //毫秒   
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
    //获取cookie
    function getcookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return (arr[2]);
        else
            return null;
    }
    //退出删除cookie
    function delCookie(name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = getcookie(name);
        if (cval != null)
            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }
    if (getcookie('username') != null)
        //document.write('用户名：' + getcookie('username'));
        console.log("用户名：" + getcookie('username'));
    else
        layer.msg('您未登录过！', { time: 1000 });
    //退出登录
    function logout() {
        $.ajax({
            type: "Post",
            url: "../Server/UserManage.aspx/logout",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { loading(); },
            success: function (data) {
                loadingClose();
                if (!isEmpty(data.d)) {
                    delCookie('username');
                    layer.msg('退出成功，欢迎再次登录！', { time: 1000 });
                    $(window).attr('location', '/web/login.html');
                } else { layer.msg('您未登录过！', { time: 1000 }); }
            },
            error: function (err) {
                opException();
            }
        });
    };
    //系统当前时间设置
    function systemCurrTime() {
        var time = new Date().format("星期w， yyyy-MM-dd hh:mm:ss ");
        try {
            $("#systemCurr_Time").text(time);
        } catch (Exception) {
            return;
        }
    };
    $(document).ready(function () {
        setInterval(systemCurrTime, 1000);
    })
</script>
</head>
<body style="background-repeat: no-repeat;">

    <form id="form1" runat="server">
    <div class="row">
        <div class="top-nav">
            <div class="container">
                <div class="navbar-header">
                    <a href="javascript:document.location.reload();" class="navbar-brand">综合分析监控信息平台</a>
                </div>
              <ul class="nav navbar-nav top-navbar-tabs">
                    <li >
                        <a href="/web/Liucheng.html" >主页</a>
                       
                    </li>
                    <li class=""><a href="/web/index.html">曲线</a> </li>
                    <li class="active"><a href="../Server/baobiao.aspx">报表</a> </li>
                </ul>
                <ul class="nav navbar-nav navbar-right" style="margin-right: -90px;">
                    <li>
                        <p class="login-user-info" style="margin: 10px 0px 0px;">
                            系统当前时间：<b class="text-danger" id="systemCurr_Time"></b>
                        </p>
                    </li>
                    <li></li>
                    <li class="">
                       <a href="javascript:void(0)" onclick="logout();return false;">退出</a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="row">     
    <div class="col-xs-12">
        <div class="col-xs-2 left-menu"> 
            <div id="container" style="min-width:100%;width:100%;height:600px;">
                <asp:Menu ID="Menu1" runat="server" Font-Names="微软雅黑" Font-Size="Medium" ForeColor="#666699" OnMenuItemClick="Menu1_MenuItemClick" StaticSubMenuIndent="16px">
                    <DynamicHoverStyle BackColor="White" />
                    <DynamicSelectedStyle BackColor="Black" />
                    <DynamicItemTemplate>
                        <%# Eval("Text") %>
                    </DynamicItemTemplate>
                    <Items>
                <asp:MenuItem Text="赵重路泵站" Value="ZCL" Selectable="False">
                    <asp:MenuItem Text="赵重路日运行报表" Value="赵重路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="赵重路月运行报表" Value="赵重路月运行报表" Selected="True"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Selectable="False" Text="通波塘泵站" Value="TBT">
                    <asp:MenuItem Text="通波塘日运行报表" Value="通波塘日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="通波塘月运行报表" Value="通波塘月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                 <asp:MenuItem Selectable="False" Text="华青路泵站" Value="HQL">
                    <asp:MenuItem Text="华青路日运行报表" Value="华青路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="华青路月运行报表" Value="华青路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Selectable="False" Text="新业路泵站" Value="XYL">
                    <asp:MenuItem Text="新业路日运行报表" Value="新业路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="新业路月运行报表" Value="新业路月运行报表"></asp:MenuItem>
                </asp:MenuItem>

                <asp:MenuItem Text="新业路泵站" Value="XYL" Selectable="False">
                    <asp:MenuItem Text="新业路日运行报表" Value="新业路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="新业路月运行报表" Value="新业路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="赵巷A泵站" Value="ZXA" Selectable="False">
                    <asp:MenuItem Text="赵巷A日运行报表" Value="赵巷A日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="赵巷A月运行报表" Value="赵巷A月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="赵巷B泵站" Value="ZXB" Selectable="False">
                    <asp:MenuItem Text="赵巷B日运行报表" Value="赵巷B日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="赵巷B月运行报表" Value="赵巷B月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="赵巷C泵站" Value="赵巷C泵站" Selectable="False">
                    <asp:MenuItem Text="赵巷C日运行报表" Value="赵巷C日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="赵巷C月运行报表" Value="赵巷C月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="金星路泵站" Value="JXL" Selectable="False">
                    <asp:MenuItem Text="金星路日运行报表" Value="金星路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="金星路月运行报表" Value="金星路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="外青松泵站" Value="外青松泵站" Selectable="False">
                    <asp:MenuItem Text="外青松日运行报表" Value="外青松日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="外青松月运行报表" Value="外青松月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="新金路泵站" Value="新金路泵站" Selectable="False">
                    <asp:MenuItem Text="新金路日运行报表" Value="新金路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="新金路月运行报表" Value="新金路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="汇金路泵站" Value="汇金路泵站" Selectable="False">
                    <asp:MenuItem Text="汇金路日运行报表" Value="汇金路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="汇金路月运行报表" Value="汇金路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="民乐路泵站" Value="民乐路泵站" Selectable="False">
                    <asp:MenuItem Text="民乐路日运行报表" Value="民乐路日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="民乐路月运行报表" Value="民乐路月运行报表"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="新城一站" Value="新城一站" Selectable="False">
                    <asp:MenuItem Text="新城一站日运行报表" Value="新城一站日运行报表"></asp:MenuItem>
                    <asp:MenuItem Text="新城一站月运行报表" Value="新城一站月运行报表"></asp:MenuItem>
                </asp:MenuItem>

            </Items>
                    <LevelMenuItemStyles>
                        <asp:MenuItemStyle Font-Underline="False" HorizontalPadding="15px" VerticalPadding="7px" />
                    </LevelMenuItemStyles>
                    <LevelSelectedStyles>
                        <asp:MenuItemStyle BackColor="White" Font-Underline="False" />
                    </LevelSelectedStyles>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle Font-Underline="False" />
                    </LevelSubMenuStyles>
                </asp:Menu>
             <div class="divider">
        </div>
            </div>
            </div> 
        <div class="col-xs-10 page-container">
             
                            <cc1:webreport id="WebReport1" runat="server" backcolor="White" font-bold="False"
                                width="100%" height="100%"
                                onstartreport="WebReport1_StartReport" toolbarcolor="#999966"
                                printinpdf="True"
                                pdfembeddingfonts="True"
                                layers="true" zoom="0.9"
                                embedpictures="false"
                                singlepage="false"
                                toolbarstyle="Large"
                                toolbarbackgroundstyle="Light"
                                toolbariconsstyle="Blue" designreport="False" pdfa="True"
                                designscriptcode="true" reportresourcestring="77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjxSZXBvcnQgU2NyaXB0TGFuZ3VhZ2U9IkNTaGFycCIgUmVwb3J0SW5mby5DcmVhdGVkPSIwOS8yOS8yMDE1IDEyOjI2OjI0IiBSZXBvcnRJbmZvLk1vZGlmaWVkPSIwMy8wMS8yMDE4IDE0OjI3OjE3IiBSZXBvcnRJbmZvLkNyZWF0b3JWZXJzaW9uPSIyMDE3LjEuMTYuMCI+DQogIDxEaWN0aW9uYXJ5Lz4NCiAgPFJlcG9ydFBhZ2UgTmFtZT0iUGFnZTEiPg0KICAgIDxSZXBvcnRUaXRsZUJhbmQgTmFtZT0iUmVwb3J0VGl0bGUxIiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMzcuOCIvPg0KICAgIDxQYWdlSGVhZGVyQmFuZCBOYW1lPSJQYWdlSGVhZGVyMSIgVG9wPSI0MS44IiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMjguMzUiLz4NCiAgICA8RGF0YUJhbmQgTmFtZT0iRGF0YTEiIFRvcD0iNzQuMTUiIFdpZHRoPSI3MTguMiIgSGVpZ2h0PSI3NS42Ii8+DQogICAgPFBhZ2VGb290ZXJCYW5kIE5hbWU9IlBhZ2VGb290ZXIxIiBUb3A9IjE1My43NSIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjE4LjkiLz4NCiAgPC9SZXBvcnRQYWdlPg0KPC9SZXBvcnQ+DQo=" designerlocale="fr" localizationfile="./assets/fastreport/Localization/Chinese (Simplified).frl" />
                          
        </div>    
            </div>      
            </div>
      </div>  
    </form>
   
    </body>
</html>
