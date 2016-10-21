<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RolePermissions.aspx.cs" Inherits="EcShop.UI.Web.Admin.RolePermissions" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

<script>
	
    $(function(){
		//var roleId = UrlParm.parm('roleid');
		var roleid_str = [];
		var roleid_str = $('#aspnetForm').attr('action').split('=');
		var roleid = roleid_str[1];
		var url = '/API/AuthValid.ashx?action=GetAllMenuByRoleId&status=1&menuId=1010&roleId=' + roleid;
		var str = '';
		$.ajax({
			url: url,
			type: 'get',
			dataType: 'text',
		
			success:function(data){
				var jsonData = eval("("+data+")");
				for(var i=0;i<jsonData.length;i++){
					str += '<div class="list-content"><p class="group-list"><span>' + jsonData[i].PrivilegeName + '</span></p>';
					if(jsonData[i].SubMenuItem){
						for(var j=0;j<jsonData[i].SubMenuItem.length;j++){
							str += '<p class="list-detail">';
							str += '<a>' + jsonData[i].SubMenuItem[j].PrivilegeName + ' ： </a>';
							if(jsonData[i].SubMenuItem[j].SubMenuItem){
								for(var k=0;k<jsonData[i].SubMenuItem[j].SubMenuItem.length;k++){
									str += '<span ischecked="' + jsonData[i].SubMenuItem[j].SubMenuItem[k].IsChecked +'"newPrivilegeId='+ jsonData[i].SubMenuItem[j].SubMenuItem[k].newPrivilegeId + '>' + jsonData[i].SubMenuItem[j].SubMenuItem[k].PrivilegeName + '</span>';
									if(jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem){
										str += '<span class="small-list-box">--(';
										for(var l =0;l<jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem.length;l++){
											str += '<span class="small-list" ischecked="' + jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].IsChecked +'"newPrivilegeId='+ jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].newPrivilegeId + '>' + jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].PrivilegeName + '</span>';
										}
										/*if(jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].SubMenuItem){
											for(var m =0;l<jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].SubMenuItem.length;m++){
												str += '<span class="small-list" ischecked="' + jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].SubMenuItem[m].IsChecked +'"privilegeid='+ jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].SubMenuItem[m].PrivilegeId + '>' + jsonData[i].SubMenuItem[j].SubMenuItem[k].SubMenuItem[l].SubMenuItem[m].PrivilegeName + '</span>';
											}
										}*/
										str += ')</span>';
									}
								}
							}
							str += '</p>';
						}
						
					}
					str += '</div>';
				}
				
				$('#grdGroupList').append(str);
				$('#grdGroupList').find('[ischecked="1"]').addClass('checked');
				$('.small-list-box').prev('span').click(function(){
					if($(this).hasClass('checked')){
						$(this).next('.small-list-box').find('span').removeClass('checked');
					}
					else{
						$(this).next('.small-list-box').find('span').addClass('checked');
					}
				});
			},
			error:function(){
				//alert('error');
			}
		});
    })
	
	//全部选定
	function check_all(){
		$('#grdGroupList').find('span').addClass('checked');
	}
	//全不选定
	function check_none(){
		$('#grdGroupList').find('span').removeClass('checked');
	}
	
	//点击多选框
	$(function(){
		$('#grdGroupList').delegate('.list-content span','click',function(){
			$(this).toggleClass('checked');
		});
		$('#grdGroupList').delegate('.group-list span','click',function(){
			if($(this).hasClass('checked')){
				$(this).parent('p').siblings('p').find('span').addClass('checked');
			}
			else{
				$(this).parent('p').siblings('p').find('span').removeClass('checked');
			}
		});
	})
	
	//保存网后台传数据
	function save_data(){
		var privilegeid = [];
		$('.small-list-box').removeClass('checked');
		$('.list-detail span').each(function(){
			if($(this).hasClass('checked')){
				privilegeid.push($(this).attr('newprivilegeid'));
			}
		});
		if (privilegeid.length !== 0) {
		   
		    $('#ctl00_contentHolder_newprivilegeid').val(privilegeid);
		  
		}
	}
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField  ID="hf_menuValues" runat="server"/>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>设置部门权限</h1>
            <span>根据不同的部门设置不同的权限，方便控制商城的管理 </span>
        </div>
        <div class="datalist">
            <table width="100%" height="30px" border="0" cellspacing="0">
                <tr class="table_title">
                    <td width="9%" class="td_left"><strong>当前部门：</strong></td>
                    <td align="left" class="td_right"><span style="font-weight: 800;"><strong>
                        <asp:Literal runat="server" ID="lblRoleName"></asp:Literal></strong></span></td>
                </tr>

            </table>
            
           
            <asp:HiddenField id="newprivilegeid" value="" runat="server" />

            <div style="margin-left: 15px; margin-top: 20px; overflow:hidden">
            	<!--<a href="javascript:save_id();" class="save-id">保存</a>-->
                <span class="submit_btnquanxuan">
                    <asp:LinkButton ID="btnSetTop" runat="server" Text="保存" OnClientClick="save_data();" /></span>
                <span class="submit_btnchexiao">
                	<a href="Roles.aspx">返回</a>
                </span>
                <a href="javascript:check_all();" style="margin-left:20px;">全部选定</a>
                <a href="javascript:check_none();" style="margin-left:20px;">全不选定</a>
            </div>
            
            <div id="grdGroupList" class="grdGroupList clear" style="padding-left:10px; padding-right:10px;margin-top:5px">
            	
            </div>
         
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <Hi:Script runat="server" Src="/admin/js/PrivilegeInRoles.js" />

    <script type="text/javascript" language="javascript">
        function link() {
            window.location.href = 'Roles.aspx';
        }
    </script>
</asp:Content>
