<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>


<asp:Repeater runat="server" ID="recordsone">
    <ItemTemplate>
        <input type="hidden" runat="server" id="hidMainCategoryId" value='<%#Eval("CategoryId")%>' />
        <div class="my_left_cat_list">
            <div id='<%# "twoCategory_" + Eval("CategoryId")%>' class="h2_cat">
                <h3>
                    <%--<em>
                    <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("Notes5")%>' /></em>--%>
                    <em>
                        <img id="imgicon" alt="icon" src="<%#Eval("Icon")%>" style="border-width: 0px;"></em>
                    <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a> </h3>
                <%--                <ul class="new-subcate">
                	<li><a>纸尿裤</a></li>
                    <li><a>营养辅食</a></li>
                    <li><a>奶瓶奶嘴</a></li>
                </ul>--%>
                <ul class="new-subcate">
                    <asp:Repeater runat="server" ID="recordTwoCategory">
                        <ItemTemplate>
                            <li><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="h3_cat" id='<%# "threeCategory_" + Eval("CategoryId")%>'>
                    <div class="shadow">
                        <div class="shadow_border">
                            <span class="brand">
                                <h5>品牌推荐：<b><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("brand")%>'>更多品牌>></a></b></h5>
                                <asp:Repeater runat="server" ID="recordsbrands">
                                    <ItemTemplate>
                                        <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("branddetails",Eval("BrandId"))%>'><%# Eval("BrandName")%></a>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </span>
                            <span class="category">
                                <asp:Repeater runat="server" ID="recordstwo">
                                    <ItemTemplate>
                                        <input type="hidden" runat="server" id="hidTwoCategoryId" value='<%#Eval("CategoryId")%>' />
                                        <div class="fenlei_jianduan">
                                            <h4>
                                                <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                                            </h4>
                                            <%--                                         <ul>
                                             	<li><a href="#">一段</a></li>
                                                <li><a href="#">一段</a></li>
                                                <li><a href="#">一段</a></li>
                                                <li><a href="#">一段</a></li>
                                                <li><a href="#">一段</a></li>
                                                <li><a href="#">羊奶粉</a></li>
                                                <li><a href="#">其他奶粉</a></li>
                                            </ul> --%>
                                            <div class="fthree">
                                                <ul>
                                                    <asp:Repeater runat="server" ID="recordsthree">
                                                        <ItemTemplate>
                                                            <li>
                                                                <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                                                            </li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </span>
                            <div class="bottom-adimgs">
                                <asp:Repeater runat="server" ID="recordtwoHotBuyProduct">
                                    <ItemTemplate>
                                        <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                        <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl60" CustomToolTip="ProductName" />
                                        </Hi:ProductDetailsLink>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>




