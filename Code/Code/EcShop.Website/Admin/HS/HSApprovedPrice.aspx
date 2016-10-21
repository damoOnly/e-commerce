<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.HSApprovedPrice" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" style="" /></em>
            <h1>价格审核</h1>
            <span>可以对供货商的商品进行价格审核操作</span>
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
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择类型--" Width="153" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server" ID="ddlImportSourceType" NullToDisplay="--请选择原产地--" Width="160" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" Width="153" />
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
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>

            <UI:Grid ID="grdProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <div style="float: left;">
                           
                            <span class="Name">  <a href='<%#Eval("ThumbnailUrl410").ToString().Replace("~","") %>'  target="_blank"> </span>
                                <span>  <%# Eval("ProductName") %></span>
                                <span class="colorC">商品编码：<%# Eval("ProductCode") %>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品图片" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <div style="float: left;">
                                <div style="float: left; margin-right: 10px; z-index:100;">
                                <a href='<%#Eval("ThumbnailUrl410").ToString().Replace("~","") %>' class="tooltipqa" target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl410"  Width="100px" Height="100px"/>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="供货商" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("SupplierName") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品价格" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>skuid</td>
                                    <td>市场价</td>
                                    <td>成本价</td>
                                    <td>销售价</td>
                                </tr>
                            <asp:Repeater ID="repeterSkuItems" runat="server">
                                <ItemTemplate>
                                   <tr>
                                     <td><%# Eval("SkuId")%></td>
                                       <td>￥<%# Eval("MarketPrice", "{0:f2}")%></td>
                                     <td>￥<%# Eval("CostPrice", "{0:f2}")%></td>
                                     <td>￥<%# Eval("saleprice", "{0:f2}")%></td>
                                  </tr> 
                                </ItemTemplate>
                               </asp:Repeater> 
                                </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="审价状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="IsApprovedPrice" runat="server" Text='<%#Eval("IsApprovedPrice").ToString()=="0"?"未审核":(Eval("IsApprovedPrice").ToString()=="1"?"审核通过":"审核不通过")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <span>
                            <a href="javascript:void(0)" style='<%#Eval("IsApprovedPrice").ToString()=="0"?"":"display:none"%>'" onclick="return CheckPrice('<%# Eval("ProductId") %>','<%# Eval("ProductName") %>')">审核</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </UI:Grid>


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
    </div>

    <div id="CheckPrice" style="display: none;">
        <div class="frame-content" style="margin-top: -20px;">
            <%--<p>审核价格</p>--%>
            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">理由:</span> <span>
                    <asp:TextBox ID="txtRefusalReason" runat="server" CssClass="forminput" TextMode="MultiLine" Height="60" Width="243" /></span>
            </p>

            <div style="text-align: center; padding-top: 10px;">
                <input type="button" id="Button2" onclick="javascript: acceptRequest();" class="submit_DAqueding" value="审核通过" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript: refuse();" class="submit_DAqueding" value="拒绝通过" />
            </div>
        </div>
    </div>

    <div style="display: none">
        <input type="hidden" id="hidProductId" runat="server" />
        <input type="hidden" id="hidProductName" runat="server" />
        <input type="hidden" id="hidRefusalReason" runat="server" />
        <asp:Button ID="btnAccept" runat="server" CssClass="submit_DAqueding" Text="审核通过" />
        <asp:Button ID="btnRefuse" runat="server" CssClass="submit_DAqueding" Text="拒绝通过" />
    </div>
    <script>

        $(function () {
            $(".tooltipqa").hover(function () {
                var src = $(this).find("img").attr("src");
                var imgwrap = $("<div class='imgwrap' style='position:fixed;width:400px;height:400px;left:50%;top:50%;margin-left:-200px;margin-top:-200px;'><img src='" + src + "' width='400px' height='400px'></div>");
                imgwrap.appendTo('body');
            }, function () {
                $(".imgwrap").remove();
            })
        })
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function CheckPrice(prodcutid,productname) {
            $("#ctl00_contentHolder_hidProductId").val(prodcutid);
            $("#ctl00_contentHolder_hidProductName").val(productname);
            setArryText('ctl00_contentHolder_txtRefusalReason', '');

            ShowMessageDialog("审核价格", "CheckPrice", "CheckPrice");
        }

        function acceptRequest() {
            var refusalReason = $("#ctl00_contentHolder_txtRefusalReason").val();
            $("#ctl00_contentHolder_hidRefusalReason").val(refusalReason);
            $("#ctl00_contentHolder_btnAccept").trigger("click");
        }

        function refuse() {
            var refusalReason = $("#ctl00_contentHolder_txtRefusalReason").val();
            if (refusalReason.length == 0) {
                alert("请输入拒绝理由！");
                return false;
            }
            $("#ctl00_contentHolder_hidRefusalReason").val(refusalReason);
            $("#ctl00_contentHolder_btnRefuse").trigger("click");
        }
        
    </script>
</asp:Content>
