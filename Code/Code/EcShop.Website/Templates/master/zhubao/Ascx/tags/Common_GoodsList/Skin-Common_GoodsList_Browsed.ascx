<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<script type="text/javascript">
    $(document).ready(function() {
        $('#clearBrowsedProduct').click(function() {
            $.ajax({
                url: "ShoppingHandler.aspx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { action: "ClearBrowsed" },
                async: false,
                success: function(data) {
                if (data.Status == "Succes") {
                        document.getElementById("listBrowsed").style.display = "none";
                    }
                }
            });
        });

    });
</script>
  

<div id="listBrowsed">
<asp:Repeater runat="server" ID="rptBrowsedProduct">
<ItemTemplate>
 <li>
               <div class="browse_pic">
               <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
               <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl180" CustomToolTip="ProductName" />
               </Hi:ProductDetailsLink></div>
               <div class="browse_name"><a href="#"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></a></div>
               <div class="browse_price">
                <strong><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("SalePrice") %>' runat="server" /></strong>
            </div>
    </li>
</ItemTemplate>
</asp:Repeater>
</div>
<div class="view_clear"><a id="clearBrowsedProduct" href="javascript:void(0)" class="cGray" >清除我的浏览记录</a></div>
<!--结束-->