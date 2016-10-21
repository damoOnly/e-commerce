<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageAppIcon.aspx.cs" Inherits="EcShop.UI.Web.Admin.App.ManageAppIcon" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Entities.VShop"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link href="/Utility/icomoon/style.css" rel="stylesheet" type="text/css" />
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>

  <h1></h1>
  <span>配置图标</span></div>  
		<!-- 添加按钮-->
   <div class="btn">  <a class="submit_jia" href="javascript:DialogFrame('App/AddAppIcon.aspx?id=0','添加一个图标',null,600)">添加一个图标 </a></div>
   
<!--结束-->
		<!--数据列表区域-->
  <div class="datalist">
  <UI:Grid ID="grdIcon" DataKeyNames="Id" runat="server" ShowHeader="true" 
          AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" 
          Width="100%" onrowcommand="grdIcon_RowCommand">
                    <Columns> 
                    <asp:TemplateField HeaderText="图标" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="Name"> <img src='<%#((IconInfo)Container.DataItem).ImageUrl==""?"/utility/pics/none.gif":((IconInfo)Container.DataItem).ImageUrl%>'  width="90px" height="50px;"/></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="描述"  HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%#((IconInfo)Container.DataItem).ShortDesc%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <UI:SortImageColumn HeaderText="排序" ItemStyle-Width="60px" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemTemplate>
                               <span class="submit_bianji"> <a  href="javascript:DialogFrame('<%# "App/AddAppIcon.aspx?Id="+ ((IconInfo)Container.DataItem).Id %>','编辑图标',null,null)">编辑</a> </span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton ID="ImageLinkButton1" runat="server" IsShow="true" Text="删除" CommandName="Delete"  DeleteMsg="确定要删除此图标吗？" /></span>
                          </span>
                         </ItemTemplate>
                     </asp:TemplateField>                     
                    </Columns>
                </UI:Grid>
  </div>
</div>

<script type="text/javascript">
    $(function () {

        $.each($('[name="icon"]'), function (i, item) {
            var img = $($(item).find('img')[0]);
            var url = img.attr('src');
            if (url.indexOf('.') == -1) { //不包含.则为字体
                img.remove();
                var slashIndex = url.lastIndexOf('/');
                url = url.substring(slashIndex + 1);
                $(item).html('<div class="' + url + '" style="display: inline; font-size: 3em;"></div>');
            }
        });
    });



</script>




</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>


