/***************************支持手动配置*********************************/
//配置需要显示的表名
var old_names = ['进水COD值', '进水NH3N值', '进水PH值', '三期1#DO', '三期2#DO', '三期3#DO', '三期4#DO', '三期MLSS', '三期出水COD值', '三期出水NH3N值', '三期出水PH值', '三期出水TN值', '三期出水TP值', '三期出水流量', '三期储泥池1#泥位', '三期储泥池2#泥位', '三期格栅后液位', '三期格栅前液位', '三期进水流量', '三期配水井液位', '三期脱水污泥流量', '一期1#DO', '一期1#MLSS', '一期1#剩余泵流量', '一期2#DO', ' 一期2#MLSS', ' 一期2#剩余泵流量', '一期3#DO', ' 一期4#DO', '一期5#DO', '一期6#DO', '一期出水COD值', '一期出水NH3N值', '一期出水PH值', '一期出水TN值', '一期出水TP值', '一期出水流量', '一期格栅后液位', '一期格栅前液位', '一期进水流量'];
var names = [];
var nameParents = [];
var nameSeries = [];
var layerTitle = logoText = "综合分析监控信息平台";
var opExceptionText = "执行操作时超时或出错，请稍后再试.";
var AnnAddMsg = '注解已添加！';
var AnnEditedMsg = '注解已编辑！';
var AnnDeleteMsg = '注解已删除！';
//注解操作权限
var opAnnPermission = "3";
/***********************************************************************/

/*********************************公用方法\变量**************************/
//zTree 选择表的菜单树设置
var setting = {
    check: {
        enable: true,
        chkboxType: { "Y": "", "N": "" }
    },
    view: {
        dblClickExpand: false
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        beforeClick: beforeClick,
        onCheck: onCheck
    }
};
function beforeClick(treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo");
    zTree.checkNode(treeNode, !treeNode.checked, null, true);
    return false;
}
function onCheck(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
			nodes = zTree.getCheckedNodes(true),
        v = "";
    u = "";
    w = "";
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
        u += nodes[i].getParentNode().name + ",";
        w +=(nodes[i].getParentNode().name + "的" + nodes[i].name) + ",";
    }
    if (v.length > 0) {
        v = v.substring(0, v.length - 1);//子节点
        u = u.substring(0, u.length - 1);//父节点
        w = w.substring(0, w.length - 1);//曲线名字集合
    }
    var nameSum = [v.split(','), u.split(','), w.split(',')];
    console.log("nameSum :" + JSON.stringify(nameSum));
    filterMenu(nameSum);
}
var editAnnTime = null;
var tbName = null;
var AnnText = null;
//获得8个小时的时间戳
var getTime8h = new Date('2015-01-01 08:00:00').getTime() - new Date('2015-01-01 00:00:00').getTime();
var isdis_Note = true;
var interval = null;
var layerLoading = null;
var loading = function () { layerLoading = layer.load(1, { shade: [0.5, '#fff'] }); };
var loadingClose = function () { layer.close(layerLoading); };
//var fun_NoOpen = function () { layer.alert("该功能暂时还未建设！", { title: layerTitle, icon: 4, skin: 'layer-ext-seaning' }); };
var fun_NoOpen = function () {  };
var opException = function () { loadingClose(); layer.alert(opExceptionText, { title: layerTitle, icon: 4, skin: 'layer-ext-seaning' }); };
var validation = function () { var v = true; $(".required").each(function (e, i) { if (isEmpty($(this).val())) { $(this).addClass("error"); v = false; } else $(this).removeClass("error"); }); return v; };
var isEmpty = function (e) { if (e == null || e == undefined || e == "" || e.length <= 0) return true; return false; };
var layerAlert = function (msg) { layer.alert(msg, { icon: 3, title: layerTitle, skin: 'layer-ext-moon' }); };
//获得两个数组的交集
var arrayIntersection = function (a, b) {
    var ai = 0, bi = 0;
    var result = new Array();
    while (ai < a.length && bi < b.length) {
        if (a[ai] < b[bi]) { ai++; }
        else if (a[ai] > b[bi]) { bi++; }
        else /* they're equal */
        {
            result.push(a[ai]);
            ai++;
            bi++;
        }
    }
    return result;
}
//显示注解设置
var displaySetting = function (o) { isdis_Note = o.checked; loading(); getStockData(); };
//模式设置
var modelSetting = function (o) { fun_NoOpen(); };
/*******************************************************************/

