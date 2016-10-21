<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<tr>
    <td>
        <span class="green-lb"><%#int.Parse(Eval("Increased").ToString())>0?"+"+Eval("Increased").ToString():"" %><%#int.Parse(Eval("Reduced").ToString())>0?"-"+Eval("Reduced").ToString():"" %></span>
    </td>
    <td><%#Eval("TradeDate")%>
    </td>
    <td><%#Eval("TradeType").ToString()=="0"? "兑换优惠券":Eval("TradeType").ToString()=="1"?"兑换礼品":Eval("TradeType").ToString()=="2"?"购物奖励":Eval("TradeType").ToString()=="3"?"退款扣积分":""%>
    </td>
    <td><%#Eval("Remark") %>
    </td>
</tr>
