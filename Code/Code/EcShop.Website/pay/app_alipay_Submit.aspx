<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app_alipay_Submit.aspx.cs" 

Inherits="EcShop.UI.Web.pay.app_alipay_Submit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-

transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../Templates/appshop/script/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../Utility/globals.js"></script>
    <script type="text/javascript" src="../Templates/appshop/script/main.js"></script>
   <script type="text/javascript">
       $(function () {
           var type = GetAgentType(); 
           var payUrl = '<%= pay_json %>';
           if (type == 2)
               window.HiCmd.webPayOrder(payUrl, "paymentReturn");
           else if (type == 1)// ios
               loadIframeURL("hishop://webPayOrder/paymentReturn/" + encodeURIComponent(payUrl));
       });

       function paymentReturn(ret) { // ret 1 为支付成功，ret 0为失败
           location.href = "/AppShop/MemberOrderDetails.aspx?orderId=" + GetQueryString("orderId");
       }
</script>
</body>
</html>
