using EcShop.ControlPanel.Commodities;
using EcShop.Entities.HS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.ControlPanel.Utility
{
    public class ElmentsListBox : ListBox
    {
        public int HS_ELMENTS_ID
        {
            get
            {
                int result = 0;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        result = int.Parse(this.Items[i].Value);
                    }
                }
                return result;
            }
            set
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Value == value.ToString())
                    {
                        this.Items[i].Selected = true;
                    }
                    else
                    {
                        this.Items[i].Selected = false;
                    }
                }
            }
        }
        public new IList<int> SelectedValue
        {
            get
            {
                IList<int> list = new List<int>();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        list.Add(int.Parse(this.Items[i].Value));
                    }
                }
                return list;
            }
            set
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.Items[i].Selected = false;
                }
                foreach (int current in value)
                {
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (this.Items[j].Value == current.ToString())
                        {
                            this.Items[j].Selected = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 申报要素编号
        /// </summary>
        public string HSElmentsName
        {
            get;
            set;
        }
        public override void DataBind()
        {
            this.Items.Clear();
            IList<ElmentsInfo> elmentsInfo;
            if (!string.IsNullOrEmpty(HSElmentsName))
            {
                elmentsInfo= HSCodeHelper.GetElmentsInfo(HSElmentsName);
            }
            else
            {
                elmentsInfo = HSCodeHelper.GetElmentsInfo();
            }
            
            foreach (ElmentsInfo elments in elmentsInfo)
            {
                this.Items.Add(new ListItem(elments.HSElmentsName,elments.HSElmentsID.ToString()));
            }
        }

    }
}
