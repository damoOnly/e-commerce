<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li><a href="<%# Globals.ApplicationPath + "/vShop/Topics.aspx?TopicId=" + Eval("TopicId") %> "><img class="lazyload" src="/images/lazy.png" data-original="<%#Eval("IconUrl")%>" /></a></li>
