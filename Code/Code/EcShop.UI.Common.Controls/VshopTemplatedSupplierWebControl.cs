using Ecdev.Weixin.MP.Api;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class VshopTemplatedSupplierWebControl : TemplatedWebControl
    {
        
        protected virtual string SkinPath
        {
            get
            {
                string vshopSkinPath = HiContext.Current.GetVshopSkinPath(null);
                if (this.SkinName.StartsWith(vshopSkinPath))
                {
                    return this.SkinName;
                }
                if (this.SkinName.StartsWith("/"))
                {
                    return vshopSkinPath + this.SkinName;
                }
                return vshopSkinPath + "/" + this.SkinName;
            }
        }

        private string skinName;
        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                value = value.ToLower(CultureInfo.InvariantCulture);
                if ( !value.EndsWith(".ascx"))
                {
                    return;
                }
                this.skinName = value;
            }
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (this.LoadThemedControl())
            {
                this.AttachChildControls();
                return;
            }
            throw new SkinNotFoundException(this.SkinPath);
        }
        protected virtual bool LoadThemedControl()
        {
            if (this.SkinFileExists && this.Page != null)
            {
                Control control = this.Page.LoadControl(this.SkinPath);
                control.ID = "_";
                this.Controls.Add(control);
                return true;
            }
            return false;
        }
    }
}
