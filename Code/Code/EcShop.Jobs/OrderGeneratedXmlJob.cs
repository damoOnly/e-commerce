using EcShop.Core.Jobs;
using EcShop.Membership.Context;
using EcShop.Messages;
using System;
using System.Globalization;
using System.Xml;
using System.Text;
using EcShop.Core.ErrorLog;
using EcShop.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.IO;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities;

namespace EcShop.Jobs
{
    /// <summary>
    /// 三单对碰作业【需提供回写文件夹权限，否则删除不了文件】
    /// </summary>
    public class OrderGeneratedXmlJob : IJob
    {
        private int number = 1;
        /// <summary>
        /// 流水号
        /// </summary>
        public int Number
        {
            get
            {
                if (number > 9999)
                {
                    number = 1;
                }
                return number;
            }
            set { number = value; }
        }

        private int errorCount = 0;
        /// <summary>
        /// 返回数据列表
        /// </summary>
        private ArrayList al = new ArrayList();
        private int tag = 0;
        /// <summary>
        /// 接收邮件人列表,错误提示
        /// </summary>
        private string[] strEailTo;

        /// <summary>
        /// 接收邮件人列表，成功提示
        /// </summary>
        private string[] arrEailTo;


        /// <summary>
        /// 配置
        /// </summary>
        private SiteSettings masterSettings;

        //private string localpath = "X:";

        //private string User;

        //private string Pwd;
        public void Execute(XmlNode node)
        {
            try
            {
                //自动生成有运单号且未通过的订单。OrderTemplate.xml 订单模板
                //配置信息
                masterSettings = SettingsManager.GetMasterSettings(true);
                if (masterSettings == null)
                {
                    errorCount++;
                    if (errorCount >= 2)
                    {
                        this.SendErrorEail("获取配置信息失败");
                        errorCount = 0;
                    }
                    return;
                }

                //保存地址
                string filename = node.Attributes["fileName"].InnerText;
                string templateName = node.Attributes["templateName"].InnerText;
                string templateNameByLog = node.Attributes["templateNameByLog"].InnerText;

                string writeBackName = node.Attributes["writeBackName"].InnerText;

                //User = node.Attributes["User"].InnerText;
                //Pwd = node.Attributes["Pwd"].InnerText;

                strEailTo = node.Attributes["ErrEailTo"].InnerText.Split(',');

                arrEailTo = node.Attributes["EailTo"].InnerText.Split(',');
                //网络目录判断有误
                //if (!Directory.Exists(filename))
                //{
                //    this.SendErrorEail("不存在文件保存目录：\"" + filename);
                //    return;
                //}

                //生成订单Xml数据
                this.GenerateXmlData(filename, templateName);

                //生成物流单Xml数据
                this.GenerateXmlDataByLog(filename, templateNameByLog);

                //网络目录判断有误
                //if (!Directory.Exists(writeBackName))
                //{
                //    this.SendErrorEail("不存在目录：\"" + writeBackName);
                //    return;
                //}
                //获取订单返回数据
                this.WriteBackStatus(writeBackName);
            }
            catch (Exception ex)
            {
                this.SendErrorEail("三单错误：" + ex.ToString());
            }
        }

