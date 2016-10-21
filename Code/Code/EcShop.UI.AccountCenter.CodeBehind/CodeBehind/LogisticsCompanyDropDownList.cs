using Commodities;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class LogisticsCompanyDropDownList : DropDownList
    {
        public override void DataBind()
        {
            this.Items.Clear();
            //this.DataSource = TradeHelper.GetPaymentModes(PayApplicationType.payOnPC);
            //this.DataTextField = "Name";
            //this.DataValueField = "ModeId";
            //foreach (PaymentModeInfo p in TradeHelper.GetPaymentModes(PayApplicationType.payOnPC))
            //{
            //    this.Items.Add(new ListItem(p.Name, p.ModeId.ToString()));
            //}
          
           // 物流选择绑定值
           
            IList<string> list = ExpressHelper.GetAllExpressName();
            this.DataSource = list.Select(a => new { Name=a,ModeId=a }).ToList();
            this.DataTextField = "Name";
            this.DataValueField = "ModeId";
            List<ListItem> item = new List<ListItem>();
            foreach (string s in list)
            {
                this.Items.Add(new ListItem(s, s));
            }
        }
    }
}
