<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ShareLine.aspx.cs" Inherits="EcShop.UI.Web.Admin.ShareLine" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>
                分享页管理</h1>
            <span>浏览并管理您制作的所有分享页</span>
        </div>
        <div class="datalist">
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><ui:pagesize runat="server" id="hrefPageSize" /></li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <ui:pager runat="server" showtotalpages="false" id="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> <span class="delete">
                                    <hi:imagelinkbutton id="btnDelete" runat="server" text="删除" isshow="true" deletemsg="确定要批量删除分享页？" />
                                </span></li>
                    </ul>
                </div>
            </div>
            <!--数据列表区域-->
            <table cellspacing="0" border="0" id="ctl00_contentHolder_grdProducts" style="width: 100%;
                border-collapse: collapse;">
                <tr class="table_title">
                    <th class="td_right td_left" scope="col">
                        选择
                    </th>
                    <th class="td_right td_left" scope="col">
                        标题
                    </th>
                    <th class="td_right td_left" scope="col">
                        时间
                    </th>
                    <th class=" td_left td_right_fff" scope="col">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rp_shareproduct" runat="server">
                    <itemtemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ShareId") %>' />
                            </td>
                            <td><%# Eval("ShareTitle")%></td>
                            <td><%# Eval("ShareTime","{0:d}") %></td>
                            <td>
                                <span class="submit_bianji">
                                <a href="javascript:void(0)" onclick="ShowShareLink('<%# Globals.HostPath(HttpContext.Current.Request.Url)+"/wapshop/ShareProducts.aspx?Id="+Eval("ShareId") %>','<%# Globals.HostPath(HttpContext.Current.Request.Url)+ Eval("ShareUrl") %>')">预览</a>
                                <a href="javascript:void(0)" onclick="UpdateShareTitle('<%#Eval("ShareId") %>','<%#Eval("ShareTitle") %>','<%#Eval("ShareUrl") %>')">编辑标题</a>
                                 <a href="EditeShareDetails.aspx?shareId=<%# Eval("ShareId") %>">相关商品</a>
                                    <asp:LinkButton ID="btnAdd" runat="server" Text="删除" CommandArgument='<%# Eval("ShareId") %>'
                                        CommandName="delete"></asp:LinkButton>
                                </span>
                            </td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <ui:pager runat="server" showtotalpages="true" id="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
  
    <div id="divShareProduct" style="display: none">
        <div class="frame-content">
            <p>
                <span id="SpanShareId"></span></p>
              
             <table style="width:300px; height:340px;">
            <tr><td><img id="imgsrc" src="" type="img" width="300px" /></td></tr>
            </table>
        </div>
    </div>
    <div id="divexpresscomputers" style="display: none;">
        <div class="frame-content">
            <input type="hidden" runat="server" id="shareId" />
            <input type="hidden" runat="server" id="shareUrl" />
            <p>
                <span class="frame-span frame-input90">标题名称：</span>
                <asp:TextBox id="txtshareTitle" CssClass="forminput" runat="server">
                </asp:TextBox></p>
        </div>
    </div>
    <div style="display: none">
        <asp:Button ID="btnUpdateValue" runat="server" Text="确 定" CssClass="submit_sure" /></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function ShowShareLink(ShareId, ShareUrl) {
 

            $("#imgsrc").attr("src", ShareUrl);
            $("#SpanShareId").html(ShareId);
            ShowMessageDialog("我要分享", 'sharedetails', 'divShareProduct');
        }

        function UpdateShareTitle(ShareId, ShareTitle, ShareUrl) {
            arrytext = null;
            setArryText('ctl00_contentHolder_shareId', ShareId);
            setArryText('ctl00_contentHolder_txtshareTitle', ShareTitle);
            setArryText('ctl00_contentHolder_shareUrl', ShareUrl);
            DialogShow("编辑标题", 'shareeditetitle', 'divexpresscomputers', 'ctl00_contentHolder_btnUpdateValue')
        }

        function validatorForm() {
            var ShareId = $("#ctl00_contentHolder_shareId").val().replace(/\s/g, "");
            var ShareTitle = $("#ctl00_contentHolder_txtshareTitle").val().replace(/\s/g, "");
            var ShareUrl = $("#ctl00_contentHolder_shareUrl").val().replace(/\s/g, "");
            if (ShareId == "") {
                alert("分享ID不能为空");
                return false;
            }
            if (ShareTitle == "") {
                alert("标题不能为空");
                return false;
            }
            if (ShareUrl == "") {
                alert("图片不能为空");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
