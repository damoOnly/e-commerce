<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

            <asp:Repeater ID="rp_guest" runat="server">
                <itemtemplate>
                    <li>
                     <a href='/countdownproduct_detail-<%# Eval("ProductId") %>.aspx' target="_blank">                         
                         <span class="panic-name"><%# Eval("ProductName") %></span>
                         <span class="panic-des"><%# Eval("ShortDescription") %></span>
                         <img src='<%#Eval("ThumbnailUrl160") %>' />
                         <span class="buynow">立即抢购</span>
                   </a>

<%--                                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                        <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" /><span class="panic-name"><%# Eval("ProductName") %></span>
                                        <span class="buynow">立即抢购</span>
                                </Hi:ProductDetailsLink>--%>
                          
                    </li>
                </itemtemplate>
            </asp:Repeater>
