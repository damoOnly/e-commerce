<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<!--<div class="footer" id="footer">
<div class="footer_ad1">
    <div class="footer_ad">
        <Hi:Common_ImageAd runat="server" AdId="9" />
    </div></div>
    
    <div class="footer2">
    <div class="footer2_1">
    <div class="footer_logo">  <Hi:Common_ImageAd runat="server" AdId="10" /></div>
    <div class="footer1_p1">
        <div class="guide">
            <ul>
                <Hi:Common_Help runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_Help.ascx" />
            </ul>
        </div>
        
    </div></div>
    <div class="footer_link">
        <Hi:Common_FriendLinks runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_FriendLinks.ascx" />
    </div>
    <div class="footer_custom">
        <div>
            <Hi:PageFooter ID="PageFooter1" runat="server" />
        </div>
        <div>
            <Hi:CnzzShow ID="CnzzShow1" runat="server" />
        </div>
    </div>
</div></div>-->
<div class="footer">
 <!-- <div class="state-ment">
     <ul class="fix">
        <li>
            <div class="st-box fix">
                <div class="st-img s1"></div>
                <div class="st-info">
                    <p><strong>国内领先</strong></p>
                    <p>优秀可信赖</p>
                </div>
            </div>
        </li>
        <li>
            <div class="st-box fix">
                <div class="st-img s2"></div>
                <div class="st-info">
                    <p><strong>安全保障</strong></p>
                    <p>国检监督入仓</p>
                </div>
            </div>
        </li>
        <li>
            <div class="st-box fix">
                <div class="st-img s3"></div>
                <div class="st-info">
                    <p><strong>正品保障</strong></p>
                    <p>100%海外进口</p>
                </div>
            </div>
        </li>       
        <li>
            <div class="st-box fix">
                <div class="st-img s4"></div>
                <div class="st-info">
                    <p><strong>优质服务</strong></p>
                    <p>全程优质服务</p>
                </div>
            </div>
        </li>
        <li>
            <div class="st-box fix">
                <div class="st-img s5"></div>
                <div class="st-info">
                    <p><strong>配送快捷</strong></p>
                    <p>国际物流配送</p>
                </div>
            </div>
        </li>
    </ul>
 </div> -->
 <style>
.guarantee{
    background-color: #009de0;
}
.guart{
    width: 1190px; 
    margin:0 auto;
}
.foot,.footer{
    background-color: #fff;
}
.foot-guide .wechat img{
    width: 110px;
    height: 110px;
    margin-left: 25px;
}
 </style>
 <div class="guarantee">
    <div class="guart">
        <img src="/templates/master/haimei/images/guarantee.jpg">
    </div>
</div>
 <div class="foot">
 <div class="footer_ad foot-guide">
    <div class="g-h25"></div>
			<Hi:Common_Help runat="server" />
  </div>
  <div class="g-h25"></div>
 </div>
 <div class="footer_ad g-tc foot-link">
	 <Hi:Common_FriendLinks runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_FriendLinks.ascx" /></div>
<div class="g-h10">
</div>
<div class="footer_ad foot-icp g-tc">
     <Hi:PageFooter ID="PageFooter2" runat="server" />
	</div>  
</div>
  <script type="text/javascript">
      var _hmt = _hmt || [];
      (function () {
          var hm = document.createElement("script");
          hm.src = "//hm.baidu.com/hm.js?4d35a99853f12e55bb76639cc97344ee";
          var s = document.getElementsByTagName("script")[0];
          s.parentNode.insertBefore(hm, s);
      })();
    </script>