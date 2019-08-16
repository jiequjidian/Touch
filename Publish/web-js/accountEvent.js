
var isEmpty = function (e) { if (e == null || e == undefined || e == "" || e.length <= 0) return true; return false; };
var loading = function () { layerLoading = layer.load(1, { shade: [0.5, '#fff'] }); };
var loadingClose = function () { layer.close(layerLoading); };
var layerTitle = logoText = "综合分析监控信息平台";
var validation = function () { var v = true; $(".required").each(function (e, i) { if (isEmpty($(this).val())) { $(this).addClass("error"); v = false; } else $(this).removeClass("error"); }); return v; };


//注解操作权限
var opAnnPermission = "3";
//-----------------------------------------用户登录----------------------------------------------+
//自动登录
var autoLogin = function () {
    $.ajax({
        type: "Post",
        url: "../Server/UserManage.aspx/autoLogin",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isEmpty(data.d)) {
                layer.msg('您已自动登录！', { time: 1000 });
                $("#userNameInfo").html(data.d[0].YongHu);
            } else {
                alert('请登录');
                window.location.href = '../web/login.html';
              // return ("<script>alert('请登录!');window.location.href='../web/login.html'</script>");
            }
        },
        error: function (err) {
            //opException();
        }
    });
};
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
//if (getcookie('username') != null)
//    //document.write('用户名：' + getcookie('username'));
//    console.log("用户名：" + getcookie('username'));
//else
//    //  document.write('<a href="/web/login.html">登录</a>');
//    $(window).attr('location', '/web/login.html');

//登录
var login = function () {
    var loginHtml = '<form class="form-horizontal" action="">';
    loginHtml += '<div class="form-group"><label class="col-sm-3 control-label">用户名：</label><div class="col-sm-9"><input class="form-control required" id="userName" type="text" onblur="validation()"></input></div></div>';
    loginHtml += '<div class="form-group"><label class="col-sm-3 control-label">密  码：</label><div class="col-sm-9"><input class="form-control required" id="password" type="password" onblur="validation()"></input></div></div></form>';
    layer.confirm(loginHtml, {
        area: ['400px', '250px'],
        btn: ['登录', '取消'],
        title: layerTitle + "·用户登录"
    }, function () {
        if (validation()) {
            $.ajax({
                type: "Post",
                url: "../Server/UserManage.aspx/login",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { loading(); },
                data: '{"userName":"' + $("#userName").val() + '","password":"' + $("#password").val() + '"}',
                success: function (data) {
                    loadingClose();
                    if (data.d != null) {
                        if (!isEmpty(data.d)) {
                            layer.msg('登录成功！', { time: 1000 });
                            $("#userNameInfo").html(data.d[0].YongHu);
                        }
                        else
                            layer.alert('用户名或密码错误！', { title: layerTitle, icon: 4, skin: 'layer-ext-seaning' });
                    } else { opException(); }
                },
                error: function (err) {
                    opException();
                }
            });
        }
    }, function () { });
};
//退出登录
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
//校验是否登录
var loginValidation = function (callback, gofunction) {
    $.ajax({
        type: "Post",
        url: "../Server/UserManage.aspx/loginValidation",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () { loading(); },
        success: function (data) {
            if (isEmpty(callback)) loadingClose();
            if (!isEmpty(data.d)) {
                callback(gofunction);
            } else { loadingClose(); layer.msg('您未登录，请先登录！', { time: 1000, end: function () { login(); } }); }
        },
        error: function (err) {
            opException();
        }
    });
};
//用户注册
var reg = function () {
    var regHtml = '<form class="form-horizontal" action="">';
    regHtml += '<div class="form-group"><label class="col-sm-3 control-label">用户名：</label><div class="col-sm-9"><input class="form-control required" id="userName" type="text" onblur="validation()"></input></div></div>';
    regHtml += '<div class="form-group"><label class="col-sm-3 control-label">密  码：</label><div class="col-sm-9"><input class="form-control required" id="password" type="password" onblur="validation()"></input></div></div></form>';
    layer.confirm(regHtml, {
        area: ['400px', '250px'],
        btn: ['注册', '取消'],
        title: layerTitle + "·用户注册"
    }, function () {
        if (validation()) {
            $.ajax({
                type: "Post",
                url: "../Server/UserManage.aspx/reg",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { loading(); },
                data: '{"userName":"' + $("#userName").val() + '","password":"' + $("#password").val() + '"}',
                success: function (data) {
                    loadingClose();
                    if (!isEmpty(data.d)) {
                        if (data.d == "UserExisted") { $("#userName").addClass("error"); layer.alert("用户已存在！"); }
                        else { layer.msg('注册成功,系统2秒之后自动为您登录！', { time: 2000, end: function () { $("#userNameInfo").html(data.d); } }); }
                    } else { opException(); }
                },
                error: function (err) {
                    opException();
                }
            });
        }
    }, function () { });
};

//获取权限
var getPermission = function (callback) {
    $.ajax({
        type: "Post",
        url: "../Server/UserManage.aspx/getPermission",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () { loading(); },
        success: function (data) {
            if (isEmpty(callback)) loadingClose();
            if (!isEmpty(data.d)) {
                if (data.d != opAnnPermission) { layer.msg('您没有权限执行操作！', { time: 2000 }); return; }
                callback();
            } else { opException(); }
        },
        error: function (err) {
            opException();
        }
    });
};




