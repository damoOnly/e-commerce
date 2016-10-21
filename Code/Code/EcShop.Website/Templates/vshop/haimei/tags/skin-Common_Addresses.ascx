<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<li><a href="#" shippingId="<%# Eval("ShippingId")%>" name="<%# Eval("RegionId")%>" briefAddress="<%# Eval("ShipTo")%> &nbsp;<%# Eval("CellPhone")%> &nbsp; <%# Eval("Address")%>" > 

<%#Eval("ShipTo")+" "+ Eval("CellPhone")+" "+RegionHelper.GetFullRegion((int)Eval("RegionId")," ")+" "+ Eval("Address") %>
</a><input type="hidden" value=<%#Eval("IdentityCard")%> class="identity">

</li>
