using System;
using System.Collections.Generic;

namespace DemoTest.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly? DateCreate { get; set; }

    public DateOnly? DateDelivery { get; set; }

    public int? PickUpPointId { get; set; }

    public int? UserId { get; set; }

    public int? Code { get; set; }

    public int? OrderStatus { get; set; }

    public virtual OrderStatus? OrderStatusNavigation { get; set; }

    public virtual PickUpPoint? PickUpPoint { get; set; }

    public virtual ICollection<ProductInOrder> ProductInOrders { get; set; } = new List<ProductInOrder>();

    public virtual User? User { get; set; }
}
