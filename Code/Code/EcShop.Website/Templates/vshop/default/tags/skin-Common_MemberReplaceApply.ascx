<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
    <div class="panel panel-default order-list">
        <div class="panel-heading">
            <h3 class="panel-title">
                <%# Eval("OrderId") %></h3> <span class="date">
                <%# Convert.ToDateTime(Eval("ApplyForTime")).ToString("yyyy-MM-dd")%></span>
        </div>
        <div class="panel-body">
            <a class="a-pic" href="">
                <img class="img-ThumbnailsUrl"  style="border-width: 0px;">
            </a>
			<span class="span-desc"></span>
			<div class="pic-info" style="display:none"><%#Eval("ThumbnailsUrl")%></div>
            <div class="info">
                <div class="status">
                    处理状态：<%#Eval("HandleStatus").ToString()=="0"?"未处理":(Eval("HandleStatus").ToString()=="1"?"已处理":"已拒绝") %>
                    </div><div>
                <div  class="action "><a href="MemberReplaceDetails.aspx?ReplaceId=<%#Eval("ReplaceId")%>" class="btn btn-warning btn-xs ">查看</a></div>
            </div>
        </div>
    </div>
</div>