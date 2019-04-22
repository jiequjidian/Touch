function showCheck(a){
	var c = document.getElementById("myCanvas");
  var ctx = c.getContext("2d");
	ctx.clearRect(0,0,1000,1000);
	ctx.font = "80px 'Microsoft Yahei'";
	ctx.fillText(a,0,100);
	ctx.fillStyle = "white";
}
var code ;    
function createCode(){       
    code = "";      
    var codeLength = 4;
    var selectChar = new Array(1,2,3,4,5,6,7,8,9,'a','b','c','d','e','f','g','h','j','k','l','m','n','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z');      
    for(var i=0;i<codeLength;i++) {
       var charIndex = Math.floor(Math.random()*60);      
      code +=selectChar[charIndex];
    }      
    if(code.length != codeLength){      
      createCode();      
    }
    showCheck(code);
}
          
function validate () {
    var inputCode = document.getElementById("J_codetext").value.toUpperCase();
    var codeToUp=code.toUpperCase();
    if(inputCode.length <=0) {
      document.getElementById("J_codetext").setAttribute("placeholder","输入验证码");
      createCode();
      return false;
    }
    else if(inputCode != codeToUp ){
      document.getElementById("J_codetext").value="";
      document.getElementById("J_codetext").setAttribute("placeholder","验证码错误");
      createCode();
      return false;
    }
    else {
      window.open(document.getElementById("J_down").getAttribute("data-link"));
      document.getElementById("J_codetext").value="";
      createCode();
      return true;
    }

}
function SetCookie(name, value) {
    var Days = 0.1*1;  //cookie 将被保存一年
    var exp = new Date(); //获得当前时间
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000); //换成毫秒
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function UserVali() {
    //location.href="index.html";
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
                        location.href = "/web/Liucheng.html";
                        $("#userNameInfo").html(data.d[0].YongHu);
                        SetCookie("username", "hello");
                        //$.cookie('username', data.d[0].YongHu); // 存储 cookie 
                      
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
}