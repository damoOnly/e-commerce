<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="cp-box fix">        
        <div class="cp-con"><span class="cp-price"><strong><%# Eval("DiscountValue", "{0:F2}")%></strong>元</span>优惠券</div>
        <div class="cp-info">
        	<span class="cp-date"><%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %>至<%# Convert.ToDateTime(Eval("ClosingTime")).ToString("yyyy-MM-dd") %></span>
            <p class="cp-name">满<%# Eval("Amount", "{0:F2}")%>减<%# Eval("DiscountValue", "{0:F2}")%></p>
        </div>
    </div>
</li>
