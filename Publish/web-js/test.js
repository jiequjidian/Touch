
var isEmpty = function (e) { if (e == null || e == undefined || e == "" || e.length <= 0) return true; return false; };
var loading = function () { layerLoading = layer.load(1, { shade: [0.5, '#fff'] }); };
var loadingClose = function () { layer.close(layerLoading); };
var layerTitle = logoText = "综合分析监控信息平台";
var validation = function () { var v = true; $(".required").each(function (e, i) { if (isEmpty($(this).val())) { $(this).addClass("error"); v = false; } else $(this).removeClass("error"); }); return v; };

function command(notation) {
    var loginHtml = '<form >';
    loginHtml += '<div ><label >泵站：</label><div ><input id="tag" type="text" value="' + "Channel2.ZXB.1HBENG_K" +'"></input></div></div>';
    loginHtml += '<div><label >命令：</label><div ><input  id="value" type="text" ></input></div></div></form>';
    //loginHtml += '<div><label >命令：</label><div ><button  id="value" type="text" style="backgrand-color:#00FF00" ></button></div></div></form>';
   // $("#tag").value = "11111";
    layer.confirm(loginHtml, {
        area: '400px',
        btn: '确认',
        title: layerTitle + "操作泵站"
    }, function () {
            dataStr = '[{"id": 2' + ',"key":"' + $("#tag").val() + '","value":"' + $("#value").val() + '"}]';
            //dataStr= {
            //    id: 2,
            //    key: $("#tag").val().toString(),
            //    value: $("#value").val().toString(),                
            //}
            //dataStr = JSON.stringify(dataStr);

            doSend(dataStr);
            layer.close(layer.index);
        
    });
}