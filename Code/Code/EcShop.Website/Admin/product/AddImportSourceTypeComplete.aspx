<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddImportSourceTypeComplete.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddImportSourceTypeComplete" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><asp:Literal ID="txtAction1" runat="server"></asp:Literal>原产地成功</h1>
            <span>原产地<asp:Literal ID="txtAction2" runat="server"></asp:Literal>成功，您还可以进行以下操作：</span>
        </div>
          <div class="formitem">
          <span class="msg">原产地<asp:Literal ID="txtAction" runat="server"></asp:Literal> 成功！</span>
         </div>
          <div class="Pg_15 Pg_45 fonts"><span class="float">你可以</span>
           <asp:HyperLink ID="hlinkImportSourceTypeEdit" runat="server" Text="编辑" />原产地
        </div>
          <div class="Pg_15 Pg_45 fonts">您还可以继续到 
            <asp:HyperLink ID="hlinkAddImportSourceType" runat="server" Text="添加原产地" />
            </span></div>
		  <div class="Pg_15 Pg_45 fonts">您可以随时到  <span class="Name"><a href="ImportSourceType.aspx">原产地列表</a></span>  去编辑原产地。</div>
      </div>
        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
