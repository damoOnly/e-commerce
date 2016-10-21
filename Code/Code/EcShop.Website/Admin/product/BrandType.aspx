<%@ Page  Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BrandType.aspx.cs"  Inherits="EcShop.UI.Web.Admin.BrandType" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
    <div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>品牌标签</h1>
	    <span>品牌标签 用于将品牌进行分组</span>
     </div>
	  <!-- 添加按钮-->
	  <div class="btn">
	    <a href="AddBrandTag.aspx" class="submit_jia">添加品牌标签</a>
    </div>        
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
	    <div class="search clearfix">
			<ul>
				<li><span>品牌标签名称：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
            <span style="width:110px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;float:right;">　　　<asp:LinkButton ID="btnorder" runat="server">批量保存排序</asp:LinkButton> </span>
	</div>
	  <UI:Grid ID="grdBrandCategriesList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="BrandTagId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>                
                  <asp:TemplateField HeaderText="品牌标签" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litName" runat="server" Text='<%# Bind("TagName") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>     
                 <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="70px"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input id="Text1" type="text" runat="server" value='<%# Eval("TagSort") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditBrandTag.aspx?brandId="+Eval("BrandTagId")%>' /></span> 
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


