<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
<div>
    <div class="zx_pic60">
        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" /></div>
    <div class="zx_name">
        <span>
            <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' runat="server" /></span> <span><b>�ҵ���ѯ��</b><%# Eval("ConsultationText") %></span>
    </div>
    <div class="zx_time">
        ��ѯ��
        <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
            runat="server"></Hi:FormatedTimeLabel></div>
</div>
<div style="clear: both">
</div>
<div>
    <asp:Repeater ID="dlstPtConsultationReply" runat="server" DataSource='<%# Eval( "ConsultationReplys") %>'>
        <ItemTemplate>
            <div class="zx_name">
                <span><b>����Ա�ظ���</b><%# Eval("ReplyText") %></span>
            </div>
            <div class="zx_time">
                �ظ���
                <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%# Eval("ReplyDate") %>' runat="server"></Hi:FormatedTimeLabel></div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div style="clear: both">
</div>
