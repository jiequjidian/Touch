$(function () {
    //获取需要显示的表，获取结果构造zTree 树
    $.ajax({
        type: "Post",
        url: "../Server/IndexDo.aspx/GetMenu",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () { loading(); },
        success: function (data) {
            var zNodes = data.d;          
            
            if (zNodes != null)
                
                $.fn.zTree.init($("#treeDemo"), setting, zNodes);
        },
        error: function (er) {
            opException();
        }
    });
    //interval = setInterval(toDayCurrTime, "1000");
    //启动显示系统时间定时器
    setInterval(systemCurrTime, "1000");
    $("#start_time").val(toDayStart);
    $("#end_time").val(toDayCurr);
    $(".top-navbar-tabs li").click(function () { $(this).addClass("active").siblings().removeClass("active") });
    getStockData();
});

//过滤菜单中没有勾选项并重新绘制图表
var filterMenu = function (v) {
    //names = arrayIntersection(v, old_names);
    names = v[0];
    nameParents = v[1];
    console.log("names:" + names);
    console.log("names:" + names);
    if (!isEmpty(names)) {
        getStockData();
    }
};

//加载图表数据并重绘
var getStockData = function () {
    loading();
    seriesOptions = [];
    var flog = 0, n = 0;
    names.forEach(function (name, i) {
        $.ajax({
            type: "Post",
            url: "../Server/IndexDo.aspx/GetData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"tb":"' + nameParents[i] + '","tg":"' + name+ '","start_time":"' + start_time + '","end_time":"' + end_time + '"}',
            success: function (data) {
                if (isEmpty(data.d)) { flog++; if (flog == names.length) { loadingClose(); layerAlert("该时间段内无记录数据或者是加载错误，请稍后重试！"); } return; }
                data = data.d;
                var new_data = new Array();
                data.forEach(function (e, j) {
                    var a = new Array();
                    var str = eval("e." + name);
                    console.log("i:" + i);
                    console.log("names:" + names);
                    e.DateTime = e.DateTime.toString();
                    a.push(e.DateTime.substring(e.DateTime.indexOf('(') + 1, e.DateTime.indexOf(')')) * 1 + getTime8h); //获取数字时间戳
                    a.push(str);
                   // console.log("a:"+a);
                    new_data.push(a);
                });
                var nameTrue = nameParents[i]+name;
                seriesOptions[i] = {

                    name: nameTrue,
                    data: new_data,
                    id: 'dataseries' + i,

                    events: { click: function (e) { editAnn(Highcharts.dateFormat('%Y-%m-%d %H:%M', e.point.x)); } }
                    //tooltip: { valueDecimals: 2 }
                };
                
               
                //以下构造注解
                if (isdis_Note) {
                    var seriesOptionsData = new Array();
                    var k = 0;
                    data.forEach(function (e, j) {
                        if (e.Ann != null || e.Ann != undefined) {
                            e.DateTime = e.DateTime.toString();
                            seriesOptionsData[k++] =
                        {
                            x: e.DateTime.substring(e.DateTime.indexOf('(') + 1, e.DateTime.indexOf(')')) * 1 + getTime8h,
                            title: '注',

                            text: '<span style="color:{point.color}">●</span><b>' + name + '</b><br/><b>注解：</b>' + e.Ann


                        }
                         
                        }
                    });
                    if (!isEmpty(seriesOptionsData)) {
                        seriesOptions[n + names.length] = {
                            type: 'flags',
                            name: name,
                            data: seriesOptionsData,
                            onSeries: 'dataseries' + i,
                            shape: 'squarepin',
                            color: Highcharts.getOptions().colors[i],
                            events: { click: function (e) { delAnn(Highcharts.dateFormat('%Y-%m-%d %H:%M', e.point.x), e.currentTarget.name, e.point.text); } }

                        };
                        n++
                    }
                }
                
                // As we're loading the data asynchronously, we don't know what order it will arrive. So
                // we keep a counter and create the chart when all the data is loaded.
                seriesCounter += 1;
                if (seriesCounter == names.length) {
                    seriesCounter = 0;
                    createChart();
                    loadingClose();
                    //if (!isEmpty(old_names)) names = old_names;
                }
            },
            error: function (er) {
                loadingClose();
                layer.alert(loadingOutTime, {
                    title: layerTitle,
                    icon: 4,
                    skin: 'layer-ext-seaning'
                })
            }
        });
    });
    
    loadingClose();
};

