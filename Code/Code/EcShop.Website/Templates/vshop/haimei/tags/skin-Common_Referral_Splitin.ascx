<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<a href="SplittinDetail_Detail.aspx?id=<%# Eval("JournalNumber") %>">
    <div class="referral clearfix">
        <Hi:HiImage ImageUrl="../images/purse.png" runat="server" CssClass="img-circle" />
        <div class="left">
            <div><%# Eval("UserName") %></div>
            <span><Hi:SplittingTypeNameLabel ID="SplittingTypeNameLabel1" runat="server" SplittingType='<%# Eval("TradeType")%>' /> <b>￥ <%# (int)Eval("TradeType") == 4 ? "-" + Eval("Expenses", "{0:F2}") : Eval("Income", "{0:F2}")%></b></span>
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>