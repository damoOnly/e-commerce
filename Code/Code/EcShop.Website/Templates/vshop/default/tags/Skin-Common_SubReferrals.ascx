<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<a href="SubReferral_Detail.aspx?userid=<%# Eval("UserId") %>">
    <div class="referral clearfix">
        <Hi:HiImage ImageUrl="../images/headerimg.png" runat="server" CssClass="img-circle" />
        <div class="left">

            <div><%# Eval("UserName") %></div>
            <span class="describe">贡献佣金 <b>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("SubReferralSplittin") %>' NullToDisplay="0.00"  runat="server" /></b>
                直接推广订单量 <b><%#Eval("ReferralOrderNumber")%></b>
            </span>
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>
