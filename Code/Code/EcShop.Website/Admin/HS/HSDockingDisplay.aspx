<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.HSDockingDisplay" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	<div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	   <h1>四单对接</h1>
    </div>
    <div class="datalist">
        <div class="search clearfix">
			<ul>
				<li><span>订单号：</span>
				    <span><asp:TextBox ID="OrderIDSearchText" runat="server" CssClass="forminput" /></span>
				</li>
                <li><span>订单验证状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="OrderIDList" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未开始</asp:ListItem>
                                <asp:ListItem Value="1">2.进行中</asp:ListItem>
                                <asp:ListItem Value="2">3.已完成</asp:ListItem>
                                <asp:ListItem Value="3">4.失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
                <li><span>运单验证状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="LogisticsNoList" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未开始</asp:ListItem>
                                <asp:ListItem Value="1">2.进行中</asp:ListItem>
                                <asp:ListItem Value="2">3.已完成</asp:ListItem>
                                <asp:ListItem Value="3">4.失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
                <li><span>支付单验证状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="PaymentList" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未开始</asp:ListItem>
                                <asp:ListItem Value="1">2.进行中</asp:ListItem>
                                <asp:ListItem Value="2">3.已完成</asp:ListItem>
                                <asp:ListItem Value="3">4.失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
                <li><span>身份证验证状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="payerIdList" runat="server" Width="125" >
                                <asp:ListItem Value="-1">--请选择状态--</asp:ListItem>
                                <asp:ListItem Value="0">1.未开始</asp:ListItem>
                                <asp:ListItem Value="1">2.进行中</asp:ListItem>
                                <asp:ListItem Value="2">3.已完成</asp:ListItem>
                                <asp:ListItem Value="3">4.失败</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                </li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
	    </div>
        <UI:Grid ID="grdHSDocking" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="HS_Docking_ID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderID" runat="server" Text='<%# Eval("OrderId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderId" runat="server" Text='<%# Eval("OrderId") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="报文号" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderPacketsID" runat="server" Text='<%# Eval("OrderPacketsID") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderPacketsID" runat="server" Text='<%# Eval("OrderPacketsID") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="订单验证状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderStatus" runat="server" Text='<%# Eval("OrderStatus") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="订单备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderRemark" runat="server" Text='<%# Eval("OrderRemark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderRemark" runat="server" Text='<%# Eval("OrderRemark") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="运单号" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblLogisticsNo" runat="server" Text='<%# Eval("LogisticsNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLogisticsNo" runat="server" Text='<%# Eval("LogisticsNo") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="报文号" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblLogisticsPacketsID" runat="server" Text='<%# Eval("LogisticsPacketsID") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLogisticsPacketsID" runat="server" Text='<%# Eval("LogisticsPacketsID") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="运单验证状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblLogisticsStatus" runat="server" Text='<%# Eval("LogisticsStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLogisticsStatus" runat="server" Text='<%# Eval("LogisticsStatus") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="运单备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblLogisticsRemark" runat="server" Text='<%# Eval("LogisticsRemark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLogisticsRemark" runat="server" Text='<%# Eval("LogisticsRemark") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="支付单验证状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblPaymentStatus" runat="server" Text='<%# Eval("PaymentStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPaymentStatus" runat="server" Text='<%# Eval("PaymentStatus") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="支付单备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblPaymentRemark" runat="server" Text='<%# Eval("PaymentRemark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPaymentRemark" runat="server" Text='<%# Eval("PaymentRemark") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="申报发送状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblisSendWMS" runat="server" Text='<%# Eval("isSendWMS") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtisSendWMS" runat="server" Text='<%# Eval("isSendWMS") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="申报发送备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblDescWMS" runat="server" Text='<%# Eval("DescWMS") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescWMS" runat="server" Text='<%# Eval("DescWMS") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="身份证验证状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblPayerIdStatus" runat="server" Text='<%# Eval("payerIdStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPayerIdStatus" runat="server" Text='<%# Eval("payerIdStatus") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="身份证验证备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblPayerIdRemark" runat="server" Text='<%# Eval("payerIdRemark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPayerIdRemark" runat="server" Text='<%# Eval("payerIdRemark") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="收件人" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblpayerName" runat="server" Text='<%# Eval("payerName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtpayerName" runat="server" Text='<%# Eval("payerName") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="身份证信息" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblpayerId" runat="server" Text='<%# Eval("payerId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtpayerId" runat="server" Text='<%# Eval("payerId") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="支付编号" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lbltradeNo" runat="server" Text='<%# Eval("tradeNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txttradeNo" runat="server" Text='<%# Eval("tradeNo") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="地址" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("RealAddress") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAddress" runat="server" Text='<%# Eval("RealAddress") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField HeaderText="电话" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblCellPhone" runat="server" Text='<%# Eval("CellPhone") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCellPhone" runat="server" Text='<%# Eval("CellPhone") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="总件数" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblShipmentQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtShipmentQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="净重" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderWeight" runat="server" Text='<%# Eval("OrderWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderWeight" runat="server" Text='<%# Eval("OrderWeight") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="毛重" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderGrossWeight" runat="server" Text='<%# Eval("OrderGrossWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderGrossWeightd" runat="server" Text='<%# Eval("OrderGrossWeight") %>'></asp:TextBox>                            
                    </EditItemTemplate>
                </asp:TemplateField>--%>
<%--                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                    <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbReSend" IsShow="true" runat="server" CommandName="ReSend"  Text="重新发送" /></span>
                         </ItemTemplate>
                </asp:TemplateField>  --%>
            </Columns>
        </UI:Grid>
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
</div>
</asp:Content>