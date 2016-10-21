<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<a href="<%#Eval("LoctionUrl")%>">
    <div>
        <div class="icon"  name="icon">            
            <span data-src="<%#Eval("ImageUrl")%>"></span>
        </div>
        <div class="name">
            <%#Eval("ShortDesc")%></div>
    </div>
</a>