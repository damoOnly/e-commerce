<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllocateOrderToStore.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.AllocateOrderToStore" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen">   
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen">   
    <script src="/utility/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="/utility/windows.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <div class="dataarea mainwidth databody">
            <div class="title">
                <em>
                    <img src="../images/05.gif" width="32" height="32" /></em>
                <h1>分配订单</h1>
                <span>为订单指定实体店</span>
            </div>
        </div>
        <div class="searcharea clearfix">
        <ul>
            <li style="margin-left:10px;">实体店 <asp:DropDownList ID="dropStores" ClientIDMode="Static" runat="server" /></li>
		</ul>
        </div>
        <div class="blank5 clearfix"></div>
        <div style="padding-left: 380px;">
            <asp:Button ID="btnDo" runat="server" CssClass="submit_DAqueding" Text="确定" />
        </div>
    </form>
</body>
</html>
