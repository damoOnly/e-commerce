<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>		
<div class="footer">
 <div class="state-ment">
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
