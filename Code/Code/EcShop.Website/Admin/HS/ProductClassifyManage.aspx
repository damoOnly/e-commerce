<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.HS.ProductClassifyManage" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/pro-list.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/autocompleter.main.css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>商品归类&备案</h1>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="javascript:void(0)" onclick="ShowTags('1')" class="submit_jia100" style="float: left; margin-right: 10px;">商品备案</a>
            <a href="javascript:void(0)" onclick="ShowTags('2')" class="submit_jia100" style="float: left;">添加批次</a>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>商品名称：</span><span>
                        <asp:TextBox ID="txtProductName" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>

                    <li><span>型号：</span><span>
                        <asp:TextBox ID="txtItemNo" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>备案批次：</span><span>
                        <asp:TextBox ID="txtBatchNo" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>供应商：</span><span>
                        <asp:TextBox ID="txtSupplierCode" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>条形码：</span><span>
                        <asp:TextBox ID="txtBarCode" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>品&nbsp;牌：</span><span>
                        <asp:TextBox ID="txtBrandName" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>料件号：</span><span>
                        <asp:TextBox ID="txtLJNoS" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>备案号：</span><span>
                        <asp:TextBox ID="txtProductRegistrationNumberS" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>

                    <li>
                        <span>添加时间：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>

                    <li><span>报关名称：</span><span>
                        <asp:TextBox ID="txtHSProductNameQ" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <%--<li><span>状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125">
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未归类</asp:ListItem>
                                <asp:ListItem Value="1">2.未备案</asp:ListItem>
                                <asp:ListItem Value="2">3.已备案</asp:ListItem>
                                <asp:ListItem Value="3">4.备案失败</asp:ListItem>
                                <asp:ListItem Value="4">5.未校验</asp:ListItem>
                                <asp:ListItem Value="5">6.已校验</asp:ListItem>
                                <asp:ListItem Value="6">7.校验失败</asp:ListItem>
                                <asp:ListItem Value="7">8.未商检</asp:ListItem>
                                <asp:ListItem Value="8">9.已商检</asp:ListItem>
                                <asp:ListItem Value="9">10.商检失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>--%>
                    <li><span>备案状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="RecordList" runat="server" Width="125">
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未归类</asp:ListItem>
                                <asp:ListItem Value="1">2.未备案</asp:ListItem>
                                <asp:ListItem Value="2">3.已备案</asp:ListItem>
                                <asp:ListItem Value="3">4.备案失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>校验状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="CheckList" runat="server" Width="125">
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未校验</asp:ListItem>
                                <asp:ListItem Value="1">2.已校验</asp:ListItem>
                                <asp:ListItem Value="2">3.校验失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>商检状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="InspectionList" runat="server" Width="125">
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未商检</asp:ListItem>
                                <asp:ListItem Value="1">2.已商检</asp:ListItem>
                                <asp:ListItem Value="2">3.商检失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>海关编码：</span><span>
                        <asp:TextBox ID="txtHSCODE" Width="120" runat="server" CssClass="forminput" /></span>
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
                    <li></li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <!--结束-->
            <asp:HiddenField ID="hidSortBy" runat="server" />
            <asp:HiddenField ID="hidSortOrder" runat="server" />
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
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
                            <span class="downproduct">
                                <a href="javascript:ExportData()">导出归类备案</a></span>
                            <asp:Button ID="btnExportData" runat="server" Text="导出" Style="display: none;" />
                        </li>
                    </ul>
                </div>
            </div>
            
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdProductClassify" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30" HeaderText="编号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("SkuId") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="RadioGroup" type="radio" value='<%#Eval("ProductId") %>' SkuId='<%#Eval("SkuId") %>' onclick="clickProduct($(this).attr('value'), $(this).attr('SkuId'))" />
                            <input type="hidden" name='hidSkuId' value='<%#Eval("SkuId") %>' />

                            <input type="hidden" id='hidLJNo_<%#Eval("SkuId") %>' value='<%#Eval("LJNo") %>' />

                            <input type="hidden" id='hidIsApprovedPrice_<%#Eval("ProductId") %>' value='<%#Eval("IsApprovedPrice") %>' />


                            <input type="hidden" id='hidProductName_<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("ProductName").ToString()) %>' />
                            <input type="hidden" id='hidHSProductName_<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("HSProductName").ToString()) %>' />
                            <input type="hidden" id='hidItemNo_<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("ItemNo").ToString()) %>' />
                            <input type="hidden" id='hidHSItemNo_<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("HSItemNo").ToString()) %>' />
                            <input type="hidden" id='hidUnit_<%#Eval("ProductId") %>' value='<%#Eval("Unit") %>' />
                            <input type="hidden" id='hidHSUnit_<%#Eval("ProductId") %>' value='<%#Eval("HSUnit") %>'/>
                            <input type="hidden" id='hidHSBrand_<%#Eval("ProductId") %>' value='<%#Eval("HSBrand") %>' />
                            <input type="hidden" id='hidBrandName_<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("BrandName").ToString()) %>' />
                            <input type="hidden" id='hidHS_CODE_<%#Eval("ProductId") %>' value='<%#Eval("HS_CODE") %>' />
                            <input type="hidden" id='hidTaxRate_<%#Eval("ProductId") %>' value='<%#Eval("TaxRate") %>' />
                            <input type="hidden" id='hidTaxId_<%#Eval("ProductId") %>' value='<%#Eval("TaxId") %>' />
                            <input type="hidden" id='hidPersonalPostalArticlesCode_<%#Eval("ProductId") %>' value='<%#Eval("PersonalPostalArticlesCode") %>' />
                            <input type="hidden" id='hidHS_CODE_ID<%#Eval("ProductId") %>' value='<%#Eval("HS_CODE_ID") %>' />
                            <input type="hidden" id='hid_Country_<%#Eval("ProductId") %>' value='<%# Eval("EnArea") %>' />
                            <input type="hidden" id='ImportSourceCode_<%#Eval("ProductId") %>' value='<%# Eval("ImportSourceCode") %>' />
                            <input type="hidden" id='hidTAX_RATE<%#Eval("ProductId") %>' value='<%#Eval("TAX_RATE") %>' />
                            <input type="hidden" id='hidTSL_RATE<%#Eval("ProductId") %>' value='<%#Eval("TSL_RATE") %>' />
                            <input type="hidden" id='hidLOW_RATE<%#Eval("ProductId") %>' value='<%#Eval("LOW_RATE") %>' />
                            <input type="hidden" id='hidOUT_RATE<%#Eval("ProductId") %>' value='<%#Eval("OUT_RATE") %>' />
                            <input type="hidden" id='hidHIGH_RATE<%#Eval("ProductId") %>' value='<%#Eval("HIGH_RATE") %>' />
                            <input type="hidden" id='hidTEMP_OUT_RATE<%#Eval("ProductId") %>' value='<%#Eval("TEMP_OUT_RATE") %>' />
                            <input type="hidden" id='hidTEMP_IN_RATE<%#Eval("ProductId") %>' value='<%#Eval("TEMP_IN_RATE") %>' />
                            <input type="hidden" id='hidCONTROL_MA<%#Eval("ProductId") %>' value='<%#Eval("CONTROL_MA") %>' />
                            <input type="hidden" id='hidPrice_<%#Eval("ProductId") %>' value='<%# Eval("CostPrice") %>' />
                            <input type="hidden" id='hidCONTROL_INSPECTION<%#Eval("ProductId") %>' value='<%#Eval("CONTROL_INSPECTION") %>' />
                            <input type="hidden" id='hidNOTE_S<%#Eval("ProductId") %>' value='<%#ReplaceStr(Eval("NOTE_S").ToString()) %>' />
                            <input type="hidden" id='hidProductRegistrationNumber<%#Eval("SkuId") %>' value='<%#Eval("ProductRegistrationNumber") %>' />
                            <input type="hidden" id='Manufacturer_<%#Eval("SkuId") %>' value='<%# Eval("Manufacturer") %>' />
                            <input type="hidden" id='hidBarCode_<%#Eval("ProductId") %>' value='<%# Eval("BarCode") %>' />
                            <input type="hidden" id='coustomSkuType_<%#Eval("ProductId") %>' value='<%# Eval("coustomSkuType") %>' />
                            <input type="hidden" id='countrySku_<%#Eval("ProductId") %>' value='<%# Eval("countrySku") %>' />
                            <input type="hidden" id='beLookType_<%#Eval("ProductId") %>' value='<%# Eval("beLookType") %>' />
                            <input type="hidden" id='madeOf_<%#Eval("ProductId") %>' value='<%# ReplaceStr(Eval("madeOf").ToString()) %>' />
                            <input type="hidden" id='hidHSUnitCode_<%#Eval("ProductId") %>' value='<%#Eval("HSUnitCode") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="批次" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField ItemStyle-Width="60px" HeaderText="状态" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <asp:Literal ID="litStatus" runat="server" Text='<%#Eval("PStatus")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="备案状态" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <asp:Literal ID="ReStatus" runat="server" Text='<%#Eval("ReStatus")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="校验状态" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <asp:Literal ID="CheckStatus" runat="server" Text='<%#Eval("CheckStatus")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="商检状态" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">
                                <asp:Literal ID="InspectionStaus" runat="server" Text='<%#Eval("InspectionStaus")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="150" HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
               

                               <div style="float: left;">
                                <div style="float: left; margin-right: 10px; z-index:100;">
                               <a href='<%#Eval("ThumbnailUrl220").ToString().Replace("~","") %>' class="tooltipqa" target="_blank">
                                    <%# Eval("ProductName") %></a>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="150" HeaderText="型号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("ItemNo") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="100" HeaderText="规格" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("ProductStandard") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="100" HeaderText="成分" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name spu_madeof" title="<%# ReplaceStr(Eval("Ingredient").ToString()) %>"></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="100" HeaderText="成本价" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("CostPrice") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="品牌" ItemStyle-Width="150" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("BrandName") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="150" HeaderText="产地" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("EnArea") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="供应商" ItemStyle-Width="150" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("SupplierCode") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="生产厂家" ItemStyle-Width="150" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("Manufacturer") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="条形码" ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("BarCode") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="60" HeaderText="计量单位" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("Unit") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60" HeaderText="备案批次" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("BatchNo") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="80" HeaderText="商品备案编号" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("ProductRegistrationNumber") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="审价状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="100">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsApprovedPrice" runat="server" Text='<%#Eval("IsApprovedPrice").ToString()=="0"?"未审核":(Eval("IsApprovedPrice").ToString()=="1"?"审核通过":"审核不通过")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="80">
                        <ItemTemplate>
                            <span>
                                <span class="Name"><%# Eval("remarks") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="100" HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%--<span class="Name"><%# Eval("AddedDate") %></span>--%>
                            <input type="button" value="立即备案" style="display: none" class="setHsMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="getHsMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
                            <input type="button" value="立即校验" style="" class="setCompanyMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="setCompanyMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
                            <input type="button" value="立即商检" style="" class="ProMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="ProMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
                            <input type="button" value="重新备案" style="display: none" class="ReSetHsMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="getHsMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
                            <input type="button" value="重新校验" style="display: none" class="ReSetCompanyMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="setCompanyMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
                            <input type="button" value="重新商检" style="display: none" class="ReProMsg" rel='<%#Eval("ProductId") %>' skuid="<%#Eval("SkuId") %>" onclick="ProMsg($(this).attr('rel'),$(this).attr('skuid'))"/>
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
    <!--归类信息区域-->
    <div id="tab1" title="" class="product-mes">
        <table width="100%" border="0">
            <tbody>
                <tr>
                    <td width="60%" style="vertical-align: top;">
                        <div class="product-mes" style="overflow: hidden; height: 100%;">
                            <table class="product-table" id="delt_table">
                                <tbody>
                                    <tr>
                                        <td class="product-title">海关编码：</td>
                                        <td class="product-content">
                                            <div class="field">
                                                <input type="text" name="hs_code" id="hs_code" placeholder="请输入海关编码" maxlength="10" />
                                                <a href="javascript:void(0)" onclick="clearAutoCompleterCache()">清除缓存</a>
                                                <asp:HiddenField runat="server" ID="hidHSCodeId" />
                                            </div>
                                        </td>
                                        <td class="product-title">行邮编码：</td>
                                        <td class="product-content">
                                            <div class="field">
                                                <input type="text" name="xy_code" id="xy_code" placeholder="请输入行邮编码" maxlength="10" />
                                                <asp:HiddenField runat="server" ID="hidXYHSCodeId" />
                                                行邮税率：<span id="product_tax_rate"></span>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="product-title" width="100px">商品名称：</td>
                                        <td class="product-content"><span id="product_name"></span></td>
                                        <td class="product-title" width="100px">报关名称：</td>
                                        <td class="product-content" width="40%">
                                            <asp:TextBox ID="txtHSProductName" Width="200" runat="server" CssClass="forminput" MaxLength="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="product-title">品牌：</td>
                                        <td class="product-content"><span id="product_brand"></span></td>
                                        <td class="product-title">报关品牌：</td>
                                        <td class="product-content">
                                            <asp:TextBox ID="txtHSProductBrand" Width="200" runat="server" CssClass="forminput" MaxLength="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="product-title">单位：</td>
                                        <td class="product-content"><span id="product_unit"></span></td>
                                        <td class="product-title">报关单位：</td>
                                        <td class="product-content">
                                            <Hi:UnitDropDownList runat="server" ID="ddlUnit" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="product-title">报关型号：<br />
                                            <input type="button" value="复制" onclick="clickCopy()" />
                                        </td>
                                        <td class="product-content">
                                            <asp:TextBox ID="txtProductItemNo" Width="200" runat="server" CssClass="forminput" TextMode="MultiLine" Height="80" MaxLength="125" />
                                        </td>

                                        <%--<td class="product-title">型号：</td>
                                        <td class="product-content"><span id="product_itemno" style="display:none;"></span></td>--%>
                                        <td class="product-title">料件号：</td>
                                        <td class="product-content">
                                            <asp:TextBox ID="txtLJNo" Width="200" runat="server" CssClass="forminput" MaxLength="100" />
                                            <span id="product_itemno" style="display: none;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="product-title">海关规格型号</td>
                                        <td class="product-conten">
                                            <asp:TextBox ID="coustomSkuType" Width="200" runat="server" CssClass="forminput" MaxLength="100" />
                                        </td>
                                        <td class="product-title">国检规格型号</td>
                                        <td class="product-conten">
                                            <asp:TextBox ID="countrySku" Width="200" runat="server" CssClass="forminput" MaxLength="100" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="product-title">监管类别</td>
                                        <td class="product-conten">
                                            <%--<asp:TextBox ID="beLookType" Width="200" runat="server" CssClass="forminput" MaxLength="100" />--%>
                                            <asp:DropDownList ID="beLookType" runat="server" Width="125">
                                                <asp:ListItem Value="-1">--请选择类别--</asp:ListItem>
                                                <asp:ListItem Value="0101">3C目录内</asp:ListItem>
                                                <asp:ListItem Value="0102">HS编码涉及3C但在3C目录外</asp:ListItem>
                                                <asp:ListItem Value="0201">普通食品、化妆品及一般要求</asp:ListItem>
                                                <asp:ListItem Value="0202">保健食品</asp:ListItem>
                                                <asp:ListItem Value="0203">有检疫要求（含肉、含蛋、含乳等）的深加工食品</asp:ListItem>
                                                <asp:ListItem Value="0204">备案管理的化妆品（婴幼儿化妆品除外）</asp:ListItem>
                                                <asp:ListItem Value="0205">实施注册管理的进口婴幼儿乳粉、实施备案管理的进口婴幼儿化妆品</asp:ListItem>
                                                <asp:ListItem Value="0206">一次性卫生用品</asp:ListItem>
                                                <asp:ListItem Value="0301">其他有特殊检验检疫监管要求的食品</asp:ListItem>
                                                <asp:ListItem Value="0401">医学微生物、人体组、生物制品、血液及其制品、环保微生物菌剂</asp:ListItem>
                                                <asp:ListItem Value="9999">其他产品</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="product-title">主要成分</td>
                                        <td class="product-conten">
                                            <asp:TextBox ID="madeOf" Width="200" runat="server" CssClass="forminput" MaxLength="100" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="4">——申报要素——</td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <%--海关编码--%>
                        <div class="product-mes" style="overflow: hidden;">
                            <table class="product-table" id="code_tb">
                                <tbody>
                                    <tr>
                                        <td class="product-title" width="10%">增值税率：</td>
                                        <td class="product-content" width="5%"><span id="taxRate"></span></td>
                                        <td class="product-title" width="10%">退税率：</td>
                                        <td class="product-content" width="5%"><span id="tslRate"></span></td>

                                    </tr>
                                    <tr>
                                        <td class="product-title">最惠国税率：</td>
                                        <td class="product-content"><span id="lowRate"></span></td>
                                        <td class="product-title">出口税率：</td>
                                        <td class="product-content"><span id="outRate"></span></td>
                                    </tr>
                                    <tr>
                                        <td class="product-title">普通国税率：</td>
                                        <td class="product-content"><span id="highRate"></span></td>
                                        <td class="product-title">临时出口税率：</td>
                                        <td class="product-content"><span id="tempOutRate"></span></td>

                                    </tr>
                                    <tr>
                                        <td class="product-title">临时进口税率：</td>
                                        <td class="product-content"><span id="tempInRate"></span></td>
                                        <td class="product-title">海关监管条件：</td>
                                        <td class="product-content"><span id="controlMa"></span></td>
                                    </tr>

                                    <tr>
                                        <td class="product-title" width="12%">商检监管条件：</td>
                                        <td class="product-content" colspan="3" width="85%"><span id="controlnspection" name="controlnspection"></span></td>
                                    </tr>
                                    <tr>
                                        <td class="product-title" width="12%">海关说明：</td>
                                        <td class="product-content" colspan="3" width="85%"><span id="noteS" style="color: red"></span></td>
                                    </tr>
                                    <tr>
                                        <td class="product-title" colspan="4" style="text-align: left;"><span style="color: blue;" id="spanHS_ELMENTS"></span></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <hr />
    <ul class="btntf Pw_110">
        <asp:Button ID="btnSave" runat="server" Text="保存归类" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding inbnt" />
    </ul>
    <asp:HiddenField runat="server" ID="hidProductId" />
    <asp:HiddenField runat="server" ID="hidSkuId" />
    <asp:HiddenField runat="server" ID="hidIsApprovedPrice" />

    <!--备案-->
    <div id="updatetag_div" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input130"><em>*</em>&nbsp;商品备案编号：</span><asp:TextBox ID="txtProductRegistrationNumber" Width="150" MaxLength="18" runat="server" CssClass="forminput" />
            </p>
        </div>
    </div>
    <!--备案批次-->
    <div id="addtag_div" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input130"><em>*</em>&nbsp;商品备案批次：</span><asp:TextBox ID="txtBatch" Width="150" MaxLength="20" runat="server" CssClass="forminput" />
            </p>
        </div>
    </div>
    <!--查看成分-->
    <div id="set_div" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input130" id="spanset"></span>
            </p>
        </div>
    </div>

    <div style="display: none">
        <asp:Button ID="btnRegistration" runat="server" Text="商品备案" />
        <asp:Button ID="btnAddBatch" runat="server" Text="添加批次" />
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="/Admin/js/jquery.min.js"></script>
    <script src="/Admin/js/jquery.autocompleter.js"></script>
    <script type="text/javascript">
        function clickProduct(ProductId,SkuId) {
            $("#spanHS_ELMENTS").html("");
            $("#<%=hidSkuId.ClientID%>").val(SkuId);

            $("#<%=hidIsApprovedPrice.ClientID%>").val($("#hidIsApprovedPrice_" + ProductId).val());
            
            if($("#hidLJNo_" + SkuId).val().length>0){
                $("#<%=txtLJNo.ClientID%>").val($("#hidLJNo_" + SkuId).val());
            }
            else{
                $("#<%=txtLJNo.ClientID%>").val("自动生成");
            }
            if(ProductId!=""&&ProductId!=$("#<%=hidProductId.ClientID%>").val()){
                $("#tab1").show();
                $("#<%=hidProductId.ClientID%>").val(ProductId);
                ClearHSCode();
                $("#product_name").html($("#hidProductName_" + ProductId).val());
                $("#<%=txtHSProductName.ClientID%>").val($("#hidHSProductName_" + ProductId).val());

                $("#product_brand").html($("#hidBrandName_" + ProductId).val());
                $("#<%=txtHSProductBrand.ClientID%>").val($("#hidHSBrand_" + ProductId).val());

                $("#product_itemno").html($("#hidItemNo_" + ProductId).val());
                $("#<%=txtProductItemNo.ClientID%>").val($("#hidHSItemNo_" + ProductId).val());

                $("#product_unit").html($("#hidUnit_" + ProductId).val());

                $("#<%=coustomSkuType.ClientID%>").val($("#coustomSkuType_" + ProductId).val());
                $("#<%=countrySku.ClientID%>").val($("#countrySku_" + ProductId).val());
                $("#<%=madeOf.ClientID%>").val($("#madeOf_" + ProductId).val());
                //根据select的显示值来为select设值
                $("#<%=ddlUnit.ClientID%>").get(0).selectedIndex = 0;
                var txtHSUnit = $("#hidHSUnit_" + ProductId).val();
                if(txtHSUnit.length>0){
                    $("#<%=ddlUnit.ClientID%> option").each(function(){
                        if($(this).text() == txtHSUnit)  
                        {
                            $(this).prop("selected", "selected");   
                            return;
                        } 
                    });
                }
                var beLookType=($("#beLookType_" + ProductId).val());
                if(beLookType.length>0){
                    $("#<%=beLookType.ClientID%> option").each(function(){
                        if($(this).text() == beLookType)  
                        {
                            $(this).prop("selected", "selected");   
                            return;
                        } 
                    });
                }
                //////海关编码信息//////
                $("#<%=hidHSCodeId.ClientID%>").val($("#hidHS_CODE_ID" + ProductId).val());
                $("#hs_code").val($("#hidHS_CODE_" + ProductId).val());
                //给海关编码赋值
                $("#taxRate").html($("#hidTAX_RATE" + ProductId).val());
                $("#tslRate").html($("#hidTSL_RATE" + ProductId).val());
                $("#lowRate").html($("#hidLOW_RATE" + ProductId).val());
                $("#outRate").html($("#hidOUT_RATE" + ProductId).val());
                $("#highRate").html($("#hidHIGH_RATE" + ProductId).val());
                $("#tempOutRate").html($("#hidTEMP_OUT_RATE" + ProductId).val());
                $("#tempInRate").html($("#hidTEMP_IN_RATE" + ProductId).val());
                $("#controlMa").html($("#hidCONTROL_MA" + ProductId).val());
                $("#controlnspection").html($("#hidCONTROL_INSPECTION" + ProductId).val());
                $("#noteS").html($("#hidNOTE_S" + ProductId).val());
                //加载海关编码对应申报要素 
                LoadHSE($("#hidHS_CODE_ID" + ProductId).val());
                //////海关编码信息//////
                $("#<%=hidXYHSCodeId.ClientID%>").val($("#hidTaxId_" + ProductId).val());
                $("#xy_code").val($("#hidPersonalPostalArticlesCode_" + ProductId).val());
                $("#product_tax_rate").html($("#hidTaxRate_" + ProductId).val());
            }
        }

        //清除海关编码和申报要素数据
        function ClearHSCode(){
            $("#taxRate").html("");
            $("#tslRate").html("");
            $("#lowRate").html("");
            $("#outRate").html("");
            $("#highRate").html("");
            $("#tempOutRate").html("");
            $("#tempInRate").html("");
            $("#controlMa").html("");
            $("#controlnspection").html("");
            $("#noteS").html("");
            //移除申报要素
            var OldTr=$(".ELMENTS");
            if(OldTr.length>0){
                $(".ELMENTS").remove();     
            }
        }

        //清除所有缓存：
        function clearAutoCompleterCache(){
            $('#xy_code').autocompleter('clearCache');
            $('#hs_code').autocompleter('clearCache');
            alert("清除成功");
        }

        //加载海关代码和行邮代码选项
        $(function () {
            $(".setHsMsg").each(function(){
                var me=$(this); 
                var tr=me.parents("tr");
                var rel=me.attr("rel");
                var sku = me.attr("skuId");
                
                // 校验状态
                var checkstatus = tr.find("td:eq(4)").find("span font").html().trim();
                if (checkstatus == '已校验') {
                    //me.show();
                    if ($("#hidProductRegistrationNumber" + sku).val() != '') {
                        me.hide();
                    }
                    me.siblings(".setCompanyMsg").hide();
                    me.siblings(".ReSetCompanyMsg").hide();
                }
                else if (checkstatus == '校验失败') {
                    //me.siblings().hide();
                    me.siblings(".setCompanyMsg").hide();
                    me.siblings(".ReSetCompanyMsg").show();
                }
                else {
                    me.siblings(".setCompanyMsg").show();
                    me.siblings(".ReSetCompanyMsg").hide();
                }

                // 商检状态
                var inspectionstatus = tr.find("td:eq(5)").find("span font").html().trim();
                if (inspectionstatus == '已商检') {
                    //me.show();
                    if ($("#hidProductRegistrationNumber" + sku).val() != '') {
                        me.hide();
                    }
                    me.siblings(".ProMsg").hide();
                    me.siblings(".ReProMsg").hide();
                }
                else if (inspectionstatus == '商检失败') {
                    me.siblings(".ProMsg").hide();
                    me.siblings(".ReProMsg").show();
                }
                else {
                    me.siblings(".ProMsg").show();
                    me.siblings(".ReProMsg").hide();
                }

                // 备案状态
                var restatus = tr.find("td:eq(3)").find("span font").html().trim();
                if (restatus == '未归类') {
                    me.hide();
                    me.siblings().hide();
                }
                else if (restatus == '已备案') {
                    me.hide();
                    me.siblings(".ReSetHsMsg").hide();
                }
                else if (restatus == '备案失败') {
                    //me.siblings().hide();
                    me.hide();
                    me.siblings(".ReSetHsMsg").show();
                }
                else {
                    me.show();
                    me.siblings(".ReSetHsMsg").hide();
                }


                //if(statu =='2.未备案'){
                //    me.show();
                //}
                //else if(statu =='3.已备案'){
                //    me.hide();
                //}
                //else if(statu =='6.已校验'){
                //    me.show();
                //    if( $("#hidProductRegistrationNumber"+sku).val()!=''){
                //        me.hide();
                //    }
                //    me.siblings(".setCompanyMsg").hide();
                //}
                //else if(statu =='9.已商检'){
                //    me.show();
                //    if( $("#hidProductRegistrationNumber"+sku).val()!=''){
                //        me.hide();
                //    }
                //    me.siblings(".ProMsg").hide();
                //}
                //else if(statu =='4.备案失败'){
                //    me.siblings().hide();
                //    me.siblings(".ReSetHsMsg").show();               
                //}
                //else if(statu =='7.校验失败'){
                //    me.siblings().hide();
                //    me.siblings(".ReSetCompanyMsg").show();
                //}
                //else if(statu =='10.商检失败'){
                //    me.siblings().hide();
                //    me.siblings(".ReProMsg").show();
                //}
            });
            $("#tab1").hide()
            $("#<%=grdProductClassify.ClientID%>").on("click", "tr", function (e) {
                $("input:radio[name='RadioGroup']:checked").prop("checked","");  
                $(this).eq(0).find("input[type='radio']").prop("checked","checked"); 
                clickProduct($(this).eq(0).find("input[type='radio']").val(),$(this).eq(0).find("input[name='hidSkuId']").val());
            });
            
            $('#xy_code').autocompleter({
                // marker for autocomplete matches
                highlightMatches: true,
                // object to local or url to remote search
                source: "/Handler/HelpHandler.ashx?action=GetXYCode",
                // custom template
                template: '{{ label }} <span>({{ TaxRate }})</span>',
                // abort source if empty field
                empty: false,
                // max results
                limit: 5,
                callback: function (value, index, selected) {
                    if (selected) {
                        $("#product_tax_rate").html(selected.TaxRate);
                        $("#<%=hidXYHSCodeId.ClientID%>").val(selected.TaxId);
                    }
                }
            });
            //成分隐藏点击后显示
            $(".spu_madeof").each(function(){
                var title=$(this).attr("title");
                $(this).click(function(){
                    ShowIngredient("查看成分", title);
                });
                if(title.length>20){
                    $(this).html(title.substring(0,20)+'......')
                }else{$(this).html(title)}
            });

            function ShowIngredient(titile,content ) {
                dialog = art.dialog({
                    id: "set",
                    title: titile,
                    content: content,
                    resize:false,
                    fixed: true,
                    height:180, 
                    width:400, 
                    modal:true, //蒙层（弹出会影响页面大小） 
                    button: [
                            { name: '关  闭' }
                    ]
                });
            }


            $('#hs_code').autocompleter({
                // marker for autocomplete matches
                highlightMatches: true,
                // object to local or url to remote search
                source: "/Handler/HelpHandler.ashx?action=GetHSCode",
                // custom template
                template: '{{ label }} <span>({{ HS_NAME }})</span>',
                // abort source if empty field
                empty: false,
                //cache:false,
                // max results
                limit: 5,
                callback: function (value, index, selected) {
                    if (selected) {
                        $("#<%=hidHSCodeId.ClientID%>").val(selected.HS_CODE_ID);
                        //给海关编码赋值
                        $("#taxRate").html(selected.TAX_RATE);
                        $("#tslRate").html(selected.TSL_RATE);
                        $("#lowRate").html(selected.LOW_RATE);
                        $("#outRate").html(selected.OUT_RATE);
                        $("#highRate").html(selected.HIGH_RATE);
                        $("#tempOutRate").html(selected.TEMP_OUT_RATE);
                        $("#tempInRate").html(selected.TEMP_IN_RATE);
                        $("#controlMa").html(selected.CONTROL_MA);
                        $("#controlnspection").html(selected.CONTROL_INSPECTION);
                        $("#noteS").html(selected.NOTE_S);
                        //加载海关编码对应申报要素
                        LoadHSE(selected.HS_CODE_ID);
                    }
                }
            });
        });

        //加载海关编码对应申报要素
        function LoadHSE(HS_CODE_ID){
            if(HS_CODE_ID)
            {
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=GetHS_CODE_ELMENTS",
                    type: "post",
                    datatype: "text/json",
                    cache: false,
                    data: { "HSCode": HS_CODE_ID,"ProductId":$("#<%=hidProductId.ClientID%>").val() },
                    success: function (data) {
                        if (data!= null&&data.success!="NO") {
                            var tb = $("#delt_table tbody");
                            //移除之前添加
                            var OldTr=$(".ELMENTS");
                            if(OldTr.length>0){
                                $(".ELMENTS").remove();     
                            }
                            var hse="";
                            $.each(data,function(index,item){
                                var tr=$("<tr class='ELMENTS'></tr>");
                                var td1=$("<td class='product-title'>"+item.HS_ELMENTS_NAME+"：<input type='hidden' name='hidElmentsId' value='"+item.HS_ELMENTS_ID+"' /></td>");
                                var td2=$("<td class='product-content' colspan='3'><textarea title='tELMENTS' onchange='ChangeELMENTS()' rows='2' cols='20' name='elments_"+item.HS_ELMENTS_ID+"' style='height:60px;width:400px;margin:5px;' validategroup='default' class='forminput'>"+item.ELMENTS_VALUE+"</textarea></td>");
                                tr.append(td1);
                                tr.append(td2);
                                tb.append(tr);
                                hse=hse+item.ELMENTS_VALUE+"|";
                            });
                            var hses=hse.substr(0,hse.length-1);
                            $("#spanHS_ELMENTS").html(hses);
                        }
                        else{
                            alert("加载申报要素失败！" + data.MSG);
                        }
                    },
                    error: function (a, b, c) {
                        alert("加载申报要素失败！" + b.error);
                    }
                });
            }
        }

        //申报要素变更
        function ChangeELMENTS(){
            var hse="";
            $.each($("textarea[title='tELMENTS']"),function(index,item){
                hse=hse+$(this).val()+"|";
            });
            var hses=hse.substr(0,hse.length-1);
            $("#spanHS_ELMENTS").html(hses);
        }

        //检查输入
        function PageIsValid(){

            
            if($("#<%=hidProductId.ClientID%>").val().length<=0){
                alert("请选择商品");
                return false;
            }


            if($("#<%=hidIsApprovedPrice.ClientID%>").val()!="1")
            {
                alert("商品还未审价");
                return false;
            }

            if($("#<%=hidHSCodeId.ClientID%>").val().length<=0){
                alert("请输入海关编码");
                return false;
            }
            if($("#<%=hidXYHSCodeId.ClientID%>").val().length<=0){
                alert("请输入行邮编码");
                return false;
            }

            //
            if($("#<%=txtHSProductName.ClientID%>").val().length<=0){
                alert("请输入报关名称");
                return false;
            }

            if($("#<%=txtHSProductBrand.ClientID%>").val().length<=0){
                alert("请输入报关品牌");
                return false;
            }
            
            if($("#<%=txtLJNo.ClientID%>").val().length<=0){
                alert("请输入料件号");
                return false;
            }

            if($("#<%=txtProductItemNo.ClientID%>").val().length<=0){
                alert("请输入报关型号");
                return false;
            }

            if($("#<%=beLookType.ClientID%> :checked").val()=='-1'){
                alert("请输入监管类别");
                return false;
            }

            return true;
        }
  
        function ShowTags(oper) {
           
            if (oper == "1") {

                if($("#<%=hidSkuId.ClientID%>").val().length<=0){
                    alert("请选择商品");
                    return false;
                }


                if($("#<%=hidIsApprovedPrice.ClientID%>").val()!="1")
                {
                    alert("商品还未审价");
                    return false;
                }

                if($("#hidHS_CODE_ID" + $("#<%=hidProductId.ClientID%>").val()).val().length<=0){
                    alert("商品还未归类，请先归类");
                    return false;
                }
                flag=0;
                setArryText("<%=txtProductRegistrationNumber.ClientID%>", $("#hidProductRegistrationNumber" + $("#<%=hidSkuId.ClientID%>").val()).val());
                DialogShow("商品备案", "editetag", "updatetag_div", "<%=btnRegistration.ClientID%>");
                $("#<%=txtProductRegistrationNumber.ClientID%>").focus();
            }
            else{
                if($("input:checkbox[name='CheckBoxGroup']:checked").length<=0){
                    alert("请选择商品");
                    return false;
                }
                flag=1;
                //一定要赋值就算未空，否则按钮不会提交后台
                setArryText("<%=txtBatch.ClientID%>", "");
                DialogShow("添加备案批次", "addtag", "addtag_div", "<%=btnAddBatch.ClientID%>");
                $("#<%=txtBatch.ClientID%>").focus();
            }
        }

        var flag=0;
        function validatorForm() {
            if(flag==0){
                if($.trim($("#<%=txtProductRegistrationNumber.ClientID%>").val()).length<=0){
                    alert("请输入商品备案编号");
                    $("#<%=txtProductRegistrationNumber.ClientID%>").focus();
                    return false;
                }

                if($.trim($("#<%=txtProductRegistrationNumber.ClientID%>").val()).length!=18){
                    alert("商品备案编号长度不正确,请输入18位字符");
                    $("#<%=txtProductRegistrationNumber.ClientID%>").focus();
                    return false;
                }
            }
            else{
                if($.trim($("#<%=txtBatch.ClientID%>").val()).length<=0){
                    alert("请输入备案批次");
                    $("#<%=txtBatch.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function ExportData() {
            $("#<%=btnExportData.ClientID%>").trigger("click");
        }
        function clickCopy(){
            if($("#<%=txtProductItemNo.ClientID%>").val().length>0&&!confirm("确认复制！"))
            {
                return;
            }
            $("#<%=txtProductItemNo.ClientID%>").val($("#spanHS_ELMENTS").html());
        }
        //海关备案
        function getHsMsg(proId,skuId){
            var setHsMsg={
                "proType":"其他商品",//商品类型
                "HS_CODE":$("#hidHS_CODE_"+proId).val(),//商品编码
                "HSProductName":$("#hidHSProductName_"+proId).val(),//商品名称
                "HSItemNo":$("#hidHSItemNo_"+proId).val(),//规格型号(报关型号)
                "PersonalPostalArticlesCode":$("#hidPersonalPostalArticlesCode_"+proId).val(),//物品税号（行邮编码）
                "Country":$("#hid_Country_"+proId).val(),//原产国
                "CountryCode":$("#ImportSourceCode_"+proId).val(),//原产国编码
                "payCode":"142",//币制（人民币）
                "payCodeName":"人民币",//币制
                "Price":$("#hidPrice_"+proId).val()//单价（成本价）
            }
            var str = "";
            if (!external.imgoodsrecord) {
                alert("请使用云报关系统进行商品备案");
            }
            str = external.imgoodsrecord(JSON.stringify(setHsMsg));
            var result=JSON.parse(str);
            if(result.d=='1'){
                var formId =result.formId;//返回的备案编号
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=setProductNum",
                    type: "post",
                    datatype: "text/json",
                    cache: false,
                    data: { SkuId:skuId,ProductNum:formId,Status:2},
                    success:function(){
                        window.location.reload();
                    }
                })
            }else{                
                $.ajax({
                url: "/Handler/HelpHandler.ashx?action=setProductNum",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { SkuId:skuId,ProductNum:formId,Status:3},
                success:function(){
                    window.location.reload();
                }
            })}
        }
        //入库数据校验
        function setCompanyMsg(proId,skuId){
            var companyMsg={
                "applyCustoms":"深圳前海湾保税港区",//申报海关
                "applyCustomsCode":"5349",//申报海关编码
                "applyBusinessCode":"4403660034",//申报单位编码
                "applyBusinessName":"深圳市信捷网科技有限公司",//申报单位名称
                "applyort":"深圳前海湾保税港区口岸",//进出口岸
                "applyortCode":"5349",//
                "transportType":"公路运输",//运输方式
                "transportTypeCode":"4",//运输方式编码
                "companyCode":"4403660034",//经营单位编码
                "companyName":"深圳市信捷网科技有限公司",//经营单位名称
                "getGoodsBusinName":"深圳市信捷网科技有限公司",//收/发货单位名称
                "businessType":"进境入区",//业务类型
                "allQlt":'1',//总件数
                "wrapType":"纸箱",//包装种类
                "wrapTypeCode":"2",//包装种类编码
                "allQty":'2',//毛重进出口岸编码
                "superviseType":"保税电商",//监管方式
                "superviseTypeCode":"1210",//监管方式编码
                "tradeType":"FOB",//成交方式
                "tradeTypeCode":"3",//成交方式代码
                "Qty":'1',//净重
                "sendCountry":"110",//运抵/启运国
                "sendCountryName":"香港",//运抵/启运国
                "RecordCompanyName":"信捷网",//备案企业名称
                "setInfo":[{//入库数据表体
                    "mergeType":"手工",//归并方式
                    "proLJNO":$("#hidLJNo_"+skuId).val(),//商品料件号
                    "HS_CODE":$("#hidHS_CODE_"+proId).val(),//商品编码
                    "ProductRegistrationNumber":$("#hidProductRegistrationNumber"+skuId).val(),//商品备案编号
                    "HSProductName":$("#hidHSProductName_"+proId).val(),//商品中文名称
                    "applyNum":"1",//申报数量
                    "AllPrice":"1",//总价
                    "FriNum":"1",//法定数量
                    "Country":$("#hid_Country_"+proId).val(),//原产国
                    "CountryCode":$("#ImportSourceCode_"+proId).val(),//原产国编码
                    "getTax":"全免",//征免方式
                    "getTaxCode":"3",//征免方式
                    "useType":"其他",//用途
                    "useTypeCode":"11",//用途编码
                    "mergeRule":"10位规则",//归并原则
                    "Price":$("#hidPrice_"+proId).val()//单价（成本价）,备案价
                }]
            } 
            var str = "";
            if (!external.goodsim) {
                alert("请使用云报关系统");
            }
            str = external.goodsim(JSON.stringify(companyMsg),1);//external.goodsim(jsonstr,checkno) checkno 1=验证，0=入库
            var result=JSON.parse(str);
            alert(str);
            if(result.d=='1'){
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=CheckProductNum",
                    type: "post",
                    datatype: "text/json",
                    cache: false,
                    data: { ProductID:proId,Remark:"已完成入库数据校验",Status:"1"},
                    success:function(){
                        window.location.reload();
                    }
                })
            }else{ $.ajax({
                url: "/Handler/HelpHandler.ashx?action=CheckProductNum",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { ProductID:proId,Remark:result.str,Status:"2"},
                success:function(){
                    window.location.reload();
                }
            })}
        }
        //商品信息
        function ProMsg(proId,skuid){
            var ProMsg={
                "proHuoNum":$("#hidBarCode_"+proId).val(),//商品货号
                "PersonalPostalArticlesCode":$("#hidPersonalPostalArticlesCode_"+proId).val(),//物品税号（行邮编码）
                "HS_CODE":$("#hidHS_CODE_"+proId).val(),//商品编码
                "HSProductName":$("#hidHSProductName_"+proId).val(),//商品中文名称
                "coustomSkuType":$("#coustomSkuType_"+proId).val(),//海关规格型号
                "countrySku":$("#countrySku_"+proId).val(),//国检规格型号
                "proBrand":$("#hidHSBrand_"+proId).val(),//品牌
                "Country":$("#hid_Country_"+proId).val(),//原产国
                "CountryCode":$("#ImportSourceCode_"+proId).val(),//原产国编码
                "madeOf":$("#madeOf_"+proId).val(),//主要成分
                "unit":$("#hidHSUnit_"+proId).val(),//计量单位
                //"unitCode":$("#hidHSUnit_"+proId).attr('unitcode'),//计量单位编码
                "unitCode":$("#hidHSUnitCode_"+proId).val(),//计量单位编码
                "payCode":"142",//币制（人民币）
                "payName":"人民币",//币制（人民币）
                "beCould":"是",//是否法检
                "Manufacturer":$("#Manufacturer_"+skuid).val(),//生产厂家
                "madeCountry":$("#hid_Country_"+proId).val(),//生产企业国别
                "madeCountryCode":$("#ImportSourceCode_"+proId).val(),//原产国编码
                "useRule":"国际标准",//适用标准
                "beKonwty":"不需要",//认证情况
                "beLookType":$("#beLookType_"+proId).val(),//监管类别
                "companyRisk":"同意",//企业风险明示标志
                "notGift":"否",//是否赠品
                "Price":$("#hidPrice_"+proId).val(),//单价（成本价）
                "picUpload":"D:@images@product@"+proId//图片文件夹的路径D:\images\product\

            }
            var str = "";
            if (!external.goodsim) {
                alert("请使用云报关系统");
            }
            str = external.sjgoodsrecord(JSON.stringify(ProMsg));
            var result=JSON.parse(str);
            if(result.d=='1'){
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=joinProductNum",
                    type: "post",
                    datatype: "text/json",
                    cache: false,
                    data: { ProductID:proId,Remark:"已完成商品检验",Status:"1"},
                    success:function(){
                        window.location.reload();
                    }
                })
            }else{
                $.ajax({
                    url: "/Handler/HelpHandler.ashx?action=joinProductNum",
                    type: "post",
                    datatype: "text/json",
                    cache: false,
                    data: { ProductID:proId,Remark:result.str,Status:"2"},
                    success:function(){
                        window.location.reload();
                    }
                })
            }
        }
    </script>
        <script type="text/javascript">

            $(function () {
                $(".tooltipqa").hover(function () {
                    var src = $(this).attr("href");
                    var imgwrap = $("<div class='imgwrap' style='position:fixed;width:400px;height:400px;left:50%;top:50%;margin-left:-200px;margin-top:-200px;'><img src='" + src + "' width='400px' height='400px'></div>");
                    imgwrap.appendTo('body');
                }, function () {
                    $(".imgwrap").remove();
                })
            })
</script>
</asp:Content>
