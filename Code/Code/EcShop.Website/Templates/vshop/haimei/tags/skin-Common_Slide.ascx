<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<div class="swiper-slide" data-src='<%# string.IsNullOrEmpty(Eval("LoctionUrl").ToString())?"javascript:;":Eval("LoctionUrl")%>'><img src="<%#(Eval("ImageUrl"))%>" width="100%"/>
    </div>