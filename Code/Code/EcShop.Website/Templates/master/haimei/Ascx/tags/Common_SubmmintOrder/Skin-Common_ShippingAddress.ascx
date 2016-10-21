<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Entities" %>
<%@ Import Namespace="System" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<div class="address_tab">
    <asp:Repeater ID="rp_shippgaddress" runat="server">
        <itemtemplate>           
            <div class="ad-ele fix" id="<%# Eval("ShippingId")%>">
            	<div class="toolbar"><a onclick="SetDefaultAddress(<%# Eval("ShippingId")%>)">设为默认地址</a><a onclick="EditAddress(<%# Eval("ShippingId")%>)">编辑</a><a onclick="DeleteAddress(<%# Eval("ShippingId")%>)">删除</a></div>
                    <label id="title_<%# Eval("ShippingId")%>" title="<%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %> <%# Eval("Address")%>">
                    <input type="radio" name="hidShippingId" value='<%# Eval("ShippingId") %>'/><span>
                    <span id="region_<%# Eval("ShippingId")%>"><%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %></span>
                    <span id="address_<%# Eval("ShippingId")%>" class="mr5"><%# Eval("Address")%></span><span id="shipto_<%# Eval("ShippingId")%>" class="mr5"><%# Eval("ShipTo") %></span>
                    <span id="cellphone_<%# Eval("ShippingId")%>" class="mr5"><%# Eval("CellPhone")%></span></span></label>
                <input type="hidden" class="hidShippingId" value='<%# Eval("ShippingId") %>' /></div>
        </itemtemplate>
    </asp:Repeater>
</div>