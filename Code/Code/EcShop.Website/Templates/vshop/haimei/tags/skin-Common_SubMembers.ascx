<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<a href="SubMember_Detail.aspx?userid=<%# Eval("UserId") %>">
    <div class="referral clearfix">
        <Hi:HiImage ImageUrl="../images/headerimg.png" runat="server" CssClass="img-circle" />
        <div class="left">

            <div><%# Eval("UserName") %></div>
             <span class="describe">注册日期 <b><%# ((DateTime)Eval("CreateDate")).ToString("yyyy-MM-dd hh:mm") %>&nbsp;&nbsp;</b>
                订单量 <b><%#Eval("OrderNumber")%></b>
            </span>
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>
