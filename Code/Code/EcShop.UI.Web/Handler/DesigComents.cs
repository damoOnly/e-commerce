using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Comments;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
namespace EcShop.UI.Web.Handler
{
	public class DesigComents : System.Web.IHttpHandler
	{
		private string message = "";
		private string modeId = "";
		private string elementId = "";
		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string key;
				switch (key = this.modeId)
				{
				case "commentarticleview":
				{
					string arg = this.GetMainArticleCategories();
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "commentCategory":
				{
					string arg = this.GetCategorys();
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "editecommentarticle":
				{
					string value = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value))
					{
						JObject articleobject = (JObject)JsonConvert.DeserializeObject(value);
						if (this.CheckCommentArticle(articleobject) && this.UpdateCommentArticle(articleobject))
						{
							var value2 = new
							{
								ComArticle = new Common_SubjectArticle
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value2));
						}
					}
					break;
				}
				case "editecommentcategory":
				{
					string value3 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value3))
					{
						JObject categoryobject = (JObject)JsonConvert.DeserializeObject(value3);
						if (this.CheckCommentCategory(categoryobject) && this.UpdateCommentCategory(categoryobject))
						{
							var value4 = new
							{
								ComCategory = new Common_SubjectCategory
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value4));
						}
					}
					break;
				}
				case "editecommentbrand":
				{
					string value5 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value5))
					{
						JObject jObject = (JObject)JsonConvert.DeserializeObject(value5);
						if (this.CheckCommentBrand(jObject) && this.UpdateCommentBrand(jObject))
						{
							var value6 = new
							{
								ComBrand = new Common_SubjectBrand
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value6));
						}
					}
					break;
				}
				case "editecommentkeyword":
				{
					string value7 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value7))
					{
						JObject keywordobject = (JObject)JsonConvert.DeserializeObject(value7);
						if (this.CheckCommentKeyWord(keywordobject) && this.UpdateCommentKeyWord(keywordobject))
						{
							var value8 = new
							{
								ComCategory = new Common_SubjectKeyword
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value8));
						}
					}
					break;
				}
				case "editecommentattribute":
				{
					string value9 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value9))
					{
						JObject attributeobject = (JObject)JsonConvert.DeserializeObject(value9);
						if (this.CheckCommentAttribute(attributeobject) && this.UpdateCommentAttribute(attributeobject))
						{
							var value10 = new
							{
								ComAttribute = new Common_SubjectAttribute
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value10));
						}
					}
					break;
				}
				case "editecommenttitle":
				{
					string value11 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value11))
					{
						JObject titleobject = (JObject)JsonConvert.DeserializeObject(value11);
						if (this.CheckCommentTitle(titleobject) && this.UpdateCommentTitle(titleobject))
						{
							var value12 = new
							{
								ComTitle = new Common_SubjectTitle
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value12));
						}
					}
					break;
				}
				case "editecommentmorelink":
				{
					string value13 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value13))
					{
						JObject morelinkobject = (JObject)JsonConvert.DeserializeObject(value13);
						if (this.CheckCommentMorelink(morelinkobject) && this.UpdateMorelink(morelinkobject))
						{
							var value14 = new
							{
								ComMoreLink = new Common_SubjectMoreLink
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value14));
						}
					}
					break;
				}
				}
			}
			catch (System.Exception ex)
			{
				this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
		public System.Collections.Generic.Dictionary<string, string> GetXmlNodeString(JObject scriptobject)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = scriptobject.ToObject<System.Collections.Generic.Dictionary<string, string>>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
			{
				dictionary2.Add(current.Key, Globals.HtmlEncode(Globals.HtmlEncode(current.Value.ToString())));
			}
			return dictionary2;
		}
		private bool CheckCommentArticle(JObject articleobject)
		{
			if (string.IsNullOrEmpty(articleobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入文字标题!\"");
				return false;
			}
			if (string.IsNullOrEmpty(articleobject["MaxNum"].ToString()) || System.Convert.ToInt32(articleobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt32(articleobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentArticle(JObject articleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改文章标签列表失败\"");
			this.elementId = articleobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			articleobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(articleobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "article", xmlNodeString);
		}
		private bool CheckCommentCategory(JObject categoryobject)
		{
			if (string.IsNullOrEmpty(categoryobject["CategoryId"].ToString()) || System.Convert.ToInt32(categoryobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (string.IsNullOrEmpty(categoryobject["MaxNum"].ToString()) || System.Convert.ToInt32(categoryobject["MaxNum"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentCategory(JObject categoryobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改分类标签列表失败\"");
			this.elementId = categoryobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			categoryobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(categoryobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "category", xmlNodeString);
		}
		private bool CheckCommentAttribute(JObject attributeobject)
		{
			if (string.IsNullOrEmpty(attributeobject["CategoryId"].ToString()) || System.Convert.ToInt32(attributeobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (string.IsNullOrEmpty(attributeobject["MaxNum"].ToString()) || System.Convert.ToInt32(attributeobject["MaxNum"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentAttribute(JObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改属性标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			attributeobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "attribute", xmlNodeString);
		}
		private bool CheckCommentBrand(JObject brandobject)
		{
			if (!string.IsNullOrEmpty(brandobject["CategoryId"].ToString()) && System.Convert.ToInt32(brandobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(brandobject["IsShowLogo"].ToString()) || string.IsNullOrEmpty(brandobject["IsShowTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"参数格式不正确!\"");
				return false;
			}
			if (string.IsNullOrEmpty(brandobject["MaxNum"].ToString()) || System.Convert.ToInt32(brandobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt32(brandobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"显示数量必须为0~100的正整数！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentBrand(JObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			attributeobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "brand", xmlNodeString);
		}
		private bool CheckCommentKeyWord(JObject keywordobject)
		{
			if (!string.IsNullOrEmpty(keywordobject["CategoryId"].ToString()) && System.Convert.ToInt32(keywordobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(keywordobject["MaxNum"].ToString()) || System.Convert.ToInt32(keywordobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt32(keywordobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"显示数量必须为1~100的正整数！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentKeyWord(JObject keywordobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = keywordobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			keywordobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(keywordobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "keyword", xmlNodeString);
		}
		private bool CheckCommentMorelink(JObject morelinkobject)
		{
			if (!string.IsNullOrEmpty(morelinkobject["CategoryId"].ToString()) && System.Convert.ToInt32(morelinkobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(morelinkobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入链接标题！\"");
				return false;
			}
			return true;
		}
		private bool UpdateMorelink(JObject morelinkobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = morelinkobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			morelinkobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(morelinkobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "morelink", xmlNodeString);
		}
		private bool CheckCommentTitle(JObject titleobject)
		{
			if (string.IsNullOrEmpty(titleobject["Title"].ToString()) && string.IsNullOrEmpty(titleobject["ImageTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入标题或上传图片！\"");
				return false;
			}
			return true;
		}
		private bool UpdateCommentTitle(JObject titleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = titleobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			titleobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(titleobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt32(this.elementId), "title", xmlNodeString);
		}
		private string GetCategorys()
		{
			string result = "";
			string[] propertyName = new string[]
			{
				"CategoryId",
				"Name",
				"Depth"
			};
			System.Data.DataTable dataTable = this.ConvertListToDataTable<CategoryInfo>(CatalogHelper.GetSequenceCategories(), propertyName);
			if (dataTable != null)
			{
				result = JsonConvert.SerializeObject(dataTable, new JsonConverter[]
				{
					new ConvertTojson()
				});
			}
			return result;
		}
		private string GetMainArticleCategories()
		{
			System.Collections.Generic.IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
			return JsonConvert.SerializeObject(articleMainCategories);
		}
		private System.Data.DataTable ConvertListToDataTable<T>(System.Collections.Generic.IList<T> list, params string[] propertyName)
		{
			System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
			if (propertyName != null)
			{
				list2.AddRange(propertyName);
			}
			System.Data.DataTable dataTable = new System.Data.DataTable();
			if (list.Count > 0)
			{
				T t = list[0];
				System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties();
				System.Reflection.PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					System.Reflection.PropertyInfo propertyInfo = array[i];
					if (list2.Count == 0)
					{
						dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
					}
					else
					{
						if (list2.Contains(propertyInfo.Name))
						{
							dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
						}
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
					System.Reflection.PropertyInfo[] array2 = properties;
					for (int k = 0; k < array2.Length; k++)
					{
						System.Reflection.PropertyInfo propertyInfo2 = array2[k];
						if (list2.Count == 0)
						{
							object value = propertyInfo2.GetValue(list[j], null);
							arrayList.Add(value);
						}
						else
						{
							if (list2.Contains(propertyInfo2.Name))
							{
								object value2 = propertyInfo2.GetValue(list[j], null);
								arrayList.Add(value2);
							}
						}
					}
					object[] values = arrayList.ToArray();
					dataTable.LoadDataRow(values, true);
				}
			}
			return dataTable;
		}
	}
}
