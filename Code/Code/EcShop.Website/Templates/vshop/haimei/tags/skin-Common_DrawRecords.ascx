<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<a href="SplittinDraw_Detail.aspx?id=<%# Eval("JournalNumber") %>">
    <div class="referral clearfix">
        <Hi:HiImage ImageUrl="../images/purse.png" runat="server" CssClass="img-circle" />
        <div class="left">
            <div>提现金额：<span class="text-danger">￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("Amount") %>' runat="server" /></span></div>
            <span><%#(int)Eval("AuditStatus") == 1?"待处理":"提现成功"%> 申请提现日期:<Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("RequestDate") %>'></Hi:FormatedTimeLabel></span>
             
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>