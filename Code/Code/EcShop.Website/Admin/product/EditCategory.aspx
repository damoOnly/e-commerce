﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.EditCategory" MasterPageFile="~/Admin/Admin.Master" CodeBehind="~/Admin/product/EditCategory.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>修改商品分类</h1>
                <span class="font">为不同类型的商品创建不同的分类，方便您管理也方便顾客浏览</span>
            </div>
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>分类名称：</span>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="forminput" />
                        <p id="ctl00_contentHolder_txtCategoryNameTip">分类名称不能为空，在1至60个字符之间</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品类型：</span>
                        <span class="formselect">
                            <Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--请选择--" Width="150px" /></span>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">启用/禁用：</span>
                        <span class="formselect">
                            <Hi:DisableRadioButtonList ID="radioButton" GroupName="disableRadioButton" runat="server" />
                        </span>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">分类标识：</span>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="productType" Width="153px" />
                    </li>
                    <li><span class="formitemtitle Pw_198">分类图标：</span>
                        <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
                        <div class="Pa_100 Pg_8 clear">
                            <table width="150" border="0" cellspacing="0">
                                <tr>
                                    <td width="100">
                                        <Hi:HiImage runat="server" ID="imgPic" CssClass="Img100_60" /></td>
                                    <td width="55" align="center">
                                        <Hi:ImageLinkButton ID="btnPicDelete" runat="server" IsShow="true" Text="删除" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>

                    <%-- <li runat="server" id="li1"> <span class="formitemtitle Pw_198">税率：</span>
                <span class="formselect"><Hi:TaxRateDropDownList runat="server" CssClass="productType" ID="dropTaxRate" Width="153px" /></span>
                 <p id="ctl00_contentHolder_dropTaxRateTip"></p>
              </li>--%>

                    <li runat="server" id="liParentCategroy"><span class="formitemtitle Pw_198">货号前缀：</span>
                        <asp:TextBox ID="txtSKUPrefix" runat="server" CssClass="forminput" />
                        <p id="ctl00_contentHolder_txtSKUPrefixTip">货号前缀长度限制在5个字符以内，必须为字母数字</p>
                    </li>
                    <li runat="server" id="liRewriteName" style="margin-bottom: 0px;">
                        <span class="formitemtitle Pw_198">URL重写名称：</span>
                        <asp:TextBox ID="txtRewriteName" runat="server" CssClass="forminput" MaxLength="50" />
                        <p id="ctl00_contentHolder_txtRewriteNameTip"></p>
                    </li>
                    <li style="margin-bottom: 0px;"><span class="formitemtitle Pw_198">搜索标题：</span>
                        <asp:TextBox ID="txtPageKeyTitle" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtPageKeyWordsTip">长度限制在100字符以内</p>
                    </li>
                    <li style="margin-bottom: 0px;"><span class="formitemtitle Pw_198">搜索关键字：</span>
                        <asp:TextBox ID="txtPageKeyWords" runat="server" CssClass="forminput" />
                        <p id="ctl00_contentHolder_txtPageKeyWordsTip">长度限制在160字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">搜索描述：</span>
                        <asp:TextBox ID="txtPageDesc" runat="server" CssClass="forminput" />
                        <p id="ctl00_contentHolder_txtPageDescTip">长度限制在240个字符以内</p>
                    </li>

                    <li class="m_none"><span class="formitemtitle Pw_198">分类广告：</span>
                        <span class="tab">
                            <div class="status">
                                <ul>
                                    <li style="clear: none;"><a onclick="ShowNotes(1)" id="tip1" style="cursor: pointer">分类广告1</a></li>
                                    <li style="clear: none;"><a onclick="ShowNotes(2)" id="tip2" style="cursor: pointer">分类广告2</a></li>
                                    <li style="clear: none;"><a onclick="ShowNotes(3)" id="tip3" style="cursor: pointer">分类广告3</a></li>
                                </ul>
                            </div>
                        </span>
                        <span style="padding-left: 198px;">
                            <div id="notes1">
                                <Kindeditor:KindeditorControl ID="fckNotes1" runat="server" Width="850" Height="300px" />
                            </div>
                            <div id="notes2" style="display: none;">
                                <Kindeditor:KindeditorControl ID="fckNotes2" runat="server" Width="850" ImportLib="false" Height="300px" />
                            </div>
                            <div id="notes3" style="display: none;">
                                <Kindeditor:KindeditorControl ID="fckNotes3" runat="server" Width="850" ImportLib="false" Height="300px" />
                            </div>
                        </span>
                    </li>
                    <li></li>
                </ul>
                <ul class="btn Pa_198 clearfix">
                    <asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <style type="text/css">
        .off {
            color: #741f0b;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowNotes(index) {

            document.getElementById("notes1").style.display = "none";
            document.getElementById("notes2").style.display = "none";
            document.getElementById("notes3").style.display = "none";
            var notesId = "notes" + index;
            document.getElementById(notesId).style.display = "block";

            document.getElementById("tip1").className = "";
            document.getElementById("tip2").className = "";
            document.getElementById("tip3").className = "";
            var tipId = "tip" + index;
            document.getElementById(tipId).className = "off"
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtCategoryName', 1, 60, false, null, '必填 分类名称不能为空，长度限制在60个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtSKUPrefix', 1, 5, true, '(?!_)(?!-)[a-zA-Z0-9_-]+', '货号前缀长度限制在5个字符以内，必须为字母数字'))
            initValid(new InputValidator('ctl00_contentHolder_txtRewriteName', 0, 60, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', 'URL重写长度限制在60个字符以内，必须为字母开头且只包含字母-和_'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyTitle', 0, 100, true, null, '长度限制在100个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyWords', 0, 160, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageDesc', 0, 240, true, null, '告诉搜索引擎此分类浏览页面的主要内容，长度限制在240个字符以内'))
            //initValid(new InputValidator('ctl00_contentHolder_dropTaxRate', 1, true, false, null, '税率不能为空'))
        }
        $(document).ready(function () {
            $("#ctl00_contentHolder_ddlType option").each(function (i) {
                var optionValue = this.value;
                switch (optionValue) {
                    case "Category":
                        this.text = "分类";
                        break;
                    case "Brand":
                        this.text = "品牌";
                        break;
                }
            });
            InitValidators();
        });
    </script>
</asp:Content>

