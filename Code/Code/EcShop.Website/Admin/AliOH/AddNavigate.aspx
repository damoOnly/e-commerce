﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddNavigate.aspx.cs" Inherits="EcShop.UI.Web.Admin.AliOH.AddNavigate" %>

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
    <link href="/Utility/icomoon/style.css" rel="stylesheet" type="text/css" />
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>添加图标</h1>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">图标描述：</span>
                        <asp:TextBox ID="txtNavigateDesc" runat="server" Width="600px" CssClass="forminput" />
                    </li>

                    
                    <li><span class="formitemtitle Pw_100">图标选择：</span>
                        <input id="Rd" name="radicon" type="radio" value="" />
                        自定义 </li>
                    <li runat="server" id="liParent" style="display: none"><span class="formitemtitle Pw_100">上传图片：</span> <span id="spanButtonPlaceholder"></span><span id="divFileProgressContainer"></span>
                        <div>图片尺寸需根据模板自行调整</div>
                    </li>
                    <li id="smallpic" style="display: none">
                        <img id="littlepic" runat="server" src="" />
                        <a id="delpic" href='javascript:void(0)' onclick='RemovePic()'>删除</a> </li>
                    <li>
                        <dd class="iconlist">
                            <%--   <asp:Repeater ID="RpIcon" runat="server">
                   <ItemTemplate>
                     <dl>
                       <input id='Rd_<%#Eval("Name")%>' name="radicon" type="radio"  value="<%#Eval("Name")%>"/>
                      <label for='Rd_<%#Eval("Name")%>'><img src="<%=tplpath%><%#Eval("Name")%>"/></label>
                       </dl>
                   </ItemTemplate>
                   </asp:Repeater>--%>
                            <asp:Repeater ID="FontIcon" runat="server">
                                <ItemTemplate>
                                    <dl>
                                        <input id='Rd_<%#Container.DataItem%>' name="radicon" type="radio" value="<%#Container.DataItem%>" />
                                        <label for='Rd_<%#Container.DataItem%>'>
                                            <div class="<%#Container.DataItem%>" style="display: inline; font-size: 3em;">
                                            </div>
                                        </label>
                                    </dl>
                                </ItemTemplate>
                            </asp:Repeater>
                        </dd>
                    </li>
                    
                    <li><span class="formitemtitle Pw_100">跳转至：</span>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="forminput droptype" ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlSubType" name="ddlSubType" runat="server" CssClass="forminput droptype"
                            Style="display: none" ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlThridType" name="ddlThridType" runat="server" CssClass="forminput droptype"
                            Style="display: none" ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:TextBox ID="Tburl" Style="display: none; width: 350px" CssClass="forminput"
                            runat="server" ClientIDMode="Static"></asp:TextBox>
                        <span id="navigateDesc" runat="server" style="display: none;"><a target="_blank"
                            href="http://www.ecdev.cn/bbs/thread-193492-1-1.html">获取导航地址</a></span>
                    </li>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnAddBanner" runat="server" OnClientClick="return GetLoctionUrl();"
                        Text="确 定" CssClass="submit_DAqueding float" OnClick="btnAddBanner_Click" />
                </ul>
                <!--隐藏图片地址-->
                <input id="fmSrc" runat="server" clientidmode="Static" type="hidden" value="" />
                <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/UploadNavigate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindType();
            $("#ddlType").val("Link").trigger("change");
            BindRadioIcon();
        }
        );

        function BindRadioIcon() {
            $("input[name='radicon']").bind("click", function () {
                if ($(this).val() != "") {
                    $("#liParent").hide();
                    $("smallpic").hide();
                    $("#fmSrc").val($(this).val());
                }
                else {
                    $("#liParent").show();
                }
            });
        }
    </script>
    <script src="../js/LocationType.js" type="text/javascript"></script>
</asp:Content>

