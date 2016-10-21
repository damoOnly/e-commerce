using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VSupplierDefault : VshopTemplatedWebControl
    {
        private int supplierId;
        //店铺收藏数
        private System.Web.UI.WebControls.Literal litSupplierFavCount;
        //店铺logo
        private HiImage imgSupplierLogo;
        

        //店铺名称
        private System.Web.UI.WebControls.Literal litSupplierName;
        //店主名称
        private System.Web.UI.WebControls.Literal litSupplierOwner;
        //店铺地址
        private System.Web.UI.WebControls.Literal litSupplierAddress;

        private System.Web.UI.WebControls.Literal isCollect;
        //开店时间
        private System.Web.UI.WebControls.Literal litSupplierCreateTime;


        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vSupplierdefault.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId))
            {
                base.GotoResourceNotFound("店铺已不存在");
            }
            //店铺收藏数
            this.litSupplierFavCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierFavCount");
            //店铺logo
            this.imgSupplierLogo = (HiImage)this.FindControl("imgSupplierLogo");

            this.isCollect = (System.Web.UI.WebControls.Literal)this.FindControl("isCollect");

            //店铺名称
            this.litSupplierName = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierName");
            //店主名称
            this.litSupplierOwner = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierOwner");
            //店铺地址
            this.litSupplierAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierAddress");
            //开店时间
            this.litSupplierCreateTime = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplierCreateTime");

            Member member = HiContext.Current.User as Member;
            int userId = 0;
            if (member != null)
            {
                userId = member.UserId;
            }
            AppSupplierInfo info = SupplierHelper.GetAppSupplier(supplierId, userId);

            if (info != null)
            {
                this.litSupplierFavCount.Text = info.CollectCount > 0 ? info.CollectCount.ToString() : "0";
                this.imgSupplierLogo.ImageUrl = info.Logo;
                this.litSupplierName.Text = info.ShopName;
                this.litSupplierOwner.Text = info.ShopOwner;
                this.isCollect.Text = info.IsCollect.ToString();
                if (info.CreateDate.HasValue)
                {
                    this.litSupplierAddress.Text = RegionHelper.GetFullRegion(info.County, ",").Split(',')[0];
                }
                if (info.County > 0)
                {
                    this.litSupplierCreateTime.Text = info.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

            }
            else
            {
                base.GotoResourceNotFound("店铺已不存在");
            }

        }
    }
}
