using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Comments
{
    public class TaxRateInfo
	{
        public int TaxRateId
		{
			get;
			set;
		}

        public decimal TaxRate
		{
			get;
			set;
		}

        /// <summary>
        /// ––” ±‡¬Î
        /// </summary>
        public string PersonalPostalArticlesCode
		{
			get;
			set;
		}
	}
}
