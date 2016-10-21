<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <a href="<%# Globals.ApplicationPath + "/Vshop/SupplierDefault.aspx?SupplierId=" + Eval("SupplierId")%>">
        <div class="supp-wrap fix">
            <div class="fr"><span class="supp-fav supp-fav-like" data-id="<%#Eval("SupplierId")%>"><%#Eval("CollectCount")%></span> </div>
            <div class="supp-img">
                <img class="img-responsive" src="<%#Eval("Logo")%>" />
            </div>
            <div class="supp-con">
                <div class="supp-name"><%#Eval("SupplierName")%></div>
                <p>店主：<%#Eval("ShopOwner")%><span class="ml20"><%# EcShop.Entities.RegionHelper.GetFullRegion( int.Parse( Eval("County").ToString()), ",").Split(',')[0] %></span></p>
                <p>开店：<%#Eval("CreateDate")%></p>
            </div>
        </div>
    </a>
    <a class="del-btn" href="javascript:void(0)" onclick="Submit('<%# Eval("SupplierId")%>')"></a>
</li>
