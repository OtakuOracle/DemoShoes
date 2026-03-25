using System;
using System.Collections.Generic;

namespace DemoTest.Models;

public partial class ProductInOrder
{
    public int ProductInOrderId { get; set; }

    public int? OrderId { get; set; }

    public string? TovarArt { get; set; }

    public int? Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Tovar? TovarArtNavigation { get; set; }
}