        #region 生成订单Xml数据
        /// <summary>
        /// 生成订单Xml数据
        /// </summary>
        private void GenerateXmlData(string filename, string templateName)
        {
            //订单xml模板
            XmlDocument xd = new XmlDocument();
            xd.Load(Globals.MapPath(templateName));
            //1.获取符合条件的订单
            DataSet ds = HSCodeHelper.GetOrders();
            if (ds != null && ds.Tables.Count > 0)
            {
                //2.遍历订单生成xml文件到指定位置
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XmlDocument xdc = new XmlDocument();
                    xdc.LoadXml(xd.OuterXml);
                    //<MessageID>[报文唯一编号]</MessageID> 报文唯一编号:用于唯一标识报文，"CEB301"+"9位组织机构代码"+"yyyyMMddHHmmssSSS"+"4位流水号"
                    XmlNode xnMessageID = xdc.SelectSingleNode("/CEB301Message/MessageHead/MessageID");
                    //<OrgCode>[组织机构代码]</OrgCode> 
                    XmlNode xnOrgCode = xdc.SelectSingleNode("/CEB301Message/MessageHead/OrgCode");
                    xnOrgCode.InnerText = masterSettings.OrgCode;
                    StringBuilder MessageID = new StringBuilder("CEB301");
                    MessageID.AppendFormat("{0}{1}{2}", xnOrgCode.InnerText, DateTime.Now.ToString("yyyyMMddHHmmssfff"), Number.ToString().PadLeft(4, '0'));
                    xnMessageID.InnerText = MessageID.ToString();
                    //<SenderID>[发送方Id]</SenderID>
                    XmlNode xnSenderID = xdc.SelectSingleNode("/CEB301Message/MessageHead/SenderID");
                    xnSenderID.InnerText = masterSettings.SenderID;
                    //<SendTime>[发送时间yyyyMMddHHmmssSSS]</SendTime> 
                    XmlNode xnSendTime = xdc.SelectSingleNode("/CEB301Message/MessageHead/SendTime");
                    xnSendTime.InnerText = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    //订单实体数据
                    #region 订单实体数据
                    //<appTime>[申报时间YYYYMMDDhhmmss]</appTime> 
                    XmlNode xnAppTime = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/appTime");
                    xnAppTime.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //<orderNo>[订单编号]</orderNo> 
                    XmlNode xnOrderNo = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/orderNo");
                    xnOrderNo.InnerText = dr["OrderId"].ToString();
                    //<appUid>[持卡人Id]</appUid>
                    XmlNode xnAppUid = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/appUid");
                    xnAppUid.InnerText = masterSettings.appUid;
                    //<appUname>[持卡人姓名]</appUname>
                    XmlNode xnAppUname = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/appUname");
                    xnAppUname.InnerText = masterSettings.appUname;
                    //<ebpCode>[电商平台代码]</ebpCode>
                    XmlNode xnEbpCode = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/ebpCode");
                    xnEbpCode.InnerText = masterSettings.ebpCode;
                    //<ebpName>[电商平台名称]</ebpName>
                    XmlNode xnEbpName = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/ebpName");
                    xnEbpName.InnerText = masterSettings.ebpName;
                    //<currency>[币制]</currency>
                    XmlNode xnCurrency = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/currency");
                    xnCurrency.InnerText = masterSettings.currency;
                    //<consigneeCountry>[收货人所在国]</consigneeCountry>
                    XmlNode xnConsigneeCountry = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/consigneeCountry");
                    xnConsigneeCountry.InnerText = masterSettings.consigneeCountry;
                    //<ebcCode>[电商企业代码]</ebcCode> 
                    XmlNode xnEbcCode = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/ebcCode");
                    xnEbcCode.InnerText = masterSettings.ebcCode;
                    //<ebcName>电商企业名称</ebcName> 
                    XmlNode xnEbcName = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/ebcName");
                    xnEbcName.InnerText = masterSettings.ebcName;
                    //<goodsValue>[订单商品货款N19,5]</goodsValue>
                    XmlNode xnGoodsValue = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/goodsValue");
                    xnGoodsValue.InnerText = dr["Amount"].ToString();
                    //<freight>[境内运杂费N19,5]</freight> 
                    XmlNode xnFreight = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/freight");
                    xnFreight.InnerText = dr["AdjustedFreight"].ToString();
                    //<consignee>[收货人名称]</consignee> 
                    XmlNode xnConsignee = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/consignee");
                    xnConsignee.InnerText = dr["ShipTo"].ToString();
                    //<consigneeAddress>[收货人地址]</consigneeAddress> 
                    XmlNode xnConsigneeAddress = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/consigneeAddress");
                    xnConsigneeAddress.InnerText = dr["RealAddress"].ToString();
                    //<consigneeTelephone>[收货人电话]</consigneeTelephone> 
                    XmlNode xnConsigneeTelephone = xdc.SelectSingleNode("/CEB301Message/MessageBody/OrderBillHead/consigneeTelephone");
                    xnConsigneeTelephone.InnerText = dr["CellPhone"].ToString();
                    #endregion

                    //订单商品数据 <OrderBillList>
                    #region 订单商品数据
                    XmlNode MessageBody = xdc.SelectSingleNode("CEB301Message/MessageBody");
                    DataRow[] drs = ds.Tables[1].Select("OrderId='" + dr["OrderId"].ToString() + "'");
                    for (int i = 0, j = 1; i < drs.Length; i++)
                    {
                        XmlNode OrderBillList = xdc.CreateElement("OrderBillList");
                        MessageBody.AppendChild(OrderBillList);
                        DataRow drDetail = drs[i];
                        //生成<gnum>[商品序号]</gnum> 
                        XmlElement xeGnum = xdc.CreateElement("gnum");
                        xeGnum.InnerText = (i + j).ToString();
                        OrderBillList.AppendChild(xeGnum);
                        //<itemNo>[企业商品货号]</itemNo> 
                        XmlElement xeItemNo = xdc.CreateElement("itemNo");
                        xeItemNo.InnerText = drDetail["SKU"].ToString();
                        OrderBillList.AppendChild(xeItemNo);
                        //<gno>[海关商品备案编号]</gno> 
                        XmlElement xeGno = xdc.CreateElement("gno");
                        xeGno.InnerText = drDetail["ProductRegistrationNumber"].ToString();
                        OrderBillList.AppendChild(xeGno);
                        //<gcode>[海关商品编码]</gcode> 
                        XmlElement xeGcode = xdc.CreateElement("gcode");
                        xeGcode.InnerText = drDetail["HS_CODE"].ToString();
                        OrderBillList.AppendChild(xeGcode);
                        //<gname>[商品名称]</gname> 
                        XmlElement xeGname = xdc.CreateElement("gname");
                        xeGname.InnerText = drDetail["ProductName"].ToString();
                        OrderBillList.AppendChild(xeGname);
                        //<gmodel>[规格型号]</gmodel> 
                        XmlElement xeGmodel = xdc.CreateElement("gmodel");
                        xeGmodel.InnerText = drDetail["ItemNo"].ToString();
                        OrderBillList.AppendChild(xeGmodel);
                        //生成<barCode /> 
                        XmlElement xeBarCode = xdc.CreateElement("barCode");
                        OrderBillList.AppendChild(xeBarCode);
                        //<brand>[品牌]</brand> 
                        XmlElement xeBrand = xdc.CreateElement("brand");
                        xeBrand.InnerText = drDetail["BrandName"].ToString();
                        OrderBillList.AppendChild(xeBrand);
                        //<unit>[计量单位]</unit> 
                        XmlElement xeUnit = xdc.CreateElement("unit");
                        xeUnit.InnerText = drDetail["UnitCode"].ToString();
                        OrderBillList.AppendChild(xeUnit);
                        //<currency>[币制]</currency> 
                        XmlElement xeCurrency = xdc.CreateElement("currency");
                        xeCurrency.InnerText = masterSettings.currency;
                        OrderBillList.AppendChild(xeCurrency);
                        //<qty>[成交数量]</qty> 
                        XmlElement xeQty = xdc.CreateElement("qty");
                        xeQty.InnerText = drDetail["Quantity"].ToString();
                        OrderBillList.AppendChild(xeQty);
                        //<price>[单价]</price> 
                        XmlElement xePrice = xdc.CreateElement("price");
                        xePrice.InnerText = drDetail["ItemAdjustedPrice"].ToString();
                        OrderBillList.AppendChild(xePrice);
                        //<total>[总价]</total> 
                        XmlElement xeTotal = xdc.CreateElement("total");
                        xeTotal.InnerText = drDetail["TotalPrice"].ToString();
                        OrderBillList.AppendChild(xeTotal);
                        //<giftFlag>[是否赠品0否]</giftFlag> 
                        XmlElement xeGiftFlag = xdc.CreateElement("giftFlag");
                        xeGiftFlag.InnerText = "0";
                        OrderBillList.AppendChild(xeGiftFlag);
                        //生成<note /> 
                        XmlElement xeNote = xdc.CreateElement("note");
                        OrderBillList.AppendChild(xeNote);

                        //发货数量和采购数量有差异，判断是否有赠送（有买有送促销）
                        if (Convert.ToInt32(drDetail["PresentQuantity"]) > 0)
                        {
                            j++;
                            //生成<gnum>[商品序号]</gnum> 
                            XmlElement xeGnumNew = xdc.CreateElement("gnum");
                            xeGnumNew.InnerText = (i + j).ToString();
                            OrderBillList.AppendChild(xeGnumNew);
                            //<itemNo>[企业商品货号]</itemNo> 
                            XmlElement xeitemNoNew = xdc.CreateElement("itemNo");
                            xeitemNoNew.InnerText = xeItemNo.InnerText;
                            OrderBillList.AppendChild(xeitemNoNew);
                            //<gno>[海关商品备案编号]</gno> 
                            XmlElement xegnoNew = xdc.CreateElement("gno");
                            xegnoNew.InnerText = xeGno.InnerText;
                            OrderBillList.AppendChild(xegnoNew);
                            //<gcode>[海关商品编码]</gcode> 
                            XmlElement xegcodeNew = xdc.CreateElement("gcode");
                            xegcodeNew.InnerText = xeGcode.InnerText;
                            OrderBillList.AppendChild(xegcodeNew);
                            //<gname>[商品名称]</gname> 
                            XmlElement xegnameNew = xdc.CreateElement("gname");
                            xegnameNew.InnerText = xeGname.InnerText;
                            OrderBillList.AppendChild(xegnameNew);
                            //<gmodel>[规格型号]</gmodel> 
                            XmlElement xegmodelNew = xdc.CreateElement("gmodel");
                            xegmodelNew.InnerText = xeGmodel.InnerText;
                            OrderBillList.AppendChild(xegmodelNew);
                            //生成<barCode /> 
                            XmlElement xeBarCodeNew = xdc.CreateElement("barCode");
                            OrderBillList.AppendChild(xeBarCodeNew);

                            //<brand>[品牌]</brand> 
                            XmlElement xebrandNew = xdc.CreateElement("brand");
                            xebrandNew.InnerText = xeBrand.InnerText;
                            OrderBillList.AppendChild(xebrandNew);
                            //<unit>[计量单位]</unit> 
                            XmlElement xeunitNew = xdc.CreateElement("unit");
                            xeunitNew.InnerText = xeUnit.InnerText;
                            OrderBillList.AppendChild(xeunitNew);
                            //<currency>[币制]</currency> 
                            XmlElement xecurrencyNew = xdc.CreateElement("currency");
                            xecurrencyNew.InnerText = xeCurrency.InnerText;
                            OrderBillList.AppendChild(xecurrencyNew);
                            //<qty>[成交数量]</qty> 
                            XmlElement xeQtyNew = xdc.CreateElement("qty");
                            xeQtyNew.InnerText = drDetail["PresentQuantity"].ToString();
                            OrderBillList.AppendChild(xeQtyNew);
                            //<price>[单价]</price> 
                            XmlElement xepriceNew = xdc.CreateElement("price");
                            xepriceNew.InnerText = xePrice.InnerText;
                            OrderBillList.AppendChild(xepriceNew);
                            //<total>[总价]</total> 
                            XmlElement xeZSTotalPrice = xdc.CreateElement("total");
                            xeZSTotalPrice.InnerText = drDetail["ZSTotalPrice"].ToString();
                            OrderBillList.AppendChild(xeZSTotalPrice);
                            //<giftFlag>[是否赠品0否]</giftFlag> 
                            XmlElement xeGiftFlagNew = xdc.CreateElement("giftFlag");
                            xeGiftFlagNew.InnerText = "1";
                            OrderBillList.AppendChild(xeGiftFlagNew);
                            //生成<note /> 
                            XmlElement xeNoteNew = xdc.CreateElement("note");
                            OrderBillList.AppendChild(xeNoteNew);
                        }
                    }
                    #endregion

                    //xdc.Save(filename + "订单-" + MessageID.ToString() + ".xml");

                    ////gbk
                    //using (StreamWriter sw = new StreamWriter(filename + "订单-" + MessageID.ToString() + ".xml", false, Encoding.UTF8))
                    //{
                    //    xdc.Save(sw);
                    //}

                    //NetworkConnection.CreateNetWorkConnection(filename, User, Pwd, localpath);

                    using (StreamWriter sw = File.CreateText(filename + MessageID.ToString() + ".xml"))
                    {
                        sw.Write(xdc.OuterXml);
                        sw.Close();
                    }

                    Number++;
                    //订单保存数据库
                    try
                    {
                        HSCodeHelper.SaveHSDocking(1, dr["OrderId"].ToString(), "", MessageID.ToString(), 1, "已生成Xml数据", xdc.OuterXml, "", "", "0", "");
                    }
                    catch (Exception e)
                    {
                        this.SendErrorEail("三单错误—订单生成修改数据库状态错误：" + e.ToString());
                    }
                    //finally
                    //{
                    //    NetworkConnection.Disconnect(localpath);
                    //}
                }
            }
        }
        #endregion

