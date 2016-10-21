<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?OrderId=" + Eval("OrderId") %>">
    <div class="panel panel-default order-list">
        <div class="panel-heading">
            <h3 class="panel-title">
                <%# Eval("OrderId") %></h3> <span class="date">
                <%# Convert.ToDateTime(Eval("ApplyForTime")).ToString("yyyy-MM-dd")%></span>
        </div>
        <div class="panel-body">
            <a href="/Vshop/MemberOrderDetails.aspx?OrderId=201504238535125">
                <img id="vMemberOrders_rptOrders_ctl01_ctl00_Repeater1_ctl00_ListImage1" src="/Storage/master/product/thumbs40/40_6d833b63cf5847d9acd1c1364e50b0c8.jpg" style="border-width: 0px;">
            </a>
            <div class="info">
                <div class="status">
                    处理状态：<%#Eval("HandleStatus").ToString()=="0"?"未处理":(Eval("HandleStatus").ToString()=="1"?"已处理":"已拒绝") %>
                    </div><div>
                <div  class="action ">退款金额：<%# Eval("OrderTotal","{0:F2}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="# " class="btn btn-warning btn-xs ">查看</a></div>
            </div>
        </div>
    </div>
</a>