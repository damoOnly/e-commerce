<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
    <div>
        <div class="title">
            <%# Eval("Title")%></sapn>
        </div>
        <a href="<%# Globals.ApplicationPath + "/Vshop/Topics.aspx?TopicId=" + Eval("TopicId") %> ">
            <Hi:ListImage runat="server" DataField="IconUrl" />
        </a>
    </div>