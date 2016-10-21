<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usercode.aspx.cs" Inherits="EcShop.UI.Web.Admin.usercode" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1;" name="viewport" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="/templates/vshop/haimei/script/jquery-1.11.0.min.js"></script>
     <script src="/templates/vshop/haimei/script/jquery.lazyload.min.js"></script>
    <script src="/templates/vshop/haimei/script/main.js?v=20160216"></script>
    <asp:Literal runat="server" id="litJSApi" />
    <title>我的邀请码</title>
    <style>
        body, article, section, aside div span img {
            padding: 0px;
            margin: 0px;
            border: none;
        }

        .shopshare {
            width: 100%;
            position: relative;
        }
            .shopshare aside {
                width: 100%;
                height: 100%;
                background: rgba(0,0,0,0.7);
                position: fixed;
                top: 0px;
                right: 0px;
            }

                .shopshare aside img {
                    float: right;
                    width:60%;
                    margin-top:20px;
                    margin-right:38px;
                    opacity: 1;
                }
        .table-code {border-left:1px solid #f48678;border-top:1px solid #f48678;width:88%;text-align:center;margin:0 auto;border-spacing:0px;line-height:24px;
        }
            .table-code td {border-bottom:1px solid #f48678;border-right:1px solid #f48678;border-spacing:0px;
            }
    </style>

</head>
<body style="padding:0px; background: #fc5e4b;" class="shopshare ">

    <article class="codepage" style="padding-bottom: 52px;">
        <section>
            <img src="../resource/default/image/code/codepage1.jpg" width="100%" style="float:left;" />
        </section>
        <section style="position: relative;clear:both;">
            <img src="../resource/default/image/code/codepage2.jpg" width="100%" />
            <div style="position: absolute; width: 57.6%; font-size: 18px; text-align: center; height: 3.4rem; line-height: 3.3rem; left: 50%; margin-left: -28.8%; top: 20px;">
                <div>
                    <img src="../resource/default/image/code/applycode.png" width="100%" /></div>
                <div style="position: absolute; width: 100%; font-size:20px; text-align: center; height: 3.4rem; line-height: 3.3rem; left: 0; top: 0;">
                    <span style="color: #fff;">邀请码</span>&nbsp;&nbsp;<span id="currecemmendCode" style="color: #ffe100;"><asp:Literal runat="server" ID="litRecemmendCode" /></span>
                    <input type="hidden" id="username" runat="server" />
                    <input type="hidden" id="vuserid" runat="server" />
                </div>
            </div>
        </section>
        <section>
            <img src="../resource/default/image/code/codepage3.jpg" width="100%" />
        </section>
        <section style="text-align: center;">
            <a href="javascript:void(0)" onclick="f1()">
                <img src="../resource/default/image/code/applybtn.png" width="53.6%" /></a>
        </section>
        <section>
            <img src="../resource/default/image/code/codepage4.jpg" width="100%" />
        </section>
        <section>
            <a href="http://mp.weixin.qq.com/s?__biz=MzIzNDEzMTU2Mg==&mid=403052816&idx=1&sn=f81caeedb8ae8d13d0c6f078894b0bc8&scene=0&previewkey=YM875HbcCBhUDXfWNd25jMNS9bJajjJKzz%2F0By7ITJA%3D#wechat_redirect">
                <img src="../resource/default/image/code/codepage5.jpg" width="100%" /></a>
        </section>
        <section style="width:88%;margin:0 auto;color:#fff;">
              <p style="padding-bottom:10px;overflow:hidden;"><span style="float:left;width:7%;"><img src="../resource/default/image/code/checked.png" style="height:16px;"/></span><span style="float:left;width:93%;">您共邀请了<span style="color:#fff100"><%--1856--%><asp:Literal runat="server" ID="litTotals" /></span>位好友注册，您在本周排名第<span style="color:#fff100"><%--225--%>
                  <asp:Literal runat="server" ID="litWeekLevel" />                                                                                                                                                                                                                                                                                                                        </span>名！</span></p>
              <p style="font-size:12px;padding-left:7%">最新注册前10位信息如下：</p>
              <table class="table-code">
                 <%-- <tr><td width="50%">135****4564</td><td  width="50%">135****4564</td></tr>
                  <tr><td>135****4564</td><td>135****4564</td></tr>
                  <tr><td>135****4564</td><td>135****4564</td></tr>
                  <tr><td>135****4564</td><td>135****4564</td></tr>
                  <tr><td>135****4564</td><td>135****4564</td></tr>--%>
                  <asp:Literal runat="server" ID="litRecommendList" />
              </table>
        </section>
    </article>
    <aside style="display:none" onclick="javascript:$(this).hide()"><img src="../resource/default/image/code/shadow.png" width="100%" /></aside>
</body>
    <script>
        var nowtime = new Date();
        if (nowtime > new Date(Date.parse("2016/3/5"))) {
            wx.ready(function () {
                wx.onMenuShareAppMessage({
                    title: '送你50元现金礼包，免税特价买全球大牌！', // 分享标题
                    desc: '海美生活特卖，全球零食、美妆和母婴用品，100%正品！', // 分享描述
                    link: 'http://' + window.location.hostname + '/Vshop/UserInfo/sharecode.aspx?VUserId=' + $("#vuserid").val() + '', // 分享链接
                    imgUrl: 'http://' + window.location.hostname + '/templates/vshop/haimei/resource/default/image/lg-logo.png', // 分享图标
                    type: 'link', // 分享类型,music、video或link，不填默认为link
                    success: function () {
                        alert('你已分享成功')
                    },
                    cancel: function () {
                        alert('你已取消分享')
                    }
                });
                wx.onMenuShareTimeline({
                    title: '送你50元现金礼包，免税特价买全球大牌！', // 分享标题
                    desc: '海美生活特卖，全球零食、美妆和母婴用品，100%正品！', // 分享描述
                    link: 'http://' + window.location.hostname + '/Vshop/UserInfo/sharecode.aspx?VUserId=' + $("#vuserid").val() + '', // 分享链接
                    imgUrl: 'http://' + window.location.hostname + '/templates/vshop/haimei/resource/default/image/lg-logo.png', // 分享图标
                    success: function () {
                        alert('你已分享成功')
                    },
                    cancel: function () {
                        alert('你已取消分享')
                    }
                });
            });
            wx.checkJsApi({
                jsApiList: ['chooseImage'], // 需要检测的JS接口列表，所有JS接口列表见附录2,
                success: function (res) {
                    // 以键值对的形式返回，可用的api值true，不可用为false
                    // 如：{"checkResult":{"chooseImage":true},"errMsg":"checkJsApi:ok"}
                }
            });
            function f1() {
                $(".shopshare aside").show();
            }
        } else {
            wx.ready(function () { wx.hideOptionMenu(); });
            $("#currecemmendCode").hide();
            $("<div style=' width: 100%; height: 100%;background:#fc5e4b;position: fixed;top: 0px;right: 0px;'><p style='text-align:center;font-size:20px;color:#fff;padding-top:60px;line-height:60px;'>亲，本年度最豪气的大礼正在路上，<br/>3月5日10:00正式为你揭晓，敬请期待哦！</p></div>").appendTo("body");
        }
</script>
</html>
