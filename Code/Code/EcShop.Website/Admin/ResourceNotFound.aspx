﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ResourceNotFound.aspx.cs" Inherits="EcShop.UI.Web.Admin.ResourceNotFound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="datalist">
		<div class="allerror mainwidth">
			<h2>系统处理错误，可能是：</h2>
			<div>
				1、您登录已经超时。<br />
				2、URL非法。<br />
				3、重复的操作，或者当前操作不可用。<br />
				<a href="http://bbs.ecdev.cn" target="_blank">以上都不是？请到B2C商城官方网站或交流社区寻求帮助！</a>
			</div>
		</div>
	  </div>

	  <div class="blank12 clearfix"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>