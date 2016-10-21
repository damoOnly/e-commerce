<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCoupon.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddCoupon" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Ddcb" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/DropDownCheckBoxes/Site.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/DropDownCheckBoxes/CustomDDStyles.css" type="text/css" />
    <link href="../../zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="areacolumn clearfix">

        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/06.gif" width="32" height="32" /></em>
                <h1>����Ż�ȯ</h1>
                <span>�����Ż�ȯ��Ϣ</span>
            </div>
            <div class="formitem validator2">
                <ul>

                    <li>
                        <span class="formitemtitle Pw_100">ʹ�÷�ʽ��</span>
                        <input type="radio" name="rdoUseType" value="0" onclick="selectNolimit()" id="rdoAll" runat="server" />ȫ��ȯ
                <input type="radio" name="rdoUseType" value="1" onclick="selectProduct()" id="rdoProduct" runat="server" />��Ʒȯ
                <input type="radio" name="rdoUseType" value="2" onclick="selectProductType()" id="rdoProductType" runat="server" />��Ʒ����
                <input type="radio" name="rdoUseType" value="3" onclick="selectBrand()" id="rdoBrand" runat="server" />Ʒ��
                <input type="radio" name="rdoUseType" value="4" onclick="selectSupplier()" id="rdoSupplier" runat="server" />������
                    </li>
                </ul>
                <ul id="hselectproduct">
                    <li><span class="formitemtitle Pw_100 ">ѡ����Ʒ��</span><span style="cursor: pointer; color: blue; font-size: 14px" onclick="ShowAddDiv();">����ѡ��</span>
                    </li>
                    <li class="binditems">
                        <table width="100%" id="addlist">
                            <tr class="table_title">
                                <th class="td_right td_left" style="width: 15%" scope="col">��Ʒ��</th>
                                <%--  <th class="td_right td_left" scope="col">sku��Ϣ</th>
                                <th class="td_right td_left" scope="col">�۸�</th>--%>
                                <th class="td_left td_right_fff" style="width: 15%" scope="col">����</th>
                                <th></th>
                            </tr>
                        </table>

                    </li>
                </ul>
                <input id="selectProductsinfo" name="selectProductsinfo" type="hidden" />
                <div id="hselectproducttype">
                    <span class="formitemtitle Pw_100">��ѡ����Ʒ����:</span>
                    <div class="zTreeDemoBackground left">
                        <ul id="treeDemo" class="ztree"></ul>
                    </div>
                    <input id="ProductTypelist" name="ProductTypelist" type="hidden" />
                </div>
                <div id="hselectBrand">
                    <span class="formitemtitle Pw_100">��ѡ��Ʒ��:</span>
                    <Ddcb:DropDownCheckBoxes ID="ddcBrand" runat="server" UseButtons="True" UseSelectAllNode="False"
                        AddJQueryReference="True" meta:resourcekey="checkBoxes3Resource1">
                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                        <Texts OkButton="ȷ��" CancelButton="ȡ��" SelectAllNode="ALL" SelectBoxCaption="----��ѡ��-----" />
                    </Ddcb:DropDownCheckBoxes>
                    <br />
                    <br />
                </div>
                <div id="hselectsupplier">
                    <span class="formitemtitle Pw_100">��ѡ�񹩻���:</span>
                    <Ddcb:DropDownCheckBoxes ID="ddcSupplier" class="formselect input100" runat="server" UseButtons="True" UseSelectAllNode="False"
                        AddJQueryReference="True" meta:resourcekey="checkBoxes3Resource1">
                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                        <Texts OkButton="ȷ��" CancelButton="ȡ��" SelectAllNode="ALL" SelectBoxCaption="----��ѡ��-----" />
                    </Ddcb:DropDownCheckBoxes>
                    <br />
                    <br />
                </div>
                <ul>
                    <li>
                        <span class="formitemtitle Pw_100">���ŷ�ʽ��</span>
                        <input type="radio" name="rdoSendType" value="0" onclick="selectMode()" id="rdoManually" runat="server" />�ֶ�����
                        <input type="radio" name="rdoSendType" value="1" onclick="selectOverMoney()" id="rdoOverMoney" runat="server" />�������ȯ
                        <input type="radio" name="rdoSendType" value="2" onclick="selectMode()" id="rdoRegist" runat="server" />��ע��ȯ
                        <input type="radio" name="rdoSendType" value="3" onclick="selectMode()" id="rdoLq" runat="server" />������ȯ
                        <input type="radio" name="rdoSendType" value="4" onclick="selectRefund()" id="rdorefund" runat="server" />�˿���ȯ
                        <input type="radio" name="rdoSendType" value="5" onclick="selectMode()" id="rdZC" runat="server" />ע������
                    </li>
                </ul>
                <ul id="hselectovermoney">
                    <li>
                        <span class="formitemtitle Pw_100">�����붩��������Ľ�</span>
                        <asp:TextBox ID="txtOverMoney" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle Pw_100"><em>*</em>�Ż�ȯ���ƣ�</span>
                        <asp:TextBox ID="txtCouponName" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCouponNameTip">���Ʋ���Ϊ�գ���1��60���ַ�֮��</p>
                    </li>
                    <li id="hselectrefund">
                        <ul>
                            <li>
                                <span class="formitemtitle Pw_100">&nbsp;<em>*</em>�����</span>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtAmountTip">������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��</p>
                            </li>

                            <li><span class="formitemtitle Pw_100"><em>*</em>�ɵֿ۽�</span>
                                <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtDiscountValueTip">�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��</p>
                            </li>
                            <li>
                                <span class="formitemtitle Pw_100">&nbsp;<em>*</em>ʹ�ÿ�ʼ���ڣ�</span>
                                <UI:WebCalendar ID="calendarStartDate" runat="server" CssClass="forminput" />
                            </li>
                            <li>
                                <span class="formitemtitle Pw_100">&nbsp;<em>*</em>ʹ�ý������ڣ�</span>
                                <UI:WebCalendar ID="calendarEndDate" runat="server" CssClass="forminput" />
                            </li>
                        </ul>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100"><em>*</em>�һ�����֣�</span>
                        <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
                        <p id="P1">�һ��������ֻ�������֣�������ڵ���O,0��ʾ���ܶһ�</p>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">&nbsp;<em>*</em>��ȯ��ʼ���ڣ�</span>
                        <UI:WebCalendar ID="outBeginDate" runat="server" CssClass="forminput" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">&nbsp;<em>*</em>��ȯ�������ڣ�</span>
                        <UI:WebCalendar ID="outEndDate" runat="server" CssClass="forminput" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">&nbsp;<em>*</em>��������</span>
                        <asp:TextBox ID="txtTotalQ" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtTotalQTip">����Ϊ��������0��0��ʾ����������</p>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">&nbsp;��Ч�ڣ�</span>
                        <asp:TextBox ID="txtValidity" runat="server" CssClass="forminput"></asp:TextBox><span>&nbsp;��</span>
                        <p id="ctl00_contentHolder_txtValidityTip">��Ч��ֻ�������֣��������O</p>
                    </li>
                    <li><span class="formitemtitle Pw_100">��ע��</span>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="4" Columns="40"></asp:TextBox>
                    </li>
                    <ul>
                        <li>
                            <span class="formitemtitle Pw_100">����״̬��</span>
                            <input type="radio" name="rdoStatus" value="1" id="rdoSEnable" checked="true" runat="server" />����
                            <input type="radio" name="rdoStatus" value="0" id="rdoSDisEnable" runat="server" />����
                        </li>
                    </ul>
                    <asp:Button ID="btnAddCoupons" runat="server" Text="���" OnClientClick="return PageIsValid()&&CollectInfos()&&CollectProductType();" CssClass="submit_DAqueding" />
                </ul>
            </div>

        </div>
    </div>







