using EcShop.ControlPanel.Sales;
using EcShop.Entities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin.sales
{
	public class Print : AdminPage
	{
		protected int pringrows;
		protected string orderIds = "";
		protected string mailNo = "";
		protected string templateName = "";
		protected string width = "";
		protected string height = "";
		protected string UpdateOrderIds = string.Empty;
		protected string ShipperName = string.Empty;
		protected string SizeShipperName = string.Empty;
		protected string CellPhone = string.Empty;
		protected string SizeCellPhone = string.Empty;
		protected string TelPhone = string.Empty;
		protected string SizeTelPhone = string.Empty;
		protected string Address = string.Empty;
		protected string SizeAddress = string.Empty;
		protected string Zipcode = string.Empty;
		protected string SizeZipcode = string.Empty;
		protected string Province = string.Empty;
		protected string SizeProvnce = string.Empty;
		protected string City = string.Empty;
		protected string SizeCity = string.Empty;
		protected string District = string.Empty;
		protected string SizeDistrict = string.Empty;
		protected string ShipToDate = string.Empty;
		protected string SizeShipToDate = string.Empty;
		protected string OrderId = string.Empty;
		protected string SizeOrderId = string.Empty;
		protected string OrderTotal = string.Empty;
		protected string SizeOrderTotal = string.Empty;
		protected string Shipitemweith = string.Empty;
		protected string SizeShipitemweith = string.Empty;
		protected string Remark = string.Empty;
		protected string SizeRemark = string.Empty;
		protected string ShipitemInfos = string.Empty;
		protected string SizeitemInfos = string.Empty;
		protected string SiteName = string.Empty;
		protected string SizeSiteName = string.Empty;
		protected string ShipTo = string.Empty;
		protected string SizeShipTo = string.Empty;
		protected string ShipTelPhone = string.Empty;
		protected string SizeShipTelPhone = string.Empty;
		protected string ShipCellPhone = string.Empty;
		protected string SizeShipCellPhone = string.Empty;
		protected string ShipZipCode = string.Empty;
		protected string SizeShipZipCode = string.Empty;
		protected string ShipAddress = string.Empty;
		protected string ShipSizeAddress = string.Empty;
		protected string ShipProvince = string.Empty;
		protected string ShipSizeProvnce = string.Empty;
		protected string ShipCity = string.Empty;
		protected string ShipSizeCity = string.Empty;
		protected string ShipDistrict = string.Empty;
		protected string ShipSizeDistrict = string.Empty;
		protected string Departure = string.Empty;
		protected string SizeDeparture = string.Empty;
		protected string Destination = string.Empty;
		protected string SizeDestination = string.Empty;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.Button btprint;
		private void PrintPage(string pagewidth, string pageheght)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<script language='javascript'>");
			stringBuilder.Append("function clicks(){");
			if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipperName=[",
					this.ShipperName.Substring(0, this.ShipperName.Length - 1),
					"];var SizeShipperName=[",
					this.SizeShipperName.Substring(0, this.SizeShipperName.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var CellPhone=[",
					this.CellPhone.Substring(0, this.CellPhone.Length - 1),
					"];var SizeCellPhone=[",
					this.SizeCellPhone.Substring(0, this.SizeCellPhone.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var TelPhone=[",
					this.TelPhone.Substring(0, this.TelPhone.Length - 1),
					"];var SizeTelPhone=[",
					this.SizeTelPhone.Substring(0, this.SizeTelPhone.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Address=[",
					this.Address.Substring(0, this.Address.Length - 1),
					"];var SizeAddress=[",
					this.SizeAddress.Substring(0, this.SizeAddress.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Zipcode=[",
					this.Zipcode.Substring(0, this.Zipcode.Length - 1),
					"];var SizeZipcode=[",
					this.SizeZipcode.Substring(0, this.SizeZipcode.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Province=[",
					this.Province.Substring(0, this.Province.Length - 1),
					"];var SizeProvnce=[",
					this.SizeProvnce.Substring(0, this.SizeProvnce.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var City=[",
					this.City.Substring(0, this.City.Length - 1),
					"];var SizeCity=[",
					this.SizeCity.Substring(0, this.SizeCity.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var District=[",
					this.District.Substring(0, this.District.Length - 1),
					"];var SizeDistrict=[",
					this.SizeDistrict.Substring(0, this.SizeDistrict.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipToDate=[",
					this.ShipToDate.Substring(0, this.ShipToDate.Length - 1),
					"];var SizeShipToDate=[",
					this.SizeShipToDate.Substring(0, this.SizeShipToDate.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var OrderId=[",
					this.OrderId.Substring(0, this.OrderId.Length - 1),
					"];var SizeOrderId=[",
					this.SizeOrderId.Substring(0, this.SizeOrderId.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var OrderTotal=[",
					this.OrderTotal.Substring(0, this.OrderTotal.Length - 1),
					"];var SizeOrderTotal=[",
					this.SizeOrderTotal.Substring(0, this.SizeOrderTotal.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Shipitemweith=[",
					this.Shipitemweith.Substring(0, this.Shipitemweith.Length - 1),
					"];var SizeShipitemweith=[",
					this.SizeShipitemweith.Substring(0, this.SizeShipitemweith.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Remark=[",
					this.Remark.Substring(0, this.Remark.Length - 1),
					"];var SizeRemark=[",
					this.SizeRemark.Substring(0, this.SizeRemark.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipitemInfos=[",
					this.ShipitemInfos.Substring(0, this.ShipitemInfos.Length - 1),
					"];var SizeitemInfos=[",
					this.SizeitemInfos.Substring(0, this.SizeitemInfos.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var SiteName=[",
					this.SiteName.Substring(0, this.SiteName.Length - 1),
					"];var SizeSiteName=[",
					this.SizeSiteName.Substring(0, this.SizeSiteName.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipTo=[",
					this.ShipTo.Substring(0, this.ShipTo.Length - 1),
					"];var SizeShipTo=[",
					this.SizeShipTo.Substring(0, this.SizeShipTo.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipTelPhone=[",
					this.ShipTelPhone.Substring(0, this.ShipTelPhone.Length - 1),
					"];var SizeShipTelPhone=[",
					this.SizeShipTelPhone.Substring(0, this.SizeShipTelPhone.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipCellPhone=[",
					this.ShipCellPhone.Substring(0, this.ShipCellPhone.Length - 1),
					"];var SizeShipCellPhone=[",
					this.SizeShipCellPhone.Substring(0, this.SizeShipCellPhone.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipZipCode=[",
					this.ShipZipCode.Substring(0, this.ShipZipCode.Length - 1),
					"];var SizeShipZipCode=[",
					this.SizeShipZipCode.Substring(0, this.SizeShipZipCode.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipAddress=[",
					this.ShipAddress.Substring(0, this.ShipAddress.Length - 1),
					"];var ShipSizeAddress=[",
					this.ShipSizeAddress.Substring(0, this.ShipSizeAddress.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipProvince=[",
					this.ShipProvince.Substring(0, this.ShipProvince.Length - 1),
					"];var ShipSizeProvnce=[",
					this.ShipSizeProvnce.Substring(0, this.ShipSizeProvnce.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipCity=[",
					this.ShipCity.Substring(0, this.ShipCity.Length - 1),
					"];var ShipSizeCity=[",
					this.ShipSizeCity.Substring(0, this.ShipSizeCity.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var ShipDistrict=[",
					this.ShipDistrict.Substring(0, this.ShipDistrict.Length - 1),
					"];var ShipSizeDistrict=[",
					this.ShipSizeDistrict.Substring(0, this.ShipSizeDistrict.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Departure=[",
					this.Departure.Substring(0, this.Departure.Length - 1),
					"];var SizeDeparture=[",
					this.SizeDeparture.Substring(0, this.SizeDeparture.Length - 1),
					"];"
				}));
			}
			if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					" var Destination=[",
					this.Destination.Substring(0, this.Destination.Length - 1),
					"];var SizeDestination=[",
					this.SizeDestination.Substring(0, this.SizeDestination.Length - 1),
					"];"
				}));
			}
			stringBuilder.Append(" var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));");
			stringBuilder.Append(" try{ ");
			stringBuilder.Append("  for(var i=0;i<" + this.pringrows + ";++i){ ");
			stringBuilder.Append("showdiv();");
			stringBuilder.Append(string.Concat(new object[]
			{
				" LODOP.SET_PRINT_PAGESIZE (1,",
				decimal.Parse(pagewidth) * 10m,
				",",
				decimal.Parse(pageheght) * 10m,
				",\"\");"
			}));
			stringBuilder.Append(" LODOP.SET_PRINT_STYLE(\"FontSize\",12);");
			stringBuilder.Append(" LODOP.SET_PRINT_STYLE(\"Bold\",1);");
			if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
			{
				stringBuilder.Append("LODOP.ADD_PRINT_TEXT(SizeShipperName[i].split(',')[0],SizeShipperName[i].split(',')[1],SizeShipperName[i].split(',')[2],SizeShipperName[i].split(',')[3],ShipperName[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeCellPhone[i].split(',')[0],SizeCellPhone[i].split(',')[1],SizeCellPhone[i].split(',')[2],SizeCellPhone[i].split(',')[3],CellPhone[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeTelPhone[i].split(',')[0],SizeTelPhone[i].split(',')[1],SizeTelPhone[i].split(',')[2],SizeTelPhone[i].split(',')[3],TelPhone[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeAddress[i].split(',')[0],SizeAddress[i].split(',')[1],SizeAddress[i].split(',')[2],SizeAddress[i].split(',')[3],Address[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeZipcode[i].split(',')[0],Zipcode[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeProvnce[i].split(',')[0],SizeProvnce[i].split(',')[1],SizeProvnce[i].split(',')[2],SizeProvnce[i].split(',')[3],Province[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeCity[i].split(',')[0],SizeCity[i].split(',')[1],SizeCity[i].split(',')[2],SizeCity[i].split(',')[3],City[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDistrict[i].split(',')[0],SizeDistrict[i].split(',')[1],SizeDistrict[i].split(',')[2],SizeDistrict[i].split(',')[3],District[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipToDate[i].split(',')[0],SizeShipToDate[i].split(',')[1],SizeShipToDate[i].split(',')[2],SizeShipToDate[i].split(',')[3],ShipToDate[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderId[i].split(',')[0],SizeOrderId[i].split(',')[1],SizeOrderId[i].split(',')[2],SizeOrderId[i].split(',')[3],OrderId[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderTotal[i].split(',')[0],SizeOrderTotal[i].split(',')[1],SizeOrderTotal[i].split(',')[2],SizeOrderTotal[i].split(',')[3],OrderTotal[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipitemweith[i].split(',')[0],SizeShipitemweith[i].split(',')[1],SizeShipitemweith[i].split(',')[2],SizeShipitemweith[i].split(',')[3]Shipitemweith[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeRemark[i].split(',')[0],SizeRemark[i].split(',')[1],SizeRemark[i].split(',')[2],SizeRemark[i].split(',')[3],Remark[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeitemInfos[i].split(',')[0],SizeitemInfos[i].split(',')[1],SizeitemInfos[i].split(',')[2],SizeitemInfos[i].split(',')[3],ShipitemInfos[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeSiteName[i].split(',')[0],SizeSiteName[i].split(',')[1],SizeSiteName[i].split(',')[2],SizeSiteName[i].split(',')[3],SiteName[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTo[i].split(',')[0],SizeShipTo[i].split(',')[1],SizeShipTo[i].split(',')[2],SizeShipTo[i].split(',')[3],ShipTo[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTelPhone[i].split(',')[0],SizeShipTelPhone[i].split(',')[1],SizeShipTelPhone[i].split(',')[2],SizeShipTelPhone[i].split(',')[3],ShipTelPhone[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipCellPhone[i].split(',')[0],SizeShipCellPhone[i].split(',')[1],SizeShipCellPhone[i].split(',')[2],SizeShipCellPhone[i].split(',')[3],ShipCellPhone[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipZipCode[i].split(',')[0],SizeShipZipCode[i].split(',')[1],SizeShipZipCode[i].split(',')[2],SizeShipZipCode[i].split(',')[3],ShipZipCode[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeAddress[i].split(',')[0],ShipSizeAddress[i].split(',')[1],ShipSizeAddress[i].split(',')[2],ShipSizeAddress[i].split(',')[3],ShipAddress[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeProvnce[i].split(',')[0],ShipSizeProvnce[i].split(',')[1],ShipSizeProvnce[i].split(',')[2],ShipSizeProvnce[i].split(',')[3],ShipProvince[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeCity[i].split(',')[0],ShipSizeCity[i].split(',')[1],ShipSizeCity[i].split(',')[2],ShipSizeCity[i].split(',')[3],ShipCity[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeDistrict[i].split(',')[0],ShipSizeDistrict[i].split(',')[1],ShipSizeDistrict[i].split(',')[2],ShipSizeDistrict[i].split(',')[3],ShipDistrict[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDeparture[i].split(',')[0],SizeDeparture[i].split(',')[1],SizeDeparture[i].split(',')[2],SizeDeparture[i].split(',')[3],Departure[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDestination[i].split(',')[0],SizeDestination[i].split(',')[1],SizeDestination[i].split(',')[2],SizeDestination[i].split(',')[3],Destination[i]);");
			}
			stringBuilder.Append(" LODOP.PRINT();");
			stringBuilder.Append("   }");
			stringBuilder.Append("hidediv()");
			stringBuilder.Append("  }catch(e){ alert(\"请先安装打印控件！\");return false;}");
			stringBuilder.Append("}");
			stringBuilder.Append(" setTimeout(\"clicks()\",1000);document.getElementById(\"divcloseprint\").style.display = \"\"; document.getElementById(\"divprint\").style.display = \"none\";  ");
			stringBuilder.Append("</script>");
			base.ClientScript.RegisterStartupScript(base.GetType(), "myscript", stringBuilder.ToString());
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btprint.Click += new System.EventHandler(this.ButPrint_Click);
			if (!base.IsPostBack)
			{
				this.mailNo = base.Request["mailNo"];
				int shipperId = int.Parse(base.Request["shipperId"]);
				this.orderIds = base.Request["orderIds"].Trim(new char[]
				{
					','
				});
				string text = System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
				if (System.IO.File.Exists(text))
				{
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.Load(text);
					System.Xml.XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
					this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
					string innerText = xmlNode.SelectSingleNode("pic").InnerText;
					string innerText2 = xmlNode.SelectSingleNode("size").InnerText;
					this.width = innerText2.Split(new char[]
					{
						':'
					})[0];
					this.height = innerText2.Split(new char[]
					{
						':'
					})[1];
					System.Data.DataSet printData = this.GetPrintData(this.orderIds);
					int num = 0;
					foreach (System.Data.DataRow dataRow in printData.Tables[0].Rows)
					{
						this.UpdateOrderIds = this.UpdateOrderIds + dataRow["orderid"] + ",";
						System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
						if (!string.IsNullOrEmpty(innerText) && innerText != "noimage")
						{
							using (System.Drawing.Image image = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", innerText))))
							{
								htmlGenericControl.Attributes["style"] = string.Format("background-image: url(../../Storage/master/flex/{0}); width: {1}px; height: {2}px;text-align: center; position: relative;", innerText, image.Width, image.Height);
							}
						}
						System.Data.DataTable dataTable = printData.Tables[1];
						ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
						string[] array = dataRow["shippingRegion"].ToString().Split(new char[]
						{
							'，'
						});
						foreach (System.Xml.XmlNode xmlNode2 in xmlNode.SelectNodes("item"))
						{
							string text2 = xmlNode2.SelectSingleNode("name").InnerText;
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(text2);
							stringBuilder.Replace("收货人-姓名", dataRow["ShipTo"].ToString());
							stringBuilder.Replace("收货人-电话", dataRow["TelPhone"].ToString());
							stringBuilder.Replace("收货人-手机", dataRow["CellPhone"].ToString());
							stringBuilder.Replace("收货人-邮编", dataRow["ZipCode"].ToString());
							stringBuilder.Replace("收货人-地址", dataRow["Address"].ToString());
							string newValue = string.Empty;
							if (array.Length > 0)
							{
								newValue = array[0];
							}
							stringBuilder.Replace("收货人-地区1级", newValue);
							newValue = string.Empty;
							if (array.Length > 1)
							{
								newValue = array[1];
							}
							stringBuilder.Replace("收货人-地区2级", newValue);
							stringBuilder.Replace("目的地-地区", newValue);
							newValue = string.Empty;
							if (array.Length > 2)
							{
								newValue = array[2];
							}
							stringBuilder.Replace("收货人-地区3级", newValue);
							string[] array2 = new string[]
							{
								"",
								"",
								""
							};
							if (shipper != null)
							{
								array2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[]
								{
									'-'
								});
							}
							stringBuilder.Replace("发货人-姓名", (shipper != null) ? shipper.ShipperName : "");
							stringBuilder.Replace("发货人-手机", (shipper != null) ? shipper.CellPhone : "");
							stringBuilder.Replace("发货人-电话", (shipper != null) ? shipper.TelPhone : "");
							stringBuilder.Replace("发货人-地址", (shipper != null) ? shipper.Address : "");
							stringBuilder.Replace("发货人-邮编", (shipper != null) ? shipper.Zipcode : "");
							string newValue2 = string.Empty;
							if (array2.Length > 0)
							{
								newValue2 = array2[0];
							}
							stringBuilder.Replace("发货人-地区1级", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 1)
							{
								newValue2 = array2[1];
							}
							stringBuilder.Replace("发货人-地区2级", newValue2);
							stringBuilder.Replace("始发地-地区", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 2)
							{
								newValue2 = array2[2];
							}
							decimal num2 = 0m;
							decimal.TryParse(dataRow["Weight"].ToString(), out num2);
							stringBuilder.Replace("发货人-地区3级", newValue2);
							stringBuilder.Replace("送货-上门时间", dataRow["ShipToDate"].ToString());
							stringBuilder.Replace("订单-订单号", "订单号：" + dataRow["OrderId"].ToString());
							stringBuilder.Replace("订单-总金额", decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2"));
							stringBuilder.Replace("订单-物品总重量", num2.ToString("F2"));
							stringBuilder.Replace("订单-备注", dataRow["Remark"].ToString());
							System.Data.DataRow[] array3 = dataTable.Select(" OrderId='" + dataRow["OrderId"] + "'");
							string text3 = string.Empty;
							if (array3.Length > 0)
							{
								System.Data.DataRow[] array4 = array3;
								for (int i = 0; i < array4.Length; i++)
								{
									System.Data.DataRow dataRow2 = array4[i];
									text3 = string.Concat(new object[]
									{
										text3,
										"规格",
										dataRow2["SKUContent"],
										" 数量",
										dataRow2["ShipmentQuantity"],
										"货号 :",
										dataRow2["SKU"]
									});
								}
								text3 = text3.Replace("；", "");
							}
							stringBuilder.Replace("订单-详情", text3);
							stringBuilder.Replace("订单-送货时间", "");
							stringBuilder.Replace("网店名称", HiContext.Current.SiteSettings.SiteName);
							stringBuilder.Replace("自定义内容", "");
							text2 = stringBuilder.ToString();
							string innerText3 = xmlNode2.SelectSingleNode("font").InnerText;
							string arg_735_0 = xmlNode2.SelectSingleNode("fontsize").InnerText;
							string innerText4 = xmlNode2.SelectSingleNode("position").InnerText;
							string innerText5 = xmlNode2.SelectSingleNode("align").InnerText;
							string str = innerText4.Split(new char[]
							{
								':'
							})[0];
							string str2 = innerText4.Split(new char[]
							{
								':'
							})[1];
							string str3 = innerText4.Split(new char[]
							{
								':'
							})[2];
							string str4 = innerText4.Split(new char[]
							{
								':'
							})[3];
							string arg_7D9_0 = xmlNode2.SelectSingleNode("border").InnerText;
							System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
							htmlGenericControl2.Visible = true;
							htmlGenericControl2.InnerText = text2.Split(new char[]
							{
								'_'
							})[0];
							htmlGenericControl2.Style["font-family"] = innerText3;
							htmlGenericControl2.Style["font-size"] = "16px";
							htmlGenericControl2.Style["width"] = str + "px";
							htmlGenericControl2.Style["height"] = str2 + "px";
							htmlGenericControl2.Style["text-align"] = innerText5;
							htmlGenericControl2.Style["position"] = "absolute";
							htmlGenericControl2.Style["left"] = str3 + "px";
							htmlGenericControl2.Style["top"] = str4 + "px";
							htmlGenericControl2.Style["padding"] = "0";
							htmlGenericControl2.Style["margin-left"] = "0px";
							htmlGenericControl2.Style["margin-top"] = "0px";
							htmlGenericControl2.Style["font-weight"] = "bold";
							htmlGenericControl.Controls.Add(htmlGenericControl2);
						}
						this.divContent.Controls.Add(htmlGenericControl);
						num++;
						if (num < printData.Tables[0].Rows.Count)
						{
							System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl3 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
							htmlGenericControl3.Attributes["class"] = "PageNext";
							this.divContent.Controls.Add(htmlGenericControl3);
						}
					}
					this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(new char[]
					{
						','
					});
				}
			}
		}
		protected void ButPrint_Click(object sender, System.EventArgs e)
		{
			this.printdata();
			string[] array = this.UpdateOrderIds.Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string str = array2[i];
				list.Add("'" + str + "'");
			}
			OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), this.templateName, this.templateName);
			OrderHelper.SetOrderShipNumber(array, this.mailNo, this.templateName);
			OrderHelper.SetOrderPrinted(array, true);
		}
		private string ReplaceString(string str)
		{
			return str.Replace("'", "＇");
		}
		private void printdata()
		{
			this.mailNo = base.Request["mailNo"];
			int shipperId = int.Parse(base.Request["shipperId"]);
			this.orderIds = base.Request["orderIds"].Trim(new char[]
			{
				','
			});
			string text = System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
			if (System.IO.File.Exists(text))
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.Load(text);
				System.Xml.XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
				this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
				string arg_D0_0 = xmlNode.SelectSingleNode("pic").InnerText;
				string innerText = xmlNode.SelectSingleNode("size").InnerText;
				this.width = innerText.Split(new char[]
				{
					':'
				})[0];
				this.height = innerText.Split(new char[]
				{
					':'
				})[1];
				System.Data.DataSet printData = this.GetPrintData(this.orderIds);
				this.pringrows = printData.Tables[0].Rows.Count;
				foreach (System.Data.DataRow dataRow in printData.Tables[0].Rows)
				{
					this.UpdateOrderIds = this.UpdateOrderIds + dataRow["orderid"] + ",";
					System.Data.DataTable dataTable = printData.Tables[1];
					ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
					string[] array = dataRow["shippingRegion"].ToString().Split(new char[]
					{
						'，'
					});
					foreach (System.Xml.XmlNode xmlNode2 in xmlNode.SelectNodes("item"))
					{
						string text2 = string.Empty;
						string innerText2 = xmlNode2.SelectSingleNode("name").InnerText;
						string innerText3 = xmlNode2.SelectSingleNode("position").InnerText;
						string text3 = innerText3.Split(new char[]
						{
							':'
						})[0];
						string text4 = innerText3.Split(new char[]
						{
							':'
						})[1];
						string text5 = innerText3.Split(new char[]
						{
							':'
						})[2];
						string text6 = innerText3.Split(new char[]
						{
							':'
						})[3];
						string str = string.Concat(new string[]
						{
							text6,
							",",
							text5,
							",",
							text3,
							",",
							text4
						});
						string[] array2 = new string[]
						{
							"",
							"",
							""
						};
						if (shipper != null)
						{
							array2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[]
							{
								'-'
							});
						}
						string text7 = string.Empty;
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-姓名")
						{
							this.ShipTo = this.ShipTo + "'" + this.ReplaceString(dataRow["ShipTo"].ToString()) + "',";
							if (!string.IsNullOrEmpty(dataRow["ShipTo"].ToString().Trim()))
							{
								this.SizeShipTo = this.SizeShipTo + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-电话")
						{
							this.ShipTelPhone = this.ShipTelPhone + "'" + dataRow["TelPhone"].ToString() + "',";
							if (!string.IsNullOrEmpty(dataRow["TelPhone"].ToString().Trim()))
							{
								this.SizeShipTelPhone = this.SizeShipTelPhone + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-手机")
						{
							this.ShipCellPhone = this.ShipCellPhone + "'" + dataRow["CellPhone"].ToString() + "',";
							if (!string.IsNullOrEmpty(dataRow["CellPhone"].ToString().Trim()))
							{
								this.SizeShipCellPhone = this.SizeShipCellPhone + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-邮编")
						{
							this.ShipZipCode = this.ShipZipCode + "'" + dataRow["ZipCode"].ToString() + "',";
							if (!string.IsNullOrEmpty(dataRow["ZipCode"].ToString().Trim()))
							{
								this.SizeShipZipCode = this.SizeShipZipCode + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-地址")
						{
							this.ShipAddress = this.ShipAddress + "'" + this.ReplaceString(dataRow["Address"].ToString()) + "',";
							if (!string.IsNullOrEmpty(dataRow["Address"].ToString().Trim()))
							{
								this.ShipSizeAddress = this.ShipSizeAddress + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-地区1级")
						{
							if (array.Length > 0)
							{
								text2 = array[0];
							}
							this.ShipProvince = this.ShipProvince + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeProvnce = this.ShipSizeProvnce + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-地区2级")
						{
							text2 = string.Empty;
							if (array.Length > 1)
							{
								text2 = array[1];
							}
							this.ShipCity = this.ShipCity + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeCity = this.ShipSizeCity + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "目的地-地区")
						{
							text2 = string.Empty;
							if (array.Length > 1)
							{
								text2 = array[1];
							}
							this.Destination = this.Destination + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.SizeDestination = this.SizeDestination + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "收货人-地区3级")
						{
							text2 = string.Empty;
							if (array.Length > 2)
							{
								text2 = array[2];
							}
							this.ShipDistrict = this.ShipDistrict + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeDistrict = this.ShipSizeDistrict + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "送货-上门时间")
						{
							this.ShipToDate = this.ShipToDate + "'" + dataRow["ShipToDate"].ToString() + "',";
							if (!string.IsNullOrEmpty(dataRow["ShipToDate"].ToString().Trim()))
							{
								this.SizeShipToDate = this.SizeShipToDate + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "订单-订单号")
						{
							this.OrderId = this.OrderId + "'订单号：" + dataRow["OrderId"].ToString() + "',";
							if (!string.IsNullOrEmpty(dataRow["OrderId"].ToString().Trim()))
							{
								this.SizeOrderId = this.SizeOrderId + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "订单-总金额")
						{
							if (!string.IsNullOrEmpty(dataRow["OrderTotal"].ToString().Trim()))
							{
								this.OrderTotal = this.OrderTotal + decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2") + "',";
							}
							if (!string.IsNullOrEmpty(dataRow["OrderTotal"].ToString().Trim()))
							{
								this.SizeOrderTotal = this.SizeOrderTotal + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "订单-详情")
						{
							System.Data.DataRow[] array3 = dataTable.Select(" OrderId='" + dataRow["OrderId"] + "'");
							string text8 = string.Empty;
							if (array3.Length > 0)
							{
								System.Data.DataRow[] array4 = array3;
								for (int i = 0; i < array4.Length; i++)
								{
									System.Data.DataRow dataRow2 = array4[i];
									text8 = string.Concat(new object[]
									{
										text8,
										"规格",
										dataRow2["SKUContent"],
										" 数量",
										dataRow2["ShipmentQuantity"],
										"货号 :",
										dataRow2["SKU"]
									});
								}
								text8 = text8.Replace(";", "");
							}
							if (!string.IsNullOrEmpty(text8.Trim()))
							{
								this.SizeitemInfos = this.SizeitemInfos + "'" + str + "',";
							}
							this.ShipitemInfos = this.ShipitemInfos + "'" + this.ReplaceString(text8) + "',";
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "订单-物品总重量")
						{
							decimal num = 0m;
							decimal.TryParse(dataRow["Weight"].ToString(), out num);
							this.Shipitemweith = this.Shipitemweith + "'" + num.ToString("F2") + "',";
							if (!string.IsNullOrEmpty(num.ToString().Trim()))
							{
								this.SizeShipitemweith = this.SizeShipitemweith + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "订单-备注")
						{
							this.Remark = this.Remark + "'" + this.ReplaceString(dataRow["Remark"].ToString()) + "',";
							if (!string.IsNullOrEmpty(dataRow["Remark"].ToString().Trim()))
							{
								this.SizeRemark = this.SizeRemark + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-姓名")
						{
							this.ShipperName = this.ShipperName + "'" + this.ReplaceString(shipper.ShipperName) + "',";
							if (!string.IsNullOrEmpty(shipper.ShipperName.Trim()))
							{
								this.SizeShipperName = this.SizeShipperName + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-电话")
						{
							this.TelPhone = this.TelPhone + "'" + shipper.TelPhone + "',";
							if (!string.IsNullOrEmpty(shipper.TelPhone.Trim()))
							{
								this.SizeTelPhone = this.SizeTelPhone + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-手机")
						{
							this.CellPhone = this.CellPhone + "'" + shipper.CellPhone + "',";
							if (!string.IsNullOrEmpty(shipper.CellPhone.Trim()))
							{
								this.SizeCellPhone = this.SizeCellPhone + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-邮编")
						{
							this.Zipcode = this.Zipcode + "'" + shipper.Zipcode + "',";
							if (!string.IsNullOrEmpty(shipper.Zipcode.Trim()))
							{
								this.SizeZipcode = this.SizeZipcode + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-地址")
						{
							this.Address = this.Address + "'" + this.ReplaceString(shipper.Address) + "',";
							if (!string.IsNullOrEmpty(shipper.Address.Trim()))
							{
								this.SizeAddress = this.SizeAddress + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-地区1级")
						{
							if (array2.Length > 0)
							{
								text7 = array2[0];
							}
							this.Province = this.Province + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeProvnce = this.SizeProvnce + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-地区2级")
						{
							text7 += string.Empty;
							if (array2.Length > 1)
							{
								text7 = array2[1];
							}
							this.City = this.City + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeCity = this.SizeCity + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "始发地-地区")
						{
							text7 += string.Empty;
							if (array2.Length > 1)
							{
								text7 = array2[1];
							}
							this.Departure = this.Departure + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeDeparture = this.SizeDeparture + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "发货人-地区3级")
						{
							text7 += string.Empty;
							if (array2.Length > 2)
							{
								text7 = array2[2];
							}
							this.District = this.District + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeDistrict = this.SizeDistrict + "'" + str + "',";
							}
						}
						if (innerText2.Split(new char[]
						{
							'_'
						})[0] == "网店名称")
						{
							this.SiteName = this.SiteName + "'" + this.ReplaceString(HiContext.Current.SiteSettings.SiteName) + "',";
							if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.SiteName.Trim()))
							{
								this.SizeSiteName = this.SizeSiteName + "'" + str + "',";
							}
						}
					}
				}
				this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(new char[]
				{
					','
				});
				this.PrintPage(this.width, this.height);
			}
		}
		private System.Data.DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return OrderHelper.GetOrdersAndLines(orderIds);
		}
		private decimal CalculateOrderTotal(System.Data.DataRow order, System.Data.DataSet ds)
		{
			decimal d = 0m;
			decimal d2 = 0m;
			decimal d3 = 0m;
			decimal d4 = 0m;
			bool flag = false;
			decimal.TryParse(order["AdjustedFreight"].ToString(), out d);
			decimal.TryParse(order["PayCharge"].ToString(), out d2);
			string value = order["CouponCode"].ToString();
			decimal.TryParse(order["CouponValue"].ToString(), out d3);
			decimal.TryParse(order["AdjustedDiscount"].ToString(), out d4);
			bool.TryParse(order["OptionPrice"].ToString(), out flag);
			System.Data.DataRow[] orderGift = ds.Tables[2].Select("OrderId='" + order["orderId"] + "'");
			System.Data.DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
			decimal d5 = this.GetAmount(orderGift, orderLine, order);
			d5 += d;
			d5 += d2;
			if (!string.IsNullOrEmpty(value))
			{
				d5 -= d3;
			}
			return d5 + d4;
		}
		public decimal GetAmount(System.Data.DataRow[] orderGift, System.Data.DataRow[] orderLine, System.Data.DataRow order)
		{
			return this.GetGoodDiscountAmount(order, orderLine) + this.GetGiftAmount(orderGift);
		}
		public decimal GetGoodDiscountAmount(System.Data.DataRow order, System.Data.DataRow[] orderLine)
		{
			decimal num = 0m;
			decimal.TryParse(order["DiscountAmount"].ToString(), out num);
			decimal result = this.GetGoodsAmount(orderLine);
			order["ReducedPromotionName"].ToString();
			if (order["ReducedPromotionAmount"] != System.DBNull.Value)
			{
				result = System.Convert.ToDecimal(order["ReducedPromotionAmount"]);
			}
			return result;
		}
		public decimal GetGoodsAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow dataRow = rows[i];
				num += decimal.Parse(dataRow["ItemAdjustedPrice"].ToString()) * int.Parse(dataRow["Quantity"].ToString());
			}
			return num;
		}
		public decimal GetGiftAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow dataRow = rows[i];
				num += decimal.Parse(dataRow["CostPrice"].ToString());
			}
			return num;
		}
	}
}
