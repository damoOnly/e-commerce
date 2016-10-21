<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li class="rookie">
<asp:Repeater ID="rptHelp" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Category") %>'>
       <ItemTemplate>
		<h3>
			<span class="g-dib"></span>新手指南
		</h3>
		<div>
			<a href="#">注册新用户</a><a href="#">帐号密码</a><a href="#">买家入门</a><br>
			<a href="#">商家入门</a>　<a href="#">购物流程</a> 
		</div>
      </ItemTemplate>
    </asp:Repeater>
</div>
</li>
 