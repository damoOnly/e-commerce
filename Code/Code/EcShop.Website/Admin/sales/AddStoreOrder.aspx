<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddStoreOrder.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.AddStoreOrder" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css?v=20150819" type="text/css" media="screen" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>添加门店订单</h1>
        </div>
        <div>
         
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <h2 class="colorE">基本信息</h2>
                    </li>
                    <li class="clearfix">
                        <a class="submit_jia"id="productsSeldect" href="javascript:void(0)">选择商品规格</a>
                        <a  href='javascript:void(0)'id='DellAll'>全部删除</a>
                    </li>
                    <li>
                   <!--数据列表区域-->
                   <div class="datalist datalist-img" style="border:none">
	                   <table cellspacing="0" border="0" class="backAddOrder_tb">
                           <thead>
                            <tr>
                               <th width="30"><input type="checkbox" id="chkAll" checked="checked"/></th>
                               <th width="80">商品图片</th>
                               <th width="350">商品名称</th>
                               <th width="150">商品单价</th>
                               <th width="120">购买数量</th>
                               <th width="150">小计</th>
                               <th width="150">操作</th>
                             </tr>
                            </thead>
                            <tbody id="grdProductSkus">
                               
                           </tbody>
	                   </table>
                    </div>

                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">商品总金额：</span>
                        <!-- 每件商品的金额 = 单价 *购买数量 -->
                        <!-- 商品总金额 = 每件商品的金额 + 税费 +运费 - 优惠抵扣 -->
                        <asp:Label runat="server" ID="lblProductTotalPrice"></asp:Label>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">运费金额：</span>
                        <asp:Label runat="server" ID="lblToalFreight"></asp:Label>
                    </li>  
                    <!-- 税费+=商品数量*商品税率*商品金额  如果总税额<= 50，那么免税-->
                    <li class="clearfix"><span class="formitemtitle Pw_198">税额：</span>
                        <asp:Label runat="server" ID="lblTotalTax"></asp:Label>
                    </li> 
                    <li class="clearfix"><span class="formitemtitle Pw_198">优惠抵扣：</span>
                         <!-- 优惠抵扣 =  -->
                        <asp:TextBox ID="txtDeductible" runat="server" CssClass="forminput"></asp:TextBox>
                         <p id="ctl00_contentHolder_txtDeductibleTip">
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">配送方式：</span>
                        <Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode"/>
                        <p id="ctl00_contentHolder_ddlshippingModeTip">
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">支付方式：</span>
                        <Hi:PaymentDropDownList runat="server" ID="ddlpayment" />
                        <p id="ctl00_contentHolder_ddlpaymentTip">
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">订单备注：</span>
                        <asp:TextBox ID="txtBak" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtBakTip">
                        </p>
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">物流信息</h2>
                    </li>
                    <li class="clearfix">
                        <a class="submit_jia"id="memberSelect" href="javascript:void(0)">选择收货地址</a>
                        <asp:TextBox ID="txtShippingId" runat="server" CssClass="forminput"  Style="display:none;"></asp:TextBox>
                        <asp:TextBox ID="txtUserId" runat="server" CssClass="forminput"  Style="display:none;"></asp:TextBox>
                        <asp:TextBox ID="txtRegionId" runat="server" CssClass="forminput"  Style="display:none;"></asp:TextBox>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"  Style="display:none;"></asp:TextBox>
                    </li>
                   
                    <li class="clearfix"><span class="formitemtitle Pw_198"><em>*</em>收货人姓名：</span>
                        <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtShipToTip">
                           
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198"><em>*</em>收货人地址：</span>
                        <Hi:RegionSelector runat="server" id="dropRegions" />
                        <p id="ctl00_contentHolder_dropRegionsTip">
                            
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198"><em>*</em>收货人详细地址：</span>
                        <asp:TextBox ID="txtDetailsAddress" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtDetailsAddressTip">
                            
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">邮政编码：</span>
                        <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtZipcodeTip">
                            
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">电话号码：</span>
                        <asp:TextBox ID="txtTelPhone" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtTelPhoneTip">
                            
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198"><em>*</em>手机号码：</span>
                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCellPhoneTip">
                            
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">身份证号码：</span>
                        <asp:TextBox ID="txtIdentityCard" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtIdentityCardTip">
                            
                        </p>
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <asp:HiddenField ID="hiddenSkus" runat="server"></asp:HiddenField>
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();"
                        CssClass="submit_DAqueding inbnt" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script src="/Utility/addStoreOrder.js?v=20150820" type="text/javascript"></script>
</asp:Content>
