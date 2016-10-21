<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>


<li>
    <div class="odbox">
    	<a class="odbox-head fix" href="<%#Globals.ApplicationPath + "/vShop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
            <label class="fr ml5 odbox-date">申请时间：<span><%# Convert.ToDateTime(Eval("ApplyForTime")).ToString("yyyy-MM-dd")%></span>

            </label><label class="odbox-ser">订单编号：<span><%# Eval("OrderId") %></span></label>
        </a>        
        <div class="odbox-con">
        	<div class="odbox-gds fix">
            	<a class="a-pic" href="">
                    <img class="img-ThumbnailsUrl"  style="border-width: 0px;">
                </a>
            	<span class="span-desc"></span>
                <div class="pic-info" style="display:none"><%#Eval("ThumbnailsUrl")%></div>
            </div>
            <div class="odbox-info fix">                
                <div class="odbox-summer">
                	<div style="float: left;">
                        <div class="odbox-tpc">
                        <label>退款金额：</label><span class="odbox-price">￥<span><%# Eval("OrderTotal","{0:F2}")%></span></span></div>
                        <div class="odbox-status">
                        处理状态：<%#Eval("HandleStatus").ToString()=="0"?"待处理":(Eval("HandleStatus").ToString()=="1"?"已完成":(Eval("HandleStatus").ToString()=="2"?"已拒绝":"已受理")) %>
                        </div>
                    </div>
                    <div class="odbox-op fix" style="float: right;">
                        <a href="MemberReturnsDetails.aspx?ReturnsId=<%#Eval("ReturnsId")%>" class="">查看</a>                         
                    </div>
                </div>
            </div>
        </div>
    </div>
</li>