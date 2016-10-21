﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageAppBanner.aspx.cs" Inherits="EcShop.UI.Web.Admin.ManageAppBanner" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Entities.VShop"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>

  <h1>广告图配置</h1>
  <span>如果您选择了首页带有广告图的模板，您需要在此配置好相关参数。</span></div>  
		<!-- 添加按钮-->
   <div class="btn">
       <a class="submit_jia" href="javascript:DialogFrame('App/AddAppBanner.aspx?id=0','添加广告',null,null)">添加广告 </a>
 </div>
   
<!--结束-->
		<!--数据列表区域-->
  <div class="datalist">
  <UI:Grid ID="grdBanner" DataKeyNames="Id" runat="server" ShowHeader="true" 
          AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" 
          Width="100%" onrowcommand="grdBanner_RowCommand">
                    <Columns> 
                    <asp:TemplateField HeaderText="图片" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"> <img src='<%#((BannerInfo)Container.DataItem).ImageUrl==""?"/utility/pics/none.gif":((BannerInfo)Container.DataItem).ImageUrl%>'  width="90px" height="50px;"/></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="描述"  HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%#((BannerInfo)Container.DataItem).ShortDesc%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="配置值"  HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%#((BannerInfo)Container.DataItem).Url%>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="类型"  HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%#CovertType(((BannerInfo)Container.DataItem).LocationType.ToShowText())%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <UI:SortImageColumn HeaderText="排序" ItemStyle-Width="60px" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemTemplate>
                            <span class="submit_bianji"> <a  href="javascript:DialogFrame('<%# "App/AddAppBanner.aspx?Id="+ ((BannerInfo)Container.DataItem).Id %>','编辑广告图',null,null)">编辑</a> </span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton ID="ImageLinkButton1" runat="server" IsShow="true" Text="删除" CommandName="DeleteBanner"  DeleteMsg="确定要删除此广告图吗？" /></span>
                          </span>
                         </ItemTemplate>
                     </asp:TemplateField>                     
                    </Columns>
                </UI:Grid>
  </div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

