using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class SlideImage
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

        public SlideImage(string imageUrl, string locationUrl)
		{
			this.ImageUrl = imageUrl;
			this.LoctionUrl = locationUrl;
		}
	}
}
