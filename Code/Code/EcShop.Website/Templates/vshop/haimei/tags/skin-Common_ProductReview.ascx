<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="com-box">
        <div class="com-head fix">   
        	<div class="fr fix"><div class='star-list fix star-level-<%# Eval("Score")%>'><span class="star light"></span><span class="star light"></span><span class="star light"></span><span class="star half"></span><span class="star"></span></div></div>         
            <div class="ovh">
                <img class="mb-img" src="/templates/vshop/haimei/resource/default/image/temp/mb.png"/>
                <span class="mb-name"><%# Eval("UserName")%></span></div>
        </div>
        <div class="com-con">
        	<div class="com-inner"><%# Eval("ReviewText")%></div>
            <div class="gray">
                <label class="fl">评论时间：</label>
                <%# Convert.ToDateTime(Eval("ReviewDate")).ToString("yyyy-MM-dd")%></div>
        </div>
    </div>
</li>