<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="hot-pro fix" id="showhotsale" runat="server">
    <h2>热销商品</h2>
    <div class="hot-pro-con">
        <div class="hot-pro-list">
            <ul class="fix">
                <asp:Repeater ID="rptHotSale" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="/product_detail-<%# Eval("Url") %>.aspx" target="_blank" class="panic">
                                <span class="panic-name"><%# Eval("ShortDesc") %></span>
                                <span class="panic-des"></span>
                                <img src="<%# Eval("ImageUrl") %>">
                            </a>

                        </li>

                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</div>
