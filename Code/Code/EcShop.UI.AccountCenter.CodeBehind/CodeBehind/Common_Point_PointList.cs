using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Point_PointList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Point_PointList";
		private System.Web.UI.WebControls.Repeater repeaterPointDetails;
		public event System.Web.UI.WebControls.RepeaterItemEventHandler ItemDataBound;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterPointDetails.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterPointDetails.DataSource = value;
			}
		}
		public Common_Point_PointList()
		{
			base.ID = "Common_Point_PointList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserPointList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterPointDetails = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterPointDetails");
			this.repeaterPointDetails.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.repeaterPointDetails_ItemDataBound);
		}
		private void repeaterPointDetails_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterPointDetails.DataSource != null)
			{
				this.repeaterPointDetails.DataBind();
			}
		}
	}
}
