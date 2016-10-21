<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<asp:Repeater ID="rpActivity" runat="server">
    <ItemTemplate>
        <li>
            <div class="act-box">
                <div class="act-opentime"><%#  DateTime.Parse(Eval("StartDate").ToString()).Day %>点开抢</div>
                <div class="fix">
                    <div class="act-img">
                        <img src="/templates/vshop/haimei/resource/default/image/lazy.png" data-original="/templates/vshop/haimei/resource/default/image/temp/act.png" class="lazyload"></div>
                    <div class="act-info">
                        <div class="act-name">秒杀商品</div>
                        <div class="act-date" start-date="<%#Eval("StartDate") %>" end-date="<%#Eval("EndDate") %>">还剩：<span></span>天<span></span>时<span></span>分<span></span>秒</div>
                        <div class="fix mt20"><a class="buy-now">立即抢购</a> <span class="act-price">￥<span><%#Eval("MarketPrice") %></span></span> </div>
                    </div>
                </div>
            </div>
        </li>
    </ItemTemplate>
</asp:Repeater>
