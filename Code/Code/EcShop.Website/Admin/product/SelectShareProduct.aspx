<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="SelectShareProduct.aspx.cs" Inherits="EcShop.UI.Web.Admin.SelectShareProduct" %>

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
    <div class="optiongroup mainwidth">
        <ul>
            <li class="optionstar"><a href="ShareProduct.aspx"><span>分享页商品</span></a></li>
            <li class="menucurrent"><a href="javascript:void(0)"><span>已选择分享商品<font style="color: Red; font-weight: bolder"><asp:Literal ID="litnumber" runat="server"></asp:Literal></font></span></a></li>
        </ul>
    </div>
    <div class="dataarea mainwidth databody">
    
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>
                分享页商品</h1>
            <span>可以批量制作需要分享的商品，通过微信营销进行商品推送分享，对客户快速在微信进行传播下单直接到达供应商进行发货 </span>
        </div>
        <asp:Panel ID="panlproducts" runat="server">
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
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> <span class="delete">
                                    <asp:LinkButton ID="btnBatchDelete" Text="删除" runat="server" OnClientClick="javascript:return BatchDelete()"></asp:LinkButton>
                                </span><span class="downproduct"><a href="javascript:void(0)" onclick="return ShowShareProducts()">
                                    制作分享页</a> </span>
                                    <span><font color="red">点击制作分享页会立刻把列表页的商品做成分享页(同时会清空已选择的商品列表),本操作无需勾选商品</font></span>
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
                    <th class=" td_left td_right_fff" scope="col">
                        操作
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
                                        <%# Eval("ProductName") %></a></span> <span class="colorC">商家编码：<%# Eval("ProductCode") %>库存：<%# Eval("Stock") %>成本：<%# Eval("CostPrice", "{0:f2}")%></span></div>
                            </td>
                            <td>
                                <span class="Name">一口价：<%# Eval("SalePrice", "{0:f2}")%></span><span class="colorC"> 市场价：<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                            </td>
                            <td>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal> 
                            </td>
                            <td>
                                <span class="submit_bianji">
                                    <asp:LinkButton ID="btnAdd" runat="server" Text="删除" CommandArgument='<%# Eval("ProductId") %>'
                                        CommandName="delete"></asp:LinkButton>
                                </span>
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
        </asp:Panel>
        <asp:Panel ID="SharePanel" runat="server" CssClass="datalist" Visible="false">
            <table cellpadding="0" cellspacing="0" border="0" class="share_table">
      <tr>
        <td width="8%" valign="top"><img src="../images/successico.jpg" /></td>
        <td width="92%" class="cart_payment_td">
            <div><span style=" color:Red; font-size:14px; font-weight:bold">分享制作成功，您可以开始给小伙伴分享您的宝贝啦！</span></div>
            <div style=" height:30px;"><input type="text" id="txtlink" clientidmode="Static" runat="server"
                    class="forminput" style="width: 300px" readonly="readonly" /><input type="button" id="btnCopy" onclick="success();" value="复制" data-clipboard-target="txtlink" class="searchbutton" /></div>
            <div><asp:Image ID="imgurl" runat="server" /></div>
        </td>
      </tr>
    </table>
        </asp:Panel>
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
    
<script type="text/javascript" src="/utility/ZeroClipboard.min.js"></script>
    <script>
        var copy = new ZeroClipboard(document.getElementById("btnCopy"), {
            moviePath: "/utility/ZeroClipboard.swf"
        });
        function success() {
            alert("复制成功，复制内容为：" + document.getElementById("txtlink").value);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">

<script type="text/jscript">


        function BatchDelete() {
            if (!confirm("确认批量删除已选商品?")) {
                return false;
            }
            var productIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                productIds += $(this).val() + ",";
            });
            if (productIds == "") {
                alert("请先选择要删除的商品");
                return false;
            }
        }

        function ShowShareProducts() {
            arrytext = null;
            var tit = $("#ctl00_contentHolder_txttitle").val();

            setArryText('ctl00_contentHolder_txttitle', tit);
            DialogShow("分享页制作", 'shareproduct', 'divShareProduct', 'ctl00_contentHolder_btnSharePage');
        }

        function validatorForm() {
            if ($("#ctl00_contentHolder_txttitle").val().length <= 0) {
                alert("请输入分享页标题");
                return false;
            }
            return true;
        }

        function ShowShareLink() {
            arrytext = null;
            DialogShow("我要分享", 'shareproduct', 'divShareProduct', 'ctl00_contentHolder_btnSharePage');
        }
    </script>
</asp:Content>
