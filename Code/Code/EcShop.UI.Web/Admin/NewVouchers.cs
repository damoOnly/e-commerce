using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.NewVouchers)]
    public class NewVouchers : AdminPage
    {
        protected Grid grdVouchers;
        protected System.Web.UI.WebControls.HiddenField txtvoucherid;
        protected System.Web.UI.WebControls.TextBox tbvoucherNum;
        protected System.Web.UI.WebControls.Button btnExport;
        protected Pager pager;
        protected System.Web.UI.WebControls.RadioButton radComplex;
        protected System.Web.UI.WebControls.RadioButton radSimple;
        protected System.Web.UI.WebControls.HiddenField hiddenPwdtype;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.grdVouchers.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdVouchers_RowDeleting);
            this.grdVouchers.ReBindData += new Grid.ReBindDataEventHandler(this.grdVouchers_ReBindData);
            if (!this.Page.IsPostBack)
            {
                this.BindVouchers();
            }

        }

        private void grdVouchers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int voucherId = (int)this.grdVouchers.DataKeys[e.RowIndex].Value;
            if(VoucherHelper.GetVoucherItemAmount(voucherId)>0)
            {
                this.ShowMsg("现金券已经发送给用户，无法删除",true);

                return;

            }
            if (VoucherHelper.DeleteVoucher(voucherId))
            {
                this.BindVouchers();
                this.ShowMsg("成功删除了选定现金券", true);
                return;
            }
            this.ShowMsg("删除现金券失败", false);
        }
        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            //默认为复杂密码
            int pwdtype = 1;

            if (!string.IsNullOrEmpty(hiddenPwdtype.Value))
            {
                pwdtype = int.Parse(hiddenPwdtype.Value);
            }

            int num;
            if (!int.TryParse(this.tbvoucherNum.Text, out num))
            {
                this.ShowMsg("导出数量必须为正数", false);
                return;
            }
            if (num <= 0)
            {
                this.ShowMsg("导出数量必须为正数", false);
                return;
            }
            int voucherId;
            if (!int.TryParse(this.txtvoucherid.Value, out voucherId))
            {
                this.ShowMsg("参数错误", false);
                return;
            }
            VoucherInfo voucher = VoucherHelper.GetVoucher(voucherId);
            string empty = string.Empty;
            VoucherActionStatus voucherActionStatus = VoucherHelper.CreateVoucher(voucher, num, out empty, pwdtype);
            if (voucherActionStatus == VoucherActionStatus.UnknowError)
            {
                this.ShowMsg("未知错误", false);
                return;
            }
            if (voucherActionStatus == VoucherActionStatus.CreateClaimCodeError)
            {
                this.ShowMsg("生成现金券号码错误", false);
                return;
            }
            if (voucherActionStatus == VoucherActionStatus.CreateClaimCodeSuccess && !string.IsNullOrEmpty(empty))
            {
                System.Collections.Generic.IList<VoucherItemInfo> voucherItemInfos = VoucherHelper.GetVoucherItemInfos(empty);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                stringBuilder.AppendLine("<td>现金券号码</td>");
                stringBuilder.AppendLine("<td>现金券密码</td>");
                stringBuilder.AppendLine("<td>现金券金额</td>");
                stringBuilder.AppendLine("<td>过期时间</td>");
                stringBuilder.AppendLine("</tr>");
                foreach (VoucherItemInfo current in voucherItemInfos)
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine("<td>" + current.ClaimCode + "</td>");
                    stringBuilder.AppendLine("<td>" + current.Password + "</td>");
                    stringBuilder.AppendLine("<td>" + voucher.DiscountValue + "</td>");
                    stringBuilder.AppendLine("<td>" + current.Deadline + "</td>");
                    stringBuilder.AppendLine("</tr>");
                }
                stringBuilder.AppendLine("</table>");
                this.Page.Response.Clear();
                this.Page.Response.Buffer = false;
                this.Page.Response.Charset = "GB2312";
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=VoucherInfo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                this.Page.Response.ContentType = "application/ms-excel";
                this.Page.EnableViewState = false;
                this.Page.Response.Write(stringBuilder.ToString());
                hiddenPwdtype.Value = "";
                this.Page.Response.End();
            }
        }
        protected bool IsVoucherEnd(object endtime)
        {
            if(endtime==null)
            {
                return false;
            }
            return System.Convert.ToDateTime(endtime).CompareTo(System.DateTime.Now) > 0;
        }
        private void grdVouchers_ReBindData(object sender)
        {
            this.BindVouchers();
        }
        private void BindVouchers()
        {
            DbQueryResult newVouchers = VoucherHelper.GetNewVouchers(new Pagination
            {
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex
            });
            this.grdVouchers.DataSource = newVouchers.Data;
            this.grdVouchers.DataBind();
            this.pager.TotalRecords = newVouchers.TotalRecords;
        }

        protected bool IsBySelf(object sendtype)
        {
            if(string.IsNullOrEmpty(sendtype.ToString()))
            {
                return false;
            }
            if (int.Parse(sendtype.ToString()) == 3)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
