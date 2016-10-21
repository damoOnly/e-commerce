<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li><a href="/vshop/SupProductList.aspx?SupplierId=<%#Eval("SupplierId")%>">
    <div class="supp-wrap fix">
        <div class="fr">
            <span class="<%#int.Parse(Eval("IsCollect").ToString())==1?"supp-fav supp-fav-like":"supp-fav" %>"  data-id="<%#Eval("SupplierId")%>">
                <%#Eval("CollectCount")%></span>
        </div>
        <div class="supp-img">
            <img class="img-responsive" src="<%#Eval("Logo")%>" />
        </div>
        <div class="supp-con">
            <div class="supp-name">
                <%#Eval("ShopName")%>
            </div>
            <p>店主：<%#Eval("ShopOwner")%><span class="ml20"><%# EcShop.Entities.RegionHelper.GetFullRegion( int.Parse( Eval("County").ToString()), ",").Split(',')[0] %></span></p>
            <p>开店：<%#Eval("CreateDate")%></p>
        </div>
    </div>

</a></li>
