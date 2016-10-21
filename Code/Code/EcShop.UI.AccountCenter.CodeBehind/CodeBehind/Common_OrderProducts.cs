using EcShop.Core.Enums;
using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_OrderProducts : AscxTemplatedWebControl
	{
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
		
        public Common_OrderProducts()
		{
            base.ID = "Common_OrderProducts";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderProducts.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			
		}
	}
}
