<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewVouchers.aspx.cs" Inherits="EcShop.UI.Web.Admin.NewVouchers" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="blank12 clearfix"></div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/06.gif" width="32" height="32" /></em>
            <h1>现金券管理 </h1>
            <span>管理商城新创建的现金券，您可以给客户发送现金券，也可以导出后线下发给客户</span>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="AddVoucher.aspx?voucherId=0" class="submit_jia">添加现金券</a>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <UI:Grid ID="grdVouchers" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="VoucherId" HeaderStyle-CssClass="table_title" GridLines="None" SortOrderBy="VoucherId" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="现金券名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:SubStringLabel ID="lblVoucherName" StrLength="60" StrReplace="..." Field="Name" runat="server"></Hi:SubStringLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="开始日期" SortExpression="StartTime" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("StartTime")%>' runat="server"></Hi:FormatedTimeLabel>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="结束日期" SortExpression="ClosingTime" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                <Hi:FormatedTimeLabel ID="lblClosingTimes" Time='<%#Eval("ClosingTime")%>' runat="server"></Hi:FormatedTimeLabel>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="满足金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("Amount") %>' runat="server"></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="现金券金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("DiscountValue") %>' runat="server"></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SentCount" HeaderText="总数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                    <asp:BoundField DataField="useAmount" HeaderText="已使用数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>

                    <%-- <asp:TemplateField HeaderText="是否查阅" SortExpression="IsRead" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                       <%# int.Parse(Eval("IsRead").ToString()) == 0 ?"未查阅":"已查阅"%>
                                  </ItemTemplate>
                               </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="自助领券" ItemStyle-Width="120" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                            <div>
                                <input type="text" runat="server" value='<%# Globals.FullPath(string.Format("/Handler/ReceivecouponsHandler.ashx?action=ReceiveVouchers&voucherid={0}", Eval("VoucherId")))%>' visible='<%# IsVoucherEnd(Eval("ClosingTime")) && IsBySelf(Eval("SendType")) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="350">
                        <ItemTemplate>
                            <span class="submit_bianji">
                                <asp:HyperLink ID="hyperLink1" runat="server" Visible='<%# IsVoucherEnd(Eval("ClosingTime")) %>' NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddVoucher.aspx?voucherId={0}", Eval("VoucherId")))%>' Text="编辑" />
                            </span>
                            <span class="submit_bianji">
                                <Hi:ImageLinkButton ID="lkbDelete" runat="server" CommandName="Delete" Text="删除" OnClientClick="javascript:return confirm('确定要执行改删除操作吗？删除后将不可以恢复')"></Hi:ImageLinkButton>
                            </span>
                            <span class="submit_bianji"><a runat="server" visible='<%# IsVoucherEnd(Eval("ClosingTime")) %>' href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/SendVoucherToUsers.aspx?voucherId={0}", Eval("VoucherId")))%>'>发送给会员</a></span>
                            <span class="submit_bianji">
                                <a runat="server" href="javascript:void(0)" visible='<%# IsVoucherEnd(Eval("ClosingTime")) %>' title='<%#Eval("VoucherId") %>' onclick='ExportVoucherById($(this).attr("title"))'>导出</a>
                            </span>
                            <span class="submit_bianji">
                                <a href='usedvouchers.aspx?VoucherId=<%#Eval("VoucherId")%>'>查看活动</a>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>


           

            <script type="text/javascript">
                function ClearPwdtype()
                {
                    $("#ctl00_contentHolder_hiddenPwdtype").val(1);
                }

                var formtype = "";
                function ExportVoucherById(voucherid) {
                    $("#ctl00_contentHolder_txtvoucherid").val(voucherid);
                    formtype = "export";
                    var arrytempstr = null;

                    setArryText("ctl00_contentHolder_txtvoucherid", voucherid);
                    DialogShow("导出现金券", "frameexportvoucher", "ExportVoucher", "ctl00_contentHolder_btnExport", ClearPwdtype);
                }

                function validatorForm() {
                    switch (formtype) {
                        case "export":
                            var voucherId = $("#ctl00_contentHolder_txtvoucherid").val().replace(/\s/g, "");
                            if ($("#ctl00_contentHolder_txtvoucherid").val().replace(/\s/g, "") == "" || parseInt(voucherId) <= 0) {
                                return false;
                            }
                            var voucherNum = $("#ctl00_contentHolder_tbvoucherNum").val().replace(/\s/g, "");
                            setArryText("ctl00_contentHolder_tbvoucherNum", voucherNum);
                            break;
                    };
                    return true;
                }


                $(function () {
                    $(".submit_bianji").not(":has(a)").css("display", "none");

                    function SetPwdType()
                    {
                        var val = $("input[name=ctl00$contentHolder$pwdtype]:checked").val();
                        var hiddenPwdtype = $("#ctl00_contentHolder_hiddenPwdtype");
                        if (val == "radComplex") {
                            hiddenPwdtype.val(1);
                        } else if (val == "radSimple") {
                            hiddenPwdtype.val(2);
                        } else {
                            hiddenPwdtype.val(1);
                        }
                    }

                    SetPwdType();


                    $(document).delegate("input[name=ctl00$contentHolder$pwdtype]", "change", function () {
                        SetPwdType();
                    })
                    
                })
            </script>
              <asp:HiddenField ID="hiddenPwdtype" runat="server"/>

            <div id="ExportVoucher" style="display: none;">
                <div class="frame-content">
                    <asp:HiddenField ID="txtvoucherid" runat="server" />
                   
                    <p><em>*</em><span class="frame-span frame-input90">导出数量：</span><asp:TextBox ID="tbvoucherNum" runat="server" /></p>
                    <p><span class="frame-span frame-input90">密码类型：</span>
                        <asp:RadioButton ID="radComplex" runat="server" Text="复杂密码" Checked="true" GroupName="pwdtype" CssClass="pwdtype"/>
                        <asp:RadioButton ID="radSimple" runat="server" Text="简单密码" GroupName="pwdtype" CssClass="pwdtype"/>
                    </p>
                </div>
            </div>

        </div>

        <div style="display: none">
            <asp:Button runat="server" ID="btnExport" Text="导出现金券"
                CssClass="submit_DAqueding inbnt" OnClick="btnExport_Click" />
        </div>

        <div class="blank5 clearfix"></div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
        <!--数据列表底部功能区域-->
    </div>
</asp:Content>
