using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Text;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ExpressTemplates)]
	public class AddExpressTemplate : AdminPage
	{
		protected string ems = "";
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				System.Data.DataTable expressTable = ExpressHelper.GetExpressTable();
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (System.Data.DataRow dataRow in expressTable.Rows)
				{
					stringBuilder.AppendFormat("<option value='{0}'>{0}</option>", dataRow["Name"]);
				}
				this.ems = stringBuilder.ToString();
			}
		}
	}
}
