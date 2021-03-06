using EcShop.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class Common_DesignScriptCss : Literal
	{
		private string css = "blue.css";
		private string srcFormat = string.Concat(new string[]
		{
			"<script src=\"",
			Globals.ApplicationPath,
			"/utility/jquery-1.6.4.min.js\" type=\"text/javascript\"></script> <script src=\"",
			Globals.ApplicationPath,
			"/utility/jquery.artDialog.js\" type=\"text/javascript\"></script><script src=\"",
			Globals.ApplicationPath,
			"/dialogtemplates/js/EcShop_Dialog.js\" type=\"text/javascript\"></script><script src=\"",
			Globals.ApplicationPath,
			"/utility/jquery_hashtable.js\" type=\"text/javascript\"></script><script src=\"",
			Globals.ApplicationPath,
			"/dialogtemplates/js/EcShop_Design.js\" type=\"text/javascript\"></script><script src=\"",
             Globals.ApplicationPath,
            "http://wpa.b.qq.com/cgi/wpa.php\" type=\"text/javascript\"></script>",
            "<link rel=\"stylesheet\" href=\"",
			Globals.ApplicationPath,
			"/dialogtemplates/css/design.css\" type=\"text/css\"/><link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\"/>",
           
		});
		public virtual string Css
		{
			get
			{
				if (this.css.StartsWith("/"))
				{
					this.css = Globals.ApplicationPath + this.css;
				}
				else
				{
					this.css = Globals.ApplicationPath + "/utility/skins/" + this.css;
				}
				return this.css;
			}
			set
			{
				this.css = value;
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Css))
			{
				writer.Write(this.srcFormat, this.Css);
			}
		}
	}
}
