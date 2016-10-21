<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<a href='<%# string.IsNullOrEmpty(Eval("LoctionUrl").ToString())?"javascript:;":Eval("LoctionUrl")%>'>
  <img src="<%#(Eval("ImageUrl"))%>" />
</a>
