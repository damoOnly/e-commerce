using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class AliOHSlideImage
	{
		public string ImageUrl
		{
			get;
			set;
		}
		public string LoctionUrl
		{
			get;
			set;
		}
		public AliOHSlideImage(string imageUrl, string locationUrl)
		{
			this.ImageUrl = imageUrl;
			this.LoctionUrl = locationUrl;
		}
	}
}
