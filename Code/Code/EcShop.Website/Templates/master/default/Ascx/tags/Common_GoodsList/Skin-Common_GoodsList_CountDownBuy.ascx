﻿<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>


<li>
    <div class="buy_product">
        <div class="buy_time">
            <span id='<%# "htmlspan"+Eval("ProductId") %>'></span>
                <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
        </div>
        <div class="buy_pic">
            <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true"
                ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'
                ImageLink="true">
            <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" /></Hi:ProductDetailsLink>
        </div>
        <div class="buy_name">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" IsCountDownProduct="true"
                    ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'
                    ImageLink="false" />
        </div>
        <div class="buy_price">
            <em>￥<span><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>' /></span></em>
            <p>
               <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'
                    ProductId='<%# Eval("ProductId") %>' ImageLink="true" Text="去看看">
               </Hi:ProductDetailsLink>
            </p>
        </div>
        <div class="buy_andere">
            <p>
                原价：<del>￥<Hi:FormatedMoneyLabel runat="server" ID="lblOldPrice" Money='<%# Eval("SalePrice") %>' /></del></p>
            <em>限购数量：<span><asp:Label runat="server" ID="lblCount" Text='<%# Eval("MaxCount") %>' /></span></em></div>
    </div>
</li>
<div style="display: none;">
    结束时间：<Hi:FormatedTimeLabel runat="server" ID="lblEndTime" Time='<%# Eval("EndDate") %>' /></div>
