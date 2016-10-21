using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin.sales
{
	public class BatchPrintData : AdminPage
	{
		protected static string orderIds = string.Empty;
		protected int pringrows;
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
		protected System.Web.UI.WebControls.Panel pnlTask;
		protected System.Web.UI.WebControls.Literal litNumber;
		protected System.Web.UI.WebControls.Panel pnlTaskEmpty;
		protected System.Web.UI.WebControls.Panel pnlShipper;
		protected ShippersDropDownList ddlShoperTag;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected RegionSelector dropRegions;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtTelphone;
		protected System.Web.UI.WebControls.TextBox txtCellphone;
		protected System.Web.UI.WebControls.Button btnUpdateAddrdss;
		protected System.Web.UI.WebControls.Panel pnlEmptySender;
		protected System.Web.UI.WebControls.Panel pnlTemplates;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.TextBox txtStartCode;
		protected System.Web.UI.WebControls.Button btnPrint;
		protected System.Web.UI.WebControls.Panel pnlEmptyTemplates;
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
			stringBuilder.Append("");
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
            stringBuilder.Append(" LODOP.PRINT();showdiv();");
			stringBuilder.Append("   }");
			stringBuilder.Append(" setTimeout(\"hidediv()\",3000);");
            stringBuilder.Append("  }catch(e){ alert(\"请先安装打印控件！\");hidediv();return false;}");
			stringBuilder.Append("}");
			stringBuilder.Append(" setTimeout(\"clicks()\",1000); ");
			stringBuilder.Append("</script>");
			base.ClientScript.RegisterStartupScript(base.GetType(), "myscript", stringBuilder.ToString());
		}
		private void printdata()
		{
			this.mailNo = this.txtStartCode.Text.Trim();
			int shipperId = int.Parse(this.ddlShoperTag.SelectedValue.ToString());
			ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
			if (shipper == null)
			{
				this.ShowMsg("请选择一个发货人", false);
				return;
			}
			string text = System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", this.ddlTemplates.SelectedValue));
			if (System.IO.File.Exists(text))
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.Load(text);
				System.Xml.XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
				this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
				string arg_BE_0 = xmlNode.SelectSingleNode("pic").InnerText;
				string innerText = xmlNode.SelectSingleNode("size").InnerText;
				this.width = innerText.Split(new char[]
				{
					':'
				})[0];
				this.height = innerText.Split(new char[]
				{
					':'
				})[1];
				System.Data.DataSet printData = this.GetPrintData(BatchPrintData.orderIds);
				this.pringrows = printData.Tables[0].Rows.Count;
				foreach (System.Data.DataRow dataRow in printData.Tables[0].Rows)
				{
					this.UpdateOrderIds = this.UpdateOrderIds + dataRow["orderid"] + ",";
					System.Data.DataTable dataTable = printData.Tables[1];
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
							else
							{
								this.SizeShipTo += "'0,0,0,0',";
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
							else
							{
								this.SizeShipTelPhone += "'0,0,0,0',";
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
							else
							{
								this.SizeShipCellPhone += "'0,0,0,0',";
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
							else
							{
								this.SizeShipZipCode += "'0,0,0,0',";
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
							else
							{
								this.ShipSizeAddress += "'0,0,0,0',";
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
							else
							{
								this.ShipSizeProvnce += "'0,0,0,0',";
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
							else
							{
								this.ShipSizeCity += "'0,0,0,0',";
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
							else
							{
								this.SizeDestination += "'0,0,0,0',";
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
							else
							{
								this.ShipSizeDistrict += "'0,0,0,0',";
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
							else
							{
								this.SizeShipToDate += "'0,0,0,0',";
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
							else
							{
								this.SizeOrderId += "'0,0,0,0',";
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
							else
							{
								this.SizeOrderTotal += "'0,0,0,0',";
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
							else
							{
								this.SizeitemInfos += "'0,0,0,0',";
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
							else
							{
								this.SizeShipitemweith += "'0,0,0,0',";
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
							else
							{
								this.SizeRemark += "'0,0,0,0',";
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
							else
							{
								this.SizeShipperName += "'0,0,0,0',";
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
							else
							{
								this.SizeTelPhone += "'0,0,0,0',";
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
							else
							{
								this.SizeCellPhone += "'0,0,0,0',";
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
							else
							{
								this.SizeZipcode += "'0,0,0,0',";
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
							else
							{
								this.SizeAddress += "'0,0,0,0',";
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
							else
							{
								this.SizeProvnce += "'0,0,0,0',";
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
							else
							{
								this.SizeCity += "'0,0,0,0',";
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
							else
							{
								this.SizeDeparture += "'0,0,0,0',";
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
							else
							{
								this.SizeDistrict += "'0,0,0,0',";
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
							else
							{
								this.SizeSiteName += "'0,0,0,0',";
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
		private string ReplaceString(string str)
		{
			return str.Replace("'", "＇");
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["OrderIds"]))
			{
				BatchPrintData.orderIds = base.Request["OrderIds"];
				this.litNumber.Text = BatchPrintData.orderIds.Trim(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				}).Length.ToString();
			}
			this.ddlShoperTag.SelectedIndexChanged += new System.EventHandler(this.ddlShoperTag_SelectedIndexChanged);
			this.btnUpdateAddrdss.Click += new System.EventHandler(this.btnUpdateAddrdss_Click);
			this.btnPrint.Click += new System.EventHandler(this.btnbtnPrint_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlShoperTag.DataBind();
				System.Collections.Generic.IList<ShippersInfo> shippers = SalesHelper.GetShippers(false);
				if (shippers != null && shippers.Count >= 1)
				{
					foreach (ShippersInfo current in shippers)
					{
						if (current.IsDefault)
						{
							this.ddlShoperTag.SelectedValue = current.ShipperId;
						}
					}
				}
				this.LoadShipper();
				this.LoadTemplates();
			}
		}
		private System.Data.DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return OrderHelper.GetOrdersAndLines(orderIds);
		}
		private void btnbtnPrint_Click(object sender, System.EventArgs e)
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
			if (!string.IsNullOrEmpty(this.UpdateOrderIds))
			{
				OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), this.templateName, this.templateName);
				OrderHelper.SetOrderShipNumber(array, this.mailNo, this.templateName);
				OrderHelper.SetOrderPrinted(array, true);
				return;
			}
			this.ShowMsg("订单当前状态不能打印！", false);
		}
		private void btnUpdateAddrdss_Click(object sender, System.EventArgs e)
		{
			if (!this.dropRegions.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择发货地区！", false);
				return;
			}
			if (this.UpdateAddress())
			{
				this.ShowMsg("修改成功", true);
				return;
			}
			this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
		}
		private bool UpdateAddress()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				shipper.Address = this.txtAddress.Text;
				shipper.CellPhone = this.txtCellphone.Text;
				shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
				shipper.ShipperName = this.txtShipTo.Text;
				shipper.TelPhone = this.txtTelphone.Text;
				shipper.Zipcode = this.txtZipcode.Text;
				return SalesHelper.UpdateShipper(shipper);
			}
			return false;
		}
		private void ddlShoperTag_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.LoadShipper();
		}
		private void LoadShipper()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				this.txtAddress.Text = shipper.Address;
				this.txtCellphone.Text = shipper.CellPhone;
				this.txtShipTo.Text = shipper.ShipperName;
				this.txtTelphone.Text = shipper.TelPhone;
				this.txtZipcode.Text = shipper.Zipcode;
				this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
				this.pnlEmptySender.Visible = false;
				this.pnlShipper.Visible = true;
				return;
			}
			this.pnlShipper.Visible = false;
			this.pnlEmptySender.Visible = true;
		}
		private void LoadTemplates()
		{
			System.Data.DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
			if (isUserExpressTemplates != null && isUserExpressTemplates.Rows.Count > 0)
			{
				this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
				foreach (System.Data.DataRow dataRow in isUserExpressTemplates.Rows)
				{
					this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem(dataRow["ExpressName"].ToString(), dataRow["XmlFile"].ToString()));
				}
				this.pnlEmptyTemplates.Visible = false;
				this.pnlTemplates.Visible = true;
				return;
			}
			this.pnlEmptyTemplates.Visible = true;
			this.pnlTemplates.Visible = false;
		}
	}
}
