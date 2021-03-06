﻿namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class ItemsAllocationModelWithDelivery
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Trackingnumber { get; set; }
        public string ItemStatus { get; set; }
        public int StatusId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int TimeId { get; set; }
        public string TimeName { get; set; }
        public string DateToDeliver { get; set; }
        public string DeliveryMan { get; set; }

    }
}
