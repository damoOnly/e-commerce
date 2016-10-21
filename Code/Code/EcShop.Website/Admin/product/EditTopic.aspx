<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditTopic.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.EditTopic" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>编辑专题</h1>
                <span>编辑专题</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <!-- <p id="ctl00_contentHolder_dropArticleCategoryTip">选择文章的所属分类</p>-->
                    <li><span class="formitemtitle Pw_100">专题名称：<em>*</em></span>
                        <asp:TextBox ID="txtTopicTitle" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="txtTopicTitleTip">限制在60个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_100">专题图片：</span>
                        <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
                        &nbsp;图片尺寸建议：650px*200px
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgPic" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnPicDelete" runat="server" IsShow="true" Text="删除"
                                            OnClick="btnPicDelete_Click" CommandArgument="pic" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_100">移动端列表图片：</span>
                        <asp:FileUpload ID="fileUploadMobileListImg" CssClass="forminput" runat="server" />
                        &nbsp;图片尺寸建议：650px * 200px
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgMobileList" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnDeleteMobileListImg" runat="server" IsShow="true" Text="删除"
                                            OnClick="btnPicDelete_Click" CommandArgument="mobile_list" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_100">移动端广告图片：</span>
                        <asp:FileUpload ID="fileUploadMobileBannerImg" CssClass="forminput" runat="server" />
                        &nbsp;图片尺寸建议：650px * 200px
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgMobileBanner" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnDeleteMobileBannerImg" runat="server" IsShow="true" Text="删除"
                                            OnClick="btnPicDelete_Click" CommandArgument="mobile_banner" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_100">PC端列表图片：&nbsp;</span>
                        <asp:FileUpload ID="fileUploadPCListImg" CssClass="forminput" runat="server" />
                        &nbsp;图片尺寸建议：650px * 200px
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgPcList" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnDeletePcListImg" runat="server" IsShow="true" Text="删除"
                                            OnClick="btnPicDelete_Click" CommandArgument="pc_list" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_100">PC端广告图片：&nbsp;</span>
                        <asp:FileUpload ID="fileUploadPCBannerImg" CssClass="forminput" runat="server" />
                        &nbsp;图片尺寸建议：650px * 200px
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgPcBanner" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnDeletePcBannerImg" runat="server" IsShow="true" Text="删除"
                                            OnClick="btnPicDelete_Click" CommandArgument="pc_banner" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_100">详细介绍：<em>*</em></span>
                        <div>单张图片尺寸建议不要超过：650px * 1000px</div>
                        <span>
                            <Kindeditor:KindeditorControl ID="fcContent" runat="server" Width="700px" Height="300px" />
                        </span>
                    </li>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnUpdateTopic" runat="server"
                        OnClientClick="return PageIsValid();" Text="保 存"
                        CssClass="submit_DAqueding" OnClick="btnUpdateTopic_Click" />
                </ul>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('txtTopicTitle', 1, 60, false, null, '专题标题不能为空，长度限制在60个字符以内'))
            //            initValid(new InputValidator('txtKeys', 1, 50, false, null, '必填 关键字不能为空，在1至50个字符之间'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>

</asp:Content>
