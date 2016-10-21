<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.HSCode" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	<div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	   <h1>海关编码</h1>
	   <span>海关编码用于商品归类</span>
    </div>
    <!-- 添加按钮-->
	<div class="btn">
	    <a href="AddHSCode.aspx" class="submit_jia">添加海关编码</a>
    </div>  
    <div class="datalist">
        <div class="search clearfix">
			<ul>
				<li><span>海关编码：</span>
				    <span><asp:TextBox ID="codeSearchText" runat="server" CssClass="forminput" /></span>
				</li>
                <li><span>海关编码名称：</span>
				    <span><asp:TextBox ID="nameSearchText" runat="server" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
	    </div>
        <UI:Grid ID="grdHSCode" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="HS_CODE_ID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="海关编码" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblHSCode" runat="server" Text='<%# Eval("HS_CODE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtHSCode" runat="server" Text='<%# Eval("HS_CODE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="海关编码名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblHSCodeName" runat="server" Text='<%# Eval("HS_NAME") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtHSCodeName" runat="server" Text='<%# Eval("HS_NAME") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="最惠国税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblLowRate" runat="server" Text='<%# Eval("LOW_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLowRate" runat="server" Text='<%# Eval("LOW_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="普通国税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblHighRate" runat="server" Text='<%# Eval("HIGH_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtHighRate" runat="server" Text='<%# Eval("HIGH_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="出口税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblOutRate" runat="server" Text='<%# Eval("OUT_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOutRate" runat="server" Text='<%# Eval("OUT_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="增值税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblTaxRate" runat="server" Text='<%# Eval("TAX_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTaxRate" runat="server" Text='<%# Eval("TAX_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="退税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblTslRate" runat="server" Text='<%# Eval("TSL_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTslRate" runat="server" Text='<%# Eval("TSL_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="消费税率" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblConsumptionRate" runat="server" Text='<%# Eval("CONSUMPTION_RATE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtConsumptionRate" runat="server" Text='<%# Eval("CONSUMPTION_RATE") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                    <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_bianji"><asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditHSCode.aspx?HS_CODE_ID="+Eval("HS_CODE_ID")%>' ></asp:HyperLink></span> 
	                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete"  Text="删除" /></span>
                         </ItemTemplate>
                </asp:TemplateField>  
            </Columns>
        </UI:Grid>
         <div class="page">
	      <div class="bottomPageNumber clearfix">
	      <div class="pageNumber">

            <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
            </div>
        </div>
        </div>
        </div>
    </div>
</div>
</asp:Content>
