<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
<tr>
    <td width="8%" align="center">

            <input name="CheckBoxGroup" type="checkbox"  value='<%#Eval("FavoriteId") %>' class="quanxuan_btn1" />
        
    </td>
    
    <td width="20%" align="center">
        <span class="quanxuan_pic1">
             <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" /></span> 
        </span>
    </td>
    
    <td width="40%" align="center">
        <span class="s_shangpin"><b>  <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                        ProductId='<%# Eval("ProductId") %>' runat="server" /></b><em>��ǩ��<span id="em<%#Eval("FavoriteId") %>"> <asp:Label ID="lblTags" runat="server" Text='<%#Bind("Tags") %>'></asp:Label></span></em> 
        </span>
    </td>
    
    <td width="20%" align="center">
        <div>
            <span class="s_span2">�г��۸�<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice") %>'
                        runat="server" /></span> <span class="s_span2">��ļ۸�<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"
                            Money='<%# Eval("RankPrice") %>' runat="server" /></span>
        </div>
        <div class="s_span1">
            ��ʡ��  <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel3" Money='<%# Math.Abs((Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice"))-(decimal)Eval("RankPrice") )%>'
                            runat="server"></Hi:FormatedMoneyLabel></div>
    </td>
    <td width="12%" align="center">
        
        <div>
          <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
            <img src="/templates/master/default/images/users/shoucang_06.jpg"/> </Hi:ProductDetailsLink>
      </div>
        <div> <a href="javascript:void(0)" tag='<%#Eval("FavoriteId") %>' onclick="DelFavorite(this)">ɾ��</a></div>
        <div>
            <a Class="editfavorite"  id='<%#Eval("FavoriteId") %>'>�༭</a></div>

    </td>
</tr>
