<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Service.aspx.cs" Inherits="EcShop.UI.Web.Admin.Service" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

 <%-- <Hi:HiControls ID="HiControls1" LinkURL="http://www.ecdev.cn/zengzhi.html" Height="1500" runat="server"/>--%>
 <div  class="areacolumn clearfix" style="width:980px;">
 <iframe src="http://www.ecdev.cn/zengzhi.html" style="border:0px;background-color:Transparent; height:1500px;width:980px;" scrolling="no" allowTransparency="true" frameborder="0"></iframe></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
          function gotoWeb(src)
          {
            window.open(src,"_blank")
          }
        </script>
</asp:Content>
