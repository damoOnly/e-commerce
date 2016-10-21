using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class SkuValue : AdminPage
	{
		private int attributeId;
		private int valueId;
		protected System.Web.UI.HtmlControls.HtmlGenericControl valueStr;
		protected System.Web.UI.WebControls.TextBox txtValueStr;
		protected System.Web.UI.HtmlControls.HtmlGenericControl valueImage;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.TextBox txtValueDec;
		protected System.Web.UI.HtmlControls.HtmlInputHidden currentAttributeId;
		protected System.Web.UI.WebControls.Button btnCreateValue;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"].ToString().Trim()))
				{
					base.GotoResourceNotFound();
					return;
				}
				string a = this.Page.Request.QueryString["action"].ToString().Trim();
				if (a == "add")
				{
					if (!int.TryParse(this.Page.Request.QueryString["attributeId"], out this.attributeId))
					{
						base.GotoResourceNotFound();
						return;
					}
				}
				else
				{
					if (!int.TryParse(this.Page.Request.QueryString["valueId"], out this.valueId))
					{
						base.GotoResourceNotFound();
						return;
					}
					AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
					this.attributeId = attributeValueInfo.AttributeId;
					this.txtValueDec.Text = (this.txtValueStr.Text = Globals.HtmlDecode(attributeValueInfo.ValueStr));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["useImg"].ToString()) && this.Page.Request.QueryString["useImg"].ToString().Equals("True"))
				{
					this.txtValueStr.Text = "";
					this.valueStr.Visible = false;
					this.valueImage.Visible = true;
				}
				this.currentAttributeId.Value = this.attributeId.ToString();
			}
			this.btnCreateValue.Click += new System.EventHandler(this.btnCreateValue_Click);
		}
		protected void btnCreateValue_Click(object sender, System.EventArgs e)
		{
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			System.Collections.Generic.IList<AttributeValueInfo> list = new System.Collections.Generic.List<AttributeValueInfo>();
			int num = int.Parse(this.currentAttributeId.Value);
			attributeValueInfo.AttributeId = num;
			string a = this.Page.Request.QueryString["action"].ToString().Trim();
			if (a == "add")
			{
				if (!string.IsNullOrEmpty(this.txtValueStr.Text.Trim()))
				{
					string text = this.txtValueStr.Text.Trim();
					text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(text)).Replace("，", ",").Replace("\\", "").Replace("/", "");
					string[] array = text.Split(new char[]
					{
						','
					});
					int num2 = 0;
					while (num2 < array.Length && array[num2].Trim().Length <= 100)
					{
						AttributeValueInfo attributeValueInfo2 = new AttributeValueInfo();
						if (array[num2].Trim().Length > 15)
						{
							this.ShowMsg("属性值限制在15个字符以内", false);
							return;
						}
						attributeValueInfo2.ValueStr = Globals.HtmlEncode(array[num2].Trim());
						attributeValueInfo2.AttributeId = num;
						list.Add(attributeValueInfo2);
						num2++;
					}
					foreach (AttributeValueInfo current in list)
					{
						ProductTypeHelper.AddAttributeValue(current);
					}
					this.CloseWindow();
				}
				if (!this.fileUpload.HasFile)
				{
					this.ShowMsg("属性值限制在15个字符以内", false);
					return;
				}
				try
				{
					attributeValueInfo.ImageUrl = ProductTypeHelper.UploadSKUImage(this.fileUpload.PostedFile);
					attributeValueInfo.ValueStr = Globals.HtmlEncode(this.txtValueDec.Text);
				}
				catch
				{
				}
				if (ProductTypeHelper.AddAttributeValue(attributeValueInfo) > 0)
				{
					this.CloseWindow();
					return;
				}
			}
			else
			{
				this.valueId = int.Parse(this.Page.Request.QueryString["valueId"]);
				attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
				AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeValueInfo.AttributeId);
				if (attribute.UseAttributeImage)
				{
					if (!string.IsNullOrEmpty(this.txtValueDec.Text))
					{
						attributeValueInfo.ValueStr = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtValueDec.Text)).Replace("，", ",").Replace("\\", "").Replace("/", "");
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(this.txtValueStr.Text))
					{
						attributeValueInfo.ValueStr = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtValueStr.Text)).Replace("，", ",").Replace("\\", "").Replace("/", "");
					}
				}
				if (this.fileUpload.HasFile)
				{
					try
					{
						StoreHelper.DeleteImage(attributeValueInfo.ImageUrl);
						attributeValueInfo.ImageUrl = ProductTypeHelper.UploadSKUImage(this.fileUpload.PostedFile);
					}
					catch
					{
					}
				}
				if (ProductTypeHelper.UpdateAttributeValue(attributeValueInfo))
				{
					this.CloseWindow();
				}
			}
		}
	}
}
