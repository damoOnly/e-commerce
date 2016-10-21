using EcShop.Core.Enums;
using EcShop.Entities.VShop;
using EcShop.SaleSystem.Vshop;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_PrizeUsers : WebControl
	{
		public LotteryActivityInfo Activity
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Activity != null)
			{
				IOrderedEnumerable<PrizeRecordInfo> orderedEnumerable = 
					from a in VshopBrowser.GetPrizeList(new PrizeQuery
					{
						ActivityId = this.Activity.ActivityId,
						SortOrder = SortAction.Desc,
						SortBy = "PrizeTime"
					})
					orderby a.PrizeTime descending
					select a;
				StringBuilder stringBuilder = new StringBuilder();
				if (orderedEnumerable != null && orderedEnumerable.Count<PrizeRecordInfo>() > 0)
				{
					foreach (PrizeRecordInfo current in orderedEnumerable)
					{
						if (!string.IsNullOrEmpty(current.CellPhone) && !string.IsNullOrEmpty(current.RealName))
						{
							stringBuilder.AppendFormat("<p>{0}&nbsp;&nbsp;{1} &nbsp;&nbsp;{2}</p>", current.Prizelevel, this.ShowCellPhone(current.CellPhone), current.RealName);
						}
					}
					writer.Write(stringBuilder.ToString());
					return;
				}
				stringBuilder.AppendFormat("<p>暂无获奖名单！</p>", new object[0]);
			}
		}
		private string ShowCellPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				return Regex.Replace(phone, "(?im)(\\d{3})(\\d{4})(\\d{4})", "$1****$3");
			}
			return "";
		}
	}
}