        #region 生成运单Xml数据
        /// <summary>
        /// 生成运单Xml数据
        /// </summary>
        private void GenerateXmlDataByLog(string filename, string templateName)
        {
            //订单xml模板
            XmlDocument xd = new XmlDocument();
            xd.Load(Globals.MapPath(templateName));

            //1.获取符合条件的订单
            DataSet ds = HSCodeHelper.GetLogs();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[1] == null || ds.Tables[1].Rows.Count <= 0)
                {
                    this.SendErrorEail("三单错误：没有发货信息。");
                    return;
                }

                string shipper = ds.Tables[1].Rows[0]["ShipperName"].ToString();
                string shipperAddress = ds.Tables[1].Rows[0]["Address"].ToString();
                string shipperTelephone = ds.Tables[1].Rows[0]["CellPhone"].ToString();
                //2.遍历订单生成xml文件到指定位置
                int i = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ErrorLog.Write("查询订单,第" + i);
                    i++;
                    XmlDocument xdc = new XmlDocument();
                    xdc.LoadXml(xd.OuterXml);
                    //<MessageID>[报文唯一编号]</MessageID> 报文唯一编号:用于唯一标识报文，"CEB301"+"9位组织机构代码"+"yyyyMMddHHmmssSSS"+"4位流水号"
                    XmlNode xnMessageID = xdc.SelectSingleNode("/CEB501Message/MessageHead/MessageID");
                    //<OrgCode>[组织机构代码]</OrgCode> 
                    XmlNode xnOrgCode = xdc.SelectSingleNode("/CEB501Message/MessageHead/OrgCode");
                    xnOrgCode.InnerText = masterSettings.OrgCode;
                    StringBuilder MessageID = new StringBuilder("CEB301");
                    MessageID.AppendFormat("{0}{1}{2}", xnOrgCode.InnerText, DateTime.Now.ToString("yyyyMMddHHmmssfff"), Number.ToString().PadLeft(4, '0'));
                    xnMessageID.InnerText = MessageID.ToString();
                    //<SenderID>[发送方Id]</SenderID>
                    XmlNode xnSenderID = xdc.SelectSingleNode("/CEB501Message/MessageHead/SenderID");
                    xnSenderID.InnerText = masterSettings.SenderID;
                    //<SendTime>[发送时间yyyyMMddHHmmssSSS]</SendTime> 
                    XmlNode xnSendTime = xdc.SelectSingleNode("/CEB501Message/MessageHead/SendTime");
                    xnSendTime.InnerText = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    //订单实体数据 粤BL3727
                    #region 订单实体数据
                    //<customsCode>[主管海关代码]</customsCode> 
                    XmlNode xnCustomsCode = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/customsCode");
                    xnCustomsCode.InnerText = masterSettings.customsCode;
                    //<appTime>[申报时间YYYYMMDDhhmmss]</appTime> 
                    XmlNode xnAppTime = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/appTime");
                    xnAppTime.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //<orderNo>[订单编号]</orderNo> 
                    XmlNode xnOrderNo = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/orderNo");
                    xnOrderNo.InnerText = dr["OrderId"].ToString();
                    //<appUid>[持卡人Id]</appUid>
                    XmlNode xnAppUid = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/appUid");
                    xnAppUid.InnerText = masterSettings.appUid;
                    //<appUname>[持卡人姓名]</appUname>
                    XmlNode xnAppUname = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/appUname");
                    xnAppUname.InnerText = masterSettings.appUname;
                    //<ebpCode>[电商平台代码]</ebpCode>
                    XmlNode xnEbpCode = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/ebpCode");
                    xnEbpCode.InnerText = masterSettings.ebpCode;
                    //<ebpName>[电商平台名称]</ebpName>
                    XmlNode xnEbpName = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/ebpName");
                    xnEbpName.InnerText = masterSettings.ebpName;
                    //<logisticsCode>[物流企业代码]</logisticsCode>
                    XmlNode xnLogisticsCode = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/logisticsCode");
                    xnLogisticsCode.InnerText = masterSettings.logisticsCode;
                    //<logisticsName>[物流企业名称]</logisticsName>
                    XmlNode xnLogisticsName = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/logisticsName");
                    xnLogisticsName.InnerText = masterSettings.logisticsName;
                    //<logNo>[运单号]</logNo>
                    XmlNode xnLogNo = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/logNo");
                    xnLogNo.InnerText = dr["ShipOrderNumber"].ToString();
                    //<freight>[运费]</freight>
                    XmlNode xnFreight = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/freight");
                    xnFreight.InnerText = dr["AdjustedFreight"].ToString();
                    //<currency>[币制]</currency>
                    XmlNode xnCurrency = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/currency");
                    xnCurrency.InnerText = masterSettings.currency;
                    //<billNo>[提运单号]</billNo>
                    XmlNode xnBillNo = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/billNo");
                    xnBillNo.InnerText = dr["ShipOrderNumber"].ToString();
                    //<netWt>[净重]</netWt>
                    XmlNode xnNetWt = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/netWt");
                    xnNetWt.InnerText = dr["OrderWeight"].ToString();
                    //<weight>[毛重]</weight>
                    XmlNode xnWeight = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/weight");
                    xnWeight.InnerText = (Convert.ToDecimal(dr["OrderWeight"]) + Convert.ToDecimal(masterSettings.ContainerWeight)).ToString();
                    //<parcelInfo>[包裹信息]</parcelInfo>
                    XmlNode xnParcelInfo = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/parcelInfo");
                    xnParcelInfo.InnerText = dr["ShipOrderNumber"].ToString();
                    //<goodsInfo>[商品信息]</goodsInfo>
                    XmlNode xnGoodsInfo = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/goodsInfo");
                    xnGoodsInfo.InnerText = dr["GoodsInfo"].ToString();
                    //<consignee>[收货人名称]</consignee> 
                    XmlNode xnConsignee = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/consignee");
                    xnConsignee.InnerText = dr["ShipTo"].ToString();
                    //<consigneeCountry>[收货人所在国]</consigneeCountry>
                    XmlNode xnConsigneeCountry = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/consigneeCountry");
                    xnConsigneeCountry.InnerText = masterSettings.consigneeCountry;
                    //省、市、区划代码Province、City、District
                    string RegionId = dr["RegionId"].ToString();
                    string xpath = string.Format("//county[@id='{0}']", RegionId);
                    XmlDocument regionDocument = RegionHelper.GetRegionDocument(Globals.MapPath("/config/region.config"));
                    XmlNode xmlNode = regionDocument.SelectSingleNode(xpath);
                    //区
                    XmlNode xnDistrict = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/District");
                    if (xmlNode == null || xmlNode.Attributes["code"] == null || xmlNode.Attributes["code"].Value == "")
                    {
                        this.SendErrorEail("三单错误：收货地区xml错误，未找到节点或属性不存在。");
                        xnDistrict.InnerText = "000000";
                    }
                    else
                    {
                        xnDistrict.InnerText = xmlNode.Attributes["code"].Value;
                    }
                    ErrorLog.Write("获取省市县");
                    if (xnDistrict.InnerText.Length < 6)
                    {
                        xnDistrict.InnerText = "000000";
                    }
                    //市
                    XmlNode xnCity = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/City");
                    //省
                    XmlNode xnProvince = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/Province");
                    try
                    {
                        xnCity.InnerText = xmlNode.ParentNode.Attributes["code"].Value;
                        if (xnCity.InnerText.Length < 6)
                        {
                            xnCity.InnerText = "000000";
                        }

                        xnProvince.InnerText = xmlNode.ParentNode.ParentNode.Attributes["code"].Value;
                        if (xnProvince.InnerText.Length < 6)
                        {
                            xnProvince.InnerText = "000000";
                        }
                    }
                    catch (Exception)
                    {
                        xnCity.InnerText = "000000";
                        xnProvince.InnerText = "000000";
                    }

