<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>


<li>
        <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true"
                ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'
                ImageLink="true">
            <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" /></Hi:ProductDetailsLink>
    <p class="purchase-title"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" IsCountDownProduct="true"
                    ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'
                    ImageLink="false" />
    </p>
    <p>
        <span class="stress">￥<Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>' /></span>
        <s>￥<Hi:FormatedMoneyLabel runat="server" ID="lblOldPrice" Money='<%# Eval("SalePrice") %>' /></s>
        <!-- <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'
                    ProductId='<%# Eval("ProductId") %>' ImageLink="true" Text="去看看">
               </Hi:ProductDetailsLink> -->
        <em>限购数量：<span><asp:Label runat="server" ID="lblCount" Text='<%# Eval("MaxCount") %>' /></span></em>
    </p>
    <p style="font-size:14px;color:#ff7800">
         <%# Eval("CurHours") %>点开始
        <!--<%# Eval("PlanCount") %>--> <!--<%# Eval("SaleCounts") %>-->
    </p>
    <p class="rush-time">
        <span id='<%# "htmlspan"+Eval("ProductId") %>'></span>
                <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
    </p>
    <a class="buy-now" hour="<%# Eval("CurHours") %>" isover="<%# Eval("IsOver") %>" href="/countdownproduct_detail-<%# Eval("ProductId") %>.aspx"><img src="/templates/master/haimei/images/<%# Eval("IsOver").ToString()=="1"?"sold_out.png":(DateTime.Parse(Eval("StartDate").ToString())>DateTime.Now)?"will_start.png":"buy_now.png" %>"></a>
    <div style="display: none; text-align: center;">
    结束时间：<Hi:FormatedTimeLabel runat="server" ID="lblEndTime" Time='<%# Eval("EndDate") %>' /></div>
</li>
