using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchUpload)]
	public class ImportFromLocal : AdminPage
	{
		private string _dataPath;
		private readonly System.Text.Encoding _encoding = System.Text.Encoding.UTF8;
		private readonly System.IO.DirectoryInfo _baseDir = new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Request.MapPath("~/storage/data/homemade"));
		private System.IO.DirectoryInfo _workDir;
		private System.Data.DataTable _exportData = new System.Data.DataTable();
		private string csvPath = "";
		private string uploadPath = HiContext.Current.GetStoragePath() + "/product";
		protected System.Web.UI.WebControls.DropDownList dropFiles;
		protected System.Web.UI.WebControls.FileUpload fileUploader;
		protected System.Web.UI.WebControls.Button btnUpload;
		protected System.Web.UI.WebControls.Button btnImport;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this._dataPath = this.Page.Request.MapPath("~/storage/data/homemade");
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindFiles();
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		private void BindFiles()
		{
			this.dropFiles.Items.Clear();
			this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(this._dataPath);
			System.IO.FileInfo[] files = directoryInfo.GetFiles("*.zip", System.IO.SearchOption.TopDirectoryOnly);
			System.IO.FileInfo[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				System.IO.FileInfo fileInfo = array[i];
				string name = fileInfo.Name;
				this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem(name, name));
			}
		}
		private void GetImg(string fileName, ref ProductInfo product, int index)
		{
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
			string str = System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + System.IO.Path.GetExtension(fileName);
			string text = this.uploadPath + "/images/" + str;
			string text2 = this.uploadPath + "/thumbs40/40_" + str;
			string text3 = this.uploadPath + "/thumbs60/60_" + str;
			string text4 = this.uploadPath + "/thumbs100/100_" + str;
			string text5 = this.uploadPath + "/thumbs160/160_" + str;
			string text6 = this.uploadPath + "/thumbs180/180_" + str;
			string text7 = this.uploadPath + "/thumbs220/220_" + str;
			string text8 = this.uploadPath + "/thumbs310/310_" + str;
			string text9 = this.uploadPath + "/thumbs410/410_" + str;
			fileInfo.CopyTo(base.Request.MapPath(Globals.ApplicationPath + text));
			string sourceFilename = base.Request.MapPath(Globals.ApplicationPath + text);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text2), 40, 40);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text3), 60, 60);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text4), 100, 100);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text5), 160, 160);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text6), 180, 180);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text7), 220, 220);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text8), 310, 310);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(Globals.ApplicationPath + text9), 410, 410);
			if (index == 1)
			{
				product.ImageUrl1 = text;
				product.ThumbnailUrl40 = text2;
				product.ThumbnailUrl60 = text3;
				product.ThumbnailUrl100 = text4;
				product.ThumbnailUrl160 = text5;
				product.ThumbnailUrl180 = text6;
				product.ThumbnailUrl220 = text7;
				product.ThumbnailUrl310 = text8;
				product.ThumbnailUrl410 = text9;
				return;
			}
			switch (index)
			{
			case 2:
				product.ImageUrl2 = text;
				return;
			case 3:
				product.ImageUrl3 = text;
				return;
			case 4:
				product.ImageUrl4 = text;
				return;
			case 5:
				product.ImageUrl5 = text;
				return;
			default:
				return;
			}
		}
		private void btnImport_Click(object sender, System.EventArgs e)
		{
			string text = this.dropFiles.SelectedValue;
			text = System.IO.Path.Combine(this._dataPath, text);
			if (!System.IO.File.Exists(text))
			{
				this.ShowMsg("选择的数据包文件有问题！", false);
				return;
			}
			int num = 0;
			int num2 = 0;
			this.PrepareDataFiles(new object[]
			{
				text
			});
			System.Collections.Generic.List<System.Collections.Generic.List<string>> list = this.ReadCsv(this.csvPath, true, '\t', System.Text.Encoding.GetEncoding("GB2312"));
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				ProductInfo productInfo = new ProductInfo();
				try
				{
					System.Collections.Generic.List<string> list2 = list[i];
					if (list2[18] != "")
					{
						System.Data.DataTable brandCategories = CatalogHelper.GetBrandCategories(list2[18]);
						if (brandCategories.Rows.Count > 0)
						{
							productInfo.BrandId = new int?(System.Convert.ToInt32(brandCategories.Rows[0]["BrandId"]));
						}
					}
					if (list2[1] != "")
					{
						System.Data.DataTable categoryes = CatalogHelper.GetCategoryes(list2[1]);
						if (categoryes.Rows.Count > 0)
						{
							productInfo.CategoryId = System.Convert.ToInt32(categoryes.Rows[0]["CategoryId"]);
						}
						else
						{
							productInfo.CategoryId = 0;
						}
					}
					else
					{
						productInfo.CategoryId = 0;
					}
					if (list2[7] != "")
					{
						string path = System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[7]);
						using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding("gb2312")))
						{
							productInfo.Description = streamReader.ReadToEnd();
						}
					}
					if (productInfo.CategoryId > 0)
					{
						productInfo.MainCategoryPath = CatalogHelper.GetCategory(productInfo.CategoryId).Path + "|";
					}
					productInfo.HasSKU = (int.Parse(list2[19]) == 1);
					productInfo.ImageUrl1 = "";
					productInfo.ImageUrl2 = "";
					productInfo.ImageUrl3 = "";
					productInfo.ImageUrl4 = "";
					productInfo.ImageUrl5 = "";
					if (list2[12] != "")
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[12]));
						if (fileInfo.Exists)
						{
							this.GetImg(fileInfo.FullName, ref productInfo, 1);
						}
					}
					if (list2[13] != "")
					{
						System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[13]));
						if (fileInfo2.Exists)
						{
							this.GetImg(fileInfo2.FullName, ref productInfo, 2);
						}
					}
					if (list2[14] != "")
					{
						System.IO.FileInfo fileInfo3 = new System.IO.FileInfo(System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[14]));
						if (fileInfo3.Exists)
						{
							this.GetImg(fileInfo3.FullName, ref productInfo, 3);
						}
					}
					if (list2[15] != "")
					{
						System.IO.FileInfo fileInfo4 = new System.IO.FileInfo(System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[15]));
						if (fileInfo4.Exists)
						{
							this.GetImg(fileInfo4.FullName, ref productInfo, 4);
						}
					}
					if (list2[16] != "")
					{
						System.IO.FileInfo fileInfo5 = new System.IO.FileInfo(System.IO.Path.Combine(this.csvPath.Replace(".csv", ""), list2[16]));
						if (fileInfo5.Exists)
						{
							this.GetImg(fileInfo5.FullName, ref productInfo, 5);
						}
					}
					if (list2[17] != "")
					{
						productInfo.MarketPrice = new decimal?(decimal.Parse(list2[17]));
					}
					if (list2[9] != "")
					{
						productInfo.MetaDescription = list2[9];
					}
					if (list2[10] != "")
					{
						productInfo.MetaKeywords = list2[10];
					}
					if (list2[4] != "")
					{
						productInfo.ProductCode = list2[4];
					}
					productInfo.ProductName = list2[3];
					string text2 = list2[11];
					string a;
					if ((a = text2) != null)
					{
						if (!(a == "出售中"))
						{
							if (!(a == "下架区"))
							{
								if (a == "仓库中")
								{
									productInfo.SaleStatus = ProductSaleStatus.OnStock;
								}
							}
							else
							{
								productInfo.SaleStatus = ProductSaleStatus.UnSale;
							}
						}
						else
						{
							productInfo.SaleStatus = ProductSaleStatus.OnSale;
						}
					}
					if (list2[5] != "")
					{
						productInfo.ShortDescription = list2[5];
					}
					if (list2[8] != "")
					{
						productInfo.Title = list2[8];
					}
					if (list2[2] != "")
					{
						int typeId = ProductTypeHelper.GetTypeId(list2[2]);
						if (typeId > 0)
						{
							productInfo.TypeId = new int?(typeId);
						}
					}
					if (!productInfo.TypeId.HasValue)
					{
						productInfo.HasSKU = false;
					}
					if (list2[6] != "")
					{
						productInfo.Unit = list2[6];
					}
					System.Collections.Generic.Dictionary<string, SKUItem> dictionary = null;
					System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary2 = null;
					System.Collections.Generic.IList<int> list3 = new System.Collections.Generic.List<int>();
					if (list2[20] == "")
					{
						dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>();
						SKUItem sKUItem = new SKUItem();
						sKUItem.SkuId = "0";
						sKUItem.CostPrice = decimal.Parse(list2[24].Split(new char[]
						{
							';'
						})[0]);
						sKUItem.SalePrice = decimal.Parse(list2[25].Split(new char[]
						{
							';'
						})[0]);
						sKUItem.SKU = list2[21].Split(new char[]
						{
							';'
						})[0];
						sKUItem.Stock = int.Parse(list2[23].Split(new char[]
						{
							';'
						})[0]);
						sKUItem.Weight = decimal.Parse(list2[22].Split(new char[]
						{
							';'
						})[0]);
						dictionary.Add(sKUItem.SKU, sKUItem);
					}
					else
					{
						if (productInfo.TypeId.HasValue)
						{
							dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>();
							int value = productInfo.TypeId.Value;
							if (productInfo.HasSKU)
							{
								ProductTypeHelper.GetAttributes(value, AttributeUseageMode.Choose);
								string[] array = list2[20].Split(new char[]
								{
									';'
								});
								int num3 = array.Length;
								for (int j = 0; j < num3; j++)
								{
									SKUItem sKUItem2 = new SKUItem();
									sKUItem2.CostPrice = decimal.Parse(list2[24].Split(new char[]
									{
										';'
									})[j]);
									sKUItem2.SalePrice = decimal.Parse(list2[25].Split(new char[]
									{
										';'
									})[j]);
									sKUItem2.SKU = list2[21].Split(new char[]
									{
										';'
									})[j];
									sKUItem2.Stock = int.Parse(list2[23].Split(new char[]
									{
										';'
									})[j]);
									sKUItem2.Weight = decimal.Parse(list2[22].Split(new char[]
									{
										';'
									})[j]);
									string text3 = array[j];
									System.Collections.Generic.Dictionary<int, int> dictionary3 = new System.Collections.Generic.Dictionary<int, int>();
									string[] array2 = text3.Split(new char[]
									{
										','
									});
									for (int k = 0; k < array2.Length; k++)
									{
										string text4 = array2[k];
										string specificationName = text4.Split(new char[]
										{
											':'
										})[0];
										string valueStr = text4.Split(new char[]
										{
											':'
										})[1];
										int specificationId = ProductTypeHelper.GetSpecificationId(value, specificationName);
										if (specificationId <= 0)
										{
											productInfo.HasSKU = false;
											break;
										}
										int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, valueStr);
										if (specificationValueId <= 0)
										{
											productInfo.HasSKU = false;
											break;
										}
										dictionary3.Add(specificationId, specificationValueId);
									}
									if (productInfo.HasSKU && dictionary3.Count > 0)
									{
										string text5 = "";
										foreach (System.Collections.Generic.KeyValuePair<int, int> current in dictionary3)
										{
											sKUItem2.SkuItems.Add(current.Key, current.Value);
											text5 = text5 + current.Value + "_";
										}
										sKUItem2.SkuId = text5.Substring(0, text5.Length - 1);
										dictionary.Add(sKUItem2.SKU, sKUItem2);
									}
								}
								if (dictionary.Count > 0)
								{
									productInfo.HasSKU = true;
								}
							}
							else
							{
								SKUItem sKUItem3 = new SKUItem();
								sKUItem3.SkuId = "0";
								sKUItem3.CostPrice = decimal.Parse(list2[24].Split(new char[]
								{
									';'
								})[0]);
								sKUItem3.SalePrice = decimal.Parse(list2[25].Split(new char[]
								{
									';'
								})[0]);
								sKUItem3.SKU = list2[21].Split(new char[]
								{
									';'
								})[0];
								sKUItem3.Stock = int.Parse(list2[23].Split(new char[]
								{
									';'
								})[0]);
								sKUItem3.Weight = int.Parse(list2[22].Split(new char[]
								{
									';'
								})[0]);
								dictionary.Add(sKUItem3.SKU, sKUItem3);
							}
						}
					}
					if (list2[26] != "" && productInfo.TypeId.HasValue)
					{
						int value2 = productInfo.TypeId.Value;
						dictionary2 = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>>();
						System.Collections.Generic.IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(value2, AttributeUseageMode.View);
						foreach (AttributeInfo current2 in ProductTypeHelper.GetAttributes(value2, AttributeUseageMode.MultiView))
						{
							attributes.Add(current2);
						}
						string[] array2 = list2[26].Split(new char[]
						{
							','
						});
						for (int k = 0; k < array2.Length; k++)
						{
							string text6 = array2[k];
							string value3 = text6.Split(new char[]
							{
								':'
							})[0];
							string valueStr2 = text6.Split(new char[]
							{
								':'
							})[1];
							bool flag = false;
							int num4 = 0;
							foreach (AttributeInfo current3 in attributes)
							{
								if (current3.AttributeName.Equals(value3))
								{
									num4 = current3.AttributeId;
									flag = true;
									break;
								}
							}
							if (flag)
							{
								int specificationValueId2 = ProductTypeHelper.GetSpecificationValueId(num4, valueStr2);
								if (specificationValueId2 > 0)
								{
									if (dictionary2.ContainsKey(num4))
									{
										dictionary2[num4].Add(specificationValueId2);
									}
									else
									{
										dictionary2.Add(num4, new System.Collections.Generic.List<int>
										{
											specificationValueId2
										});
									}
								}
							}
						}
					}
					if (list2[27] != "")
					{
						list3 = new System.Collections.Generic.List<int>();
						System.Data.DataTable tags = CatalogHelper.GetTags();
						string[] array2 = list2[27].Split(new char[]
						{
							','
						});
						for (int k = 0; k < array2.Length; k++)
						{
							string obj = array2[k];
							foreach (System.Data.DataRow dataRow in tags.Rows)
							{
								if (dataRow["TagName"].Equals(obj))
								{
									list3.Add(System.Convert.ToInt32(dataRow["TagId"]));
									break;
								}
							}
						}
					}
					productInfo.AddedDate = System.DateTime.Now;
					ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, dictionary, dictionary2, list3);
					if (productActionStatus == ProductActionStatus.Success)
					{
						num++;
					}
					else
					{
						if (productActionStatus == ProductActionStatus.AttributeError)
						{
							num2++;
						}
						else
						{
							if (productActionStatus == ProductActionStatus.DuplicateName)
							{
								num2++;
							}
							else
							{
								if (productActionStatus == ProductActionStatus.DuplicateSKU)
								{
									num2++;
								}
								else
								{
									if (productActionStatus == ProductActionStatus.SKUError)
									{
										num2++;
									}
									else
									{
										num2++;
									}
								}
							}
						}
					}
				}
				catch
				{
					num2++;
				}
				i++;
			}
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(this.csvPath.Replace(".csv", ""));
			directoryInfo.Delete(true);
			System.IO.File.Delete(this.csvPath);
			System.IO.File.Delete(text);
			this.BindFiles();
			if (num2 == 0)
			{
				this.ShowMsg("此次商品批量导入操作已成功！", true);
				return;
			}
			this.ShowMsg("此次商品批量导入操作," + num2 + "件商品导入失败！", false);
		}
		public string PrepareDataFiles(params object[] initParams)
		{
			string path = (string)initParams[0];
			this._workDir = this._baseDir.CreateSubdirectory("product");
			using (ZipFile zipFile = ZipFile.Read(System.IO.Path.Combine(this._baseDir.FullName, path)))
			{
				foreach (ZipEntry current in zipFile)
				{
					current.Extract(this._workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			System.IO.FileInfo[] files = this._workDir.GetFiles("*.csv", System.IO.SearchOption.TopDirectoryOnly);
			int num = 0;
			if (num < files.Length)
			{
				System.IO.FileInfo fileInfo = files[num];
				this.csvPath = fileInfo.FullName;
			}
			return this._workDir.FullName;
		}
		public System.Collections.Generic.List<System.Collections.Generic.List<string>> ReadCsv(string csvName, bool hasHeader, char colSplit, System.Text.Encoding encoding)
		{
			System.Collections.Generic.List<System.Collections.Generic.List<string>> list = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();
			string[] array = System.IO.File.ReadAllLines(csvName, encoding);
			int i = 0;
			if (hasHeader)
			{
				i = 1;
			}
			while (i < array.Length)
			{
				string text = array[i];
				System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
				string[] array2 = text.Split(new char[]
				{
					colSplit
				});
				for (int j = 0; j < array2.Length; j++)
				{
					list2.Add(array2[j].Replace("\"", "").Replace("'", ""));
				}
				list.Add(list2);
				i++;
			}
			return list;
		}
		protected void btnUpload_Click(object sender, System.EventArgs e)
		{
			if (!this.fileUploader.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
				return;
			}
			if (this.fileUploader.PostedFile.ContentLength == 0 || System.IO.Path.GetExtension(this.fileUploader.FileName).Trim(new char[]
			{
				'.'
			}).ToLower() != "zip")
			{
				this.ShowMsg("请上传正确的数据包文件", false);
				return;
			}
			string fileName = System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
			this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, fileName));
			this.BindFiles();
			this.dropFiles.SelectedValue = fileName;
		}
	}
}
