﻿<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<tr>
    <td><a href="<%# Eval("LoctionUrl")%>"> <img  src='<%#Eval("ImageUrl")%>' data-original="<%#Eval("ImageUrl")%>"/></a></td>
</tr>
