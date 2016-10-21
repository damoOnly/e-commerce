<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
<div>
    <div class="zx_pic60">
         <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40"/></div>
    <div class="zx_name">
        <span>
            <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' runat="server" /></span> <span><b>нр╣двия╞ё╨</b><%# Eval("ConsultationText") %></span>
    </div>
    <div class="zx_time">
        вия╞сз
        <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
            runat="server"></Hi:FormatedTimeLabel></div>
    <div style="clear: both">
    </div>
</div>
