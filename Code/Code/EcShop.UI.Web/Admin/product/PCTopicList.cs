using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.PCTopicManager)]
    public class PCTopicList : AdminPage
    {
        protected System.Web.UI.WebControls.LinkButton Lksave;
        protected System.Web.UI.WebControls.Repeater rpTopic;
        protected Pager pager;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindTopicList();
            }
        }
        protected void BindTopicList()
        {
            DbQueryResult dbQueryResult = VShopHelper.GettopicList(new TopicQuery
            {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "DisplaySequence",
                SortOrder = SortAction.Asc
            });
            this.rpTopic.DataSource = dbQueryResult.Data;
            this.rpTopic.DataBind();
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
        }
        protected void rpTopic_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int num = System.Convert.ToInt32(e.CommandArgument);
                if (VShopHelper.Deletetopic(num))
                {
                    VShopHelper.RemoveReleatesProductBytopicid(num);
                    this.ShowMsg("删除成功！", true);
                    this.BindTopicList();
                }
            }
        }
        protected void Lksave_Click(object sender, System.EventArgs e)
        {
            foreach (System.Web.UI.WebControls.RepeaterItem repeaterItem in this.rpTopic.Items)
            {
                int num = 0;
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)repeaterItem.FindControl("txtSequence");
                if (int.TryParse(textBox.Text.Trim(), out num))
                {
                    System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)repeaterItem.FindControl("Lbtopicid");
                    int num2 = System.Convert.ToInt32(label.Text);
                    TopicInfo topicInfo = VShopHelper.Gettopic(num2);
                    if (topicInfo.DisplaySequence != num)
                    {
                        VShopHelper.SwapTopicSequence(num2, num);
                    }
                }
            }
            this.BindTopicList();
        }
    }
}
