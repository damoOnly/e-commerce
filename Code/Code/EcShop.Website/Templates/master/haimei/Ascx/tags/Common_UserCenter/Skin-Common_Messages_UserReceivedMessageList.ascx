<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
        
  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1 top_border">
      <tr id="spqingdan_title">
        <td width="7%" align="center">选择</td>
        <td width="11%" align="center">状态</td>
        <td width="37%" align="center">标题</td>
        <td width="14%" align="center">发件人</td>
        <td width="16%" align="center">时间</td>
        <td width="15%" align="center">操作</td>
      </tr>     
        <asp:Repeater ID="repeaterMessageList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                       <input type="checkbox" name="CheckBoxGroup" value='<%# Eval("MessageId") %>'/>
                    </td>
                    <td align="center">
                       <%#Convert.ToInt32(Eval("IsRead")) == 0 ? "<image src=\'/Templates/master/default/images/users/shouyj_03.jpg\' title='未读'  />" : "<image src=\'/Templates/master/default/images/users/shouyj_06.jpg\'  title='已读' />"%>
                        </td>
                    <td align="center">
                        <%# Eval("Title")%>
                    </td>
                    <td align="center">
                        管理员
                    </td>
                    <td align="center">
                        <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("Date")%>' runat="server" />
                    </td>
                    <td align="center">
                        <a href="javascript:void(0);"onclick="javascript:ReplayMessages(<%# Eval("MessageId")%>,this)" class="SmallCommonTextButton">查看回复</a>         
                        <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("MessageId") %>' CommandName="Delete" Text="删除"></asp:LinkButton>
                    </td>
                </tr> 
            </ItemTemplate>
</asp:Repeater>
</table>
<script type="text/javascript">
    function ReplayMessages(mid, obj) {
        DialogFrame("ReplyReceivedMessage.aspx?MessageId=" + mid, "查看站内消息", 508, 620);
        $(obj).parents('tr:first').children(':eq(1)').find('img').attr({ 'src': '/Templates/master/default/images/users/shouyj_06.jpg', 'title': '已读' });
    }
</script>