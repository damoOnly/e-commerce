<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="coupon-wrap" id="showcoupon" runat="server">
    <div class="coupon-list">
        <ul class="fix">
            <asp:Repeater ID="rptsuppliercouponlist" runat="server">
                <ItemTemplate>
                    <li>
                        <a class="coupon-box fix" onclick="GetCoupon(<%# Eval("CouponId")%>);">
                            <span class="coupon-ex">
                                <span class="coupon-date">活动时间<br />
                                    <%# string.Format("{0:yy/MM/dd}", Eval("StartTime")) %>-<%# string.Format("{0:yy/MM/dd}", Eval("ClosingTime")) %></span>
                                <span class="coupon-get">领取<br />
                                    优惠</span>
                            </span>
                            <span class="coupon-as">
                                <span class="coupon-price">￥<strong><%# Eval("DiscountValue","{0:N0}")%></strong></span>
                                <span class="coupon-des"><span class="coupon-tit">满<%# Eval("Amount","{0:N0}")%>使用</span>优惠券</span>
                            </span>
                        </a>
                    </li>

                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>

