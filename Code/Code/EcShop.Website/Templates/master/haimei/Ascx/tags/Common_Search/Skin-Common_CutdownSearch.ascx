<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>		    
        <div class="category_search2">
           关键字： <asp:TextBox ID="txtKeywords" CssClass="catesearch_key"   runat="server" MaxLength="50" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;价格范围：<asp:TextBox ID="txtStartPrice" CssClass="catesearch_range" runat="server"  Text="" /> - <asp:TextBox ID="txtEndPrice" CssClass="catesearch_range"  runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
           筛选：<Hi:ProductTagsCheckBoxList RepeatLayout="Flow" ID="ckbListproductSearchType" RepeatDirection="Horizontal" BorderWidth="0" runat="server"></Hi:ProductTagsCheckBoxList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:Button ID="btnSearch" runat="server" Text=" " CssClass="cut_down_button" />
       </div>