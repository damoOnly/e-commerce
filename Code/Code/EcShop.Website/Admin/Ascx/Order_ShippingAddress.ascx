<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ShippingAddress.ascx.cs" Inherits="EcShop.UI.Web.Admin.Order_ShippingAddress" %>
<h1>物流信息</h1>
        <div class="Settlement">
        <table width="100%" border="0" cellspacing="0">
         <tr id="tr_company" runat="server" visible="false">
            <td align="right">物流公司：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litCompanyName" runat="server" /></td>
          </tr>
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="60%"><asp:Literal ID="lblShipAddress" runat="server" /></td>
            <td width="25%"><span class="Name"><asp:Label ID="lkBtnEditShippingAddress" runat="server">
                <a href="javascript:DialogFrame('sales/ShippAddress.aspx?action=update&OrderId=<%=Page.Request.QueryString["OrderId"] %>','修改收货地址',600,350);" visible="false">修改收货地址</a>
            </asp:Label></span></td>
          </tr>
          <tr>
            <td align="right">送货上门时间：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litShipToDate" runat="server" /></td>
          </tr>
          <tr>
            <td align="right">配送方式：</td>
            <td colspan="2" width="85%"><asp:Literal ID="litModeName" runat="server" /><%=edit %></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" ><asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>


 


<div id="updatetag_div" style="display:none;">
      <div class="frame-content">
             <p><span class="frame-span frame-input90">发货单号：<em>*</em> </span><asp:TextBox ID="txtpost" ClientIDMode="Static" CssClass="forminput" runat="server" /></p>
             <input type="hidden" id="OrderId" runat="server" clientidmode="Static" />
      </div>
</div>

<div style="display:none">
  <asp:Button ID="btnupdatepost" runat="server" Text="修 改"  CssClass="submit_DAqueding" />
  <input type="hidden" id="hdtagId" runat="server" />
</div>




    <!--物流信息-->
    <asp:Panel ID="plExpress" runat="server" Visible="false" style="widht:730px;margin-bottom:10px;">
    <h1>快递单物流信息</h1>

    <div id="expressInfo">正在加载中....</div>
    </asp:Panel>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>
<script>
    function ShowPurchaseOrder() {
        formtype = "changeorder";
        arrytext = null;
        DialogShow("修改发货单号", 'changepurcharorder', 'updatetag_div', 'ctl00_contentHolder_shippingAddress_btnupdatepost');
    }

    $(function () {
        var orderId = $('#OrderId').val();
        if (orderId) 
            $('#expressInfo').expressInfo(orderId);

    });



</script>