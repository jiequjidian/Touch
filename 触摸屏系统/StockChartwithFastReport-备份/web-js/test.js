var chart = null;
var names = [];
var getTime8h = new Date('2015-01-01 08:00:00').getTime() - new Date('2015-01-01 00:00:00').getTime();
var loading = function () { layerLoading = layer.load(1, { shade: [0.5, '#fff'] }); };
var loadingClose = function () { layer.close(layerLoading); };
var isEmpty = function (e) { if (e == null || e == undefined || e == "" || e.length <= 0) return true; return false; };
var layerAlert = function (msg) { layer.alert(msg, { icon: 3, title: layerTitle, skin: 'layer-ext-moon' }); };

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
    //  document.write('<a href="/web/login.html">登录</a>');
    $(window).attr('location', '/web/login.html');
//js 日期格式化输出
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

//系统当前时间设置
function systemCurrTime() {
    var time = new Date().format("星期w， yyyy-MM-dd hh:mm:ss ");
    try {
        $("#systemCurr_Time").text(time);
    } catch (Exception) {
        return;
    }
};

var logout = function () {
    $.ajax({
        type: "Post",
        url: "../Server/UserManage.aspx/logout",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () { loading(); },
        success: function (data) {
            loadingClose();
            if (!isEmpty(data.d)) {
                delCookie("username");
                layer.msg('退出成功，欢迎再次登录！', { time: 1000 });
                $(window).attr('location', '/web/login.html');
                $("#userNameInfo").html('<a href="javascript:void(0);" onclick="login();return false;" style="text-decoration: none;" target="_blank">【请登录】</a>');
            } else { layer.msg('您未登录过！', { time: 1000 }); }
        },
        error: function (err) {
            opException();
        }
    });
};
// Create the chart
$(document).ready(function () {
    getdata();
   
    $("table input").attr('readonly', 'readonly');
    $("table input").attr('text-align', 'center');


    function getcookie(name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null) return unescape(arr[2]);
        return null;
    }
    if (getcookie('username') != null)
        //document.write('用户名：' + getcookie('username'));
        console.log("用户名：" + getcookie('username'));
    else
        //  document.write('<a href="/web/login.html">登录</a>');
        $(window).attr('location', '/web/login.html');
    //function getsession() {

    //    console.log(a);
    //    if(a=="")
    //    {
    //        alert("未登录");
    //        window.location.href = "http://www.baidu.com";
    //        return false;
    //    }
    //    else {
    //        alert("登录成功");
    //        return false;
    //    }
    //}

    function getdata() {
        var datatim = [];
        var time = [];
        $.ajax({
            type: "post",
            async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
            url: "../Server/test.aspx?method=gettest",
            data: {},
            dataType: "json",        //返回数据形式为json
            success: function (result) {
                for (i in result) {
                    var r = result[i];
                    $("#Val").attr("value", r.Val);
                    $("#Val1").attr("value", r.Val1);
                    $("#Val2").attr("value", r.Val2);
                    $("#Val3").attr("value", r.Val3);
                    $("#Val4").attr("value", r.Val4);
                    $("#Val5").attr("value", r.Val5);
                    $("#Val6").attr("value", r.Val6);
                    $("#Val7").attr("value", r.Val7);
                }
               // console.log(result);
           
             
            },
           
        })
    }
    setInterval(getdata, 10000);
    //启动显示系统时间定时器
    setInterval(systemCurrTime,1000);
})