var columnsArray = [];
var table;
var oTableInit;

var tagArray = [["ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "ML", "qpgs", "XCYZ"], ["泵站", "1HBENG_YUNXING", "2HBENG_YUNXING", "3HBENG_YUNXING", "4HBENG_YUNXING", "1HGESHAN_YUNXING", "QIANYEWEI", "HOUYEWEI", "PH", "COD", "NH3N", "TP", "1HBENG_SSLL", "2HBENG_SSLL", "3HBENG_SSLL", "4HBENG_SSLL"]];
var valueName = ["泵站", "1号泵", "2号泵", "3号泵", "4号泵", "格栅", "前液位</br>(m）", "后液位</br>(m)", "PH", "COD</br>(mg/L)", "NH3N</br>(mg/L)", "TP</br>(mg/L)", "1号瞬时</br>(m³/h)", "2号瞬时</br>(m³/h)", "3号瞬时</br>(m³/h)", "4号瞬时</br>(m³/h)"];//列名，注意与tagArray[1]对应
var pumpName = ["赵重路", "通波塘", "华青路", "新业路", "新区路", "赵巷A", "赵巷B", "赵巷C", "金星路", "外青松", "新金路", "汇金路", "民乐路", "青浦公司", "新城一站"];//行名，注意与tagArray[0]对应
var dataOne;
dataOne = {};

$(function () {
    //1.初始化Table
    var strA = "pumpTable";
    oTable = new TableInit(strA);
    oTable.Init();

})

//表格相关的方法----初始化、更新
var TableInit = function (tableName) {
    oTableInit = new Object();
    oTableInit.update = function (data) {
        $("#" + tableName).bootstrapTable("load", data)
    };
    oTableInit.append = function (data) {
        $("#" + tableName).bootstrapTable('append', data);//_data----->新增的数据
    }


    //初始化Table
    oTableInit.Init = function () {

        for (var i = 0; i < valueName.length; i++) {//Object.keys(obj) 获取key名称
            var property = valueName[i];//id   username
            var proName = valueName[i];
            if (property[property.length-1] == "泵" || property[property.length-1]=="栅") {
                columnsArray.push({
                    field: property,
                    title: proName,
                    formatter: function (value, row, index) {
                        var a = "";
                        if (value == "True") {
                            a = '<span style="color:#ff0000">' + "运行" + '</span>';
                        } else if (value == null) {
                            a = null;
                        } else if (value == "False") {
                            a = '<span style="color:#0000ff">' + "停止" + '</span>';
                        }
                        else {
                            a = '<span style="color:#0000ff">' + value + '</span>';
                        }
                        return a;
                    }
                });
            }
            else {
                columnsArray.push({
                    field: property,
                    title: proName
                });
            }
        };

        $('#' + tableName).bootstrapTable({
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 14,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            contentType: "application/x-www-form-urlencoded",
            strictSearch: true,
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行x
            //height: 700,                        //表格高度，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表

            columns: columnsArray,
            data: dataOne,
            //将表格设为可以选中行
            onClickRow: function (row, $element, filed) {
                $('.info').removeClass('info');//移除class
                $($element).addClass('info');//添加class

                var i = $element.data('index');//可通过此参数获取当前行号
                var j = $element.data();
                //获取用户信息，若拥有权限，则向服务器发送命令，若没有权限，则提示没有权限
                loginValidation(getPermission, commandPop);  //如果已经获取权限，则执行
                //alert(i + "," + row["泵站"] + "," + filed + ",");
                function commandPop() {
                    var strTag = row["泵站"] + "." + filed;
                    command(strTag);
                }
            },

            rowStyle: function (row, index) {
                var classesArr = ['success', 'info'];
                var strclass = "";
                if (index % 2 === 0) {//偶数行
                    strclass = classesArr[0];
                } else {//奇数行
                    strclass = classesArr[1];
                }
                return { classes: strclass };
            },//隔行变色
        });      

    };   


    return oTableInit;
}


