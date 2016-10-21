<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
 <li>
              <div class="category_pro_pic">
              <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
                </Hi:ProductDetailsLink>
                  <div class="category_pro_source">海外进口 海外价格 闪电发货</div>
              </div>

              <div class="category_pro_name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
              <div class="category_pro_price"><p>￥<strong><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("RankPrice") %>' runat="server" /></strong></p><%--<em>￥:<span>10</span></em>--%>
                 <%# string.IsNullOrEmpty(Eval("PromotionName").ToString())?"":"<span class='discount'>"+Eval("PromotionName")+"</span>" %> 
                  <%--<span class="discount">5.6折</span>--%></div>
              
              <div class="bars fix cart-tip">
             
             <span><%#Eval("ShippingMode") %></span>
             <span onclick="AddToFav(<%#Eval("ProductId") %>)" style="cursor:pointer"><label id="<%#Eval("ProductId")%>"><%#Eval("CollectCount")%></label></span>
                 <div class="pd-size">
             	<div class="pd-size-tit">请选择商品规格<div class="arrow-btn">收起∨</div></div>            
              <Hi:SKUSelector4List ID="SKUSelector" runat="server" ProductId='<%#Eval("ProductId") %>' cssclass="Product_WareFun_bg"/>
                  </div>			 
             	<div class="num-wrap fix" style="display:none;">
                	<div class="num-con">
                		<input type="text" value="1" old-v="1" class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"":"disabled" %>'/>
                    </div>
                    <div class="num-bar">
                        <a class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"btn-add":"btn-add disabled" %>'></a>
                        <a class='btn-reduce disabled'></a>
                    </div>
                </div>
                  <a class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"addcartButton cart-btn":"addcartButton cart-btn disabled" %>' productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>">加入购物车</a>	
              <%--<a class="addcartButton cart-btn" productid="<%#Eval("ProductId") %>" price="<%#Eval("SalePrice") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>"></a>--%>
              </div>
</li>