<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<asp:Repeater ID="rptProducts" runat="server">
    <ItemTemplate>
        <tr>
            <td  colspan="3" align="center"  width="45%"  class="bor">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" id="cbg-table">
                    <tr>
                      <td  width="10%"><input name="CheckBoxGroup" type="checkbox" value="<%#Eval("ProductId") %>" class="quanxuan_btn1" /></td>
                      <td   width="30%"><Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" /></td>
                      <td   width="60%" class="bor"> 
                    <p><Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                        ProductId='<%# Eval("ProductId") %>' runat="server" /></p> <em style="color: #8c8c8c;">
                            商家编码：
                            <%#Eval("ProductCode") %>
                        </em></td>
                    </tr>
                </table>
                
            </td>
            <td colspan="4"  align="center" width="55%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" id="guige-list">
                    <asp:Repeater ID="rptSkus" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td width="52%"  align="center"  class="bor">
                                   
                                        <input name="chkskus" type="checkbox" chk="<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "ProductId") %>" value="<%#Eval("skuid") %>" class="guige_fuxuan" />
                                        <span>货号：<%# Eval("sku") %></span> <asp:Literal ID="skuContent" runat="server"></asp:Literal> 
                                </td>
                                <td width="16%" align="center">
                                    <div>
                                        <input name="<%#Eval("skuid") %>" value="1" type="text" class="goumai_input" /></div>
                                </td>
                                <td width="16%" align="center"><div><%# Eval("stock")%></div>
                                </td>
                                <td width="16%" align="center"  class="bor">
                                    <div>
                                        <%# Eval("saleprice","{0:F2}")%></div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
