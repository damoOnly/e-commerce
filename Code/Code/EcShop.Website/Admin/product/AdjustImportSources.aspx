<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdjustImportSources.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.AdjustImportSources" Title="无标题页" %>
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
	    <h1 class="title_line">批量修改商品原产地</h1>
	    <span class="font">手工输入您想要修改的信息后在页底处保存设置即可</span>
     </div>

     <div class="datalist">
	      <UI:Grid ID="grdSelectedProducts" DataKeyNames="ProductId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>    
                     <asp:TemplateField HeaderText="商品图片" ItemStyle-Width="5%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                        </ItemTemplate>
                    </asp:TemplateField>             
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="25%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%#Eval("ProductName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="原产地" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <Hi:ImportSourceTypeDropDownList runat="server"  ID="ddlImportSourceType"  NullToDisplay="--请选择原产地--" CssClass="dropImportSourceType" />
                        </ItemTemplate>
                    </asp:TemplateField> 
                    </Columns>
                </UI:Grid>
                 
                <table cellspacing="0" border="0" id="OperateAll" class="OperateAll">
	            <tbody>
                    <tr>
                        <td  style="float:right;padding-right:128px;border-bottom:0" rowspan="3">   
                        设置原产地：<Hi:ImportSourceTypeDropDownList runat="server" ID="ddlImportSourceTypeAll" NullToDisplay="--请选择--" />   
                     </td>
	            </tr>
               </tbody>
             </table>
                 
            </div>     
                 
     <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveInfo" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
    
<script type="text/javascript">
    function CloseWindow() {
        var win = art.dialog.open.origin; //来源页面
        // 如果父页面重载或者关闭其子对话框全部会关闭
        win.location.reload();
    }
    $(function () {
        $("#ctl00_contentHolder_ddlImportSourceTypeAll").bind("change", function () {
            var importSourceTypId = $(this).val();
            $(".dropImportSourceType").each(function (index, elem) {
                $(elem).val(importSourceTypId);
            });
        });
    });
</script>
</asp:Content>
