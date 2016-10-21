<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<li>
    <div class="addre-box">
        <a href="javascript:void(0)" onclick='SetDefault(<%#Eval("ShippingId") %>,this)'>
        <dl class="addre-dl fix">
            <dt>收  货 人：</dt>
            <dd><%#Eval("ShipTo")%></dd>
        </dl>
        <!--<dl class="addre-dl fix">
            <dt>身份证号：</dt>
            <dd>45587798741560</dd>
        </dl>-->
        <dl class="addre-dl fix">
            <dt>联系电话：</dt>
            <dd><%#Eval("CellPhone")%></dd>
        </dl>
        <dl class="addre-dl fix">
            <dt>详细地址：</dt>
            <dd><Hi:RegionAllName ID="regionname" runat="server" RegionId='<%#Eval("RegionId") %>'></Hi:RegionAllName><%#Eval("Address")%></dd>
        </dl>
            </a>
        <div class="btn-op fix">
            <a class="del-btn" href="javascript:void(0)" onclick='DeleteShippingAddress(<%#Eval("ShippingId") %>,this)'></a><a class="edit-btn" onclick='UpdateShipping(<%#Eval("ShippingId") %>)' href="javascript:void(0)"></a>          
            
            <a  href="javascript:void(0)" onclick='SetDefault(<%#Eval("ShippingId") %>,this)' class="def-btn <%#Eval("IsDefault").ToString()=="True" ?"default":"" %>"><%#Eval("IsDefault").ToString()=="True" ?"默认地址":"设为默认地址" %></a>
        </div>
    </div>
</li>