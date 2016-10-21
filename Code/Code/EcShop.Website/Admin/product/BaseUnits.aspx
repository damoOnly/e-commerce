<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.product.BaseUnits" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
    <div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>计量单位</h1>
	    <span>计量单位 用于规范商品单位和后续与海关对接</span>
     </div>
	  <!-- 添加按钮-->
	  <div class="btn">
	    <a href="AddUnit.aspx?type=add" class="submit_jia">添加计量单位</a>
    </div>        
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
	    <div class="search clearfix">
			<ul>
				<li><span>关键词：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
            <span style="width:110px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;float:right;">　　　<asp:LinkButton ID="btnorder" runat="server">批量保存排序</asp:LinkButton> </span>
	</div>
	  <UI:Grid ID="grdUnitList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="Id" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>    
                  <asp:TemplateField HeaderText="海关代码" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litHSJoinID" runat="server" Text='<%# Bind("HSJoinID") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>                
                  <asp:TemplateField HeaderText="计量单位名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litName" runat="server" Text='<%# Bind("Name_CN") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>     
                 <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="90px"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input id="Text1" type="text" runat="server" value='<%# Eval("Sort") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "AddUnit.aspx?type=edit&unitId="+Eval("Id")%>' /></span> 
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
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
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
</asp:Content>