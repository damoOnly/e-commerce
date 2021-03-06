﻿<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<div class="footer" id="footer">
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
</div></div>

</body>
</html> 