                    ErrorLog.Write("获取收货人");
                    //<consigneeAddress>[收货人地址]</consigneeAddress> 
                    XmlNode xnConsigneeAddress = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/consigneeAddress");
                    xnConsigneeAddress.InnerText = dr["RealAddress"].ToString();
                    //<consigneeTelephone>[收货人电话]</consigneeTelephone> 
                    XmlNode xnConsigneeTelephone = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/consigneeTelephone");
                    xnConsigneeTelephone.InnerText = dr["CellPhone"].ToString();

                    ErrorLog.Write("获取发货人信息");
                    //<shipper>[发货人名称]</shipper> 
                    XmlNode xnShipper = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/shipper");
                    xnShipper.InnerText = shipper;
                    //<shipperAddress>[发货人地址]</shipperAddress> 
                    XmlNode xnShipperAddress = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/shipperAddress");
                    xnShipperAddress.InnerText = shipperAddress;
                    //<shipperTelephone>[发货人电话]</shipperTelephone> 
                    XmlNode xnShipperTelephone = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/shipperTelephone");
                    xnShipperTelephone.InnerText = shipperTelephone;
                    //<shipperCountry>142</shipperCountry> 
                    XmlNode xnShipperCountry = xdc.SelectSingleNode("/CEB501Message/MessageBody/Logistics/shipperCountry");
                    xnShipperCountry.InnerText = masterSettings.shipperCountry;
                    #endregion
                    ErrorLog.Write("数据获取完毕");
                    //NetworkConnection.CreateNetWorkConnection(filename, User, Pwd, localpath);

