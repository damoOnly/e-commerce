<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.Main" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Default.aspx.cs"%>
<%@ Import Namespace="EcShop.Core"%>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="ecshop_quick">运营快捷入口：</div>
<div class="ecshop_list">
    <ul>
        <li><a href="javascript:ShowSecondMenuLeft('商 品','product/selectcategory.aspx',null)"><img src="images/1.png" /><br />添加商品</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('CRM管理','tools/sendmessagetemplets.aspx',null)"><img src="images/2.png" /><br />短信营销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('统计报表','tools/cnzzstatistictotal.aspx',null)"><img src="images/3.png" /><br />网站流量</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx',null)"><img src="images/4.png" /><br />订单列表</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('会 员','member/managemembers.aspx',null)"><img src="images/5.png" /><br />会员管理</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('财务管理','member/accountsummarylist.aspx',null)"><img src="images/6.png" /><br />会员预存款</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/productpromotions.aspx?isWholesale=true',null)"><img src="images/7.png" /><br />批发规则</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/productpromotions.aspx',null)"><img src="images/8.png" /><br />商品促销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/orderpromotions.aspx',null)"><img src="images/9.png" /><br />订单促销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('统计报表','sales/salereport.aspx',null)"><img src="images/10.png" /><br />零售统计</a></li>
    </ul>
    <div class="clear"></div>
</div>	 

<div class="ecshop_list_content">
	<div class="ecshop_list_r">
             <div class="ecshop_contenttitle">B2C商城动态</div>
             
            
          <div class="ecshop_list_tab">
              <div style="height:400px;">
              <iframe src="about:blank"  frameborder="0" width="100%" height="400px"></iframe>
           
              </div>
              <iframe src="about:blank"  frameborder="0" width="100%"></iframe>
          </div>
    </div>
    <div class="ecshop_list_l">
        <div class="ecshop_contenttitle">待处理事务</div>
        <table>
            <tr>
                <td class="td_cont_left">等待发货订单</td><td class="sp_img"><asp:HyperLink ID="ltrWaitSendOrdersNumber" runat="server" ></asp:HyperLink></td>
                <td class="td_cont_left">未处理会员提现申请</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblMemberBlancedrawRequest"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">未回复站内信</td><td class="sp_img"><span class="sp_img"><asp:HyperLink ID="hpkMessages" runat="server" ></asp:HyperLink></span></td>
                <td class="td_cont_left">未回复商城留言</td><td class="sp_img"><span class="sp_img"><asp:HyperLink ID="hpkLiuYan" runat="server" ></asp:HyperLink></span></td>
            </tr>
            <tr>
                <td class="td_cont_left">未回复的商品咨询</td><td class="sp_img"><asp:HyperLink ID="hpkZiXun" runat="server" ></asp:HyperLink></td>
                <td class="td_cont_left"></td><td class="sp_img"></td>
            </tr> 
        </table>

        <div class="clear"></div>

        <div class="ecshop_contenttitle">近两日业务量</div>
        <table>
            <tr>
                <td class="td_cont_left">今日成交订单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTodayFinishOrder"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">昨日成交订单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblYesterdayFinishOrder"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日订单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodayOrderAmout" /></td>
                <td class="td_cont_left">昨日订单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblOrderPriceYesterDay"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日新增会员数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0位" Id="ltrTodayAddMemberNumber" /></td>
                <td class="td_cont_left">昨日新增会员数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblUserNewAddYesterToday"></Hi:ClassShowOnDataLitl></td>
            </tr>
        </table>
        
        <div class="ecshop_contenttitle">商城信息</div>
        <table>
            <tr>
                <td class="td_cont_left">会员总数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblTotalMembers"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">商品总数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTotalProducts"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">会员预付款总额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblMembersBalanceTotal" /></td>
                <td class="td_cont_left">最近30天的订单总金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblOrderPriceMonth"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">当前版本</td><td class="sp_img">V1.3</td>
                 <td class="td_cont_left"></td><td class="sp_img"></td>
            </tr>
        </table>

        <div class="clear"></div>
    </div>    
</div>
<script type="text/javascript">
    function ShowSecondMenuLeft(firstnode, secondurl,threeurl) {
        window.parent.ShowMenuLeft(firstnode, secondurl, threeurl);
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

