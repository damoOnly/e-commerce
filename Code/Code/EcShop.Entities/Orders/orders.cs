using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.Entities.Orders
{
    public class AdOrderInfo
    {
        public List<orders> orders
        {
            get;
            set;
        }
    }
    public class AdOrerStatus
    {
        public List<orderStatus> orderStatus
        {
            get;
            set;
        }
    }
    public class orderStatus
    {
        public string feedback
        {
            get;
            set;
        }
        public string orderNo
        {
            get;
            set;
        }
        public string orderstatus
        {
            get;
            set;
        }
        public string paymentStatus
        {
            get;
            set;
        }
        public string paymentType
        {
            get;
            set;
        }
        public string updateTime
        {
            get;
            set;
        }
    
    }
    public class orders
    {
        private string orderNo;

        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        private int campaignid;

        public int Campaignid
        {
            get { return campaignid; }
            set { campaignid = value; }
        }
        private string feedback;

        public string Feedback
        {
            get { return feedback; }
            set { feedback = value; }
        }
        private string orderTime;

        public string OrderTime
        {
            get { return orderTime; }
            set { orderTime = value; }
        }

        private string updateTime;

        public string UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }
      
     
        private double fare;

        public double Fare
        {
            get { return fare; }
            set { fare = value; }
        }
        private double favorable;

        public double Favorable
        {
            get { return favorable; }
            set { favorable = value; }
        }
        private string favorableCode;

        public string FavorableCode
        {
            get { return favorableCode; }
            set { favorableCode = value; }
        }
        private string orderStatus;

        public string OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }
        private string paymentStatus;

        public string PaymentStatus
        {
            get { return paymentStatus; }
            set { paymentStatus = value; }
        }
        private string paymentType;

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        public List<products> products
        {
            get;
            set;
        }
        /// <summary>
        /// 是否退款
        /// </summary>
        public int IsRefund
        {
            get;
            set;
        }
        /// <summary>
        /// 是否执行取消
        /// </summary>
        public int IsCancelOrder
        {
            get;
            set;
        }
    }
    public class products
    {
        private int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string productNo;

        public string ProductNo
        {
            get { return productNo; }
            set { productNo = value; }
        }
        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        private string commissionType;

        public string CommissionType
        {
            get { return commissionType; }
            set { commissionType = value; }
        }
    
    }
}
