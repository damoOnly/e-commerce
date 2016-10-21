<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.POList" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/pro-list.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/autocompleter.main.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>采购订单管理</h1>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="POAdd.aspx" class="submit_jia100" style="float: left; margin-right: 10px;">新增采购单</a>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>PO号：</span><span>
                        <asp:TextBox ID="txtPONumber" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li><span>PO状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125">
                                <asp:ListItem Value="-1" Selected="True">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">创建订单</asp:ListItem>
                                <asp:ListItem Value="1">招商确认</asp:ListItem>
                                <asp:ListItem Value="2">关务认领</asp:ListItem>
                                <asp:ListItem Value="3">关务申报</asp:ListItem>
                                <asp:ListItem Value="4">申报成功</asp:ListItem>
                                <asp:ListItem Value="5">申报失败</asp:ListItem>
                                <asp:ListItem Value="6">已入库</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>入库申报状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlQPStatus" runat="server" Width="125">
                                <asp:ListItem Value="-1" Selected="True">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">未入库申报</asp:ListItem>
                                <asp:ListItem Value="1">已入库申报</asp:ListItem>
                                <asp:ListItem Value="2">入库申报失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>商检状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlCIStatus" runat="server" Width="125">
                                <asp:ListItem Value="-1" Selected="True">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">未商检入库</asp:ListItem>
                                <asp:ListItem Value="1">已商检入库</asp:ListItem>
                                <asp:ListItem Value="2">商检入库失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span>供应商名称：</span><span>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" />
                        </abbr></li>
                    <li>
                        <span>预计到货时间：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                </ul>
                <ul>
                    <li style="display: none;"><span>海关入区单号：</span><span>
                        <asp:TextBox ID="txtHSInOut" Width="120" runat="server" CssClass="forminput" /></span>
                    </li>
                    <li></li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea">

                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span>
                            <span class="downproduct">
                                <a href="javascript:ExportData()">导出采购单</a>
                            </span>
                            <asp:Button ID="btnExportData" runat="server" Text="导出" Style="display: none;" />

                            <span class="downproduct">

                                <a href="javascript:ExportItem()">导出明细</a>
                            </span>
                            <asp:Button ID="btnExportItem" runat="server" Text="导出" Style="display: none;" />

                            <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminput"/>
                            <span class="downproduct">
                                <a href="javascript:ImportItem()">导入明细</a>
                            </span>
                            
                            <asp:Button runat="server" ID="btnImportItem" Text="导入明细" Style="display: none;" />               
                            

                            <span class="downproduct" >
                                <asp:Button ID="btnExportPdf" runat="server" Text="导出PDF" />
                            </span>
                        </li>
                    </ul>
                </div>
            </div>
            <!--数据列表区域-->
            <asp:Repeater ID="rpPO" runat="server" OnItemCommand="rpPO_ItemCommand" OnItemDataBound="rpt_ItemDataBound">
                <HeaderTemplate>
                    <table border="0" cellspacing="0" width="80%" cellpadding="0">
                        <tr class="table_title">
                            <td>选择</td>
                            <td>PO号</td>
                            <td>单据类型</td>
                            <td>PO状态</td>
                            <td>入库申报状态</td>
                            <td>商检状态</td>
                            <%--<td>供应商编号</td>--%>
                            <td>供应商名称</td>
                            <td>预计到货时间</td>
                            
                            <td>创建人</td>
                            <td>创建时间</td>
                            <td>备注</td>
                            <td>操作</td>
                            <td>PO流程</td>
                            <td>商检流程</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input name="RadioGroup" type="radio" value='<%#Eval("Id") %>' />
                        </td>
                        <td>
                            <%#Eval("PONumber")%>
                        </td>
                        <td>
                            <asp:Label ID="lblPOType" runat="server" Text='<%#Eval("POType")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPOStatus" runat="server" Text='<%#Eval("POStatus")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblQPStatus" runat="server" Text='<%#Eval("QPStatus")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCIStatus" runat="server" Text='<%#Eval("CIStatus")%>'></asp:Label>
                        </td>
                        <%-- <td>
                            <%#Eval("SupplierCode")%>&nbsp;
                        </td>--%>
                        <td>
                            <%#Eval("SupplierName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ExpectedTime").ToString().Length>0?Convert.ToDateTime(Eval("ExpectedTime")).ToString("yyyy-MM-dd"):""%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateUserName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateTime")%>&nbsp;
                        </td>
                        <td>
                            <span class="Name spu_madeof" title="<%#ReplaceStr(Eval("Remark").ToString())%>"></span>
                        </td>
                        <td>
                            <a href='<%# "../../admin/POManage/POAdd.aspx?Id=" + Eval("Id")%> ' class="SmallCommonTextButton">编辑</a>
                            <asp:LinkButton ID="LinkButton1" CommandName="del" CommandArgument='<%#Eval("Id")%>' OnClientClick="javascript:return confirm('确定要执行删除操作吗？删除后将不可以恢复')"
                                runat="server">删除</asp:LinkButton>
                            <a href='<%# "../../admin/POManage/POItemList.aspx?Id=" + Eval("Id")+"&SupplierId="+ Eval("SupplierId")%> ' class="SmallCommonTextButton">采购明细</a>
                        </td>

                        <td>
                            <input id="hid_<%#Eval("Id")%>" value="<%#Eval("POStatus")%>" QPStatus="<%#Eval("QPStatus")%>" CIStatus="<%#Eval("CIStatus")%>" type="hidden" class="liucheng_type" declareuserid='<%#Eval("DeclareUserId")%>' />
                            <a href="javascript:void(0)" class="button0" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, 0)'>取消确认</a>
                            <a href="javascript:void(0)" class="button1" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, 1)'>确认</a>
                            <a href="javascript:void(0)" class="button2" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, 2)'>认领</a>
                            <a href="javascript:void(0)" class="button3" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, -1)'>退回</a>
                            <a href="javascript:void(0)" class="button4" style="display: none;" onclick='setQPStore(<%#Eval("Id")%>)'>入库申报</a>
                            <a href="javascript:void(0)" class="button6" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, 7)'>发送WMS</a>
                            <%--<a href="javascript:void(0)" class="button6" style="/*display: none;*/" onclick='loadstaut(<%#Eval("Id")%>, 6)'>入库申报失败</a>--%>
                            <%--<a href="javascript:void(0)" class="button7" style="display: none;" onclick='loadstaut(<%#Eval("Id")%>, 6)'>生成PDF</a>--%>
                            <a href='<%# "../../admin/POManage/PODeclareAdd.aspx?Id=" + Eval("Id")%> ' style="display: none;" class="DeclareTextButton">编辑报关信息</a>
                        </td>
                        <td>
                            <a href="javascript:void(0)" class="button5" style="display: none;" onclick='setPoStore(<%#Eval("Id")%>)'>国检入库</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidUserid" />
    <asp:HiddenField runat="server" ID="hidUserName" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            //成分隐藏点击后显示
            $(".spu_madeof").each(function () {
                var title = $(this).attr("title");
                $(this).click(function () {
                    ShowRemark("查看备注", title);
                });
                if (title.length > 10) {
                    $(this).html(title.substring(0, 10) + '......')
                } else { $(this).html(title) }
            });

            function ShowRemark(titile, content) {
                dialog = art.dialog({
                    id: "set",
                    title: titile,
                    content: content,
                    resize: false,
                    fixed: true,
                    height: 180,
                    width: 400,
                    modal: true, //蒙层（弹出会影响页面大小） 
                    button: [
                            { name: '关  闭' }
                    ]
                });
            }

            $(".liucheng_type").each(function () {
                var val = $(this).val();
                var QPStatus = $(this).attr("QPStatus");
                var CIStatus = $(this).attr("CIStatus");
                if (val == 0) {//创建
                    $(this).parent().find(".button1").show();
                } else if (val == 1 || val == 6) {//确认、申报失败
                    $(this).parent().find(".button0,.button2").show();
                } else if (val == 2) {//认领
                    $(this).parent().find(".button3,.DeclareTextButton").show();
                } else if (val == 4) {//入库申报
                    $(this).parent().find(".button4").show();
                } else if (val == 5 && CIStatus==0) {//国检申报
                    $(this).parent().next().find(".button5").show();
                } else if (val == 5 && QPStatus == 1 && CIStatus == 1)
                {
                    $(this).parent().find(".button6").show();
                }
            });
        });

        function loadstaut(Id, flag) {
            if (flag == 4) {
                //不是自己认领的单不能申报
                if ($("#hid_" + Id).attr('DeclareUserId') != $("#<%=hidUserid.ClientID%>").val()) {
                    alert("不是自己单，不允许操作");
                    return;
                }
            }
            //if (flag == 4) {
            //    if (!confirm("确认申报成功！")) {
            //        return;
            //    }
            //}
            UpdateStatus(Id, flag, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
        }

        

        function UpdateStatus(Id, flag, Remark, Userid, UserName) {
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=GetPO_Status",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "Id": Id, 'Status': flag, "Remark": Remark, "Userid": Userid, "UserName": UserName },
                success: function (data) {
                    if (data != null && data.success == "NO") {
                        alert(data.MSG);
                    } else {
                        location = location;
                    }
                },
                error: function () {
                    alert("提交失败！");
                }
            });
        }
        function UpdateCIStatus(Id, flag, Remark, Userid, UserName) {
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=GetCI_Status",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "Id": Id, 'Status': flag, "Remark": Remark, "Userid": Userid, "UserName": UserName },
                success: function (data) {
                    if (data != null && data.success == "NO") {
                        alert(data.MSG);
                    } else {
                        location = location;
                    }
                },
                error: function () {
                    alert("提交失败！");
                }
            });
        }
        function ExportData() {
            if ($("input[type='radio']:checked").length > 0) {
                $("#<%=btnExportData.ClientID%>").trigger("click");
            }
            else {
                alert("请选择要导出数据");
            }
        }

        function setQPStore(id) {
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=PODeclareRequest",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "Id": id },
                success: function (data) {
                    var str = "";//external.goodsim是报关那边的接口，到时换成申报的接口
                    if (!external.goodsim) {
                        alert("请使用云报关系统");
                    }
                    //str = external.goodsim(JSON.stringify(data), 0);
                    //str='{"bgd":{"goods":[{"amount":"20.00000","amount1":"2.26400","country_name":"\u7f8e\u56fd","currency_name":"\u4eba\u6c11\u5e01","goods_name":"\u539f\u7c92\u8170\u679c","goods_no":"2008199990","goods_spec":"\u8170\u679c,\u82b1\u751f\u6cb9\u548c\u6d77\u76d0|\u5e38\u6e29\u5e72\u71e5\u4fdd\u5b58|\u70d8\u5e72|1.13kg/\u7f50|KIRKLAND\u724c","lj":[{"ljno":"LJ1600027","SkuId":"10255_0"},{"ljno":"LJ1600026","SkuId":"10254_0"}],"num_no":"1","totalamount":"2560.00000","unit_name":"\u5343\u514b","unit_name1":"\u5343\u514b"}],"header":{"formId":"60034I100164499824"}},"d":1,"formId":"60034I100164499824","str":""}';
                    //alert(str);
                    //alert(JSON.stringify(data));
                    var result = JSON.parse(str);
                    var json = JSON.stringify(result.bgd);

                    if (result.d == '1') {
                        $.ajax({
                            url: "/Handler/HelpHandler.ashx?action=PODeclareResponse",
                            type: "post",
                            datatype: "text/json",
                            cache: false,
                            data: { "json": json, "ID": id },
                            success: function () {
                                UpdateStatus(id, 5, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                                alert("申报成功");
                                
                            },
                            error: function (data) {
                                UpdateStatus(id, 6, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                                alert(json)
                            }
                        })
                    }
                    else {
                        UpdateStatus(Id, 6, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                        alert("申报失败")
                        
                    }
                }, error: function () {
                    UpdateStatus(Id, 6, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                    alert("提交失败");
                }
            })
        };

        function setPoStore(Id) {
            $.ajax({
                url: "/Handler/HelpHandler.ashx?action=POCommodityInspectionInfo",
                type: "post",
                datatype: "text/json",
                cache: false,
                data: { "ID": Id },
                success: function (data) {
                    var str = "";
                    if (!external.sjgoodsim) {
                        alert("请使用云报关系统");
                    }
                    //alert(JSON.stringify(data));
                    str = external.sjgoodsim(JSON.stringify(data));
                    var result = JSON.parse(str);
                    //alert(str);
                    //if (result.d == '1') {
                    if ('1' == '1') {
                        UpdateCIStatus(Id, 1, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                        alert("申报成功");
                    }
                    else {
                        UpdateCIStatus(Id, 2, "", $("#<%=hidUserid.ClientID%>").val(), $("#<%=hidUserName.ClientID%>").val());
                        alert("申报失败");                        
                    }
                }

            })
        };

        function ExportItem() {
            if ($("input[type='radio']:checked").length > 0) {
                $("#<%=btnExportItem.ClientID%>").trigger("click");
            }
            else {
                alert("请选择要导出数据");
            }
        }

        function ImportItem() {
            if ($("input[type='radio']:checked").length > 0) {
                $("#<%=btnImportItem.ClientID%>").trigger("click");
            }
            else {
                alert("请选择要导入数据");
            }
        }
    </script>

</asp:Content>
