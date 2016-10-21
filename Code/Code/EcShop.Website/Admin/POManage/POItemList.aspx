<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.POItemList" %>


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
    <input type="hidden" id="hidPOId" runat="server" />
    <input type="hidden" id="hidSupplierId" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>采购订单明细管理</h1>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="javascript:void(0)" onclick="ShowAddDiv()" class="submit_jia100" style="float: left; margin-right: 10px;">添加商品</a>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>商品名称：</span><span>
                        <asp:TextBox ID="txtProductsName" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>产品条形码：</span><span>
                        <asp:TextBox ID="txtBarCode" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
               
                    <li></li>
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
                <div >
                    <asp:Label ID="lblTotal" runat="server" style="color:#ff4800;"></asp:Label>
                </div>
            </div>
            <!--数据列表区域-->
            <div style="overflow-x:auto;">
            <asp:Repeater ID="rpPOItem" runat="server" OnItemCommand="rpPOItem_ItemCommand" OnItemDataBound="rpPOItem_ItemDataBound">
                <HeaderTemplate>
                    <table border="0" cellspacing="0"  cellpadding="0">
                        <tr class="table_title">
                            <td>商品代码</td>
                            <td width="10%">商品名称</td>
                            <td>产品条形码</td>
                            <td>外箱条形码</td>
                            <td>订单数量</td>
                            <td>入库数量</td>
                            <td>是否样品</td>
                            <td>生产日期</td>
                            <td>有效日期</td>
                            <td>生产批号</td>
                            <td>商品总净重</td>
                            <td>商品总毛重</td>
                            <td>币别</td>
                            <%--<td>汇率</td>--%>
                            <td>原币价</td>
                            <td>原币总价</td>
                            <td>销售价</td>
                            <td>销售总价</td>
                            <td>装箱规格</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("SkuId")%>&nbsp;
                        </td>
                        <td>
                            <a href="../../ProductDetails.aspx?productId=<%#Eval("ProductId")%>" target="_blank"><%#Eval("ProductName")%></a>
                        </td>
                        <td>
                            <%#Eval("BarCode")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("BoxBarCode")%>&nbsp;
                        </td>

                        <td>
                            <%#Eval("ExpectQuantity")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("PracticalQuantity")%>&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblIsSample" runat="server" Text='<%#Eval("IsSample")%>'></asp:Label>
                        </td>

                        <td>
                            <%#Eval("ManufactureDate").ToString().Length>0?Convert.ToDateTime(Eval("ManufactureDate")).ToString("yyyy-MM-dd"):""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("EffectiveDate").ToString().Length>0?Convert.ToDateTime(Eval("EffectiveDate")).ToString("yyyy-MM-dd"):""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("BatchNumber")%>&nbsp;
                        </td>

                        <td>
                            <%#Eval("NetWeight")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("RoughWeight")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Name_CN")%>&nbsp;
                        </td>
                        <%--<td> ,
                            <%#Eval("Rate")%>&nbsp;
                        </td>--%> 
                        <td>
                            <%#Eval("OriginalCurrencyPrice").ToString()!="" ? decimal.Parse(Eval("OriginalCurrencyPrice").ToString()).ToString("F2") : ""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("OriginalCurrencyTotalPrice").ToString()!="" ? decimal.Parse(Eval("OriginalCurrencyTotalPrice").ToString()).ToString("F2") : ""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("SalePrice").ToString()!="" ? decimal.Parse(Eval("SalePrice").ToString()).ToString("F2") : ""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TotalSalePrice").ToString()!="" ? decimal.Parse(Eval("TotalSalePrice").ToString()).ToString("F2") : ""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CartonSize")%>&nbsp;
                        </td>

                        <td>
                            <a href='<%# "../../admin/POManage/POItemAdd.aspx?Id=" + Eval("Id")+"&POId="+POId+"&SupplierId="+SupplierId%> ' class="SmallCommonTextButton">编辑</a>
                            <asp:LinkButton ID="LinkButton1" CommandName="del" CommandArgument='<%#Eval("Id")%>' OnClientClick="javascript:return confirm('确定要执行删除操作吗？删除后将不可以恢复')"
                                runat="server">删除</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <a href="POList.aspx">返 回</a><UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function ShowAddDiv() {
            var POId = $("#ctl00_contentHolder_hidPOId").val().replace(/\s/g, "");
            var supplierId = $("#ctl00_contentHolder_hidSupplierId").val().replace(/\s/g, "");
            if (POId != "" && parseInt(POId) > 0) {
                DialogFrame("POManage/SearchSuppliearchProduct.aspx?POId=" + POId + "&supplierId=" + supplierId, "添加商品", 975, null);
            }
        }
    </script>
</asp:Content>