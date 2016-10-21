<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<li>
    <a href='<%# Globals.ApplicationPath + "/Vshop/ProductList.aspx?categoryId=" + Eval("CategoryId") %> '>
        
        <%# Eval("Name") %>
    </a>
</li>