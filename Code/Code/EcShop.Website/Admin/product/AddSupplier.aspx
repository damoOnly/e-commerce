<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSupplier.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddSupplier" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript" language="javascript">

        function PageIsValid() {
            var supplierName = $.trim($("#ctl00_contentHolder_txtSupplierName").val());
            if (supplierName == "") {
                alert("供货商名称不能为空");
                return false;
            }
            var supplierOwnerName = $.trim($("#ctl00_contentHolder_txtSupplierOwnerName").val());
            if (supplierOwnerName == "") {
                alert("店主名称不能为空");
                return false;
            }
            var shopname = $.trim($("#ctl00_contentHolder_txtShopName").val());
            if (shopname == "") {
                alert("店铺名称不能为空");
                return false;
            }

            var supplierCode = $.trim($("#<%:txtSupplierCode.ClientID%>").val());
            if (supplierCode == "") {
                alert("供货商编码不能为空");
                return false;
            }

            var address = $.trim($("#ctl00_contentHolder_txtAddress").val());
            if (address == "") {
                alert("详细地址不能为空");
                return false;
            }

            var phone = $.trim($("#ctl00_contentHolder_txtPhone").val());
            var regphone = /^\d{1,12}-?\d{1,8}-?\d{1,8}$/; //^(\d{3,4}\-)?[1-9]\d{6,13}$/;
            if (phone != "") {
                if (!regphone.test(phone)) {
                    alert("电话号码格式不正确");
                    return false;
                }
            }

            //var mobile = $.trim($("#ctl00_contentHolder_txtMobile").val());
            //var regmobile = /^1[3|4|5|7|8][0-9]\d{4,8}$/;
            //if (mobile != "") {
            //    if (!regmobile.test(mobile)) {
            //        alert("手机号码格式不正确");
            //        return false;
            //    }
            //}
            var email = $.trim($("#ctl00_contentHolder_txtEmail").val());
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (email.length <= 0 || filter.test(email)) return true;
            else {
                alert('邮箱格式不正确');
                return false;
            }
            return true;
        }

    </script>

    <script type="text/javascript">
        var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
    </script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/SupplierUploadHandler.js" type="text/javascript"></script>
    <script src="../js/UploadSupplier.js" type="text/javascript"></script>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title clearfix">
                <em>
                    <img src="../images/05.gif" width="32" height="32" /></em>
                <h1>添加供货商</h1>
            </div>
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>供货商名称：</span>
                        <asp:TextBox ID="txtSupplierName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSupplierNameTip">供货商名称不能为空，长度限制在30个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>店主名称：</span>
                        <asp:TextBox ID="txtSupplierOwnerName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSupplierOwnerNameTip">店主名称不能为空，长度限制在30个字符以内</p>

                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>店铺名称：</span>
                        <asp:TextBox ID="txtShopName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtShopNameTip">店铺名称不能为空，长度限制在30个字符以内</p>

                    </li>

                    <li runat="server" id="liParent1"><span class="formitemtitle Pw_198">供货商Logo：</span>
                        <span id="spanButtonPlaceholder1"></span>
                        <span id="divFileProgressContainer1"></span>
                        <div>图片建议尺寸：650px * 320px</div>
                    </li>
                    <li id="smallpic1" style="display: none;">
                        <img id="littlepic1" runat="server" src="" />
                        <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                    </li>


                    <li runat="server" id="liParent2"><span class="formitemtitle Pw_198">PC端图片：</span>
                        <span id="spanButtonPlaceholder2"></span>
                        <span id="divFileProgressContainer2"></span>
                        <div>图片建议尺寸：650px * 320px</div>
                    </li>
                    <li id="smallpic2" style="display: none;">
                        <img id="littlepic2" runat="server" src="" />
                        <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                    </li>



                    <li runat="server" id="liParent3"><span class="formitemtitle Pw_198">手机端图片：</span>
                        <span id="spanButtonPlaceholder3"></span>
                        <span id="divFileProgressContainer3"></span>
                        <div>图片建议尺寸：650px * 320px</div>
                    </li>
                    <li id="smallpic3" style="display: none;">
                        <img id="littlepic3" runat="server" src="" />
                        <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                    </li>




                    <li><span class="formitemtitle Pw_198"><em>*</em>供货商编码：</span>
                        <asp:TextBox ID="txtSupplierCode" MaxLength="10" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSupplierCodeTip">供货商编码不能为空，长度限制在10个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">仓库名称：</span>
                        <asp:TextBox ID="txtWarehouseName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p>长度限制在30个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">地区：</span>
                        <abbr class="formselect">
                            <Hi:RegionSelector runat="server" ID="ddlReggion" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>详细地址：</span>
                        <asp:TextBox ID="txtAddress" MaxLength="50" CssClass="forminput" runat="server" Width="300px" />
                        <p id="ctl00_contentHolder_txtAddressTip">供货商地址不能为空，长度限制在30个字符以内</p>
                    </li>

                    <li><span class="formitemtitle Pw_198">联系人：</span>
                        <asp:TextBox ID="txtContact" MaxLength="50" CssClass="forminput" runat="server" />
                        <p>长度限制在50个字符以内</p>
                    </li>

                    <li><span class="formitemtitle Pw_198">邮箱：</span>
                        <asp:TextBox ID="txtEmail" MaxLength="256" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtEmailTip">长度限制在256个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">传真：</span>
                        <asp:TextBox ID="txtFax" MaxLength="50" CssClass="forminput" runat="server" />
                        <p>长度限制在50个字符以内</p>
                    </li>


                    <li><span class="formitemtitle Pw_198">手机号码：</span>
                        <asp:TextBox ID="txtMobile" MaxLength="15" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtMobileTip">手机号码的长度限制在15个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">联系电话：</span>
                        <asp:TextBox ID="txtPhone" MaxLength="15" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtPhoneTip">电话号码的长度限制在15个字符以内</p>
                    </li>

                    <li><span class="formitemtitle Pw_198">商品品类：</span>
                        <asp:TextBox ID="txtCategory" MaxLength="500" CssClass="forminput" runat="server" />
                    </li>
                    <li><span class="formitemtitle Pw_198">审核开关：</span>
                        <asp:CheckBox ID="ckbApproveKey" runat="server" />
                        <p id="ctl00_contentHolder_ckbApproveKey">勾上,供货商添加的商品将需要审核</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">简介：</span>
                        <%--<asp:TextBox ID="txtDescription" MaxLength="50" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtDescriptionTip">备注长度限制在50个字符以内</p>--%>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtDescription"
                            TextMode="MultiLine" />
                    </li>
                    <li>
                        <h2 class="colorE">银行信息</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">收款方名称：</span>
                        <asp:TextBox ID="txtBeneficiaryName" MaxLength="50" CssClass="forminput" runat="server" />
                        <p>长度限制在50个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">Swift Code：</span>
                        <asp:TextBox ID="txtSwiftCode" MaxLength="50" CssClass="forminput" runat="server" Width="250" />
                        <p>长度限制在50个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">银行卡号：</span>
                        <asp:TextBox ID="txtBankAccount" MaxLength="50" CssClass="forminput" runat="server" Width="250" />
                        <p>长度限制在50个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">银行名称：</span>
                        <asp:TextBox ID="txtBankName" MaxLength="100" CssClass="forminput" runat="server" Width="250" />
                        <p>长度限制在100个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">银行地址：</span>
                        <asp:TextBox ID="txtBankAddress" MaxLength="200" CssClass="forminput" runat="server" Width="250" />
                        <p>长度限制在200个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">银行代码：</span>
                        <asp:TextBox ID="txtIBAN" MaxLength="200" CssClass="forminput" runat="server" Width="250" />
                        <p>长度限制在200个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">备注：</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="TtxtRemark"
                            TextMode="MultiLine" />
                    </li>
                </ul>
                <ul class="btntf Pa_100 clear">
                    <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server" />
                </ul>

                <input id="fmSrc1" runat="server" clientidmode="Static" type="hidden" value="" />

                <input id="fmSrc2" runat="server" clientidmode="Static" type="hidden" value="" />

                <input id="fmSrc3" runat="server" clientidmode="Static" type="hidden" value="" />
            </div>

        </div>
    </div>
</asp:Content>

