<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditAndroidVersion.aspx.cs" Inherits="EcShop.UI.Web.Admin.App.EditAndroidVersion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>android版本编辑</h1>
        <span>在这里您可以修改版本信息。</span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
            <ul>
                <li><span class="formitemtitle Pw_198">更新包：</span>
                 <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
                </li>
                <li><span class="formitemtitle Pw_198">版本号：</span>
                   <asp:TextBox ID="txtVersion" runat="server"  />
                </li>
                <li><span class="formitemtitle Pw_198">版本描述：</span>
                   <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server"  />
                </li>
                 <li><span class="formitemtitle Pw_198">链接地址：</span>
                   <asp:TextBox ID="txtUpgradeUrl" runat="server"/>
                </li>

                <li><span class="formitemtitle Pw_198">是否强制升级：</span>
                    <asp:CheckBox ID="ChkisForceUpgrade" runat="server" />
                </li>
                <li>
                 <asp:Button ID="btnSaveVersion" runat="server" Text="保存" CssClass="submit_queding" style="margin-left:240px" />        
                  
                </li>
            </ul>
         </div>
      </div>
  </div>
</asp:Content>


