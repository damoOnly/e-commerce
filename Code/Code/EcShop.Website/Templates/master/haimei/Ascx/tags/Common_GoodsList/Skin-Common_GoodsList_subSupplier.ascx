<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
              <div class="category_pro_pic">
              <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl310" CustomToolTip="ProductName" />
                </Hi:ProductDetailsLink>
                <div class="category_pro_source">海外进口 海外价格 闪电发货</div>
              </div>              
              <div class="category_pro_name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
             <div class="bars fix">
             <%--<div class="pd-size">
             	<div class="pd-size-tit">请选择商品规格<div class="arrow-btn">收起∨</div></div>
                <div class="pd-size-con">
                	<input type="hidden" id="hiddenProductType" value="">
                    <input type="hidden" id="hiddenProductId" value="960">
                    <div id="productSkuSelector" class="Product_WareFun_bg">
                        <div class="SKURowClass">
                        	<span>型号：</span><input type="hidden" name="skuCountname" attributename="型号" id="skuContent_71"><dl id="skuRow_71"><dd>
                        <input type="button" class="SKUValueClass" id="skuValueId_71_297" attributeid="71" valueid="297" value="102"></dd></dl></div>
                        <div class="SKURowClass">
                        <span>容量：</span><input type="hidden" name="skuCountname" attributename="容量" id="skuContent_65"><dl id="skuRow_65"><dd>
                        <input type="button" class="SKUValueClass" id="skuValueId_65_289" attributeid="65" valueid="289" value="64G"></dd></dl></div>                        
					</div>
                </div>
             </div>--%>
                 <div class="pd-size">
             	<div class="pd-size-tit">请选择商品规格<div class="arrow-btn">收起∨</div></div>            
              <Hi:SKUSelector4List ID="SKUSelector" runat="server" ProductId='<%#Eval("ProductId") %>' cssclass="Product_WareFun_bg"/>
                  </div>
                  <div class="category_pro_price"><!--<span class="rate">税率：<%# Eval("TaxRate", "{0:0%}") %></span>--><p><Hi:RankPriceName ID="RankPriceName1" runat="server" PriceName="" />￥<strong><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("RankPrice") %>' runat="server" /></strong></p><em><%# !string.IsNullOrEmpty(Eval("MarketPrice").ToString()) ? (Decimal.Parse(Eval("MarketPrice").ToString()) > 0?Decimal.Parse(Eval("MarketPrice").ToString()).ToString("C"):""):"" %></em></div>			 
             	<div class="num-wrap fix" style="display:none;">
                	<div class="num-con">
                		<input type="text" value="1" old-v="1" class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"":"disabled" %>'/>
                    </div>
                    <div class="num-bar">
                        <a class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"btn-add":"btn-add disabled" %>'></a>
                        <a class='btn-reduce disabled'></a>
<%--                        <a class="btn-add"></a>
                        <a class="btn-reduce disabled"></a>--%>
                    </div>
                </div>		
                <a class='<%# Convert.ToInt32(Convert.IsDBNull(Eval("Stock"))?"0":Eval("Stock"))>0?"addcartButton cart-btn":"addcartButton cart-btn disabled" %>' productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>">加入购物车</a>	
             <%-- <a class="addcartButton cart-btn" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>">加入购物车</a>--%>
              </div>
</li>