function loading(msg) {
    return layer.msg(msg,{icon:16,time:0,shade:[0.3, '#393D49']});
}

function alertSuccess(msg){
    layer.msg(msg,{icon:1});
}

function alertSuccessRedirect(msg,url,time){
    layer.msg(msg,{icon:1});
    setTimeout(function(){
        window.location.href = url;
    }, time);
}

function alertError(msg){
    layer.msg(msg,{icon:2});
}



//校验邮箱地址
function CheckMail(mail) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (filter.test(mail)) {
        return true;
    }
    else {
        return false;
    }
}
//校验固话号码
function checkPhone(str) {
    var re = /^0\d{2,3}-?\d{7,8}$/;
    if (re.test(str)) {
        return true;
    } else {
        return false;
    }
}
//校验手机号码
function checkMobile(str) {
    var re = /^1\d{10}$/
    if (re.test(str)) {
        return true;
    } else {
        return false;
    }
}
//校验登录账号
function checkUser(str) {
    var re = /^[a-zA-z]\w{5,15}$/;
    if (re.test(str)) {
        return true;
    } else {
        return false;
    }
}

//检验金额
function checkMoney(str) {
    var pt = /^(-)?(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/;
    if (pt.test(str)) {
        return true;
    } else {
        return false;
    }
}
