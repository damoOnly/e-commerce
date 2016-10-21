<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BathSendOrderByExcel.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.BathSendOrderByExcel" %>
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
                <h1>订单批量发货</h1>
                <span>这里的上传将执行批量发货操作,请按照发货模版的格式上传excel表格，发货失败的订单将会已excel表格返回</span>
            </div>
        </div>
        <div class="searcharea clearfix">
            <asp:FileUpload ID="excelFile" runat="server" /></div><a href="/Storage/master/help/bathsendgoods.xls">下载发货模版</a>
        <div class="blank5 clearfix"></div>
        <div style="padding-left: 200px;">
            <asp:Button ID="btnBatchSend" runat="server" CssClass="submit_DAqueding" Text="上传" />
        </div>
    </form>
</body>
</html>
