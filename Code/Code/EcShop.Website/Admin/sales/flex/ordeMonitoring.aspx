<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ordeMonitoring.cs" Inherits="EcShop.UI.Web.Admin.sales.ordeMonitoring" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
		  <em><img src="../../images/02.gif" width="32" height="32" /></em>
          <h1><strong>订单统计</strong></h1>
          <span>查看订单统计 </span>
		</div>
     

    <div class="datalist">
       <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
            
                <li>
                    <span>下单日期：</span>
                    <span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calendarStartDate" /></span>
                    <span class="Pg_1010">至</span>
                    <span>
                        <UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calendarEndDate" IsStartTime="false"/>
                    </span>
                     <span class="Pg_1010"> 结转时间</span>
                    <span>
                        <Hi:HourDropDownList runat="server" AllowNull="true" ID="ddListHour" Width="55" CssClass="forminput selhour" />
                    </span>
                </li>
		        <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReState" runat="server" class="searchbutton" Text="刷新" Visible="false" />
		        </li>
			</ul>

	   </div>
    <div class="functionHandleArea clearfix">
      <!--分页功能-->
          <%--<div class="pageHandleArea">
            <ul>
              <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
            </ul>
          </div>--%>
          <div>
              <table style="text-align:center">
                <thead >
                    <tr>
                       <th style="text-align:center">日期</th>
                       <th style="text-align:center">订单</th>
                       <th style="text-align:center">未支付</th>
                       <th style="text-align:center">已支付</th>
                       <th style="text-align:center">身份证验证失败</th>
                       <th style="text-align:center">身份证验证通过</th>
                       <th style="text-align:center">未发WMS</th>
                       <th style="text-align:center">已发WMS</th>
                       <th style="text-align:center">四单未完成</th>
                       <th style="text-align:center">四单完成</th>
                       <th style="text-align:center">未认领</th>
                       <th style="text-align:center">已认领</th>
                       <th style="text-align:center">申报失败</th>
                       <th style="text-align:center">申报成功</th>
                       <th style="text-align:center">未发货</th>
                       <th style="text-align:center">已发货</th>
                       <th style="text-align:center">未签收</th>
                       <th style="text-align:center">已签收</th>
                    </tr>
               </thead>
               <tbody id="orderApplyOver">
               </tbody>
              </table>
          </div>
        <div class="pageNumber"> 
          <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
        </div>
       </div>
       </div>
    </div>
    <script type="text/javascript">
        var orderStat = {
            init: function () {
                var param = {
                    StartDate: orderStat.StartDate,
                    Hour: orderStat.Hour,
                    EndDate: orderStat.EndDate
                };
                orderStat.binddata(param);
                $("#<%=btnSearchButton.ClientID%>").click(function () {
                    orderStat.binddata(param);
                });
                $("#<%=btnReState.ClientID%>").click(function () {
                    orderStat.binddata(param);
                });
            },
            StartDate: $("#<%=calendarStartDate.ClientID%>").val(),
            Hour: $('#<%=ddListHour.ClientID%>').val(),
            EndDate: $('#<%=calendarEndDate.ClientID%>').val(),
            binddata: function (param) {
                if ((new Date(orderStat.EndDate) - new Date(orderStat.StartDate)) > (60 * 60 * 24 * 31 * 1000)) {
                    alert("查询跨度不能超过一个月");
                    return false;
                }
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=GetOrderMonitoring",
                    type: 'post',
                    dataType: 'json',
                    data: param,
                    success: function (data) {
                        if (data != null && data.success == "NO") {
                            alert(data.MSG);
                        }
                        else {
                            var html = '';
                            var styleTd = "style='cursor:pointer;background-color: rgb(250, 255, 189); ' status='1' onclick='orderStat.getRes(this)'";

                            for (var i = 0; i < data.length;i++) {
                                html += "<tr><td>" + data[i].OrderDate + "</td>";
                                html += "<td>" + data[i].OrderCount + "</td>";
                                if (data[i].NotPayCount > 0) {
                                    html += "<td " + styleTd + " type='Pay' date='" + data[i].OrderDate + "'>" + data[i].NotPayCount + "</td>";
                                }
                                else {
                                    html += "<td>" + data[i].NotPayCount + "</td>";
                                }
                                html += "<td>" + data[i].PayCount + "</td>";
                                if (data[i].NotPayerIDStatusCount>0) {
                                    html += "<td " + styleTd + " type='PayerID' date='" + data[i].OrderDate + "'>" + data[i].NotPayerIDStatusCount + "</td>";
                                }
                                else {
                                    html += "<td>" + data[i].NotPayerIDStatusCount + "</td>";
                                }
                                html += "<td>" + data[i].PayerIDStatusCount + "</td>"
                                if (data[i].NotSendWMSCount > 0) {
                                    html += "<td " + styleTd + " type='SendWMS' date='" + data[i].OrderDate + "'>" + data[i].NotSendWMSCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotSendWMSCount + "</td>";
                                }
                                html += "<td>" + data[i].SendWMSCount + "</td>";
                                if (data[i].NotHSDockingStatusCount > 0) {
                                    html += "<td " + styleTd + " type='HSDocking' date='" + data[i].OrderDate + "'>" + data[i].NotHSDockingStatusCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotHSDockingStatusCount + "</td>";
                                }
                                html += "<td>" + data[i].HSDockingStatusCount + "</td>";
                                if (data[i].NotDeclareStatusNameCount>0) {
                                    html += "<td " + styleTd + " type='DeclareName' date='" + data[i].OrderDate + "'>" + data[i].NotDeclareStatusNameCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotDeclareStatusNameCount + "</td>";
                                }
                                html += "<td>" + data[i].DeclareStatusNameCount + "</td>";
                                if (data[i].NotDeclareStatusWMSCount>0) {
                                    html += "<td " + styleTd + " type='Declare' date='" + data[i].OrderDate + "'>" + data[i].NotDeclareStatusWMSCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotDeclareStatusWMSCount + "</td>";
                                }
                                html += "<td>" + data[i].DeclareStatusWMSCount + "</td>";
                                if (data[i].NotShpiCount > 0) {
                                    html += "<td " + styleTd + " type='Shpi'  date='" + data[i].OrderDate + "'>" + data[i].NotShpiCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotShpiCount + "</td>";
                                }
                                html += "<td>" + data[i].ShpiCount + "</td>";

                                if (data[i].NotSignCount > 0) {
                                    html += "<td " + styleTd + " type='SignIn'  date='" + data[i].OrderDate + "'>" + data[i].NotSignCount + "</td>";
                                } else {
                                    html += "<td>" + data[i].NotSignCount + "</td>";
                                }
                                //html += "<td>" + data[i].NotSignCount + "</td>";
                                html += "<td>" + data[i].SignCount + "</td></tr>";
                            }
                            $("#orderApplyOver").html(html);
                        }
                    },
                    error: function () {
                        //alert("查询失败！");
                    }
                  })
            },
            getExpress: function () {
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=GetOrderMonitoringItem",
                    type: 'post',
                    dataType: 'json',
                    data: param,
                    success: function (data) {
                        if (data != null && data.success == "NO") {
                            alert(data.MSG);
                        }
                        else {
                            if (type == "Pay" || type == "SendWMS") {
                                var html = "<table><tr><th style='width:60px;text-align:center;'>序列</th><th style='width:200px;text-align:center;'>订单号</th><th style='width:350px;text-align:center;'>失败原因</th></tr>"
                                for (var i = 0; i < data.length; i++) {
                                    html += "<tr><td style='text-align:center;'>" + (i + 1) + "</td><td style='text-align:center'>" + data[i].OrderId + "</td><td style='text-align:center'>" + data[i].OrderRemark + "</td></tr>";
                                }
                                html += "</table>";
                                ShowIngredient("查看详情", html);
                            } else {
                                var dateNow = new Date();
                                var html = "<table><tr><th style='width:60px;text-align:center;'>序列</th><th style='width:200px;text-align:center;'>订单号</th><th style='width:260px;text-align:center;'>支付时间</th><th style='width:350px;text-align:center;'>失败原因</th><th style='width:260px;text-align:center;'>验证时间</th><th style='width:260px;text-align:center;'>处理时长(/小时)</th></tr>"
                                for (var i = 0; i < data.length; i++) {
                                    var hours;
                                    var completeDate;
                                    if (data[i].CompleteDate != null) {
                                        completeDate = data[i].CompleteDate;
                                        var date3 = dateNow.getTime() - new Date(data[i].CompleteDate).getTime()  //时间差的毫秒数
                                        var leave1 = date3 % (24 * 3600 * 1000)    //计算天数后剩余的毫秒数
                                        var hours = (leave1 / (3600 * 1000)).toFixed(2)
                                    } else {
                                        hours = "空"
                                        completeDate = "空"
                                    }
                                    html += "<tr><td style='text-align:center;'>" + (i + 1) + "</td><td style='text-align:center'>" + data[i].OrderId + "</td><td style='text-align:center'>" + data[i].PayData + "</td><td style='text-align:center'>" + data[i].OrderRemark + "</td><td style='text-align:center'>" + completeDate + "</td><td style='text-align:center'>" + hours + "</td></tr>";
                                }
                                html += "</table>";
                                ShowIngredient("查看详情", html);
                            }
                        }
                    },
                    error: function () {
                        //alert("查询失败！");
                    }
                })
            },
            getRes: function (obj) {
                var type = $(obj).attr("type");
                var status = $(obj).attr("status");
                var date=$(obj).attr("date");
                var param = {
                    date: date,
                    Hour: orderStat.Hour,
                    type: type,
                    status: status
                };
                function ShowIngredient(titile, content) {
                    dialog = art.dialog({
                        id: "set",
                        title: titile,
                        content: content,
                        resize: false,
                        fixed: true,
                        height: 300,
                        width: 800,
                        modal: true, //蒙层（弹出会影响页面大小） 
                        button: [
                                { name: '关  闭' }
                        ]
                    });
                };
                
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=GetOrderMonitoringItem",
                    type: 'post',
                    dataType: 'json',
                    data: param,
                    success: function (data) {
                        if (data != null && data.success == "NO") {
                            alert(data.MSG);
                        }
                        else {
                            if (type == "Pay" || type == "SendWMS") {
                                var html = "<table><tr><th style='width:60px;text-align:center;'>序列</th><th style='width:200px;text-align:center;'>订单号</th><th style='width:350px;text-align:center;'>失败原因</th></tr>"
                                for (var i = 0; i < data.length; i++) {
                                    html += "<tr><td style='text-align:center;'>" + (i + 1) + "</td><td style='text-align:center'>" + data[i].OrderId + "</td><td style='text-align:center'>" + data[i].OrderRemark + "</td></tr>";
                                }
                                html += "</table>";
                                ShowIngredient("查看详情", html);
                            }
                            else { 
                                var dateNow = new Date();
                                var html = "<table><tr><th style='width:60px;text-align:center;'>序列</th><th style='width:200px;text-align:center;'>订单号</th><th style='width:260px;text-align:center;'>支付时间</th><th style='width:350px;text-align:center;'>失败原因</th><th style='width:260px;text-align:center;'>验证时间</th><th style='width:260px;text-align:center;'>处理时长(/小时)</th></tr>"
                                for (var i = 0; i < data.length; i++) {
                                    var hours;
                                    var completeDate;
                                    if (data[i].CompleteDate != null) {
                                        completeDate = data[i].CompleteDate;
                                        var date3 = dateNow.getTime() - new Date(data[i].CompleteDate).getTime()  //时间差的毫秒数
                                        var leave1 = date3 % (24 * 3600 * 1000)    //计算天数后剩余的毫秒数
                                        var hours = (leave1 / (3600 * 1000)).toFixed(2)
                                    } else {
                                        hours = "空"
                                        completeDate="空"
                                    }
                                    html += "<tr><td style='text-align:center;'>" + (i + 1) + "</td><td style='text-align:center'>" + data[i].OrderId + "</td><td style='text-align:center'>" + data[i].PayData + "</td><td style='text-align:center'>" + data[i].OrderRemark + "</td><td style='text-align:center'>" + completeDate + "</td><td style='text-align:center'>" + hours + "</td></tr>";
                                }
                                html += "</table>";
                                ShowIngredient("查看详情", html);
                            }
                        }
                    },
                    error: function () {
                        //alert("查询失败！");
                    }
                })

            }
         };
     $(function(){
         orderStat.init();
    })
</script>
</asp:Content>