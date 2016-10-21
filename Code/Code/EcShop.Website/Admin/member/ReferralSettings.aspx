<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralSettings.aspx.cs" Inherits="EcShop.UI.Web.Admin.ReferralSettings" Title="无标题页" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1 class="title_line">推广员申请设置 </h1>
            <span class="font">这里可以设置推广员介绍，推广员申请是否需要平台审核</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>推广员介绍设置：</span>
                        <span style="display:block; float:left;width:78%;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fckReferralIntroduction" runat="server" Width="98%" Height="300px"/></span>
                        <p>用户在申请成为推广员时能看到这些介绍。</p>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>推广员申请是否需要审核：</span>
                        <asp:CheckBox runat="server" ID="chkIsAudit" Text="需要审核" />
                    </li>
                </ul>
                <div style="clear: both"></div>
                <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
