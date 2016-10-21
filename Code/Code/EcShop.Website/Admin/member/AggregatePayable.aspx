<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.AggregatePayable"  CodeBehind="AggregatePayable.aspx.cs"    MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  

<div class="dataarea mainwidth databody">
  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>平台销售额及应付总汇表</h1>
      <span>跨境电商（海美生活）平台销售额及应付总汇表</span>
  </div>
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
             <li><span>交易时间从：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server"  class="forminput" /></li>
            <li><span>至：</span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate"   class="forminput"/></li>
		    <li>
				<asp:Button ID="btnQueryBalanceDetails" runat="server" Text="查询" class="searchbutton"/>
            </li>
            <li  style=" display:none">
                <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
			</li>
			</ul>
	  </div>
	  <div class="datalist" style="width:100%; clear:both">
          <table  border="0" cellpadding="3" cellspacing="1" width="100%" align="center" >
              <tr  style="text-align: center;  font-weight: bold; font-size:14px" class="table_title">
                   <td class="td_right td_left"  style="width:200px" ><h5>日期</h5></td>
                   <td class="td_right td_left"  style="width:200px"><h5>银行收款</h5></td>
                   <td colspan="2"  class="td_right td_left"><h5>平台销售额及代收运费</h5></td>
                   <td></td>
              </tr>
              <tr align="center">
                   <td  class="Rp_td3"><%=this.DateTitle %></td>
                   <td  class="Rp_td3"><%=this.OrderTotal==".00"?"0": this.OrderTotal%></td>
                   <td  class="Rp_td3" style="width:200px"><%=this.supplyTopName %></td>
                   <td  class="auto-style1" style="width:200px"><%=this.supplyToAmout%></td>
                   <td></td>
              </tr>
             <%if(this.Dtsuppley!=null)
              {
                  for (int i = 1; i < this.Dtsuppley.Rows.Count;i++ )
                  {     %>
                    <tr align="center">
                      <td  class="Rp_td3"></td>
                      <td  class="Rp_td3"></td>
                      <td  class="Rp_td3"><%=this.Dtsuppley.Rows[i]["供应商"] %></td>
                      <td  class="auto-style1"><%=this.Dtsuppley.Rows[i]["商品金额"].ToString()!="0"? decimal.Round(Convert.ToDecimal(this.Dtsuppley.Rows[i]["商品金额"].ToString()),2).ToString():"0" %></td>
                      <td></td>
                    </tr>
              <%}} %>
              <tr align="center">
                   <td  class="Rp_td3">&nbsp;</td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">减去退货款</td>
                   <td class="auto-style1"><%=this.RefundAmount=="-0.00"?"0":this.RefundAmount%></td>
             <td></td>
                   </tr>
                  <tr align="center">
                   <td  class="Rp_td3">&nbsp;</td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="auto-style1"></td>
             <td></td>
                       </tr>
              <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"  style="background: #fbfbfb;">平台销售额合计</td>
                   <td class="auto-style1" style="background: #fbfbfb;"><%=this.Amount %></td>
             <td></td>
                   </tr>
               <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">代收快递费</td>
                   <td class="auto-style1"><%=this.AdjustedFreight %></td>
             <td></td>
                    </tr>
               <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">减去退货运费</td>
                   <td class="auto-style1">0</td>
            <td></td>
                     </tr>
               <tr align="center">
                   <td  class="Rp_td3">&nbsp;</td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="auto-style1"></td>
             <td></td>
                    </tr>
               <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3" style="background: #fbfbfb;">代收快递费合计</td>
                   <td class="auto-style1" style="background: #fbfbfb;"><%=this.AdjustedFreight %></td>
             <td></td>
                    </tr>
               <tr align="center">
                   <td class="Rp_td3">&nbsp;</td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="auto-style1"></td>
            <td></td>
                     </tr>
              <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">减微信转账手续费</td>
                   <td class="auto-style1"><%=this.OrderCounterFee=="-0.00"?"0":this.OrderCounterFee%></td>
           <td></td>
                     </tr>
               <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">加测试收款</td>
                   <td class="auto-style1">0</td>
            <td></td>
                     </tr>
                 <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3">尾数调整</td>
                   <td class="auto-style1">0</td>
             <td></td>
                      </tr>
              <tr>
                   <td class="Rp_td3">&nbsp;</td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="auto-style1"></td>
                  <td></td>
              </tr>
               <tr align="center">
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3"></td>
                   <td class="Rp_td3" style="background: #fbfbfb;">转账手续费合计</td>
                   <td class="auto-style1" style="background: #fbfbfb;"><%=this.OrderCounterFee=="-0.00"?"0":this.OrderCounterFee %></td>
                   <td></td>
              </tr>
               <tr align="center">
                   <td class="Rp_td3"><h5>总计</h5></td>
                   <td class="Rp_td3"><%=this.OrderTotal %></td>
                   <td class="Rp_td3"></td>
                   <td class="auto-style1"><%=this.AllTotleAmout %></td>
                   <td></td>
              </tr>
          </table>
	  </div>

</div>

</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="validateHolder">
     <!--客户端验证-->
    <style type="text/css">
        .auto-style1 {
            font-size: 12px;
            BORDER-RIGHT: #f6f6f6 1px solid;
            BORDER-TOP: #f9f9f9 0px solid;
            BORDER-LEFT: #f9f9f9 0px solid;
            BORDER-BOTTOM: #f5f5f5 0px solid;
            width: 199px;
        }
    </style>
</asp:Content>


