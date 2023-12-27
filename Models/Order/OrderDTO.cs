﻿namespace POS_System_DotNet.Models.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public double GivenMoney { get; set; }
        public double ExcessMoney { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
    }
}
