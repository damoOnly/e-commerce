using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;

using Ecdev.TransferManager;

namespace Ecdev.Transfers.TaobaoImporters
{
    public class Yfx1_2_from_Taobao5_0 : ImportAdapter
    {
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/taobao"));
        private readonly Target _importTo = new YfxTarget("1.2");
        private DirectoryInfo _productImagesDir;
        private readonly Target _source = new TbTarget("5.0");
        private DirectoryInfo _workDir;
        private const string ProductFilename = "products.csv";

        public override object[] CreateMapping(params object[] initParams)
        {
            throw new NotImplementedException();
        }

        private DataTable GetProductSet()
        {
            DataTable table = new DataTable("products");
            DataColumn column = new DataColumn("ProductName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("ImageUrl1") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("ImageUrl2") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("ImageUrl3") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column5);
            DataColumn column6 = new DataColumn("ImageUrl4") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column6);
            DataColumn column7 = new DataColumn("ImageUrl5") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column7);
            DataColumn column8 = new DataColumn("SKU") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column8);
            DataColumn column9 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column9);
            DataColumn column10 = new DataColumn("SalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column10);
            DataColumn column11 = new DataColumn("Weight") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column11);
            DataColumn column12 = new DataColumn("Cid") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column12);
            DataColumn column13 = new DataColumn("StuffStatus") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column13);
            DataColumn column14 = new DataColumn("Num") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column14);
            DataColumn column15 = new DataColumn("LocationState") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column15);
            DataColumn column16 = new DataColumn("LocationCity") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column16);
            DataColumn column17 = new DataColumn("FreightPayer") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column17);
            DataColumn column18 = new DataColumn("PostFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column18);
            DataColumn column19 = new DataColumn("ExpressFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("EMSFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("HasInvoice") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column21);
            DataColumn column22 = new DataColumn("HasWarranty") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column22);
            DataColumn column23 = new DataColumn("HasDiscount") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column23);
            DataColumn column24 = new DataColumn("ValidThru") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column24);
            DataColumn column25 = new DataColumn("ListTime") {
                DataType = Type.GetType("System.DateTime")
            };
            table.Columns.Add(column25);
            DataColumn column26 = new DataColumn("PropertyAlias") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column26);
            DataColumn column27 = new DataColumn("InputPids") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column27);
            DataColumn column28 = new DataColumn("InputStr") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column28);
            DataColumn column29 = new DataColumn("SkuProperties") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column29);
            DataColumn column30 = new DataColumn("SkuQuantities") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column30);
            DataColumn column31 = new DataColumn("SkuPrices") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column31);
            DataColumn column32 = new DataColumn("SkuOuterIds") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column32);
            return table;
        }

        public override object[] ParseIndexes(params object[] importParams)
        {
            throw new NotImplementedException();
        }

        public override object[] ParseProductData(params object[] importParams)
        {
            string str = (string) importParams[0];
            HttpContext current = HttpContext.Current;
            DataTable productSet = this.GetProductSet();
            StreamReader reader = new StreamReader(Path.Combine(str, "products.csv"), Encoding.Unicode);
            string str2 = reader.ReadToEnd();
            reader.Close();
            str2 = str2.Substring(str2.IndexOf('\n') + 1);
            str2 = str2.Substring(str2.IndexOf('\n') + 1);
            StreamWriter writer = new StreamWriter(Path.Combine(str, "products.csv"), false, Encoding.Unicode);
            writer.Write(str2);
            writer.Close();
            using (CsvReader reader2 = new CsvReader(new StreamReader(Path.Combine(str, "products.csv"), Encoding.Default), true, '\t'))
            {
                int num = 0;
                while (reader2.ReadNextRecord())
                {
                    num++;
                    DataRow row = productSet.NewRow();
                    new Random();
                    row["SKU"] = reader2["商家编码"];
                    row["SalePrice"] = decimal.Parse(reader2["宝贝价格"]);
                    row["Num"] = 0;
                    if (!string.IsNullOrEmpty(reader2["宝贝数量"]))
                    {
                        row["Num"] = row["Stock"] = Convert.ToInt64(reader2["宝贝数量"]);
                    }
                    row["ProductName"] = this.Trim(reader2["宝贝名称"]);
                    if (!string.IsNullOrEmpty(reader2["宝贝描述"]))
                    {
                        row["Description"] = this.Trim(reader2["宝贝描述"].Replace("\"\"", "\"").Replace("alt=\"\"", "").Replace("alt=\"", "").Replace("alt=''", ""));
                    }
                    string str3 = this.Trim(reader2["新图片"]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        if (str3.EndsWith(";"))
                        {
                            string[] strArray = str3.Split(new char[] { ';' });
                            for (int i = 0; i < (strArray.Length - 1); i++)
                            {
                                string str4 = strArray[i].Substring(0, strArray[i].IndexOf(":"));
                                string str5 = str4 + ".jpg";
                                if (File.Exists(Path.Combine(str + @"\products", str4 + ".tbi")))
                                {
                                    File.Copy(Path.Combine(str + @"\products", str4 + ".tbi"), current.Request.MapPath("~/Storage/master/product/images/" + str5), true);
                                    switch (i)
                                    {
                                        case 0:
                                            row["ImageUrl1"] = "/Storage/master/product/images/" + str5;
                                            break;

                                        case 1:
                                            row["ImageUrl2"] = "/Storage/master/product/images/" + str5;
                                            break;

                                        case 2:
                                            row["ImageUrl3"] = "/Storage/master/product/images/" + str5;
                                            break;

                                        case 3:
                                            row["ImageUrl4"] = "/Storage/master/product/images/" + str5;
                                            break;

                                        case 4:
                                            row["ImageUrl5"] = "/Storage/master/product/images/" + str5;
                                            break;
                                    }
                                }
                            }
                        }
                        else if (File.Exists(Path.Combine(str + @"\products", str3.Replace(".jpg", ".tbi"))))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3.Replace(".jpg", ".tbi")), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl1"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    row["Cid"] = 0;
                    if (!string.IsNullOrEmpty(reader2["宝贝类目"]))
                    {
                        row["Cid"] = Convert.ToInt64(reader2["宝贝类目"]);
                    }
                    row["StuffStatus"] = (reader2["新旧程度"] == "1") ? "new" : "second";
                    row["LocationState"] = reader2["省"];
                    row["LocationCity"] = reader2["城市"];
                    row["FreightPayer"] = (reader2["运费承担"] == "1") ? "seller" : "buyer";
                    try
                    {
                        row["PostFee"] = decimal.Parse(reader2["平邮"]);
                        row["ExpressFee"] = decimal.Parse(reader2["快递"]);
                        row["EMSFee"] = decimal.Parse(reader2["EMS"]);
                    }
                    catch
                    {
                        row["PostFee"] = 0M;
                        row["ExpressFee"] = 0M;
                        row["EMSFee"] = 0M;
                    }
                    row["HasInvoice"] = reader2["发票"] == "1";
                    row["HasWarranty"] = reader2["保修"] == "1";
                    row["HasDiscount"] = reader2["会员打折"] == "1";
                    if (!string.IsNullOrEmpty(reader2["有效期"]))
                    {
                        row["ValidThru"] = long.Parse(reader2["有效期"]);
                    }
                    if (!string.IsNullOrEmpty(reader2["开始时间"]))
                    {
                        row["ListTime"] = DateTime.Parse(reader2["开始时间"]);
                    }
                    row["PropertyAlias"] = reader2["宝贝属性"];
                    row["InputPids"] = reader2["用户输入ID串"];
                    row["InputStr"] = reader2["用户输入名-值对"];
                    string str6 = string.Empty;
                    string str7 = string.Empty;
                    string str8 = string.Empty;
                    string str9 = string.Empty;
                    string str10 = reader2["销售属性组合"];
                    if (!string.IsNullOrEmpty(str10))
                    {
                        string pattern = @"(?<Price>[^:]+):(?<Quantities>[^:]+):(?<Outid>[^:]*):(?<Skuprop>[^;]+;(?:\d+:\d+;)?)";
                        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                        foreach (Match match in regex.Matches(str10))
                        {
                            str7 = str7 + match.Groups["Quantities"] + ",";
                            str6 = str6 + match.Groups["Price"] + ",";
                            str8 = str8 + match.Groups["Outid"] + ",";
                            str9 = str9 + match.Groups["Skuprop"].ToString().Substring(0, match.Groups["Skuprop"].ToString().Length - 1) + ",";
                        }
                        if (str7.Length > 0)
                        {
                            str7 = str7.Substring(0, str7.Length - 1);
                        }
                        if (str6.Length > 0)
                        {
                            str6 = str6.Substring(0, str6.Length - 1);
                        }
                        if (str8.Length > 0)
                        {
                            str8 = str8.Substring(0, str8.Length - 1);
                        }
                        if (str9.Length > 0)
                        {
                            str9 = str9.Substring(0, str9.Length - 1);
                        }
                    }
                    row["SkuProperties"] = str9;
                    row["SkuQuantities"] = str7;
                    row["SkuPrices"] = str6;
                    row["SkuOuterIds"] = str8;
                    productSet.Rows.Add(row);
                }
            }
            return new object[] { productSet };
        }

        public override string PrepareDataFiles(params object[] initParams)
        {
            string path = (string) initParams[0];
            this._workDir = this._baseDir.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(this._baseDir.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(this._workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return this._workDir.FullName;
        }

        private string Trim(string str)
        {
            while (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            while (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public override Target ImportTo
        {
            get
            {
                return this._importTo;
            }
        }

        public override Target Source
        {
            get
            {
                return this._source;
            }
        }
    }
}

