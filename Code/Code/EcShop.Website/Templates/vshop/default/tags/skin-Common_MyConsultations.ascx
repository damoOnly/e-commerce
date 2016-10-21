<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="well myConsultation">
    <div class="consultation-box">
        <div class="ask text-muted">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                <%# Eval("ProductName")%></a>
            <div class="detail bcolor">
                <%# Eval("ConsultationText")%></div>
        </div>
        <div class="answer text-muted">
            客服回复
            <div class="detail text-warning answerDetail">
                <%# Eval("ReplyText")%></div>
        </div>
        <div class="dateTime font-s text-muted">
            <%# Eval("ReplyDate") %>
           </div>
    </div>
</div>
