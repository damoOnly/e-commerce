<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<div class="sw-hcon fix">
    <a href="javascript:void(0)" class="more-link" onclick="HuanBrandClicek()">
        换一批>>
    </a>
    <a class="see-all-brands" href="/Brand.aspx">全部品牌</a>
    <div class="sw-head">
        <ul class="fix">
            <asp:Repeater ID="rp_BrandTagguest" runat="server">
                <itemtemplate>
                    <li id="<%#Eval("BrandTagId") %>"><%#Eval("TagName") %></li>
                </itemtemplate>
            </asp:Repeater>
            <%-- <li class="selected">热门品牌</li>
                        <li>国际品牌</li>
                        <li>国内名牌</li>
                        <li>时尚名牌</li>
                        <li>热销品牌</li>--%>
        </ul>
    </div>
</div>