//-----------------------------------------注解操作----------------------------------------------+
//编辑\添加注解
var editAnn = function (_editAnnTime) {
    editAnnTime = _editAnnTime;
    loginValidation(getPermission, editAnnSave);
};
var editAnnSave = function () {
    var editAnnHtml = '<form class="form-horizontal" action="">';
    editAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">注解事件时间：</label><div class="col-sm-9"><span class="form-control">' + editAnnTime + '</span></div></div>';
    editAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">选择表名：</label><div class="col-sm-9"><select class="form-control required" id="tb" onblur="validation()">';
    editAnnHtml += '<option value ="">==请选择==</option>';
    if (names != null || names.length <= 0)
        names.forEach(function (e, i) {
            editAnnHtml += '<option value ="' + e + '">' + e + '</option>';
        });
    editAnnHtml += '</select></div></div>';
    editAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">注解内容：</label><div class="col-sm-9"><textarea class="form-control required" id="Ann" onblur="validation()"></textarea></div></div></form>';
    layer.confirm(editAnnHtml, {
        area: ['500px', '300px'],
        btn: ['确定', '取消'],
        title: layerTitle + "·注解添加"
    }, function () {
        if (validation()) {
            $.ajax({
                type: "Post",
                url: "../Server/IndexDo.aspx/SaveAnnEditData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"_DateTime":"' + editAnnTime + '","tb":"' + $("#tb").val() + '","Ann":"' + $("#Ann").val() + '"}',
                success: function (data) {
                    if (!isEmpty(data.d)) {
                        layer.msg(AnnAddMsg, {
                            icon: 1,
                            time: 1000,
                            end: function () {
                                getStockData();
                            }
                        });
                    } else { opException(); }
                },
                error: function (err) {
                    opException();
                }
            });
        }
    }, function () { });
};

//删除注解
var delAnn = function (_editAnnTime, _tbName, text) {
    editAnnTime = _editAnnTime;
    tbName = _tbName;
    AnnText = text.substring(text.lastIndexOf('</b>') + 4);
    loginValidation(getPermission, delAnnDo);
};
var delAnnDo = function () {
    var delAnnHtml = '<form class="form-horizontal" action="">';
    delAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">注解事件时间：</label><div class="col-sm-9"><span class="form-control">' + editAnnTime + '</span></div></div>';
    delAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">选择表名：</label><div class="col-sm-9"><span class="form-control" id="tb">' + tbName + '</span></div></div>';
    delAnnHtml += '<div class="form-group"><label class="col-sm-3 control-label">注解内容：</label><div class="col-sm-9"><textarea class="form-control required" id="Ann" onblur="validation()">' + AnnText + '</textarea></div></div></form>';
    layer.open({
        content: delAnnHtml,
        closeBtn: false,
        area: ['500px', '300px'],
        btn: ['编辑', '删除', '取消'],
        title: layerTitle + "·编辑/删除注解",
        yes: function () {
            if (validation()) {
                $.ajax({
                    type: "Post",
                    url: "../Server/IndexDo.aspx/SaveAnnEditData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"_DateTime":"' + editAnnTime + '","tb":"' + $("#tb").text() + '","Ann":"' + $("#Ann").val() + '"}',
                    success: function (data) {
                        if (!isEmpty(data.d)) {
                            layer.msg(AnnEditedMsg, {
                                icon: 1,
                                time: 1000,
                                end: function () {
                                    getStockData();
                                }
                            });
                        } else { opException(); }
                    },
                    error: function (err) {
                        opException();
                    }
                });
            }
        }, cancel: function () {
            if (validation() || true) {
                $.ajax({
                    type: "Post",
                    url: "../Server/IndexDo.aspx/DelAnn",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"_DateTime":"' + editAnnTime + '","tb":"' + $("#tb").text() + '"}',
                    success: function (data) {
                        if (!isEmpty(data.d)) {
                            layer.msg(AnnDeleteMsg, {
                                icon: 1,
                                time: 1000,
                                end: function () {
                                    getStockData();
                                }
                            });
                        } else { opException(); }
                    },
                    error: function (err) {
                        opException();
                    }
                });
            }
        }
    });
};

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
function delCookie(name)
{
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval=getcookie(name);
    if(cval!=null) 
        document.cookie= name + "="+cval+";expires="+exp.toGMTString();
}
if (getcookie('username') != null)
    //document.write('用户名：' + getcookie('username'));
    console.log("用户名：" + getcookie('username'));
else
    //  document.write('<a href="/web/login.html">登录</a>');
    $(window).attr('location', '/web/login.html');

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
                if (data.d != opAnnPermission) { layer.msg('您没有权限执行注解操作！', { time: 2000 }); return; }
                callback();
            } else { opException(); }
        },
        error: function (err) {
            opException();
        }
    });
};
