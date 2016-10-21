using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.HS;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.sales
{
    public class ordeMonitoring : AdminPage
    {
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected HourDropDownList ddListHour;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ddListHour.DataBind();
                calendarStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                calendarEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                ddListHour.SelectedValue = 18;
            }
        }
    }
}
