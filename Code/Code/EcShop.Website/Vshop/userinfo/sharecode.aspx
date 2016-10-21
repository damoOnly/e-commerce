<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sharecode.aspx.cs" Inherits="EcShop.UI.Web.Admin.sharecode" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1.0, user-scalable=1;"   name="viewport" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="/templates/vshop/haimei/script/jquery-1.11.0.min.js"></script>
    <title>我的分享</title>
    <style>
        body, article, section, aside div span img {
            padding:0px;margin:0px;border:none;
        }
        .shopshare {
            width:100%;
            position:relative;

        }
            .shopshare aside {
                width:100%;height:100%;background:rgba(0,0,0,0.7);position:fixed;top:0px;right:0px;
            }
        .shopshare aside img {
                float:right;margin-right:35px; opacity:1;width:60%;margin-top:20px;
            }
    </style>
    
</head>
<body style="padding:0px;background:#fc5e4b;" class="shopshare">
   <%-- <form id="form1" runat="server">
    <div>
        分享成功！
    </div>
    </form>--%>
  <article class="codepage" style="padding-bottom:72px;">
    <section style="position:relative;">
      <p style="text-align:center;position:absolute;color:#fd5e4b;width:100%;font-size:18px;line-height:36px;margin:5px 0px;"><span id="username"><asp:Literal runat="server" ID="litusername" /></span>邀请您领取海美现金大礼</p>
      <img src="../resource/default/image/code/beshare_02.jpg" width="100%"/>
    </section>
    <section style="position:relative;">
      <img src="../resource/default/image/code/beshare_03.jpg" width="100%"/>
    </section>
    <section style="text-align:center;padding:0px 20px;">
        <p style="color:#fff;font-size:16px;">长按二维码扫描关注“海美生活网”微信公众号，绑定手机，
输入我的<span style="color:#ffe100;font-size:18px;">邀请码<asp:Literal runat="server" ID="litRecemmendCode" /></span>，即可免费领取大礼包哦~</p>
    </section>
    <section>
      <a href="javascript:void(0)" id="gotoapp"><img src="../resource/default/image/code/beshare_05.jpg" width="100%"/></a>
    </section>
   </article>
   <aside style="display:none;" onclick="javascript:$(this).hide()"><img src="../../Templates/vshop/haimei/images/sharetext.png" /></aside>
</body>
   <script type="text/javascript"> 
       var share = {
           onload: function () {
               var u = navigator.userAgent;
               if (u.indexOf('Android') > -1 || u.indexOf('Linux') > -1) {
                   window.location.href = "http://app.qq.com/#id=detail&appid=1105008390";
               } else if (u.indexOf('iPhone') > -1) {
                   window.location.href = "https://itunes.apple.com/us/app/hai-mei-sheng-huo/id1065742573?l=zh&ls=1&mt=8";
               } else {
                   alert("没有合适该设备的APP");
               }
           },
           showmask: function () {
               $(".shopshare aside").show();
           },
           winload:function() {
               var ua = navigator.userAgent.toLowerCase();
               if (ua.match(/MicroMessenger/i) == "micromessenger") {
                   $("#gotoapp").click(function () {
                       share.showmask();
                      })
                  }
               else { share.onload() }
       }
       };
       $(function () { share.winload() });
    </script>
</html>
