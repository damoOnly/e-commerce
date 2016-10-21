<%@ Control Language="C#"%>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<Hi:Script runat="server" Src="/utility/tags/onlineserver.js" />
<div id="qq_right" style="top:30px;left:-146px;position:absolute;z-index:100;">
<div class="a">
  <div class="b"></div>
  <div class="c">
  <asp:Literal runat="server" ID="litOnlineServer" />  
	<div class="closekf" onclick="closekf();">关闭在线客服</div>
  </div>
  <div class="d"></div>
</div>
<div class="e" id="e" onclick="showKefu(this);"></div>
</div>
