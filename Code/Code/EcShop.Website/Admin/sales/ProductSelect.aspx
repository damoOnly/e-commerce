<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductSelect.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.ProductSelect" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32"  style=" " /></em>
            <h1>
                ��Ʒ���ѡ��</h1>
            <span>
			</span>
        </div>
        <div class="datalist">
            <!--����-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li>
                        <input type="hidden" id="chekSkuid" />
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
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--��ѡ������--" Width="153" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server"  ID="ddlImportSourceType"  NullToDisplay="--��ѡ��ԭ����--" Width="160"/>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                             <Hi:SupplierDropDownList runat="server"  ID="ddlSupplier" Width="153"/>
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding:3px 0px 10px 0px;">
                <ul>
                    <li><span>��Ʒ���룺</span><span>
                        <asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li></li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <!--����-->
            <div class="functionHandleArea clearfix">
                <!--��ҳ����-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
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
                             <span class="submit_btnxiajia">
                                 <a id="btnAdd" href="javascript:void(0)">���</a>
                             </span>
                        </li>
                    </ul>
                </div>
            </div>
            <!--�����б�����-->
            <UI:Grid runat="server" ID="grdProductSkus" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="SkuId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("SkuId") %>' />
                            <input type="hidden" id="hidProductId" value='<%#Eval("ProductId") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="����" DataField="DisplaySequence" ItemStyle-Width="35px"
                        HeaderStyle-CssClass="td_right td_left" />
                    <asp:TemplateField ItemStyle-Width="42%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;" class="productInfo">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                </a>
                            </div>
                            <div style="float: left;" class="produtName">
                                <span class="Name">
                                    <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <%# Eval("ProductName") %></a>
                                </span> 
                                <span class="colorC">
                                    �̼ұ��룺<%# Eval("ProductCode") %>
                                    ��棺<%# Eval("Stock") %>
                                    ˰�ʣ�<%# Eval("TaxRate", "{0:0%}")%>
                                </span>
                            </div>
                            <div style="float:left;" class="strAttName">
                                <span class="colorC"> <%# Eval("strAttName") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="��Ʒ�۸�" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name">һ�ڼۣ�<%# Eval("SalePrice", "{0:f2}")%></span><span class="colorC">�г��ۣ�<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ԭ����" ItemStyle-Width="120" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span style="float:left;">
                                <img src='<%#Eval("Icon") %>' width="50px" height="50px" />
                            </span>
                            <span style=" line-height:50px;"> <%#Eval("CnArea")%></span>  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="����" DataField="ProductCode" ItemStyle-Width="0" HeaderStyle-CssClass="td_hideen" FooterStyle-CssClass="td_hideen" ItemStyle-CssClass="td_hideen"/>
                    <asp:BoundField HeaderText="���" DataField="Stock" ItemStyle-Width="0" HeaderStyle-CssClass="td_hideen" FooterStyle-CssClass="td_hideen" ItemStyle-CssClass="td_hideen"/>
                    <asp:BoundField HeaderText="˰��" DataField="TaxRate" ItemStyle-Width="0" HeaderStyle-CssClass="td_hideen" FooterStyle-CssClass="td_hideen" ItemStyle-CssClass="td_hideen"/>
                    <asp:BoundField HeaderText="�۸�" DataField="SalePrice" ItemStyle-Width="0" HeaderStyle-CssClass="td_hideen" FooterStyle-CssClass="td_hideen" ItemStyle-CssClass="td_hideen"/>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" src="/Utility/productSelect.js"></script>
</asp:Content>
