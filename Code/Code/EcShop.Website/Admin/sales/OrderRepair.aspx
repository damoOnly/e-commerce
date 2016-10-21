<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderRepair.aspx.cs" Inherits="EcShop.UI.Web.Admin.OrderRepair" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>订单修复</h1>
        <span>对于顾客使用微信支付方式支付成功了，但是系统没有回写支付成功的状态，可通过此页面点击执行恢复订单状态</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <div class="clear"></div>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="执 行" CssClass="submit_DAqueding inbnt"  />
	          </ul>
          </div>
      </div>
</div>  
    <script>
</script>
</asp:Content>