/****************************HighStock setting****************************/
Highcharts.setOptions({
    global: {
        useUTC: true
    },
    /**
    * 语言配置
    * 
    * @param {array} months 配置月份语言
    * @param {array} shortMonths 配置短月份
    * @param {array} weekdays 配置星期
    * 
    */
    lang: {
        months: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        shortMonths: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一', '十二'],
        weekdays: ['星期天', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
        printChart: "打印绘图",
        downloadPNG: "导出PNG图",
        downloadJPEG: "导出JPEG图",
        downloadPDF: "导出PDF",
        downloadSVG: "导出SVG",
        contextButtonTitle: "打印报表"
    }
});
var seriesOptions = [],
        seriesCounter = 0,
        createChart = function () {
            $('#container').highcharts('StockChart', {
                /**
                * 版权信息配置
                * 
                * @param {boolean} enabled 是否显示版权信息
                * @param {string} href 版权信息所链接到的地址
                * @param {string} text 版权信息所显示的文字内容
                */
                credits: {
                    enabled: false
                },
                /**
                * 导出配置
                * 
                * @param {boolean} enabled 是否允许导出
                * @param {object} buttons 关于与导出和打印按钮相关的配置对象
                * @param {string} filename 导出文件的文件名
                * @param {string} type 默认导出文件的格式
                */
                exporting: {
                    enabled: true
                },
                rangeSelector: {
                    selected: 4
                },
                title: {
                    text: logoText
                },
                xAxis: {
                    // 如果X轴刻度是日期或时间，该配置是格式化日期及时间显示格式
                    dateTimeLabelFormats: {
                        millisecond: '%Y-%m-%d<br/> %H:%M:%S',
                        second: '%Y-%m-%d<br/> %H:%M:%S',
                        minute: '%Y-%m-%d<br/> %H:%M',
                        hour: '%Y-%m-%d<br/> %H:%M',
                        day: '%Y<br/> %m-%d',
                        week: '%Y<br/> %m-%d',
                        month: '%Y-%m',
                        year: '%Y'
                    }
                },
                yAxis: {
                    title: {
                        text: '数  据'
                    },
                    labels: {
                        align: "left", //"left", "center","right"
                        formatter: function () {
                            return (this.value > 0 ? ' + ' : '') + this.value + '';
                        }
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: 'silver'
                    }]
                },
                rangeSelector: {
                    buttons: [{
                        type: 'hour',
                        count: 1,
                        text: '小时'
                    }, {
                        type: 'day',
                        count: 1,
                        text: '一天'
                    }, {
                        type: 'all',
                        text: '所有'
                    }],
                    inputEnabled: false,
                    selected: 0
                },
                scrollbar: {
                    barBackgroundColor: 'gray',
                    barBorderRadius: 7,
                    barBorderWidth: 0,
                    buttonBackgroundColor: 'gray',
                    buttonBorderWidth: 0,
                    buttonBorderRadius: 7,
                    trackBackgroundColor: 'none',
                    trackBorderWidth: 1,
                    trackBorderRadius: 8,
                    trackBorderColor: '#CCC'
                },
                plotOptions: {
                },
                tooltip: {
                    dateTimeLabelFormats: {
                        millisecond: '%Y-%m-%d %H:%M:%S',
                        second: '%Y-%m-%d %H:%M:%S',
                        minute: '%Y-%m-%d %H:%M',
                        hour: '%Y-%m-%d %H:%M',
                        day: '%Y %m-%d',
                        week: '%Y %m-%d',
                        month: '%Y-%m',
                        year: '%Y'
                    }
                },
                series: seriesOptions
            });
        };

/*

                    series: {
                        compare: 'percent'
                    }
                    */
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

//初始化加载数据时间段
var start_time = toDayStart = new Date().format("yyyy-MM-dd 00:00");
var end_time = toDayCurr = new Date().format("yyyy-MM-dd hh:mm");
var toDayCurrTime = function () {
    var end_time = time = new Date().format("yyyy-MM-dd hh:mm");
    try {
        $("#end_time").val(time);
    } catch (Exception) {
        return;
    }
};
//时间段培改变时更新数据加载时间段
var timeSetting = function (o) { if (o.id == "start_time") start_time = o.value; else end_time = o.value; getStockData(); };

//系统当前时间设置
var systemCurrTime = function () {
    var time = new Date().format("星期w， yyyy-MM-dd hh:mm:ss ");
    try {
        $("#systemCurr_Time").text(time);
    } catch (Exception) {
        return;
    }
};

