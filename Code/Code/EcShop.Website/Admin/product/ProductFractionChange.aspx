<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductFractionChange.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.ProductFractionChange" Title="无标题页" %>
<%@ Import Namespace="EcShop.Core"%>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	<div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">批量修改商品权重</h1>
	    <span class="font">您可以对已选的这些商品权重直接改成某个值，或增加/减少某个值，也可以手工输入您想要的权重后在页底处保存设置</span>
    </div>

    <div class="searcharea clearfix">
        <ul>
            <li>将原权重改为：<asp:TextBox ID="txtTagetStock" runat="server" Width="80px" /></li>
            <li><asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
            <li>将原权重增加(输入负数为减少)：<asp:TextBox ID="txtAddStock" runat="server" Width="80px" /></li>
			<li><asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
		</ul>
     </div>

    <div class="datalist">
	      <UI:Grid ID="grdSelectedProducts" DataKeyNames="ProductId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="货号" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            &nbsp;<%#Eval("SKU") %>                                             
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                            <%#Eval("ProductName") %> <%--<%#Eval("SKUContent")%> --%>                                                 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="提升权重值" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAdminFraction" runat="server" Text = '<%#Eval("AdminFraction") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>          
                        <asp:TemplateField HeaderText="权重值" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSFraction" runat="server" Enabled="false" Text = '<%#Eval("Fraction") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    </Columns>
                </UI:Grid>
    </div>        
              
     <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveStock" runat="server" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
</asp:Content>
