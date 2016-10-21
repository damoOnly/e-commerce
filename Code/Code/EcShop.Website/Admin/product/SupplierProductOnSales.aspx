<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="SupplierProductOnSales.aspx.cs" Inherits="EcShop.UI.Web.Admin.SupplierProductOnSales" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
     <script type="text/javascript">
         var $div = $("div[class='dataarea mainwidth databody']");
         var divstyle = "display:none; position: absolute; z-index: 500; top: 0px; left: 0px; opacity: 0.4; background-color: rgb(51, 51, 51);";
         $("#popDivLock1").attr("style", divstyle + "height:" + $div.height() + ";width:" + $div.width());
         function UploadingClick() {
             //弹出遮罩层
             $("#popDivLock1").show();
             $("#divUpload").show();
         }
         function hideDiv() {
             $("#popDivLock1").hide();
             $("#divUpload").hide();
         }
    </script>    
    <div class="dataarea mainwidth databody">
     <div id="popDivLock1" style="display:none; height: 100%; width: 100%; position: absolute; z-index: 500; top: 0px; left: 0px; opacity: 0.4; background-color: rgb(51, 51, 51);"></div>
        <div id="divUpload" style="display:none;height:200px; background: #fff none repeat scroll 0 0; left: 0; margin: 0 auto; opacity: 1; position: absolute; right: 0; text-align: center; vertical-align: middle; width: 450px;padding:25px 5px 5px; z-index: 501;">
            <table style="text-align: left; width: 100%;">
                <tr>
                    <td ><span class="pop-close" onclick="hideDiv()"></span></td>
                </tr>
                <tr>
                    <td style=" text-align:center">上传文件:<asp:FileUpload ID="fileCheck" runat="server"/>&nbsp;&nbsp;<a href="../TemplatesFile/供应商商品上传模版.csv">上传模版</a></td>
                </tr>
                <tr>
                    <td class="submit_btnshangjia"  style="padding-left:40%; padding-top:10px"> <asp:LinkButton ID="linkUpload" runat="server" Text="上传" /></td>
                </tr>
            </table>
        </div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32"  style=" " /></em>
            <h1>
                商品管理</h1>
            <span>商城中所有的商品，您可以对商品进行搜索，也能对商品进行编辑、上架、下架、入库操作</span>
        </div>
        <div class="datalist">
            <!--搜索-->
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
                    <li>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server"  ID="ddlImportSourceType"  NullToDisplay="--请选择原产地--" Width="160"/>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                             <Hi:SupplierDropDownList runat="server"  ID="ddlSupplier" Width="153"/>
                        </abbr>
                    </li>

                    <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsApproved" runat="server">
                                <asp:ListItem Value="-1">--请选择审核状态--</asp:ListItem>
                                <asp:ListItem Value="0">未审核</asp:ListItem>
                                <asp:ListItem Value="1">已审核</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>

                     <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsApprovedPrice" runat="server">
                                <asp:ListItem Value="-1">--请选择审价状态--</asp:ListItem>
                                <asp:ListItem Value="0">未审核</asp:ListItem>
                                <asp:ListItem Value="1">审核通过</asp:ListItem>
                                <asp:ListItem Value="2">审核不通过</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsAllClassify" runat="server">
                                <asp:ListItem Value="-1">--请选择备案状态--</asp:ListItem>
                                <asp:ListItem Value="0">未完成</asp:ListItem>
                                <asp:ListItem Value="1">已完成</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>

                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>商品编码：</span><span>
                        <asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
                     <li><span>条形码：</span><span>
                        <asp:TextBox ID="txt_BarCode" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li><span>添加时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li></li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                     <li>
                        <p>
                            <asp:LinkButton ID="btnCreateReport" runat="server" Text="导出Excel" />
                        </p>
                    </li>
                    <li>
                        <p><a id="btnUploadingCheck" href="#" onclick="UploadingClick()">导入商品</a></p>

                    </li>
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
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> 
                             <span class="reverseSelect">  <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> 
                             <span class="reverseSelect">
                                  <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" />
                            </span>

                             <span class="reverseSelect">
                                  <Hi:ImageLinkButton ID="btnReSubmitPriceApprove" runat="server" Text="审价" IsShow="true" DeleteMsg="确定把这些商品重新提交审价？" />
                            </span>

                            <span class="printProduct"><a href="javascript:void(0)" onclick="PrintProduct()"  class ="submit_jia_1">打印二维码</a></span>
                            <span class="btn generationQCode"  >
                                    <Hi:ImageLinkButton ID="btnGenerationQCode" runat="server" Text="生成二维码" class ="submit_jia_1"  /></span>
                            <select id="dropBatchOperation">
                                <option value="">批量操作..</option>
                                <option value="1">商品上架</option>
                                <option value="2">商品下架</option>
                                <option value="3">商品入库</option>
                                <option value="5">设置包邮</option>
                                <option value="6">取消包邮</option>
                                <option value="10">调整基本信息</option>
                                <option value="11">调整显示销售数量</option>
                                <option value="12">调整库存</option>
                                <option value="13">调整会员零售价</option>
                                <option value="15">调整商品关联标签</option>
                                <option value="16">调整商品推广佣金</option>
                            </select>
                        </li>
                    </ul>
                </div>
                <div class="filterClass">
                    <span><b>出售状态：</b></span> <span class="formselect">
                        <Hi:SaleStatusDropDownList AutoPostBack="true" ID="dropSaleStatus" runat="server" />
                    </span><span class="formselect"></span>
                </div>
            </div>
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="商品代码" DataField="SkuId" ItemStyle-Width="35px" HeaderStyle-CssClass="td_right td_left" />
                    <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="35px"
                        HeaderStyle-CssClass="td_right td_left" />
                    <asp:TemplateField ItemStyle-Width="42%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;">
                                <%# ProductImg(Eval("IsApproved").ToString(),Eval("ProductId").ToString(),Eval("ThumbnailUrl40").ToString()) %>
                                <%--<a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                </a>--%>
                            </div>
                            <div style="float: left;">
                                <span class="Name"><%# ProductDetails(Eval("IsApproved").ToString(),Eval("ProductId").ToString(),Eval("ProductName").ToString()) %>
                                    <%--<a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                    target="_blank">
                                    <%# Eval("ProductName") %></a>--%></span> <span class="colorC">商品编码：<%# Eval("ProductCode") %>库存：<%# Eval("Stock") %>成本：<%# Eval("CostPrice", "{0:f2}")%></span></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="商品价格" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">一口价：<%# Eval("SalePrice", "{0:f2}")%></span><span class="colorC">市场价：<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品状态" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="审核状态" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litIsApproved" runat="server" Text='<%#Eval("IsApproved").ToString()=="True"?"审核":"未审核"%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="审价状态" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsApprovedPrice" runat="server" Text='<%#Eval("IsApprovedPrice").ToString()=="0"?"未审核":(Eval("IsApprovedPrice").ToString()=="1"?"审核通过":"审核不通过,拒绝原因:"+Eval("ApprovedPriceDescription").ToString())%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="备案状态" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsAllClassify" runat="server" Text='<%#Eval("IsAllClassify").ToString()=="1"?"已完成":"未完成"%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="二维码" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <a href='<%#Eval("QRcode").ToString().Replace("~","") %>' class="tooltip" target="_blank"><img src='<%#Eval("QRcode").ToString().Replace("~","") %>' alt="二维码"  width="50px" height="50px"/></a>

                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="原产地" ItemStyle-Width="120" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span style="float:left;">
                                <img src='<%#Eval("Icon") %>' width="50px" height="50px" />
                            </span>
                            <span style=" line-height:50px;"> <%#Eval("CnArea")%></span>  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="180px" HeaderStyle-CssClass=" td_left td_right_fff">
                        <ItemTemplate> 
                            <span class="submit_bianji"><a href="<%# "../../admin/default.html?product/SupplierEditProduct.aspx?productId="+Eval("ProductId")%>" target="_blank"> 
                                编辑</a></span> <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:CollectionProduct('<%# "SupplierEditReleteProducts.aspx?productId="+Eval("ProductId")%>')">
                                    相关商品</a></span> <span class="submit_shanchu">
                                        <Hi:ImageLinkButton ID="btnDel" CommandName="Delete" runat="server" Text="删除" IsShow="true"
                                            DeleteMsg="确定要把商品移入回收站吗?" /></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
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
    <%-- 上架商品--%>
    <div id="divOnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要上架商品？上架后商品将前台出售</em></p>
        </div>
    </div>
    <%-- 下架商品--%>
    <div id="divUnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要下架商品？</em></p>
        </div>
    </div>
    <%-- 入库商品--%>
    <div id="divInStockProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要将商品入库？</em></p>
        </div>
    </div>
    <%-- 设置包邮--%>
    <div id="divSetFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要设置这些商品包邮？</em></p>
        </div>
    </div>
    <%-- 取消包邮--%>
    <div id="divCancelFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要取消这些商品的包邮？</em></p>
        </div>
    </div>
    <%-- 商品标签--%>
    <div id="divTagsProduct" style="display: none;">
        <div class="frame-content">
            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
        </div>
    </div>
     <%-- 商品推广佣金--%>
    <div id="divDeduct" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
             <table cellpadding="0" cellspacing="0" width="550px" border="0" class="fram-retreat">
                  <tr>
                    <td align="right" width="20%">直接推广佣金：</td>
                    <td align="left"  class="bd_td"><asp:TextBox ID="txtReferralDeduct" CssClass="forminput" runat="server" />&nbsp;% <asp:Literal ID="litReferralDeduct" runat="server" /><p>推广员分享链接产生有效订单时享受的佣金比例</p></td>
                  </tr>             
                  <tr>
                    <td align="right">下级会员佣金：</td>
                    <td align="left"  class="bd_td"><asp:TextBox ID="txtSubMemberDeduct" CssClass="forminput" runat="server" />&nbsp;%<asp:Literal ID="litSubMemberDeduct" runat="server" /><p>下级会员未通过分享链接进入商城时产生的订单，推广员可享受的佣金比例</p></td>
                  </tr>
                  <tr>
                    <td align="right">下级推广员佣金：</td>
                    <td align="left"  class="bd_td"> <asp:TextBox ID="txtSubReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%<asp:Literal ID="litSubReferralDeduct" runat="server" /><p>推广员分享链接产生有效订单时，其上级推广员也可获得相应的佣金比例</p></td>
                  </tr>                 
                </table>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnUpdateProductTags" runat="server" Text="调整商品标签" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUpdateProductDeducts" runat="server" Text="调整商品推广佣金" CssClass="submit_DAqueding" />
        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
        <asp:Button ID="btnInStock" runat="server" Text="入库商品" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUnSale" runat="server" Text="下架商品" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUpSale" runat="server" Text="上架商品" CssClass="submit_DAqueding" />
        <asp:Button ID="btnSetFreeShip" runat="server" Text="设置包邮" CssClass="submit_DAqueding" />
        <asp:Button ID="btnCancelFreeShip" runat="server" Text="取消包邮" CssClass="submit_DAqueding" />
    </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js?v=20150919"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dropBatchOperation").bind("change", function () { SelectOperation(); });
        });

        function SelectOperation() {
            var Operation = $("#dropBatchOperation").val();
            var productIds = GetProductId();
            if (productIds.length > 0) {
                switch (Operation) {
                    case "1":
                        formtype = "onsale";
                        arrytext = null;
                        DialogShow("商品上架", "productonsale", "divOnSaleProduct", "ctl00_contentHolder_btnUpSale");
                        break;
                    case "2":
                        formtype = "unsale";
                        arrytext = null;
                        DialogShow("商品下架", "productunsale", "divUnSaleProduct", "ctl00_contentHolder_btnUnSale");
                        break;
                    case "3":
                        formtype = "instock";
                        arrytext = null;
                        DialogShow("商品入库", "productinstock", "divInStockProduct", "ctl00_contentHolder_btnInStock");
                        break;
                    case "5":
                        formtype = "setFreeShip";
                        arrytext = null;
                        DialogShow("设置包邮", "setFreeShip", "divSetFreeShip", "ctl00_contentHolder_btnSetFreeShip");
                        break;
                    case "6":
                        formtype = "cancelFreeShip";
                        arrytext = null;
                        DialogShow("取消包邮", "cancelFreeShip", "divCancelFreeShip", "ctl00_contentHolder_btnCancelFreeShip");
                        break;
                    case "10":
                        DialogFrame("product/EditBaseInfo.aspx?ProductIds=" + productIds, "调整商品基本信息", null, null);
                        break;
                    case "11":
                        DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "调整前台显示的销售数量", null, null);
                        break;
                    case "12":
                        DialogFrame("product/EditStocks.aspx?ProductIds=" + productIds, "调整库存", 880, null);
                        break;
                    case "13":
                        DialogFrame("product/EditMemberPrices.aspx?ProductIds=" + productIds, "调整会员零售价", 1000, null);
                        break;
                    case "15":
                        formtype = "tag";
                        setArryText('ctl00_contentHolder_txtProductTag', "");
                        DialogShow("设置商品标签", "producttag", "divTagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
                        break;
                    case "16":
                        formtype = "deduct";
                        DialogShow("设置商品推广佣金", "productdeduct", "divDeduct", "ctl00_contentHolder_btnUpdateProductDeducts");
                        break;
                }
            }
            $("#dropBatchOperation").val("");
        }

        function GetProductId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                alert("请选择商品");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "相关商品");
        }

        function validatorForm() {
            switch (formtype) {
                case "tag":
                    if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                        alert("请选择商品标签");
                        return false;
                    }
                    break;
                case "onsale":
                case "unsale":
                case "instock":
                case "setFreeShip":
                case "cancelFreeShip":
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                    break;
                case "deduct":
                    if($("#ctl00_contentHolder_txtReferralDeduct").val() == ""){
                        alert("请输入直接推广佣金");
                        return false;
                    }
                    if($("#ctl00_contentHolder_txtSubMemberDeduct").val() == ""){
                        alert("请输入下级会员佣金");
                        return false;
                    }
                    if($("#ctl00_contentHolder_txtSubReferralDeduct").val() == ""){
                        alert("请输入下级推广员佣金");
                        return false;
                    }
                    setArryText('ctl00_contentHolder_txtReferralDeduct', $("#ctl00_contentHolder_txtReferralDeduct").val());
                    setArryText('ctl00_contentHolder_txtSubMemberDeduct', $("#ctl00_contentHolder_txtSubMemberDeduct").val());
                    setArryText('ctl00_contentHolder_txtSubReferralDeduct', $("#ctl00_contentHolder_txtSubReferralDeduct").val());
                    break;
            };
            return true;
        }
        function PrintProduct()
        {
            var v_str = "";
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });
            if (v_str.length == 0) {
                alert("请选择商品");
                return "";
            }
            DialogFrame("./product/PrintProduct.aspx?ProductsId=" + v_str, "商品打印", 975, null);
        }
    </script>
</asp:Content>
