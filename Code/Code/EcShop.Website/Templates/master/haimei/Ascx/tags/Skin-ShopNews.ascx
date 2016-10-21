<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<ul>
<asp:Repeater runat="server" ID="recordshopNews">
     <ItemTemplate>
        <li><a href="AffichesDetails.aspx?AfficheId=<%#Eval("AfficheId")%>"><%# Eval("Title")%></a></li>
     </ItemTemplate>
</asp:Repeater>
</ul>



 