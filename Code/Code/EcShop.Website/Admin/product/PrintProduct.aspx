<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="PrintProduct.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.PrintProduct " %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

	<div>
        <div  style=" float:left"><em><img src="../images/01.gif" width="32" height="32" /></em></div>
		<div  style=" float:left">
       　　 <h1>&nbsp;商品打印列表</h1>
   　　 </div>
     </div>

    <!--数据列表区域-->
	<div class="datalist goods-list">
          <UI:Grid ID="grdproducts" runat="server" AutoGenerateColumns="false" ShowHeader="true"  GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
        
                  <asp:TemplateField HeaderText="" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                        <div style="border:1px; margin:5px; "  align="center">
                            <div>
                                <img alt="商品二维码" src='<%#Eval("QRcode").ToString().Replace("~","") %>' width="218px" height="218px" />
                            </div>
                            <div style="font-size:14px;">
                                <div >商品名称：<%#Eval("ProductName") %></div>
                                <div>商家编码：<%#Eval("ProductCode") %></div>
                            </div>
                        </div>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
   

　　</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
   
</asp:Content>