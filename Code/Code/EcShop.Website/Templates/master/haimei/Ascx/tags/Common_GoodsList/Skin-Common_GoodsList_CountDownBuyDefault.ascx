<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>

<li>
    <div class="buy_product">
        <div class="buy_time">
            <span id='<%# "htmlspan"+Eval("CurrNumber") %>'></span>
            <Hi:CountDownLeaveListTime runat="server" ID="LeaveListTime1" />
        </div>
        <div class="buy_pic">
            <a target="_blank" href='/CountDownProductsDetails.aspx?productId=<%# Eval("ProductId") %>&countDownId=<%# Eval("CountDownId") %>' > 
                <img src='<%#Eval("ThumbnailUrl160") %>'  style="border-width: 0px;width:86px;"/>
                <%--<img id="CountDownProducts_rptProduct_ctl00_ctl00_HiImage1" width="85" src="http://www.haimylife.com/Storage/master/product/thumbs160/160_b07bd0096b9449708df99e4f846d4955.jpg" style="border-width: 0px;">--%></a>
        </div>
        <div class="buy_name">
            <%--<a href="#" target="_blank">波兰Bogutti椰子风味奶油饼干150g</a>--%>
            <a href='/CountDownProductsDetails.aspx?productId=<%# Eval("ProductId") %>&countDownId=<%# Eval("CountDownId") %>' target="_blank">                         
                         <%# Eval("ProductName") %>
                   </a>
        </div>
        <div class="buy_price">
            <em>￥<span><Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("CountDownPrice") %>' /></span></em>
            <p>
                <a href='/CountDownProductsDetails.aspx?productId=<%# Eval("ProductId") %>&countDownId=<%# Eval("CountDownId") %>' style='background: none' target="_blank">                     
                         <%# DateTime.Now >= Convert.ToDateTime(Eval("StartDate"))?"<span style='background: #d21676'>立即抢购</span>":"<span style='background:#2fb06d'>即将开始</span>" %>
                   </a>
                <%--<a id="" href="/countdownproduct_detail-10062.aspx" style="background: #d21676" target="_blank">立即抢购</a>--%>
            </p>
        </div>
    </div>
</li>
<div style="display: none;">
    结束时间：<Hi:FormatedTimeLabel runat="server" ID="lblEndTime" Time='<%# Eval("EndDate") %>' /></div>