﻿<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Entities" %>
<%@ Import Namespace="System" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<div class="address_tab">
    <asp:Repeater ID="rp_shippgaddress" runat="server">
        <ItemTemplate>
            <div class="list">
                <div class="inner ">
                    <div title="<%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %>" (<%# Eval("ShipTo") %>收)" class="addr-hd">
                        <%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %>（<span class="name"><%# Eval("ShipTo") %></span><span>
                            收）</span></div>
                    <div class="addr-bd" title='<%# Eval("Address")%>'>
                        <span class="street"><%# Eval("Address")%></span><span class="phone"><%# Eval("CellPhone")%></span><span
                            class="last">&nbsp;</span>
                    </div>
                </div>
                <em class="curmarker"></em>
                <input type="hidden" class="hidShippingId" value='<%# Eval("ShippingId") %>' />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>