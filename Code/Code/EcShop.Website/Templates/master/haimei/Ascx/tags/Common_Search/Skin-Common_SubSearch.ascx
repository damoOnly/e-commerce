<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>		    
<input type="text" id="txt_Search_SubKeywords" class="search-input" />
<%--<input type="button" value="" onclick="searchs()" class="search_btn" />--%>
<img style="margin-left:-50px;" src="/templates/master/haimei/images/search_btn.png" onclick="searchs()" />

<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server">
<span class="cart-box">&nbsp;&nbsp;&nbsp;我的购物车
    <b id="cart-num"><asp:Literal ID="cartNum" runat="server" Text="0"/>
    </b>
</span>
</Hi:SiteUrl>
<script type="text/javascript">
    function searchs() {
        var item = $("#drop_Search_Class").val();
        var key = $("#txt_Search_SubKeywords").val();
        if (key == undefined)
            key = "";

        key = key.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

        var url = applicationPath + "/SubCategory.aspx?keywords=" + encodeURIComponent(key);
        if (item != undefined)
            url += "&categoryId=" + item;
        window.location.href = url;
    }

    $(document).ready(function () {
        $('#txt_Search_SubKeywords').keydown(function (e) {
            if (e.keyCode == 13) {
                searchs();
                return false;
            }
        })
    });
  </script>