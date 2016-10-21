<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="HSDeclareDisplay.aspx.cs" Inherits="EcShop.UI.Web.Admin.HSDeclareDisplay" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript" src="../js/json.js"></script>
    <div class="dataarea mainwidth databody">
        <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>海关申报</strong></h1>
          <span>查看海关申报操作记录 </span>
		</div>
     

    <div class="datalist">
       <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                <li>
                    <li><span>订单号：</span>
				        <span><asp:TextBox ID="OrderIDSearchText" runat="server" CssClass="forminput" /></span>
				    </li>

		        </li>
                <li>
                    <li><span>出库单号：</span>
				        <span><asp:TextBox ID="OutNoSearchText" runat="server" CssClass="forminput" /></span>
				    </li>

		        </li>
                <li>
                    <li><span>运单号：</span>
				        <span><asp:TextBox ID="LogisticsNoSearchText" runat="server" CssClass="forminput" /></span>
				    </li>

		        </li>
                <li>
                    <span>认领人：</span>
                    <span><asp:TextBox ID="txtOperationUserName" runat="server" CssClass="forminput" /></span>

		        </li>
			</ul>
           <ul>
               <li>
                    <span>运输号：</span>
                    <span><asp:TextBox ID="txtShipOrderNumber" runat="server" CssClass="forminput" /></span>
		        </li>
                <li><span>申报状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="DeclareStatusList" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未认领</asp:ListItem>
                                <asp:ListItem Value="4">5.已认领</asp:ListItem>
                                <asp:ListItem Value="1">2.已申报</asp:ListItem>
                                <asp:ListItem Value="2">3.申报成功</asp:ListItem>
                                <asp:ListItem Value="3">4.申报失败</asp:ListItem>
                                <asp:ListItem Value="5">6.异常处理</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
               <li><span>放行状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlWMSStatus" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未放行</asp:ListItem>
                                <asp:ListItem Value="1">2.已放行</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
               <li>
                    <span>下单时间：</span>
                    <span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calendarStartDate"  Width="125"/></span>
                    <Hi:HourDropDownList runat="server" AllowNull="true" ID="ddListStartHour" Width="55" CssClass="forminput selhour" />
                    <span class="Pg_1010">至</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calendarEndDate" IsStartTime="false"  Width="125"/>
                        <Hi:HourDropDownList runat="server" AllowNull="true" ID="ddListEndHour" Width="55" CssClass="forminput selhour" />
                                                  </span>
                </li>
		        <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
		        </li>
           </ul>
	   </div>
    <div class="functionHandleArea clearfix">
      <!--分页功能-->
        
          <div class="pageHandleArea">
            <ul>
              <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
            </ul>
          </div>
        
        <div class="pageNumber"> 
          <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
        </div>
    </div>

        <UI:Grid ID="dlstHSDeclare" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="HS_Docking_ID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" rel="<%#Eval("OrderId")%>" flag="0">
                                <%--<asp:Literal ID="litStatus" runat="server" Text='<%#Eval("HS_Docking_ID")%>'></asp:Literal>--%>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("HS_Docking_ID") %>' onclick=clickOrder('<%#Eval("OrderId")%>',<%#Eval("DeclareStatus")%>)>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="30px" HeaderText="认领人" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:30px;">
                                <asp:Literal ID="litDeclareName" runat="server" Text='<%#Eval("DeclareName")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="出库单号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:60px;" name="formId" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litOutNo" runat="server" Text='<%#Eval("OutNo")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="申报状态" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <asp:Literal ID="litDeclareStatus" runat="server" Text='<%#Eval("DeclareStatus")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="订单号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:50px;" name="orderNo" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litOrderId" runat="server" Text='<%#Eval("OrderId").ToString()+Eval("SonNumber").ToString()%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="运单号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:40px;" name="logNo" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litLogisticsNo" runat="server" Text='<%#Eval("LogisticsNo")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="支付编码" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:60px;" name="payNo" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litTradeNo" runat="server" Text='<%#Eval("tradeNo")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="下单时间" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:50px;" name="orderdate" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litOrderDate" runat="server" Text='<%#Eval("OrderDate")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="收货人城市" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:60px;" name="Province" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litRealAddress" runat="server" Text='<%#Eval("RealAddress")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="收货人姓名" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" name="consignee" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litPayerName" runat="server" Text='<%#Eval("payerName")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  ItemStyle-Width="55px" HeaderText="身份证号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:55px;" name="consigneeCardId" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litPayerId" runat="server" Text='<%#Eval("payerId")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="38px" HeaderText="手机" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break" style="width:38px;" name="consigneeTelephone" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litCellPhone" runat="server" Text='<%#Eval("CellPhone")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="总件数" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" name="qty" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%#Eval("ShipmentQuantity")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="总净重" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" name="netWt" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litOrderWeight" runat="server" Text='<%#Eval("OrderWeight")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="总毛重" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" name="weight" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litOrderGrossWeight" runat="server" Text='<%#Eval("OrderGrossWeight")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="总金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name" name="orderNo" rel="<%#Eval("OrderId")%>">
                                <asp:Literal ID="litAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="运输号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                                <asp:Literal ID="litShipOrderNumber" runat="server" Text='<%#Eval("ShipOrderNumber")%>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="失败原因" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name word-break"  style="width:60px;">
                                <asp:Literal ID="litRemark" runat="server" Text='<%#Eval("Remark")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <input value="<%#Eval("DeclareStatus")%>" type="hidden" class="shenbao_type" orderId="<%#Eval("OrderId")%>" IsSendWMS="<%#Eval("IsSendWMS")%>" SendWMSDate="<%#Eval("SendWMSDate")%>"/>
                                <input type="button" value="认领" class="button button4" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 4)"/>
                                <input type="button" value="退回" class="button button5" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 0)"/>
                                <input type="button" value="申报" class="button button0" style="display:none;"/>
                                <input type="button" value="申报成功" class="button button2" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 2)"/>
                                <input type="button" value="申报失败" class="button button3" style="display:none;"  onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 3)"/>
                                <input type="button" value="重新申报" class="button button6" style="display:none;"  onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 1)"/>
                                <input type="button" value="重新认领" class="button button1" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 4)"/>
                                <input type="button" value="重发四单" class="button button7" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 99)"/>
                                <input type="button" value="重新放行" class="button button8" style="display:none;" onclick="loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 98)"/>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
            </Columns>
            </UI:Grid>
      </div>
      <div class="bottomPageNumber clearfix">
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
        </div>
    </div>
    </div>
    <asp:HiddenField runat="server" ID="hidUserid" />
    <asp:HiddenField runat="server" ID="hidUserName" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function clickOrder(OrderId,DeclareStatus) {
            LoadOrderItem(OrderId,DeclareStatus);
        }
        $(function () {
            $(".shenbao_type").each(function () {
                var val = $(this).val();
                if (val == 0) {
                    $(this).parent().find(".button4").show();
                } else if (val == 4) {
                    $(this).parent().find(".button0,.button5").show();
                } else if (val == 3) {
                    $(this).parent().find(".button1").show();
                    $(this).parent().find(".button7").show();
                }
                else if (val == 2 && $(this).attr('IsSendWMS') == 1 && $(this).attr('SendWMSDate')>10) {
                    $(this).parent().find(".button8").show();
                }
                else if (val == 1) {
                    $(this).parent().find(".button2,.button3").show();
                };
            });
            $(".button0").click(function () {
                loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 1);
                $(this).unbind("click");
            });
            $(".ReMoveHS_Declare").live('click', function () {
                var orderid = $(this).attr("OrderID");
                var SkuId = $(this).attr("rel");
                ReMoveHS_Declare($(this),orderid, SkuId)
            })
        });
        function loadstaut(orderId, flag) {
            //选择失败时需填写原因
            if (flag == 3) {
                ShowIngredient("填写失败原因", "失败原因：<textarea rows='6' cols='30' id='txtRemark' />", orderId, flag);
            }
            else if (flag == 1) {
                SendHSData(orderId);
              //  ShowIngredient("填写海关出库单号", "出库单号：<input type='text' id='txtOutNo' style='width:200px;' />", orderId, flag);
                //if($("#txtOutNo").val()!=""){
                //    $(".aui_state_highlight").live("click", function () {});
                // }
            } 
            else {
                if(flag == 2)
                {
                    if(!confirm("确认申报成功！"))
                    {
                        return;
                    }
                }
                if (flag == 99) {
                    if (!confirm("确认重发四单！")) {
                        return;
                    }
                }
                if (flag == 98) {
                    if (!confirm("确认重新放行！")) {
                        return;
                    }
                }
                UpdateStatus(orderId, flag, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
            }
        };

function UpdateStatus(orderId, flag, Remark, Userid, UserName) {
    $.ajax({
        url: "/Handler/HelpHandler.ashx?action=GetHS_Status",
        type: "post",
        datatype: "text/json",
        cache: false,
        data: { "OrderID": orderId, 'Status': flag, "Remark": Remark, "Userid": Userid, "UserName": UserName },
        success: function (data) {
            if (data != null && data.success == "NO") {
                alert(data.MSG);
            } else {
                window.location.reload();
            }
        },
        error: function () {
            alert("提交失败！");
        }
    })
}

function ShowIngredient(titile, content, orderId, flag) {
    dialog = art.dialog({
        id: "Input",
        title: titile,
        content: content,
        resize: false,
        fixed: true,
        height: 150,
        width: 400,
        modal: true, //蒙层（弹出会影响页面大小） 
        button: [{
            name: '保 存', callback: function () {
                if (flag == 3) {
                    if ($("#txtRemark").val() == "") {
                        alert("请输入失败原因！");
                        return false;
                    }
                    if ($("#txtRemark").val().length>100) {
                        alert("失败原因，长度不得超过100个字符！");
                        return false;
                    }
                    UpdateStatus(orderId, flag, $("#txtRemark").val(), $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                }
                else if (flag == 1) {
                    if ($("#txtOutNo").val() == "") {
                        alert("请输入出库单号！");
                        return false;
                    }
                   
                   UpdateStatus(orderId, flag, $("#txtOutNo").val(), $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                }
            }, focus: true
        },
        { name: '关 闭' }
        ]
    });
}

        function LoadOrderItem(OrderID,DeclareStatus) {
    if (OrderID && $("[rel='" + OrderID + "']").attr("flag") == 0) {
        $.ajax({
            url: "/Handler/HelpHandler.ashx?action=GetHS_Declare",
            type: "post",
            datatype: "text/json",
            cache: false,
            data: { "OrderID": OrderID },
            success: function (data) {
                if (data != null && data.success != "NO") {
                    //var htm = "<tr class='getOrder'><td colspan='13'><table width='50%'><tbody><tr><th>商品料件号</th><th>商品名称</th><th>商品备案号</th><th>单项商品单位</th><th>单项商品数量</th><th>单项商品总净重</th><th>单项商品单价</th><th>单项商品总价</th></tr>";
                    var htm = "<tr class='getOrder'><td colspan='18'><table width='50%'><tbody><tr><th>序号</th><th>商品料件号</th><th>商品名称</th><th>商品备案号</th><th>单项商品单位</th><th>单项商品数量</th><th>单项商品总净重</th><th>单项商品总价</th><th>操作</th></tr>";
                    $.each(data, function (index, item) {
                        //htm += "<tr><td width='15%'>" + item.LJNo + "</td><td width='10%'>" + item.HSProductName + "</td><td width='10%'>" + item.ProductRegistrationNumber + "</td><td width='15%'>" + item.Unit + "</td><td width='15%'>" + item.ShipmentQuantity + "</td><td width='15%'>" + item.Weight + "</td><td width='15%'>" + item.ItemAdjustedPrice + "</td><td width='20%'>" + item.ItemAdjustedSumPrice + "</td>";
                        htm += "<tr><td>" + (index + 1) + "</td><td width='15%'>" + item.LJNo + "</td><td width='10%'>" + item.HSProductName + "</td><td width='10%'>" + item.ProductRegistrationNumber + "</td><td width='15%'>" + item.Unit + "</td><td width='15%'>" + item.ShipmentQuantity + "</td><td width='10%'>" + item.Weight + "</td><td width='15%'>" + item.ItemAdjustedSumPrice + "</td>";
                        //只有失败的单才显示作废按钮
                        if (DeclareStatus==3){
                            htm += "<td width='10%'><input type='button' value='作废' class='ReMoveHS_Declare' rel='" + item.SkuId + "' OrderID='" + OrderID + "'></td>";
                        }
                    });
                    htm += "</tbody></table></td></tr>";
                    $("[rel='" + OrderID + "']").parent().parent().after(htm);
                    $("[rel='" + OrderID + "']").attr("flag", '1');
                }
                else {
                    alert("加载失败！" + data.MSG);

                }
            },
            error: function (b) {
                alert("加载失败！" + b.error);
            }
        });
    } else if (OrderID && $("[rel='" + OrderID + "']").attr("flag") == '1') {
        var input = $("[rel='" + OrderID + "']").find("input");
        if (input.attr("checked") == "checked") {
            $("[rel='" + OrderID + "']").parent().parent().next().show();
        }
        else {
            $("[rel='" + OrderID + "']").parent().parent().next().hide();
        }

    }
}
function ReMoveHS_Declare(btn,OrderID, SkuId) {
    //只有一条数据时不能作废
    if (btn.parent().parent().parent().find('tr').length <= 2)
    {
        alert("只有一条记录不允许作废操作！");
        return;
    }
    var a=confirm("确定要作废该条数据吗？");
    if (a==true) {
        $.ajax({
            url: "/Handler/HelpHandler.ashx?action=ReMoveHS_Declare",
            type: "post",
            datatype: "text/json",
            cache: false,
            data: { "OrderID": OrderID, "SkuId": SkuId,"UserId": $("#<%=hidUserid.ClientID%>").val(),"Username": $("#<%=hidUserName.ClientID%>").val() },
            success: function (data) {
                if (data != null && data.success != "NO") {
                    window.location.reload();
                }
                else {
                    alert("加载失败！" + data.MSG);

                }
            },
            error: function (b) {
                alert("加载失败！" + b.error);
            }
        })
    }
};
function SendHSData(orderId) {
    var date = new Date();
    var year = date.getFullYear().toString();
    var month = (date.getMonth() + 1).toString();
    if (month < 10) { month = 0 + month; }
    var getday = (date.getDate()).toString();
    if (getday < 10) { getday = 0 + getday; }
    var sendDate = "01";
    $(".shenbao_type[orderId='" + orderId + "']").parent().find('.button0').unbind("click");
    var arr = ["trafMode", "qty", "weight", "netWt", "consignee", "Province", "consigneeCardId", "consigneeTelephone", "orderNo", "logNo", "payNo"];
    var SendHSData = {
        "trafMode": "",//运输工具名称
        "wrapType": "2",//包装种类
        "wrapTypeName": "纸箱",//包装种类名称
        "qty": "",//总件数
        "weight": "12",//商品毛重
        "netWt": "12",//商品净重
        "OutCountry": "142",//启运国
        "OutCountryName": "中国",//启运国
        "SenderID": "深圳市信捷网科技有限公司",//发件人
        "shipperCountry": "142",//发件人国别
        "shipperCountryName": "中国",//发件人国别
        "shipperAddress": "深圳",//发件人城市
        "consignee": "",//收件人
        "consigneeCountry": "142",//收件人国别
        "consigneeCountryName": "中国",//收件人国别
        "Province": "",//收件人城市
        "consigneeCardId": "",//收件人身份证
        "consigneeTelephone": "",//收件人电话
        "SalerNo": "4403660034",//经营单位编码
        "SalerName": "深圳市信捷网科技有限公司",//经营单位名称
        "ebpCode": "4403660034",//电商平台代码
        "orderNo": "",//订单编号
        "logisticsCode": "4403660034",//物流企业编号
        "logNo": "123456668",//运单编号
        "payCode": "5801646760",//支付企业代码
        "payNo": "",//支付信息编号
        "sendDateNo":(year + month + getday + sendDate),//物流运输编号
        "ebpName": "信捷网/深圳市信捷网科技有限公司",//备案企业名称
        "goodsInfo": []//商品信息
    };
    $.each(arr, function (i, item) {
        SendHSData[item] = $("[rel='" + orderId + "'][name='" + item + "']").html();
    });
    var prove = SendHSData.Province.split("，");
    SendHSData.Province = prove[1].replace("市",'');
    $.ajax({
        url: "/Handler/HelpHandler.ashx?action=GetHS_Declare",
        type: "post",
        datatype: "text/json",
        cache: false,
        data: { "OrderID": orderId },
        success: function (data) {
            if (data != null && data.success != "NO") {
                SendHSData["goodsInfo"] = [];
                $.each(data, function (index, item) {
                    var info = {
                        "LJNo": item.LJNo,//料件号 
                        "ShipmentQuantity": item.ShipmentQuantity,//商品数量
                        "ItemAdjustedSumPrice": item.ItemAdjustedSumPrice,//商品总价
                        "currency": "142",//币制
                        "RuleNo": item.Weight//法定数量
                    }
                    SendHSData["goodsInfo"].push(info);
                });
                var str = "";
                if (!external.xlk2kj) {
                    alert("请使用云报关系统进行报关");
                }
                str = external.xlk2kj(JSON.stringify(SendHSData));
                    $("[rel='" + orderId + "'][name='formId']").html(JSON.parse(str).formId);
                    UpdateStatus(orderId, 1, JSON.parse(str).formId, $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                    if (JSON.parse(str).d == "0") {
                       // alert(JSON.parse(str).str);
                        $(".shenbao_type[orderId='" + orderId + "']").parent().find('.button0').bind("click", function () {
                            loadstaut($(this).parent().find('.shenbao_type').attr('orderId'), 1);
                        });
                        UpdateStatus(orderId,3, JSON.parse(str).str, $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                    }
            }
            else {
                alert("加载失败！" + data.MSG);
            }
        }
    });
};
    </script>
</asp:Content>