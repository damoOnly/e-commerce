<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AppRunImg.aspx.cs" Inherits="EcShop.UI.Web.Admin.AppRunImg" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script src="../js/ajaxfileupload.js?a=1" type="text/javascript"></script>
    <style type="text/css">
        .mytable {
            border: solid 1px #808080;
        }

            .mytable td {
                border: solid 1px #808080;
                width: 200px;
            }

            .mytable .ImgTr td {
                height: 360px;
            }

        .tdSpan {
        }

            .tdSpan span {
                width: 98px;
                height: 40px;
                border: solid 1px #808080;
                text-align: center;
                vertical-align: middle;
                display: block;
                float: left;
                cursor: pointer;
            }

        .td_button button {
            width: 70px;
            height: 25px;
            border: solid 1px #808080;
            text-align: center;
            vertical-align: middle;
            display: block;
            float: left;
            cursor: pointer;
            margin-right: 20px;
        }

        .td_text td {
            text-align: center;
        }

        .button {
            width: 70px;
            height: 25px;
            border: solid 1px #808080;
            text-align: center;
            vertical-align: middle;
            display: block;
            float: left;
            cursor: pointer;
            margin-right: 20px;
            margin-top: 5px;
            margin-bottom: 5px;
        }

        .asp_btn {
            text-align: center;
            vertical-align: middle;
            display: block;
            float: left;
            cursor: pointer;
            background: none;
            margin: 0px auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div>
        <div style="display: none">
            <form id="ajaxForm" method="post" action="/Handler/MemberHandler.ashx">
                <input type="file" id="fileUpload" name="fileUpload" onchange="ajaxFileUpload();" />
                <input type="hidden" id="hidImgType" />
            </form>
        </div>
        <h4>启动图</h4>
        <table class="mytable">
            <thead>
                <tr class="td_text">
                    <td>Android：720*1280</td>
                    <td>iOS：750*1334</td>
                    <td>iOS：640*960</td>
                    <td>iOS：1242*2208</td>
                </tr>
            </thead>
            <tbody>
                <tr class="ImgTr" id="ImgTr">
                    <td><a href="javascript:void()" target="_blank">
                        <asp:HiddenField ID="Android720_1280hid" runat="server" />
                        <asp:Image ID="Android720_1280" runat="server" Width="100%" Height="100%" /></a></td>
                    <td><a href="javascript:void()" target="_blank">
                        <asp:HiddenField ID="iOS750_1334hid" runat="server" />
                        <asp:Image ID="iOS750_1334" runat="server" Width="100%" Height="100%" /></a></td>
                    <td><a href="javascript:void()" target="_blank">
                        <asp:HiddenField ID="iOS640_960hid" runat="server" />
                        <asp:Image ID="iOS640_960" runat="server" Width="100%" Height="100%" /></a></td>
                    <td><a href="javascript:void()" target="_blank">
                        <asp:HiddenField ID="iOS1242_2208hid" runat="server" />
                        <asp:Image ID="iOS1242_2208" runat="server" Width="100%" Height="100%" /></a></td>
                </tr>
                <tr class="tdSpan">
                    <%--<td><span onclick="itemClick(this,0)">删除图片</span></td>
                    <td><span onclick="itemClick(this,1)">修改图片</span><span onclick="itemClick(this,1)">删除图片</span></td>
                    <td><span onclick="itemClick(this,2)">修改图片</span><span onclick="itemClick(this,2)">删除图片</span></td>
                    <td><span onclick="itemClick(this,3)">修改图片</span><span onclick="itemClick(this,3)">删除图片</span></td>--%>
                    <td><span onclick="itemClick(this,0)">修改图片</span><span style="display: none;">
                        <asp:FileUpload ID="fileUpload1" runat="server" Width="62px" CssClass="asp_btn" /></span><span onclick="itemClick(this,0)">删除图片</span></td>
                    <td><span onclick="itemClick(this,1)">修改图片</span><span style="display: none;">
                        <asp:FileUpload ID="fileUpload2" runat="server" Width="62px" CssClass="asp_btn" /></span><span onclick="itemClick(this,1)">删除图片</span></td>
                    <td><span onclick="itemClick(this,2)">修改图片</span><span style="display: none;">
                        <asp:FileUpload ID="fileUpload3" runat="server" Width="62px" CssClass="asp_btn" /></span><span onclick="itemClick(this,2)">删除图片</span></td>
                    <td><span onclick="itemClick(this,3)">修改图片</span><span style="display: none;">
                        <asp:FileUpload ID="fileUpload4" runat="server" Width="62px" CssClass="asp_btn" /></span><span onclick="itemClick(this,3)">删除图片</span></td>
                </tr>
            </tbody>
            <tfoot>
                <tr class="td_button">
                    <td colspan="10">
                        <asp:Button Text="保存" ID="btnSave" runat="server" CssClass="button" />
                        <asp:Button Text="取消" ID="btnCancel" runat="server" CssClass="button" />
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            var temop = $("#ImgTr img");
            $(temop).each(function (index, item) {
                var href = $(item).attr("src");
                $(item).parent().attr("href", href);
            });
        });
        var id = "";
        function itemClick(item, index) {

            var text = $(item).html();
            if (index == 0) {
                $("#hidImgType").val("Android720_1280");
                id = "ctl00_contentHolder_Android720_1280";
            }
            else if (index == 1) {
                $("#hidImgType").val("iOS750_1334");
                id = "ctl00_contentHolder_iOS750_1334";
            }
            else if (index == 2) {
                $("#hidImgType").val("iOS640_960");
                id = "ctl00_contentHolder_iOS640_960";
            }
            else if (index == 3) {
                $("#hidImgType").val("iOS1242_2208");
                id = "ctl00_contentHolder_iOS1242_2208";
            }

            if (text == "修改图片") {
                $("#ctl00_contentHolder_fileUpload"+(index+1)).trigger("click");
                return;
            }
            else if (text == "删除图片") {
                var imgName = $("#" + id).attr("src");
                $.ajax({
                    url: "/Handler/MemberHandler.ashx?action=RemoveAppImg",
                    type: "get",
                    datatype: "text/json",
                    cache: false,
                    data: { imageName: imgName, columnIndex: index },
                    success: function (data) {
                        var temp = data;// eval("(" + data + ")");
                        if (temp.success == "SUCCESS") {
                            $("#" + id).attr("src", "");
                            $("#" + id + "hid").val("");
                            $("#" + id).parent().attr("href", "javascript:void()");
                        }
                        else {
                            alert("图片删除失败！" + data.MSG);
                        }
                    },
                    error: function (a, b, c) {

                    }
                });

            }
        }
        function ajaxFileUpload() {
            $.ajaxFileUpload
            (
                {
                    url: '/Handler/MemberHandler.ashx?action=UpLoadAppImg&hidImgType=' + $("#hidImgType").val(),
                    secureuri: false,
                    fileElementId: 'fileUpload',
                    dataType: 'text/json',//返回值类型 一般设置为json
                    //jsonp: 'jsoncallback',
                    //jsonpCallback:
                    //function success_jsonpCallback(data) {
                    //    // alert("1");
                    //},
                    cache: false,
                    success: function (data, success) {
                        //alert(data);
                        //var temp = eval("(" + data + ")");
                        data = data.replace("<pre style=\"word-wrap: break-word; white-space: pre-wrap;\">", "").replace("</pre>", "");
                        var temp = eval("(" + data + ")");
                        if (temp.success == "SUCCESS") {
                            var path = "/Storage/master/app/" + temp.name + "." + temp.suffix;//+ "." + temp.suffix;
                            $("#" + id).attr("src", path);
                            $("#" + id + "hid").val(path);
                            $("#" + id).parent().attr("href", path);
                        }
                        else {
                            alert("图片上传失败！" + data.MSG);
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                }
            );
        }
    </script>
</asp:Content>
