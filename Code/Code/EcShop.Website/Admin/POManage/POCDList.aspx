<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.POCDList" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/pro-list.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/autocompleter.main.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>报关单列表</h1>
        </div>
       
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>入库单号：</span><span>
                        <asp:TextBox ID="txtfromID" Width="150" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>PO单号：</span><span>
                        <asp:TextBox ID="txtPONumber" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
<%--                    <li><span>状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125">
                                <asp:ListItem Value="-1" Selected="True">--请单据状态--</asp:ListItem>
                                <asp:ListItem Value="0">****</asp:ListItem>
                                <asp:ListItem Value="1">****</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>--%>
                    <li>
                        <span>创建时间：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="CreateStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="CreateEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>

                </ul>
                <ul>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea">

                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>

                    </ul>
                </div>
            </div>
            <!--数据列表区域-->
            <asp:Repeater ID="rpPO" runat="server" OnItemCommand="rpPO_ItemCommand" >
                <HeaderTemplate>
                    <table border="0" cellspacing="0" width="80%" cellpadding="0">
                        <tr class="table_title">
                            <td>选择</td>
                            <td>PO订单号</td>
                            <td>入库单号</td>
                            <td>合同号</td>
                            <td>总毛重</td>
                            <td>总净重</td>
                            <td>总件数</td>
                            <td>报关总价</td>
                            <td>报关币别</td>
                            <td>成交方式</td>
                            <td>供货商</td>
                            <td>单据日期</td>
                            <td>创建时间</td>
                            <td>创建人</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr rel='<%#Eval("HeaderID") %>'>
                        <td>
                            <input name="RadioGroup" type="radio" value='<%#Eval("HeaderID") %>' onclick="lookPoDetail('<%#Eval("HeaderID")%>')"/>
                        </td>
                        <td>
                            <%#Eval("PONumber")%>
                        </td>
                        <td>
                            <%#Eval("formID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("contractNo")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("RoughWeight")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("NetWeight")%>&nbsp;
                        </td>                     
                        <td>
                            <%#Eval("ExpectQuantity")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TotalCostPrice")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Name_EN")%>&nbsp;
                        </td>      
                        <td>
                            <%#Eval("TradeType")%>&nbsp;
                        </td>  
                        <td>
                            <%#Eval("SupplierName")%>&nbsp;
                        </td>                                                                               
                        <td>
                            <%#Eval("CreateTime1")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateTime")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateUserName")%>&nbsp;
                        </td>
