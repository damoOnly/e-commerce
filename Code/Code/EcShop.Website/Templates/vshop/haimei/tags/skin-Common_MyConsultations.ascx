<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="ct-wrap">
        <div class="ct-box">
            <div class="ct-info"><a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                <div class="ct-name"><%# Eval("ProductName")%></div>
                </a> </div>
        </div>
    </div>
    <div class="fix">
        <div class="my-cons">
            <label>我的咨询：</label>
            <span><%# Eval("ConsultationText")%></span></div>
        <ol class="eval-wrap">
            <div class='no-eval <%# string.IsNullOrEmpty(Eval("ReplyText").ToString()) ?"":"dis-none" %>'>客服暂时没有回复，请稍后……</div>
            <div class='eval-box fix <%# string.IsNullOrEmpty(Eval("ReplyText").ToString()) ?"dis-none":"" %>'>
                <div class='eval-right'>
                    <p class='eval-name'>客服回复：</p>
                    <div class='date'><%# Eval("ReplyDate") %></div>
                </div>
                <div class='eval-left'>
                    <p><%# Eval("ReplyText")%></p>
                </div>
            </div>
        </ol>
    </div>
</li>
