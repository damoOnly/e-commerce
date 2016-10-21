<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="floor_hd">
    <div class="title cssEdite" type="title">
        <div>
            <span class="icon">
                <img src="/Templates/master/haimei/images/common/like2.png" /></span><span class="title">猜你喜欢</span>
        </div>
    </div>
    <div class="morelink tab-next" type="morelink"><em><a href="javascript:void(0)" onclick="HuanClicek()">换一批>></a></em> </div>
</div>
<div class="floor-main">
    <table class="fav-table floor-table gd-table bd-top-none" cellpadding="0" cellspacing="0">
        <tr class="border-top-none">
            <td>
            <asp:Repeater ID="rp_guest" runat="server">
                <itemtemplate>
                        <div class="gd-box" style="float:left;width:189px;margin:0px 10px;">                            
                            <div class="gd-info">
                                <div class="gd-name">
                                    <Hi:ProductDetailsLink runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink>
                                </div>
                                <div class="gd-price">￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></div>
                            </div>
                            <div class="gd-img">
                                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                        <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
                                </Hi:ProductDetailsLink>
                            </div>
                        </div>
                </itemtemplate>
            </asp:Repeater>
           </td>
        </tr>
    </table>
</div>