<%--                        <td>
                            <a href='<%# "../../admin/POManage/POAdd.aspx?Id=" + Eval("Id")%> ' class="SmallCommonTextButton">编辑</a>
                            <asp:LinkButton ID="LinkButton1" CommandName="del" CommandArgument='<%#Eval("Id")%>' OnClientClick="javascript:return confirm('确定要执行删除操作吗？删除后将不可以恢复')"
                                runat="server">删除</asp:LinkButton>
                            <a href='<%# "../../admin/POManage/POItemList.aspx?Id=" + Eval("Id")+"&SupplierId="+ Eval("SupplierId")%> ' class="SmallCommonTextButton">采购明细</a>
                        </td>--%>
                        <td>
                            <a href="javascript:void(0)" onclick="lookPoDetail('<%#Eval("HeaderID")%>')" >查看明细</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="blank12 clearfix">
            </div>
        </div>
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
    <asp:HiddenField runat="server" ID="hidUserid" />
    <asp:HiddenField runat="server" ID="hidUserName" />
    <script>
        function lookPoDetail(id){
            if($("tr[rel='"+id+"']").next().hasClass("detail")){
                if($("tr[rel='"+id+"']").next().css('display')=='table-row')
                { $("tr[rel='"+id+"']").next().hide()}
                else{$("tr[rel='"+id+"']").next().show()}
            }else{
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=POCDPurchaseOrderList",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "Id": id },
                success:function(data){
                    var htm="<tr class='detail'><td colspan='15'><table>";//<tr><td><span>PO订单号："+id+"</td></tr>
                    htm+="<tr><td>单据状态</td><td>备案名称</td><td>数量</td><td>品牌</td><td>原产国</td><td>单价</td><td>总价</td><td>净重</td>";
                    htm+="<td>件数</td><td>料件号</td><td>海关编码</td><td>备案编号</td><td>备案单位</td><td>规格型号</td><td>海关监管条件</td><td>日期</td><td>供货商</td><td>创建日期</td><td>创建人</td><td>操作</td></tr>";
                    for(var i=0;i<data.length;i++){
                        htm+="<tr><td>"+data[i].POStatus+"</td><td>"+data[i].ProductName+"</td><td>"+data[i].Qty+"</td><td>"+data[i].Brand+"</td><td>"+data[i].OriginCountry+"</td><td>"+data[i].UnitPrice+"</td><td>"+data[i].Amoun+"</td><td>"+data[i].NetWight+"</td>";
                        htm+="<td>"+data[i].QtyCnts+"</td><td>"+data[i].LJNo+"</td><td>"+data[i].HS_CODE+"</td><td>"+data[i].ProductRegistrationNumber+"</td><td>"+data[i].HSUnit+"</td><td>"+data[i].HSItemNo+"</td><td>"+data[i].Control_Inspection+"</td><td>"+data[i].createtime+"</td>";
                        htm+="<td>"+data[i].SupplierName+"</td><td>"+data[i].poCreateTime+"</td><td>"+data[i].CreateUser+"</td><td><a onclick='loadthird(\""+data[i].BodyID+"\")' href='javascript:void(0)'>查看详情</a></td></tr>";}
                        htm+="</table></td></tr>";
                    $("tr[rel='"+id+"']").after(htm);
                },
                error:function(){alert(id);}
            })
           }
        }
        function loadthird(id){
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=POCDPurchaseOrderBodyLj",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "Id": id },
                success:function(data){
                    var htm="<table>";//<tr><td><span>PO订单号："+id+"</td></tr>
                    htm+="<tr><td>单据状态</td><td>PO号</td><td>备案名称</td><td>数量</td><td>品牌</td><td>原产国</td><td>单价</td><td>总价</td><td>净重</td><td>毛重</td>";
                    htm+="<td>件数</td><td>料件号</td><td>海关编码</td><td>备案编号</td><td>备案单位</td><td>规格型号</td><td>条形码</td><td>生产日期</td><td>保质日期</td><td>日期</td><td>供货商</td><td>创建日期</td><td>创建人</td></tr>";
                    for(var i=0;i<data.length;i++){
                        htm+="<tr><td>"+data[i].POStatus+"</td><td>"+data[i].POId+"</td><td>"+data[i].ProductName+"</td><td>"+data[i].Qty+"</td><td>"+data[i].Brand+"</td><td>"+data[i].OriginCountry+"</td><td>"+data[i].UnitPrice+"</td><td>"+data[i].Amoun+"</td><td>"+data[i].NetWight+"</td><td>"+data[i].GWWight+"</td>";
                        htm+="<td>"+data[i].QtyCnts+"</td><td>"+data[i].LJNo+"</td><td>"+data[i].HS_CODE+"</td><td>"+data[i].ProductRegistrationNumber+"</td><td>"+data[i].HSUnit+"</td><td>"+data[i].HSItemNo+"</td><td>"+data[i].BarCode+"</td><td>"+data[i].ManufactureDate+"</td><td>"+data[i].EffectiveDate+"</td><td>"+data[i].createtime+"</td>";
                        htm+="<td>"+data[i].SupplierName+"</td><td>"+data[i].poCreateTime+"</td><td>"+data[i].CreateUser+"</td></tr>";}
                    htm+="</table>";
                    //$("tr[rel='"+id+"']").after(htm);
                    dialog = art.dialog({
                        id: "Input",
                        title:'查看明细',
                        content: htm,
                        resize: false,
                        fixed: true,
                        height:'',
                        width:'100%',
                        modal: true
                    })
                },
                error:function(){alert('加载失败');}
            })
        
        
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
   
</asp:Content>
