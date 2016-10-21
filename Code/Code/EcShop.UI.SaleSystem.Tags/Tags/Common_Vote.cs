using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_Vote : ThemedTemplatedRepeater
	{
		private DataList dlstVoteItems;
		private DataList datalist1;
		private DataSet voteds;
		protected override void OnLoad(EventArgs e)
		{
			this.voteds = CommentBrowser.GetVoteByIsShow();
			if (this.voteds != null && this.voteds.Tables.Count >= 2 && this.voteds.Tables[0] != null && this.voteds.Tables[1] != null && this.voteds.Tables[0].Rows.Count > 0)
			{
				this.voteds.Tables[0].Columns.Add("Vote", typeof(DataView));
				this.voteds.Tables[0].Rows[0]["Vote"] = this.voteds.Tables[1].DefaultView;
			}
			base.DataSource = this.voteds;
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (base.Items.Count == 0)
			{
				return;
			}
			base.Render(writer);
			string text = string.Empty;
			text += "<script language=\"jscript\" type=\"text/javascript\">";
			text += "function setcheckbox(checkbox){";
			text += "var group = document.getElementsByName(checkbox.name);";
			text += "var voteValue = document.getElementById(checkbox.name + '_Value');";
			text += "var maxVote = parseInt(document.getElementById(checkbox.name + '_MaxVote').value);";
			text += "voteValue.value =''; var n = 0;";
			text += "for (index = 0;index < group.length;index ++){";
			text += "if (group[index].checked){n++; voteValue.value += group[index].value + ',';}}";
			text += "if (n > maxVote){var msg='";
			text += "最多能投票：";
			text += "'; alert(msg + maxVote); checkbox.checked = false; setcheckbox(checkbox);}}";
			text += "function voteOption(voteId, voteItemId) {";
			text += "window.document.location.href = applicationPath + \"/VoteResult.aspx?VoteId=\" + voteId + \"&&VoteItemId=\" + voteItemId;";
			text += "}";
			text += "</script>";
			writer.Write(text);
		}
		public DataView GetSource()
		{
			return this.voteds.Tables[1].DefaultView;
		}
	}
}
