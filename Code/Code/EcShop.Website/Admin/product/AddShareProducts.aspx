<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddShareProducts.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddShareProducts" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
<style>

.dataarea .datalist table.share_table{ width:500px; overflow:hidden; margin:0 auto; padding:100px 0px;}
.dataarea .datalist table td,.dataarea .datalist table.share_table{ border:0px;}
.share_table .cart_payment_td div{ clear:both; margin-bottom:20px;}
.share_table .cart_payment_td div input{ margin-right:10px;}
</style>
    <div class="dataarea mainwidth databody">
    
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>分享页商品</h1>
            <span>添加分享商品</span>
        </div>
        <div class="datalist">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--"
                                Width="150" />
                        </abbr>
                    </li>
                
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--"
                                Width="153">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择标签--"
                                Width="153">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择类型--" Width="153" />
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
               
                    <li><span>添加时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                
                    <li>
                        <asp:CheckBox runat="server" ID="chkIsAlert" Text="库存报警" />
                    </li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <div class="functionHandleArea clearfix">
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
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> 
                            <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                                <span class="downproduct"><asp:LinkButton ID="btnBtachAdd" runat="server" Text="添加商品"></asp:LinkButton></span>
                               </li>
                    </ul>
                </div>
            </div>
            <div class="blank8 clearfix">
            </div>
            <table cellspacing="0" border="0" id="ctl00_contentHolder_grdProducts" style="width: 100%;
                border-collapse: collapse;">
                <tr class="table_title">
                    <th class="td_right td_left" scope="col">
                        选择
                    </th>
                    <th class="td_right td_left" scope="col">
                        排序
                    </th>
                    <th class="td_right td_left" scope="col">
                        商品
                    </th>
                    <th class="td_right td_left" scope="col">
                        商品价格
                    </th>
                    <th class="td_right td_left" scope="col">
                        状态
                    </th>
                </tr>
                <asp:Repeater ID="rp_shareproduct" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </td>
                            <td>
                                <%# Eval("DisplaySequence")%>
                            </td>
                            <td>
                                <div style="float: left; margin-right: 10px;">
                                    <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                    </a>
                                </div>
                                <div style="float: left;">
                                    <span class="Name"><a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                        target="_blank">
                                        <%# Eval("ProductName") %></a></span> <span class="colorC">商家编码：<%# Eval("ProductCode") %>
                                            库存：<%# Eval("Stock") %>
                                            成本：<%# Eval("CostPrice", "{0:f2}")%>
                                        </span>
                                </div>
                            </td>
                            <td>
                                <span class="Name">一口价：<%# Eval("SalePrice", "{0:f2}")%>
 
                                        市场价：<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                            </td>
                            <td>
                               <asp:Literal ID="litMakeState" runat="server" Text='<%# Eval("MakeState") %>'></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
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
    <div id="divShareProduct" style="display: none;">
        <div class="frame-content">
            <p>
                为您的分享页制作标题 <span class="formitemtitle Pw_198"></span>
            </p>
            <p>
                <input type="text" id="txttitle" runat="server" class="forminput" style="width: 400px" /></p>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnSharePage" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script>

        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>
</asp:Content>
