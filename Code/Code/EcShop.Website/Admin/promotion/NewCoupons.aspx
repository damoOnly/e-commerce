<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewCoupons.aspx.cs" Inherits="EcShop.UI.Web.Admin.NewCoupons" %>

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
            <h1>优惠券管理 </h1>
            <span>管理商城新创建的优惠券，您可以给客户发送优惠券，也可以导出后线下发给客户</span>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="AddCoupon.aspx?couponid=0" class="submit_jia">添加优惠券</a>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <UI:Grid ID="grdCoupons" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="CouponId" HeaderStyle-CssClass="table_title" GridLines="None" SortOrderBy="CouponId" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="优惠券名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:SubStringLabel ID="lblCouponName" StrLength="60" StrReplace="..." Field="Name" runat="server"></Hi:SubStringLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="发送方式" SortExpression="SendTypeName" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%#Eval("SendTypeName") %>
                        </ItemTemplate>

                    </asp:TemplateField>


                   <%-- <asp:TemplateField HeaderText="发劵开始日期" SortExpression="OutBeginDate" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                <Hi:FormatedTimeLabel ID="lblOutbegindate" Time='<%#Eval("OutBeginDate")%>' runat="server"></Hi:FormatedTimeLabel>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="发劵结束日期" SortExpression="OutEndDate" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                <Hi:FormatedTimeLabel ID="lblOutenddate" Time='<%#Eval("OutEndDate")%>' runat="server"></Hi:FormatedTimeLabel>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="开始日期" SortExpression="StartTime" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                   <%# checkStartTime(Eval("SendType"))?Eval("StartTime"):""%>
                                  <%-- <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("StartTime")%>' Visible='<%# checkStartTime(Eval("SendType")) %>'  runat="server"></Hi:FormatedTimeLabel> --%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="结束日期" SortExpression="ClosingTime" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 120px;">
                                <%#checkStartTime(Eval("SendType"))?Eval("ClosingTime"):""%>
                                <%-- <Hi:FormatedTimeLabel ID="lblClosingTimes" Time='<%#Eval("ClosingTime")%>' Visible='<%# checkStartTime(Eval("SendType")) %>' runat="server"></Hi:FormatedTimeLabel>--%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="满足金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <div>
                                 <%# checkStartTime(Eval("SendType"))?Eval("Amount"):"" %>
                             </div>
                            <%-- <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("Amount") %>'   Visible='<%# checkStartTime(Eval("SendType")) %>'  runat="server"></Hi:FormatedMoneyLabel>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="可抵扣金额" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div>
                                 <%# checkStartTime(Eval("SendType"))?Eval("DiscountValue"):"" %>
                             </div>
                           <%-- <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("DiscountValue") %>'  Visible='<%# checkStartTime(Eval("SendType")) %>'  runat="server"></Hi:FormatedMoneyLabel>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SentCount" HeaderText="总数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                    <asp:BoundField DataField="useAmount" HeaderText="已使用数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                    <asp:BoundField DataField="NeedPoint" HeaderText="兑换所需积分" HtmlEncode="false" Visible="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                    <%--                            
                               <asp:TemplateField HeaderText="是否查阅" SortExpression="IsRead" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                       <%# int.Parse(Eval("IsRead").ToString()) == 0 ?"未查阅":"已查阅"%>
                                  </ItemTemplate>
                               </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="自助领券" ItemStyle-Width="120" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                            <div>
                                <input type="text" runat="server" value='<%# Globals.FullPath(string.Format("/Handler/ReceivecouponsHandler.ashx?action=ReceiveCoupons&couponid={0}", Eval("CouponId")))%>' visible='<%# IsCouponEnd(Eval("ClosingTime")) && IsBySelf(Eval("SendType")) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="AddUserName" HeaderText="发劵人" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="是否启用" SortExpression="Status" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%# int.Parse(Eval("Status").ToString()) == 1 ?"启用":"禁用"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="350">
                        <ItemTemplate>
                            <span class="submit_bianji">
                                <asp:HyperLink ID="hyperLink1" runat="server" Visible='<%# IsCouponEnd(Eval("ClosingTime")) %>' NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddCoupon.aspx?couponId={0}", Eval("CouponId")))%>' Text="编辑" /></span>
                            <span class="submit_bianji">
                                <Hi:ImageLinkButton ID="lkbDelete" runat="server" Visible='<%# IsDelete(Eval("HaveItem")) %>' CommandName="Delete" Text="删除" OnClientClick="javascript:return confirm('确定要执行改删除操作吗？删除后将不可以恢复')"></Hi:ImageLinkButton>
                            </span>
                            <span class="submit_bianji"><a runat="server" visible='<%# IsSendToMember(Eval("ClosingTime"),Eval("SendType")) %>' href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/SendCouponToUsers.aspx?couponId={0}&SendType={1}", Eval("CouponId"),Eval("SendType")))%>'>发送给会员</a></span>
                            <span class="submit_bianji">
                                <a runat="server" href="javascript:void(0)" visible='<%# IsCouponEnd(Eval("ClosingTime")) %>' title='<%#Eval("couponid") %>' onclick='ExportCouponById($(this).attr("title"))'>导出</a>
                            </span>
                            <span class="submit_bianji">
                                <a href='usedcoupons.aspx?CouponId=<%#Eval("CouponId")%>'>发劵详情</a>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>

            <script type="text/javascript">
                var formtype = "";
                function ExportCouponById(couponid) {
                    $("#ctl00_contentHolder_txtcouponid").val(couponid);
                    formtype = "export";
                    var arrytempstr = null;

                    setArryText("ctl00_contentHolder_txtcouponid", couponid);
                    DialogShow("导出优惠券", "frameexportcoup", "ExportCoupon", "ctl00_contentHolder_btnExport");
                }
                function validatorForm() {
                    switch (formtype) {
                        case "export":
                            var coupId = $("#ctl00_contentHolder_txtcouponid").val().replace(/\s/g, "");
                            if ($("#ctl00_contentHolder_txtcouponid").val().replace(/\s/g, "") == "" || parseInt(coupId) <= 0) {
                                return false;
                            }
                            var couponNum = $("#ctl00_contentHolder_tbcouponNum").val().replace(/\s/g, "");
                            setArryText("ctl00_contentHolder_tbcouponNum", couponNum);
                            break;
                    };
                    return true;
                }

                $(function () {
                    $(".submit_bianji").not(":has(a)").css("display", "none");

                })
            </script>

            <div id="ExportCoupon" style="display: none;">
                <div class="frame-content">
                    <asp:HiddenField ID="txtcouponid" runat="server" />
                    <p><span class="frame-span frame-input90">导出数量：<em>*</em></span><asp:TextBox ID="tbcouponNum" runat="server" /></p>
                </div>
            </div>

        </div>

        <div style="display: none">
            <asp:Button runat="server" ID="btnExport" Text="导出优惠券"
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
