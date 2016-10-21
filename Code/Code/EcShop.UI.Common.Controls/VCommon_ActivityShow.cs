using EcShop.ControlPanel.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Common.Controls
{
    public class VCommon_ActivityShow : VAscxTemplatedWebControl
    {
        private System.Web.UI.WebControls.Repeater rptTopics;
        private string topicids = "";
        private int maxNum = 8;

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

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }


        public string Topicids
        {
            get
            {
                return this.topicids;
            }
            set
            {
                this.topicids = value;
            }
        }
        public VCommon_ActivityShow()
        {
            base.ID = "list_Common_ActivityShow";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/Tags/skin-Common_GoodsList_VActiveShow.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rptTopics = (System.Web.UI.WebControls.Repeater)this.FindControl("rptTopics");

            this.BindList();
        }

        private void BindList()
        {
            if (this.rptTopics != null)
            {
                var topicList = ProductHelper.GetActiveProductListByTopicIds(this.topicids, this.maxNum);
                this.rptTopics.DataSource = topicList;
                this.rptTopics.DataBind();
            }
        }


    }
}

