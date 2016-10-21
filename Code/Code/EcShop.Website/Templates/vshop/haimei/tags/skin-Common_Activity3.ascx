<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<a class="dis-block mb10 banner-ad" href="<%# Globals.ApplicationPath + "/vShop/Topics.aspx?TopicId=" + Eval("TopicId") %> ">
    <img class="img-responsive"  src="<%#Eval("IconUrl")%>" />
</a>