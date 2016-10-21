<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
<a href="#" onclick="javascript:location.href='./Favorites.aspx?tags='+escape('<%#Eval("TagName") %>')"><%#Eval("TagName") %>(<%#Eval("ProductNum") %>)</a>  