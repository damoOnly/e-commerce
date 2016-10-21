<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<li>
    <div class="sub-cate-tit"><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></div>
</li>
