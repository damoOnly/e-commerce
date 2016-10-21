<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BFDSet.aspx.cs" Inherits="EcShop.UI.Web.Admin.BFDSet" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>商业智能分析系统</h1>
      <span>商业智能分析系统
是由惠众云商联合北京百分点信息科技有限公司推出的一款面向惠众云商旗下商城系统的商业智能分析系统。系统采集目标站点的前后端数据进行整合，向企业提供基于流量、通路、访客、内容、商品和订单六大对象的数据分析，通过图形报表形式向企业管理者展示电子商务的核心数据，如动销、转化率、复购率和销售集中度等，同时满足企业对于WA和BA分析需求。在同类分析商品中，BAE首次提出基于商品维度的数据指标分析，更符合电商企业精细化管理需求.</span>
    </div>
   <div class="Tempimg" id="div_pan1" runat="server">
      <table width="98%" border="0" cellspacing="0">
        <tr>
          <td rowspan="3" class="style1"></td>
          <td width="2%" rowspan="3">&nbsp;</td>
          <td width="19%" >
           
          </td>
          <td width="54%" rowspan="3" style="vertical-align:middle">&nbsp;</td>
        </tr>
        <tr>
          <td><span class="colorG"><strong><asp:LinkButton ID="hlinkCreate"   CssClass="submit_jia submit_a"  Text="创建分析账号" runat="server"></asp:LinkButton></strong></span></td>
        </tr>
        </table>
    </div>
    <div class="blank12 clearfix"></div>
    <div class="datafrom" id="div_pan2" runat="server">
        <div class="Template">
        <h1>分析引擎设置</h1>
        <p style="margin-left:50px;">您的帐号已经创建，可以通过点击开启或关闭按钮对智能分析引擎进行开启和关闭，如果开启动就会在前台进行统计,可以通过查看统计的链接查看统计结果。</p>
        <p style="margin-left:50px;">如果您是第一次创建统计账号，请点击开启统计功能，这样百分点分析引擎即可开始使用.</p>
          <div style="margin:10px auto;width:50%; text-align:center"> <asp:LinkButton ID="hplinkSet" CssClass="submit_jia submit_a" Text="开启分析引擎" runat="server"></asp:LinkButton>
                 </div>
         </div>
     </div>
     
         <div class="datafrom" id="div_total" runat="server">
        <div class="Template">
        <h1>查看分析结果</h1>
        <p style="margin-left:50px;">分析引擎已经开通，该分析引擎将向您提供基于流量、通路、访客、内容、商品、订单六大对象的数据分析，并通过图形报表形式向您展示电子商务网站的核心数据，如动销、转化率、复购率和销售集中度等。<asp:LinkButton 
                ID="LkClose"  Text="关闭分析引擎" runat="server" onclick="LkClose_Click"></asp:LinkButton></p>
          <div style="margin:10px auto;width:50%"> <asp:HyperLink  ID="Lktoal"   Target="_blank" NavigateUrl="~/Admin/tools/BFDTotal.aspx"   CssClass="submit_jia submit_a"  Text="立即查看分析结果" runat="server"></asp:HyperLink>
                 </div>
         </div>
     </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <style type="text/css">
        .style1
        {
            width: 25%;
        }
    </style>
</asp:Content>

