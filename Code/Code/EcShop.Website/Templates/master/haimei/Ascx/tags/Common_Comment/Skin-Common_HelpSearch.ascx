<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>    

 <li class="article_list">
     <a href='<%#  ResolveUrl("/helpItem.aspx?helpid="+Eval("HelpId")) %>'><%# Eval("Title")%></a>
     <%--<label ><%# Eval("Content")%></label>--%>
     <label ><%# Eval("Name")%></label>
</li>
 
