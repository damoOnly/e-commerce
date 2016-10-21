using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_Actitvitys : AscxTemplatedWebControl
    {
        public const string TagID = "list_Actitvitys";
        private System.Web.UI.WebControls.Repeater rpActivity;
        private string times = "20151";
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
        public string Times
        {
            get
            {
                return this.times;
            }
            set
            {
                this.times = value;
            }
        }
        public Common_Actitvitys()
        {
            base.ID = "list_Actitvitys";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Actitvitys.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rpActivity = (System.Web.UI.WebControls.Repeater)this.FindControl("rpActivity");
            this.BindList();

        }
        private void BindList()
        {
            if (this.rpActivity != null)
            {
                ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
                productBrowseQuery.IsCount = true;
                productBrowseQuery.PageIndex = 1;
                productBrowseQuery.PageSize = 10;
                productBrowseQuery.SortBy = "DisplaySequence";
                productBrowseQuery.SortOrder = SortAction.Asc;
                DbQueryResult countDownProductList = ProductBrowser.GetCountDownProductList(productBrowseQuery);
                if (countDownProductList != null && countDownProductList.TotalRecords > 0)
                {
                    DataTable temp = countDownProductList.Data as DataTable;
                    DataRow[] curtemp = null;
                    if (this.times == "20151")
                    {
                        curtemp = temp.Select("StartDate = '2015-12-28 10:00:00.000'");
                    }
                    this.rpActivity.DataSource = curtemp;
                    this.rpActivity.DataBind();
                }
            }
        }

    }
}
