﻿<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="consultation-box">
    <div class="ask text-muted">
        用户咨询
        <div class="detail bcolor">
            <%# Eval("ConsultationText")%></div>
    </div>
    <div class="answer text-muted">
        客服回复
        <div class="detail text-warning answerDetail">
            <%# Eval("ReplyText")%></div>
    </div>
    <div class="dateTime font-s text-muted">
        <%# Eval("ReplyDate")%></div>
</div>
