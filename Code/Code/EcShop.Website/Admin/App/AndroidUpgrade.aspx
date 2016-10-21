<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AndroidUpgrade.aspx.cs" Inherits="EcShop.UI.Web.Admin.AndroidUpgrade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
   <div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>Android升级</h1>
        <span>在这里您可以上传我们提的供升级包进行版本升级。</span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
            <ul>
                <li><span class="formitemtitle Pw_198"><em >*</em>更新包：</span>
                 <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
                 <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" style=" margin-left:5px;"/>        
                  <p runat="server">上传了升级包之后将会覆盖原来的版本，上传前请确认上传的更新包版本比当前版本新</p>
                </li>
                <h2 class="clear">当前版本信息</h2>
                <li><span class="formitemtitle Pw_198">App名称：</span>
                   <asp:Literal ID="litAppName" runat="server" Text="" />
                </li>
                <li><span class="formitemtitle Pw_198">Apk名称：</span>
                   <asp:Literal ID="litApkName" runat="server" Text="" />
                </li>
                <li><span class="formitemtitle Pw_198">版本：</span>
                   <asp:Literal ID="litVersionName" runat="server" Text="1.00" />
                </li>
                <li><span class="formitemtitle Pw_198">版本号：</span>
                   <asp:Literal ID="litVersion" runat="server" Text="1" />
                   <input type="hidden" id="hidIsForcibleUpgrade" runat="server" />
                </li>
                <li><span class="formitemtitle Pw_198">版本描述：</span>
                   <asp:Literal ID="litDescription" runat="server" Text="初始版本" />
                </li>
                 <li><span class="formitemtitle Pw_198">链接地址：</span>
                   <asp:Literal ID="litUpgradeUrl" runat="server" Text="" />
                </li>
                 <li><span class="formitemtitle Pw_198">升级描述地址：</span>
                   <asp:Literal ID="litUpgradeInfoUrl" runat="server" Text="" />
                </li>
            </ul>
         </div>
      </div>
  </div>
</asp:Content>
