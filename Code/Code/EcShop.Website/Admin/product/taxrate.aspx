<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TaxRate.aspx.cs" Inherits="EcShop.UI.Web.Admin.TaxRate" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>行邮编码管理</h1>
  <span></span></div>
  	
		<!-- 添加按钮-->
   <div class="btn"><a href="javascript:void(0)" onclick="ShowTags('add',null,this)" class="submit_jia">添加行邮编码</a></div>
   <div class="batchHandleArea" style="margin-left:10px;">
		<ul>
			<li class="batchHandleButton">
			</li>
		</ul>
   </div>

<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
		<div class="searcharea clearfix br_search">
            <ul>
                 <li><span>关键字：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li style=" margin-top:3px;">
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
            </ul>
		</div>
	<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
		<tr class="table_title">
           <th scope="col" class="td_left td_right_fff">行邮编码</th>
           <th scope="col" class="td_left td_right_fff">税率</th>
           <th scope="col" class="td_left td_right_fff">行邮编码名称</th>
           <th class="td_left td_right_fff" scope="col">操作</th>
		</tr>
		<asp:Repeater ID="rp_prducttag" runat="server">
		<ItemTemplate>
		<tr>
            <td><%# Eval("PersonalPostalArticlesCode") %></td>
            <td><%# Eval("TaxRate") %></td>
            <td><%# Eval("CodeDescription") %></td>
			<td>
			    <span class="submit_shanchu">
			    <a href="javascript:void(0)" onclick="ShowTags('update',<%# Eval("TaxId") %>,this)">编辑</a>
			    <asp:LinkButton ID="btndelete" runat="server" CommandName="delete" CommandArgument='<%# Eval("TaxId") %>' OnClientClick="return confirm('确认删除税率？');"  Text="删除"></asp:LinkButton>
			    </span>
             </td>
		</tr>
		</ItemTemplate>
		</asp:Repeater>
		
		</table>
	</div>
</div>


<%--修改税率值--%>
<div  id="updatetag_div" style="display:none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;行邮编码：</span><asp:TextBox ID="txtcode" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;税率：</span><asp:TextBox ID="txttagname" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;行邮名称：</span><asp:TextBox ID="txtCodeDescription" CssClass="forminput" runat="server" MaxLength="200"/>  </p>
    </div>
</div>

<%--添加税率值--%>
<div id="addtag_div" style="display:none">
    <div class="frame-content">
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;行邮编码：</span> <asp:TextBox ID="txtaddcode" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;税率：</span> <asp:TextBox ID="txtaddtagname" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;行邮名称：</span><asp:TextBox ID="txtAddCodeDescription" CssClass="forminput" runat="server" MaxLength="200"/>  </p>
    </div>
</div>
<div style="display:none">
<input type="hidden" id="hdtagId" runat="server" />
<asp:Button ID="btnaddtag" runat="server" Text="添加税率"  CssClass="submit_DAqueding" />
 <asp:Button ID="btnupdatetag" runat="server" Text="修改税率"  CssClass="submit_DAqueding"/>
</div>
<script type="text/javascript" language="javascript">
    var formtype = "";
    function ShowTags(oper, tagId, link_obj) {
        arrytext = null;
        if (oper == "add") {
            formtype = "add";
            setArryText('ctl00_contentHolder_txtaddtagname', "");
            setArryText('ctl00_contentHolder_txtaddcode', "");
            setArryText('ctl00_contentHolder_txtAddCodeDescription', "");
            DialogShow("添加税率", "addtag", "addtag_div", "ctl00_contentHolder_btnaddtag");
        } else {
            formtype = "edite";
            var tagName = $(link_obj).parents("tr").find("td").eq(1).text();
            var tagCode = $(link_obj).parents("tr").find("td").eq(0).text();
            var txtCodeDescription = $(link_obj).parents("tr").find("td").eq(2).text();
            setArryText('ctl00_contentHolder_hdtagId', tagId);
            setArryText('ctl00_contentHolder_txttagname', tagName);
            setArryText('ctl00_contentHolder_txtcode', tagCode);
            setArryText('ctl00_contentHolder_txtCodeDescription', txtCodeDescription);
            DialogShow("编辑税率", "editetag", "updatetag_div", "ctl00_contentHolder_btnupdatetag");
        }

    }

    function validatorForm() {
        var zz = /^\d+\.\d+$/;
        switch (formtype) {
            case "add":
                if ($("#ctl00_contentHolder_txtaddtagname").val().replace(/\s/g, "") == "") {
                    alert("请输入税率");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtaddcode").val().replace(/\s/g, "") == "") {
                    alert("请输入行邮编码");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtaddcode").val().length > 8) {
                    alert("行邮编码不能超过八个字符");
                    return false;
                }
                if (!zz.test($("#ctl00_contentHolder_txtaddtagname").val())) {
                    alert("税率必须为正小数");
                    return false;
                }
                
                break;
            case "edite":
                if ($("#ctl00_contentHolder_txttagname").val().replace(/\s/g, "") == "") {
                    alert("请输入税率");
                    return false;
                }
                if (!zz.test($("#ctl00_contentHolder_txttagname").val())) {
                    alert("税率必须为正小数");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtcode").val().replace(/\s/g, "") == "") {
                    alert("请输入行邮编码");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtcode").val().length > 8) {
                    alert("行邮编码不能超过八个字符");
                    return false;
                }
                if ($("#ctl00_contentHolder_hdtagId").val().replace(/\s/g, "") == "") {
                    alert("请选择要修改的税率！");
                    return false;
                }
                break;
        };
        return true;
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
       
    <style type="text/css">
        .auto-style1 {
            width: 356px;
        }
        .auto-style2 {
            width: 260px;
        }
    </style>
       
</asp:Content>
