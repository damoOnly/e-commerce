<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div style="display:none;">
<asp:GridView ID="grdPayment" runat="server" AutoGenerateColumns="false" DataKeyNames="Gateway"  Width="100%"  BorderWidth="0" CssClass="cart_Order_deliver2">
    <Columns>
        <asp:TemplateField HeaderText="选择" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <Hi:ListRadioButton ID="radioButton" GroupName="paymentMode" runat="server" value='<%# Eval("ModeId") %>' />
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
        <asp:BoundField HeaderText="支付方式" ItemStyle-Width="20%" DataField="Name" ItemStyle-HorizontalAlign="Center"  />
        <asp:TemplateField HeaderText="详细介绍"   ItemStyle-Width="65%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <span style="word-break:break-all;"><%# Eval("Description") %></span>
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
<ul class="pay-way">
	<li>
    	<label><input type="radio" name="paymentMode" value="5" class="first"/><img  src="/templates/master/haimei/images/payway/tempus.png"/></label>
    </li>
    <li>
    	<label><input type="radio" name="paymentMode" value="3"/><img  src="/templates/master/haimei/images/payway/zfb.png"/></label>
    </li>
</ul>