</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="/admin/js/AddSelectProduct.js"></script>
    <script type="text/javascript" src="../../zTree/js/jquery.ztree.core-3.5.js"></script>
    <script type="text/javascript" src="../../zTree/js/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript">

       <!--
    var setting = {
        check: {
            enable: true
        },
        data: {
            simpleData: {
                enable: true,
                idKey: "id",
                pidKey: "pId",
                rootPid: 0

            }
        }
    };

    var zNodes;
    $.ajax({
        url: "/API/ProductTypeHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetAllProduct" },
        async: false,
        success: function (resultData) {
            zNodes = resultData;
        }
    });



    //$(document).ready(function () {
    //    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
    //setCheck();
    //$("#py").bind("change", setCheck);
    //$("#sy").bind("change", setCheck);
    //$("#pn").bind("change", setCheck);
    //$("#sn").bind("change", setCheck);
    //});
    //-->

    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtTotalQ', 0, 10, false, '-?[0-9]\\d*', '����Ϊ��������0��0��ʾ����������'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtTotalQ', 0, 10000000, '����Ϊ��������0��0��ʾ����������'));
        initValid(new InputValidator('ctl00_contentHolder_txtCouponName', 1, 60, false, null, '�Ż�ȯ�����ƣ���1��60���ַ�֮��'));
        if (this.rdorefund.check = true) {
            initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000.00, '������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
            initValid(new InputValidator('ctl00_contentHolder_txtDiscountValue', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDiscountValue', 0.01, 10000000.00, '�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
        }
        initValid(new InputValidator('ctl00_contentHolder_txtCount', 0, 10, false, '-?[0-9]\\d*', '��������ֻ�������֣�������ڵ�O,0��ʾ������'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 0, 1000, '��������ֻ�������֣�������ڵ�O,С��1000��0��ʾ������'));
    }


    function selectNolimit() {
        $("#hselectproduct").css("display", "none");
        $("#hselectproducttype").css("display", "none");
        $("#hselectBrand").css("display", "none");
        $("#hselectsupplier").css("display", "none");
        $("#ctl00_contentHolder_rdoProduct").attr("checked", false);
        $("#ctl00_contentHolder_rdoProductType").attr("checked", false);
        $("#ctl00_contentHolder_rdoBrand").attr("checked", false);
        $("#ctl00_contentHolder_rdoSupplier").attr("checked", false);
        $("#ctl00_contentHolder_rdoAll").attr("checked", true);
    }

    function selectProduct() {
        $("#hselectproduct").css("display", "");
        $("#hselectproducttype").css("display", "none");
        $("#hselectBrand").css("display", "none");
        $("#hselectsupplier").css("display", "none");
        $("#ctl00_contentHolder_rdoAll").attr("checked", false);
        $("#ctl00_contentHolder_rdoProductType").attr("checked", false);
        $("#ctl00_contentHolder_rdoBrand").attr("checked", false);
        $("#ctl00_contentHolder_rdoSupplier").attr("checked", false);
        $("#ctl00_contentHolder_rdoProduct").attr("checked", true);
    }

    function selectProductType() {
        $("#hselectproduct").css("display", "none");
        $("#hselectproducttype").css("display", "");
        $("#hselectBrand").css("display", "none");
        $("#hselectsupplier").css("display", "none");
        $("#ctl00_contentHolder_rdoAll").attr("checked", false);
        $("#ctl00_contentHolder_rdoProduct").attr("checked", false);
        $("#ctl00_contentHolder_rdoBrand").attr("checked", false);
        $("#ctl00_contentHolder_rdoSupplier").attr("checked", false);
        $("#ctl00_contentHolder_rdoProductType").attr("checked", true);
    }

    function selectBrand() {
        $("#hselectproduct").css("display", "none");
        $("#hselectproducttype").css("display", "none");
        $("#hselectBrand").css("display", "");
        $("#hselectsupplier").css("display", "none");
        $("#ctl00_contentHolder_rdoAll").attr("checked", false);
        $("#ctl00_contentHolder_rdoProduct").attr("checked", false);
        $("#ctl00_contentHolder_rdoProductType").attr("checked", false);
        $("#ctl00_contentHolder_rdoSupplier").attr("checked", false);
        $("#ctl00_contentHolder_rdoBrand").attr("checked", true);
    }

    function selectSupplier() {
        $("#hselectproduct").css("display", "none");
        $("#hselectproducttype").css("display", "none");
        $("#hselectBrand").css("display", "none");
        $("#hselectsupplier").css("display", "");
        $("#ctl00_contentHolder_rdoAll").attr("checked", false);
        $("#ctl00_contentHolder_rdoProduct").attr("checked", false);
        $("#ctl00_contentHolder_rdoProductType").attr("checked", false);
        $("#ctl00_contentHolder_rdoBrand").attr("checked", false);
        $("#ctl00_contentHolder_rdoSupplier").attr("checked", true);
    }

    function selectMode() {
        $("#hselectovermoney").css("display", "none");
        $("#hselectrefund").css("display", "");
    }

    function selectOverMoney() {
        $("#hselectovermoney").css("display", "");
        $("#hselectrefund").css("display", "");
    }
    function selectRefund() {
        $("#hselectovermoney").css("display", "none");
        $("#hselectrefund").css("display", "none");
    }


    $(document).ready(function () {

        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
        if ($('#ctl00_contentHolder_rdoAll').attr("checked") == "checked") {
            selectNolimit();
        }
        else if ($('#ctl00_contentHolder_rdoProduct').attr("checked") == "checked") {
            selectProduct();
            BindProduct();
        }
        else if ($('#ctl00_contentHolder_rdoProductType').attr("checked") == "checked") {
            selectProductType();
            BindProductType();
        }
        else if ($('#ctl00_contentHolder_rdoBrand').attr("checked") == "checked") {
            selectBrand();
        }
        else if ($('#ctl00_contentHolder_rdoSupplier').attr("checked") == "checked") {
            selectSupplier();
        }
        else {

            selectNolimit();
        }

        if ($('#ctl00_contentHolder_rdoOverMoney').attr("checked") == "checked") {
            selectOverMoney();
        } else if 
         ($('#ctl00_contentHolder_rdorefund').attr("checked") == "checked") {
            selectRefund();
        }

        else {
            selectMode();
        }

        InitValidators();


    });

    function filter(node) {
        var parentnode = node.getParentNode();
        return (node.checked && (parentnode == null || (parentnode != null && parentnode.check_Child_State != 2)) && (node.check_Child_State == -1 || node.check_Child_State == 2));
    }
    function CollectProductType() {
        if ($('#ctl00_contentHolder_rdoProductType').attr("checked") == "checked") {
            var inputstr = '';
            var flag = true;
            var treeObj = $.fn.zTree.getZTreeObj("treeDemo");



            var nodes = treeObj.getNodesByFilter(filter);

            if (nodes == null) {
                alert("����ѡ��һ����Ʒ");
                flag = false;

            }

            else {

                for (var i = 0; i < nodes.length; i++) {

                    inputstr += nodes[i].id + ",";
                }
                inputstr = inputstr.substr(0, inputstr.length - 1);
                $("#ProductTypelist").val(inputstr);
            }
            return flag;
        }

    }

    function BindProduct() {
        var couponId = GetQueryString("couponId");

        if (couponId > 0) {
            var productlist;
            $.ajax({
                url: "/API/ProductTypeHandler.ashx?action=GetSelectProduct",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { couponId: couponId },
                async: false,
                success: function (resultData) {
                    productlist = resultData;
                    var tr = '';
                    $listparent = $(document.getElementById("addlist"));
                    for (var i = 0; i < productlist.length; i++) {




                        if ($listparent.find("span[id='" + productlist[i].id + "']").length == 0)
                            tr += String.format("<tr name='appendlist'><td>{0}</td><td style='display:none'>{1}</td><td ><span  id='{1}' style='cursor:pointer;color:blue' onclick='Remove(this)'>ɾ��</span></td></tr>", productlist[i].name, productlist[i].id);
                    };
                    $listparent.append(tr);



                }
            });
        }
    }

    function BindProductType() {
        var couponId = GetQueryString("couponId");

        if (couponId > 0) {
            var productlist;
            $.ajax({
                url: "/API/ProductTypeHandler.ashx?action=GetSelectProductType",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { couponId: couponId },
                async: false,
                success: function (resultData) {
                    productlist = resultData;

                    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    //var nodes = treeObj.getNodes();
                    //var allnodes = nodes.transformToArray();
                    //for (var i = 0; i < allnodes.length; i++) {
                    //    for (var j = 0;j<productlist.length;j++)
                    //    {
                    //        if (allnodes[i].id == productlist[i].id) {
                    //            treeObj.checkNode(allnodes[i], true, true, false);
                    //        }
                    //    }
                    //}
                    for (var j = 0; j < productlist.length; j++) {
                        node = treeObj.getNodeByParam("id", productlist[j].id, null);
                        treeObj.checkNode(node, true, true, false);
                    }




                }
            });
        }
    }

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return (r[2]); return null;
    }

    function formatStringyyyyMMddToyyyy_MM_dd(value) {
        if (value.length == 8) {
            return value.substring(0, 4) + "-" + value.substring(4, 6) + "-" + value.substring(6, 8);
        } else if (value.length == 6) {
            return value.substring(0, 4) + "-" + value.substring(4, 6);
        } else {
            return value;
        }
    }
    </script>
</asp:Content>

