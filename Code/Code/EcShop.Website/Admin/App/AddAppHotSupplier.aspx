<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddAppHotSupplier.aspx.cs" Inherits="EcShop.UI.Web.Admin.App.AddAppHotSupplier" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<%@ Import Namespace="EcShop.Core" %>
<script type="text/javascript">
    var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
</script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/UploadHandler.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1 id="h1title">添加热门商家</h1>
            <span id="spanTitle" class="font">添加热门商家</span>
      </div>
          <div class="formitem validator2">
            <ul>
              <li><span class="formitemtitle Pw_100">热门商家描述：</span>
                <asp:TextBox ID="txtDesc" runat="server" Width="600px" CssClass="forminput" />
              </li>              
              <li runat="server" id="liParent"><span class="formitemtitle Pw_100">上传图片：</span>
                <span id="spanButtonPlaceholder"></span>
                                <span id="divFileProgressContainer"></span>
								<%--<div>图片建议尺寸：650px * 320px</div>--%>
              </li>  
              <li id="smallpic" style="display: none;"> 
                  <img id="littlepic" runat="server" src=""/>
                             <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                           </li>      
              <li> <span class="formitemtitle Pw_100">请选择商家：</span>
                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="forminput droptype" ClientIDMode="Static">
                </asp:DropDownList>
              </li>
            </ul>
              <ul class="btn Pa_100 clearfix">
                <asp:Button ID="btnAddSupplier" runat="server" 
                      Text="确 定"  
                      CssClass="submit_DAqueding float" onclick="btnAddSupplier_Click" />
         </ul>
         <!--隐藏图片地址-->
               <input id="fmSrc" runat="server" clientidmode="Static" type="hidden" value="" />
          </div>
  </div>    
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/UploadBanner.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ShowType();
            BindType();
            // $("#ddlType").val("Link").trigger("change");
            //ShowThirdDropDown();
            var fmSrc = $("#fmSrc").val();
            if (fmSrc == null || fmSrc == "") {
                $("#ddlType").val("Link").trigger("change");
                return;
            }
            else {
                $("#h1title").text("编辑热门商家");
                $("#spanTitle").html("编辑热门商家");
                $("#smallpic").show();
            }
        }
        );


    </script>
    <script src="../js/AppLocationType.js" type="text/javascript"></script>
</asp:Content>
