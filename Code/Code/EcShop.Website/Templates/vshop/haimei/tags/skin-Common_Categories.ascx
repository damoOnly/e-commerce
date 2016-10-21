<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<li> <!--class="selected"-->
    <a name="<%# Eval("HasChildren") %>" onclick="ShowSubCategory(this,<%# Eval("CategoryId") %>,'<%# Eval("HasChildren") %>')" attrCategoryId="<%# Eval("CategoryId") %>">
        <span class="img">
           <!-- <img src="<%# Eval("Icon") %>" class="img-resp" />-->
        </span>
        <span><%# Eval("Name") %></span>
    </a>
</li>

