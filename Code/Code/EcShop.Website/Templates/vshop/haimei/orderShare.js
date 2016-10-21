var content = '晒晒更健康，晒是一种态度，秀出你的海美生活范儿，将晒单进行到底！';
var link = document.location.href;
var orderId = (link.split('?')[1]).split('=')[1];

function ShareOperator(shareType) {
    //分享操作
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "onMenuShare", ShareContent: content, Link: link, ShareType: shareType, OrderId: orderId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                alert("分享成功");
            }
            else if (resultData.Status == "-1") {
                alert("分享失败，请重试");
            }
            else {
                alert("分享失败, 请重试");
            }
        }
    });
}

wx.ready(function () {
    //1. 监听“分享到朋友圈”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareTimeline({
        title: content,
        trigger: function (res) {
            // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
            alert('用户点击分享到朋友圈');
        },
        success: function (res) {
            ShareOperator(1);
        },
        cancel: function (res) {
            alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
    //2. 监听“分享给朋友”，按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareAppMessage({
        title: content,
        trigger: function (res) {
            // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
            alert('用户点击发送给朋友');
        },
        success: function (res) {
            //分享操作
            ShareOperator(2);
        },
        cancel: function (res) {
            alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });

    //3. 监听“分享到QQ”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareQQ({
        title: content,
        trigger: function (res) {
            alert('用户点击分享到QQ');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //分享操作
            ShareOperator(3);
        },
        cancel: function (res) {
            alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });

    //4. 监听“分享到微博”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareWeibo({
        title: content,
        trigger: function (res) {
            alert('用户点击分享到微博');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //分享操作
            ShareOperator(4);
        },
        cancel: function (res) {
            alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
});