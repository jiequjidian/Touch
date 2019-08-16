var wsUrl = "ws://10.0.0.146:6690/WsServices";

if (wsc == null) {
    var wsc = new WebSocket(wsUrl);
}



//二维数组对应为表中的行/列----------bootstrap所需数据
var tagArray = [["ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "ML", "qpgs", "XCYZ"], ["泵站", "1HBENG_YUNXING", "2HBENG_YUNXING", "3HBENG_YUNXING", "4HBENG_YUNXING", "1HGESHAN_YUNXING", "QIANYEWEI", "HOUYEWEI", "PH", "COD", "NH3N", "TP", "1HBENG_SSLL", "2HBENG_SSLL", "3HBENG_SSLL", "4HBENG_SSLL"]];
var valueName = ["泵站", "1号泵", "2号泵", "3号泵","4号泵", "格栅", "前液位</br>(m）", "后液位</br>(m)", "PH", "COD</br>(mg/L)", "NH3N</br>(mg/L)", "TP</br>(mg/L)", "1号瞬时</br>(m³/h)", "2号瞬时</br>(m³/h)", "3号瞬时</br>(m³/h)", "4号瞬时</br>(m³/h)"];//列名，注意与tagArray[1]对应
var pumpName = ["赵重路", "通波塘", "华青路", "新业路", "新区路", "赵巷A", "赵巷B", "赵巷C", "金星路", "外青松", "新金路", "汇金路", "民乐路", "青浦公司", "新城一站"];//行名，注意与tagArray[0]对应
var dataOne;
dataOne = {};
//创建一个二维数组
var dataArray1 = new Array();         //先声明一维
for (var i = 0; i < tagArray[0].length; i++) {
    dataArray1[i] = new Array(i);
}
//=====================管网图所需数据====================================================
var messageStr;
var elementId = ["ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "qpgs", "XCYZ"];
//var elementId = tagArray[0];
var water = ["QIANYEWEI", "HOUYEWEI"];
var waterNum = ["前液位：", "后液位："];
var yewei = ["", ""];
var bengState = ["1HBENG_YUNXING", "2HBENG_YUNXING", "3HBENG_YUNXING", "4HBENG_YUNXING"]

//$(function () {
//    initWs();


//})

function initWs() {
    //if (wsc.readyState != 1) {
    //    wsc.close();
       
    //    startWs();
    //}
    startWs();
}

function startWs() {


    wsc.onopen = function (evt) {
        onOpen(evt)
    };
    wsc.onclose = function (evt) {
        onClose(evt)
    };
    wsc.onmessage = function (evt) {
        onMessage(evt)
    };
    wsc.onerror = function (evt) {
        onError(evt)
    };

}
//websocket打开事件
function onOpen(evt) {
    writeToScreen("连接成功");
    alert("连接成功");
}
//websocket关闭事件
function onClose(evt) {
    console.log("连接断开了");
}
//websocket消息到达事件
function onMessage(evt) {

    
    var dataStr = JSON.parse(evt.data);
    console.log("消息到达");
    //以nameID为依据，将数据按顺序放入二维数组
    for (let c1 in dataStr) {//遍历消息中的每一个元素
        for (let c2 in elementId) {
            //var elementId = ["ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "qpgs", "XCYZ"];
            ////var elementId = tagArray[0];
            //var water = ["QIANYEWEI", "HOUYEWEI"];
            //var waterNum = ["前液位：", "后液位："];
            //var yewei = ["", ""];
            //var bengState = ["1HBENG_YUNXING", "2HBENG_YUNXING", "3HBENG_YUNXING", "4HBENG_YUNXING"]
            if (dataStr[c1].ID1 == elementId[c2]) {

                //=================管网图===============================

                for (let c3 in water) {
                    if (water[c3] == dataStr[c1].ID2) {
                        let a = dataStr[c1].Value;
                        var res = a.indexOf('.');
                        a = a.substring(0, res + 3);
                        yewei[c3] = waterNum[c3].toString() + a;
                        console.log(dataStr[c1].ID1 + ":  " + yewei);
                    }
                }
                try {
                    // var elementWho = document.getElementById[elementId[c2].toString()];
                    var elementIdStr = elementId[c2].toString();

                    var textStr = yewei[0] + "\n" + yewei[1] + "\n";

                    var elementMember = document.getElementById(elementIdStr);
                    elementMember.innerText = textStr;
                }
                catch (err) {
                }

            }


            //elementId[c2].innerText =dataStr[c1].Value;    
            for (let b3 in bengState) {
                if (bengState[b3] == dataStr[c1].ID2) {
                    var num = parseInt(b3) + 1;
                    var str = dataStr[c1].ID1 + "_" + num.toString();
                    var stra = "";
                    let a = dataStr[c1].Value;
                    if (a != "") {
                        switch (a) {
                            case "True":
                                stra = "";
                                stra = "../image/yunxing.png";
                                break;
                            case "False":
                                stra = "";
                                stra = "../image/tingzhi.png";
                                break;
                            default:
                                stra = "";
                                stra = "../image/guzhang.png";
                                break;
                        }
                        try {
                            document.getElementById(str).src = stra;
                        }
                        catch (err) {

                        }

                    }
                }
            }
                //=================管网图结束============================
        }
        for (let c2 in tagArray[0]) {//遍历列表中所有泵站


           

                //=================bootstrap===========================================================
                //dataArray1[c2][0] = '\"' + "泵站" + '\"' + ':' + '\"' + pumpName[c2] + '\"';
            if (dataStr[c1].ID1 == tagArray[0][c2]) {
                dataArray1[c2][0] = '\"' + "泵站" + '\"' + ':' + '\"' + pumpName[c2] + '\"';
                for (let a3 in tagArray[1]) {
                    if (dataStr[c1].ID2 == tagArray[1][a3]) {//将PH、COD等数据存入dataFree
                        //let a = '\"' + dataStr[c1].Value + '\"';
                        let a = '\"' + dataStr[c1].Value + '\"';
                        if (a != "") {
                            var c = a.split('.');
                            if (c[1] != null) {
                                if (c[1].length > 2) {
                                    a = c[0] + '.' + c[1].substring(0, 2) + '"';
                                }
                                else {
                                    a = c[0] + '"';
                                }
                            }

                        }
                        var singleStr = '\"' + valueName[a3] + '\"' + ':' + a;
                        // dataArray1[c2][c3] = singleStr.substring(1, singleStr.length-1);
                        dataArray1[c2][a3] = singleStr;
                        var ccc = "";
                    }
                }
            }
            }

            //=================================bootstrap结束=============================================================


        } 

    updateDableData(dataArray1);//将此次更新的数据解析后发送给liucheng.html页面
    // writeToScreen( evt.data);
    //messageStr = evt.data;

}


function updateDableData(dataArray) {
    var dataSum = "[";
    var dataSumZ = "";
    //将dataArray1中的数据以c2为排序标准，按顺序放入dataSum
    for (let t1 in dataArray) {
        var dataSum0 = "{";
        var dataSum1 = "";
        for (let t2 in dataArray[t1]) {
            dataSum1 += dataArray[t1][t2] + ',';
        }
        dataSum0 += dataSum1.substring(0, dataSum1.length - 1) + "},";
        if (dataSum0.substring(1, dataSum0.length - 2) != "") {
            dataSumZ += dataSum0;
        }
    }
    dataSum += dataSumZ.substring(0, dataSumZ.length - 1) + "]";
    dataOne = eval(dataSum);
    try {      
        //updateTable(dataOne);
        oTableInit.update(dataOne);
    }
    catch(err){ }
    
   
}



function changeImage(nameStr, url) {
    var ccc = document.getElementById(nameStr);
    ccc.src = url;
}
//websocket异常事件
function onError(evt) {
}
//websocket发送消息
function doSend(evt) {
    alert("即将发送: " + evt);
    //writeToScreen("发送消息成功: " + message); 
    if (wsc.readyState == 1) {
        wsc.send(evt);        
        alert("发送消息成功: " + evt + "/n+@@ websocket:" + wsc);
    }
}

window.addEventListener("load", initWs, false);  