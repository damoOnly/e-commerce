<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<li item='<%# Eval("VoteItemId")%>'>
    <i>&nbsp;</i>
    <div>
        <p><%# Eval("VoteItemName")%></p>
        <samp><em>&nbsp;<%# Eval("Lenth")%></em><span><%# Eval("Percentage")%>%(<%# Eval("ItemCount")%>)</span><samp>
    </div>
</li>