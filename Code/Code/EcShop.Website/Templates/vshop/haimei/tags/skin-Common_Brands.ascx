<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<a href="BrandDetail.aspx?BrandId=<%# Eval("BrandId")%>">
    <div class="well">
        <img src="<%# Eval("Logo")%>" class="img-responsive">
        <div class="name font-l">
            <%# Eval("BrandName")%></div>
    </div>
</a>