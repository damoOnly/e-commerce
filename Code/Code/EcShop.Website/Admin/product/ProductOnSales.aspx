<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductOnSales.aspx.cs" Inherits="EcShop.UI.Web.Admin.ProductOnSales" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" style="" /></em>
            <h1>��Ʒ����</h1>
            <span>�̳������е���Ʒ�������Զ���Ʒ����������Ҳ�ܶ���Ʒ���б༭���ϼܡ��¼ܡ�������</span>
        </div>
        <div class="datalist">
            <!--����-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--��ѡ����Ʒ����--"
                                Width="150" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--��ѡ��Ʒ��--"
                                Width="153">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--��ѡ���ǩ--"
                                Width="153">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--��ѡ������--" Width="153" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server" ID="ddlImportSourceType" NullToDisplay="--��ѡ��ԭ����--" Width="160" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" Width="153" />
                        </abbr>
                    </li>

                    <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsApproved" runat="server">
                                <asp:ListItem Value="-1">--��ѡ�����״̬--</asp:ListItem>
                                <asp:ListItem Value="0">δ���</asp:ListItem>
                                <asp:ListItem Value="1">�����</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>

                     <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsApprovedPrice" runat="server">
                                <asp:ListItem Value="-1">--��ѡ�����״̬--</asp:ListItem>
                                <asp:ListItem Value="0">δ���</asp:ListItem>
                                <asp:ListItem Value="1">���ͨ��</asp:ListItem>
                                <asp:ListItem Value="2">��˲�ͨ��</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>


                    <li>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropIsAllClassify" runat="server">
                                <asp:ListItem Value="-1">--��ѡ�񱸰�״̬--</asp:ListItem>
                                <asp:ListItem Value="0">δ���</asp:ListItem>
                                <asp:ListItem Value="1">�����</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>


                    <li>
                        <abbr class="formselect">
                            <Hi:ShippingTemplatesDropDownList runat="server" ID="ddlShipping" NullToDisplay="--��ѡ���˷�ģ��--" />
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>��Ʒ���룺</span><span>
                        <asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
                        <li><span>�����룺</span><span>
                        <asp:TextBox ID="txt_BarCode" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li><span>���ʱ�䣺</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">��</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li></li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton" /></li>

                </ul>
            </div>
            <!--����-->
            <div class="functionHandleArea clearfix">
                <!--��ҳ����-->
                <div class="pageHandleArea">
                    <asp:HiddenField ID="hidSortBy" runat="server" />
                    <asp:HiddenField ID="hidSortOrder" runat="server" />
                    <ul>
                        <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
                <!--����-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
                            <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">��ѡ</a></span>
                            <span class="reverseSelect">
                                <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="ɾ��" IsShow="true" DeleteMsg="ȷ��Ҫ����Ʒ�������վ��" />
                            </span>
                            <span class="reverseSelect">
                                <Hi:ImageLinkButton ID="btnApprove" runat="server" Text="���" IsShow="true" DeleteMsg="ȷ��Ҫ�����Ʒ��" />
                            </span>
                            <span class="reverseSelect">
                                <Hi:ImageLinkButton ID="btnUnApprove" runat="server" Text="����" IsShow="true" DeleteMsg="ȷ��Ҫ������Щ��Ʒ��" />
                            </span>

                             <span class="reverseSelect">
                                  <Hi:ImageLinkButton ID="btnReSubmitPriceApprove" runat="server" Text="���" IsShow="true" DeleteMsg="ȷ������Щ��Ʒ�����ύ��ۣ�" />
                            </span>


                            <span class="printProduct"><a href="javascript:void(0)" onclick="PrintProduct()" class="submit_jia_1">��ӡ��ά��</a></span>
                            <span class="btn generationQCode">
                                <Hi:ImageLinkButton ID="btnGenerationQCode" runat="server" Text="���ɶ�ά��" class="submit_jia_1" /></span>
                            <select id="dropBatchOperation">
                                <option value="">��������..</option>
                                <option value="1">��Ʒ�ϼ�</option>
                                <option value="2">��Ʒ�¼�</option>
                                <option value="3">��Ʒ���</option>
                                <option value="5">���ð���</option>
                                <option value="6">ȡ������</option>
                                <option value="10">����������Ϣ</option>
                                <option value="11">������ʾ��������</option>
                                <option value="12">�������</option>
                                <option value="13">������Ա���ۼ�</option>
                                <option value="15">������Ʒ������ǩ</option>
                                <option value="16">������Ʒ�ƹ�Ӷ��</option>
                                <option value="17">��ƷȨ�ص���</option>
                                <option value="18">������Ʒ˰��</option>
                                <option value="19">������ƷƷ��</option>
                                <option value="20">������Ʒԭ����</option>
                            </select>
                            <asp:LinkButton ID="btnExcelSupplierProDetatil" runat="server" Text="������Ӧ����Ʒ��ϸ" />
                        </li>
                    </ul>
                </div>
                <div class="filterClass">
                    <span><b>����״̬��</b></span> <span class="formselect">
                        <Hi:SaleStatusDropDownList AutoPostBack="true" ID="dropSaleStatus" runat="server" />
                    </span><span class="formselect"></span>
                </div>
            </div>
            <!--�����б�����-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="����" DataField="DisplaySequence" SortExpression="DisplaySequence" ItemStyle-Width="35px"
                        HeaderStyle-CssClass="td_right td_left" />
                    <asp:BoundField DataField="SkuId" HeaderText="��Ʒ����" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%" />
                    <asp:TemplateField ItemStyle-Width="40%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
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
                                    <%# Eval("ProductName") %></a>--%></span> <span class="colorC">��Ʒ���룺<%# Eval("ProductCode") %>�ɱ���<%# Eval("CostPrice", "{0:f2}")%></span></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SupplierName" HeaderText="������" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%" />
                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="���" SortExpression="Stock" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("Stock") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="��Ʒ�۸�" SortExpression="SalePrice" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">һ�ڼۣ�<%# Eval("SalePrice", "{0:f2}")%></span><span class="colorC">�г��ۣ�<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="��Ʒ״̬" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="���״̬" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litIsApproved" runat="server" Text='<%#Eval("IsApproved").ToString()=="True"?"���":"δ���"%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="���״̬" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsApprovedPrice" runat="server" Text='<%#Eval("IsApprovedPrice").ToString()=="0"?"δ���":(Eval("IsApprovedPrice").ToString()=="1"?"���ͨ��":"��˲�ͨ��")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="����״̬" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsAllClassify" runat="server" Text='<%#Eval("IsAllClassify").ToString()=="1"?"�����":"δ���"%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="��ά��" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <a href='<%#Eval("QRcode").ToString().Replace("~","") %>' class="tooltip" target="_blank">
                                <img src='<%#Eval("QRcode").ToString().Replace("~","") %>' alt="��ά��" width="50px" height="50px" /></a>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ԭ����" ItemStyle-Width="120" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span style="float: left;">
                                <img src='<%#Eval("Icon") %>' width="50px" height="50px" />
                            </span>
                            <span style="line-height: 50px;"><%#Eval("CnArea")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="����" HeaderStyle-CssClass=" td_left td_right_fff min_width">
                        <ItemTemplate>
                            <span class="submit_bianji"><a href="<%# "../../admin/default.html?product/EditProduct.aspx?productId="+Eval("ProductId")%>" target="_blank">�༭</a></span> <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:CollectionProduct('<%# "EditReleteProducts.aspx?productId="+Eval("ProductId")%>')">�����Ʒ</a></span> <span class="submit_shanchu">
                                <Hi:ImageLinkButton ID="btnDel" CommandName="Delete" runat="server" Text="ɾ��" IsShow="true"
                                    DeleteMsg="ȷ��Ҫ����Ʒ�������վ��?" /></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <style>
            .min_width {
                min-width: 185px;
            }
        </style>
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
    <%-- �ϼ���Ʒ--%>
    <div id="divOnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ȷ��Ҫ�ϼ���Ʒ���ϼܺ���Ʒ��ǰ̨����</em>
            </p>
        </div>
    </div>
    <%-- �¼���Ʒ--%>
    <div id="divUnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ȷ��Ҫ�¼���Ʒ��</em>
            </p>
        </div>
    </div>
    <%-- �����Ʒ--%>
    <div id="divInStockProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ȷ��Ҫ����Ʒ��⣿</em>
            </p>
        </div>
    </div>
    <%-- ���ð���--%>
    <div id="divSetFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ȷ��Ҫ������Щ��Ʒ���ʣ�</em>
            </p>
        </div>
    </div>
    <%-- ȡ������--%>
    <div id="divCancelFreeShip" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ȷ��Ҫȡ����Щ��Ʒ�İ��ʣ�</em>
            </p>
        </div>
    </div>
    <%-- ��Ʒ��ǩ--%>
    <div id="divTagsProduct" style="display: none;">
        <div class="frame-content">
            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
        </div>
    </div>
    <%-- ��Ʒ�ƹ�Ӷ��--%>
    <div id="divDeduct" style="display: none;">
        <div class="frame-content" style="margin-top: -20px;">
            <table cellpadding="0" cellspacing="0" width="550px" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="20%">ֱ���ƹ�Ӷ��</td>
                    <td align="left" class="bd_td">
                        <asp:TextBox ID="txtReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <asp:Literal ID="litReferralDeduct" runat="server" /><p>�ƹ�Ա�������Ӳ�����Ч����ʱ���ܵ�Ӷ�����</p>
                    </td>
                </tr>
                <tr>
                    <td align="right">�¼���ԱӶ��</td>
                    <td align="left" class="bd_td">
                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="forminput" runat="server" />&nbsp;%<asp:Literal ID="litSubMemberDeduct" runat="server" /><p>�¼���Աδͨ���������ӽ����̳�ʱ�����Ķ������ƹ�Ա�����ܵ�Ӷ�����</p>
                    </td>
                </tr>
                <tr>
                    <td align="right">�¼��ƹ�ԱӶ��</td>
                    <td align="left" class="bd_td">
                        <asp:TextBox ID="txtSubReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%<asp:Literal ID="litSubReferralDeduct" runat="server" /><p>�ƹ�Ա�������Ӳ�����Ч����ʱ�����ϼ��ƹ�ԱҲ�ɻ����Ӧ��Ӷ�����</p>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnUpdateProductTags" runat="server" Text="������Ʒ��ǩ" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUpdateProductDeducts" runat="server" Text="������Ʒ�ƹ�Ӷ��" CssClass="submit_DAqueding" />
        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
        <asp:Button ID="btnInStock" runat="server" Text="�����Ʒ" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUnSale" runat="server" Text="�¼���Ʒ" CssClass="submit_DAqueding" />
        <asp:Button ID="btnUpSale" runat="server" Text="�ϼ���Ʒ" CssClass="submit_DAqueding" />
        <asp:Button ID="btnSetFreeShip" runat="server" Text="���ð���" CssClass="submit_DAqueding" />
        <asp:Button ID="btnCancelFreeShip" runat="server" Text="ȡ������" CssClass="submit_DAqueding" />
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
                        DialogShow("��Ʒ�ϼ�", "productonsale", "divOnSaleProduct", "ctl00_contentHolder_btnUpSale");
                        break;
                    case "2":
                        formtype = "unsale";
                        arrytext = null;
                        DialogShow("��Ʒ�¼�", "productunsale", "divUnSaleProduct", "ctl00_contentHolder_btnUnSale");
                        break;
                    case "3":
                        formtype = "instock";
                        arrytext = null;
                        DialogShow("��Ʒ���", "productinstock", "divInStockProduct", "ctl00_contentHolder_btnInStock");
                        break;
                    case "5":
                        formtype = "setFreeShip";
                        arrytext = null;
                        DialogShow("���ð���", "setFreeShip", "divSetFreeShip", "ctl00_contentHolder_btnSetFreeShip");
                        break;
                    case "6":
                        formtype = "cancelFreeShip";
                        arrytext = null;
                        DialogShow("ȡ������", "cancelFreeShip", "divCancelFreeShip", "ctl00_contentHolder_btnCancelFreeShip");
                        break;
                    case "10":
                        DialogFrame("product/EditBaseInfo.aspx?ProductIds=" + productIds, "������Ʒ������Ϣ", null, null);
                        break;
                    case "11":
                        DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "����ǰ̨��ʾ����������", null, null);
                        break;
                    case "12":
                        DialogFrame("product/EditStocks.aspx?ProductIds=" + productIds, "�������", 880, null);
                        break;
                    case "13":
                        DialogFrame("product/EditMemberPrices.aspx?ProductIds=" + productIds, "������Ա���ۼ�", 1000, null);
                        break;
                    case "15":
                        formtype = "tag";
                        setArryText('ctl00_contentHolder_txtProductTag', "");
                        DialogShow("������Ʒ��ǩ", "producttag", "divTagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
                        break;
                    case "16":
                        formtype = "deduct";
                        DialogShow("������Ʒ�ƹ�Ӷ��", "productdeduct", "divDeduct", "ctl00_contentHolder_btnUpdateProductDeducts");
                        break;
                    case "17":
                        DialogFrame("product/ProductFractionChange.aspx?ProductIds=" + productIds, "��ƷȨ�ص�����", 880, null);
                        break;
                    case "18":
                        DialogFrame("product/AdjustTaxRate.aspx?ProductIds=" + productIds, "������Ʒ˰��", null, null);
                        break;
                    case "19":
                        DialogFrame("product/AdjustBrand.aspx?ProductIds=" + productIds, "������ƷƷ��", null, null);
                        break;
                    case "20":
                        DialogFrame("product/AdjustImportSources.aspx?ProductIds=" + productIds, "������Ʒԭ����", null, null);
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
                alert("��ѡ����Ʒ");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "�����Ʒ");
        }

        function validatorForm() {
            switch (formtype) {
                case "tag":
                    if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                        alert("��ѡ����Ʒ��ǩ");
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
                    if ($("#ctl00_contentHolder_txtReferralDeduct").val() == "") {
                        alert("������ֱ���ƹ�Ӷ��");
                        return false;
                    }
                    if ($("#ctl00_contentHolder_txtSubMemberDeduct").val() == "") {
                        alert("�������¼���ԱӶ��");
                        return false;
                    }
                    if ($("#ctl00_contentHolder_txtSubReferralDeduct").val() == "") {
                        alert("�������¼��ƹ�ԱӶ��");
                        return false;
                    }
                    setArryText('ctl00_contentHolder_txtReferralDeduct', $("#ctl00_contentHolder_txtReferralDeduct").val());
                    setArryText('ctl00_contentHolder_txtSubMemberDeduct', $("#ctl00_contentHolder_txtSubMemberDeduct").val());
                    setArryText('ctl00_contentHolder_txtSubReferralDeduct', $("#ctl00_contentHolder_txtSubReferralDeduct").val());
                    break;
            };
            return true;
        }
        function PrintProduct() {
            var v_str = "";
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });
            if (v_str.length == 0) {
                alert("��ѡ����Ʒ");
                return "";
            }
            DialogFrame("./product/PrintProduct.aspx?ProductsId=" + v_str, "��Ʒ��ӡ", 975, null);
        }
    </script>
</asp:Content>