                    using (StreamWriter sw = File.CreateText(filename + MessageID.ToString() + ".xml"))
                    {
                        sw.Write(xdc.OuterXml);
                        sw.Close();
                    }
                    ErrorLog.Write("保存文件");
                    Number++;
                    //订单生成修改数据库状态错误
                    try
                    {
                        HSCodeHelper.SaveHSDocking(4, dr["OrderId"].ToString(), dr["ShipOrderNumber"].ToString(), MessageID.ToString(), 1, "已生成Xml数据", xdc.OuterXml, "", "", "0", "");
                    }
                    catch (Exception e)
                    {
                        this.SendErrorEail("三单错误—订单生成修改数据库状态错误：" + e.ToString());
                    }
                    //finally
                    //{
                    //    NetworkConnection.Disconnect(localpath);
                    //}
                }
            }
        }
        #endregion

        #region 获取订单返回数据
        /// <summary>
        /// 获取订单返回数据
        /// </summary>
        /// <param name="writeBackName">回写目录</param>
        private void WriteBackStatus(string writeBackName)
        {
            //获得目录下所有文件
            al.Clear();
            tag = 0;
            //NetworkConnection.CreateNetWorkConnection(writeBackName, User, Pwd, localpath);
            this.GetAllDirList(writeBackName);
            //依次扫描文件回写状态
            foreach (string item in al)
            {
                //发送目录不需要
                if (item.IndexOf("\\send\\") > 0)
                {
                    continue;
                }
                string OrderId = "";
                string LogisticsNo = "";
                int Status = 0;
                string OPacketsID = "";
                string Remark = "";
                bool isOrder = true;
                XmlDocument xd = new XmlDocument();
                try
                {
                    using (FileStream fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        //加载文件流
                        xd.Load(fs);
                    }
                }
                catch (Exception io)
                {
                    this.SendErrorEail("三单错误—文件解析错误：" + item + "错误信息：" + io.ToString());
                    continue;
                }


                if (xd.SelectSingleNode("/CEB301Message") == null && xd.SelectSingleNode("/CEB501Message") == null)
                {
                    this.SendErrorEail("三单错误—文件格式不对：" + item);
                    continue;
                }
                //判断是订单还是运单 CEB301Message CEB501Message
                if (xd.SelectSingleNode("/CEB301Message") != null)
                {
                    isOrder = true;
                }
                else
                {
                    isOrder = false;
                }

                //接收目录
                if (item.IndexOf("\\receive\\") > 0)
                {
                    if (isOrder)
                    {
                        OPacketsID = xd.SelectSingleNode("/CEB301Message/MessageBody/ResultInformation/MessageID").InnerText;
                        //1-成功；2 -失败；3-其它
                        Status = xd.SelectSingleNode("/CEB301Message/MessageBody/ResultInformation/Result").InnerText == "1" ? 2 : 3;
                        Remark = Status == 2 ? "成功" : xd.SelectSingleNode("/CEB301Message/MessageBody/ResultInformation/NoteS").InnerText;
                        XmlNode xn = xd.SelectSingleNode("/CEB301Message/MessageBody/ResultInformation/FormID");
                        if (xn != null)
                        {
                            OrderId = xn.InnerText;
                        }
                    }
                    else
                    {
                        OPacketsID = xd.SelectSingleNode("/CEB501Message/MessageBody/ResultInformation/MessageID").InnerText;
                        //1-成功；2 -失败；3-其它
                        Status = xd.SelectSingleNode("/CEB501Message/MessageBody/ResultInformation/Result").InnerText == "1" ? 2 : 3;
                        Remark = Status == 2 ? "成功" : xd.SelectSingleNode("/CEB501Message/MessageBody/ResultInformation/NoteS").InnerText;
                        XmlNode xn = xd.SelectSingleNode("/CEB501Message/MessageBody/ResultInformation/FormID");
                        if (xn != null)
                        {
                            LogisticsNo = xn.InnerText;
                        }
                    }
                }
                else
                {
                    if (isOrder)
                    {
                        OPacketsID = xd.SelectSingleNode("/CEB301Message/MessageHead/MessageID").InnerText;
                    }
                    else
                    {
                        OPacketsID = xd.SelectSingleNode("/CEB501Message/MessageHead/MessageID").InnerText;
                    }
                    if (item.IndexOf("\\fail\\") > 0)
                    {
                        //上传失败目录
                        Remark = "上传失败";
                        Status = 3;
                    }
                    else if (item.IndexOf("\\signfail\\") > 0)
                    {
                        //加签失败目录
                        Remark = "加签失败";
                        Status = 3;
                    }
                    else if (item.IndexOf("\\success\\") > 0)
                    {
                        //上传成功目录
                        Remark = "上传成功";
                        Status = 1;
                    }
                    else
                    {
                        this.SendErrorEail("三单错误—目录错误：" + item);
                        continue;
                    }
                }

                if (Status == 3)
                {
                    //错误状态发送邮件
                    this.SendErrorEail("订单报文ID:" + OPacketsID + ",订单返回错误,错误信息[" + Remark + "]");
                }

                try
                {
                    if (OPacketsID != "" && Status != 0)
                    {
                        if (isOrder)
                        {
                            HSCodeHelper.SaveHSDocking(2, OrderId, LogisticsNo, OPacketsID, Status, Remark, "", "", "", "0", "");
                        }
                        else
                        {
                            HSCodeHelper.SaveHSDocking(5, OrderId, LogisticsNo, OPacketsID, Status, Remark, "", "", "", "0", "");
                            //运单是2发邮件通知关务
                            if (Status == 2)
                            {
                                this.SendEail("三单报关提示", "三单已成功，请录入出库", arrEailTo);
                            }
                        }

                        //把文件移到完成文件夹
                        string newFile = item.Replace("\\trans\\", "\\transbak\\");
                        string di = Path.GetDirectoryName(newFile);
                        if (!Directory.Exists(di))
                        {
                            Directory.CreateDirectory(di);
                        }
                        File.Move(item, newFile);
                    }
                }
                catch (Exception e)
                {
                    this.SendErrorEail("三单错误—获取返回数据错误：" + e.ToString());
                }
                //finally
                //{
                //    NetworkConnection.Disconnect(localpath);
                //}
            }
        }
        #endregion

        #region 获得目录下所有文件
        /// <summary>
        /// 获得目录下所有文件
        /// </summary>
        /// <param name="strBaseDir">根目录</param>
        public void GetAllDirList(string strBaseDir)
        {
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            DirectoryInfo[] diA = di.GetDirectories();
            if (tag == 0)
            {
                FileInfo[] fis2 = di.GetFiles();   //有关目录下的文件   
                for (int i2 = 0; i2 < fis2.Length; i2++)
                {
                    if (Path.GetExtension(fis2[i2].FullName) == ".xml" || Path.GetExtension(fis2[i2].FullName) == "")
                    {
                        al.Add(fis2[i2].FullName);
                        //fis2[i2].FullName是根目录中文件的绝对地址，把它记录在ArrayList中
                    }
                }
            }
            for (int i = 0; i < diA.Length; i++)
            {
                tag++;
                //diA[i].FullName是某个子目录的绝对地址，把它记录在ArrayList中
                DirectoryInfo di1 = new DirectoryInfo(diA[i].FullName);
                DirectoryInfo[] diA1 = di1.GetDirectories();
                FileInfo[] fis1 = di1.GetFiles();   //有关目录下的文件   
                for (int ii = 0; ii < fis1.Length; ii++)
                {
                    if (Path.GetExtension(fis1[ii].FullName) == ".xml" || Path.GetExtension(fis1[ii].FullName) == "")
                    {
                        al.Add(fis1[ii].FullName);
                        //fis1[ii].FullName是某个子目录中文件的绝对地址，把它记录在ArrayList中
                    }
                }
                GetAllDirList(diA[i].FullName);
            }
        }
        #endregion

        /// <summary>
        /// 发送错误邮件
        /// </summary>
        /// <param name="Content">内容</param>
        private void SendErrorEail(string Content)
        {
            //SendEail("三单错误", Content, strEailTo);
            ErrorLog.Write("三单错误:" + Content);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Content">内容</param>
        private void SendEail(string Title, string Content, string[] EailTo)
        {
            //if (masterSettings.EmailEnabled)
            //{
            //    string msg = "";
            //    SendStatus sendStatus = Messenger.SendMail(Title, Content, EailTo, null, masterSettings, out msg);
            //    if (sendStatus != SendStatus.Success)
            //    {
            //        ErrorLog.Write("三单错误:" + Content + "发送邮件错误：" + msg);
            //    }
            //}
            ErrorLog.Write("三单错误:" + Content);
        }
    }
}
