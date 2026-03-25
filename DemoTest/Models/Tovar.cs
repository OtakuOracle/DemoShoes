using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace DemoTest.Models;

public partial class Tovar
{
    public string TovarArt { get; set; } = null!;

    public int? TovarTypeId { get; set; }

    public int? UnitId { get; set; }

    public int? CategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? SupplierId { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }
    public string ColorDiscount
    {
        get
        {
            if (Discount > 15)
            {
                return "#2E8B57";
            }
            else
            {
                return "";
            }
        }
    }

    public int? Quantity { get; set; }
    public string ColorQuantity
    {
        get
        {
            if (Quantity == 0)
            {
                return "#8ac8ff";
            }
            else
            {
                return "";
            }
        }
    }


    public bool isAdmin
    {
        get
        {
            return Class1.isAdmin;
        }
    }

    public string? Description { get; set; }

    public string? Photo { get; set; }
    public Bitmap GetPhoto
    {
        get
        {
            if (Photo != null && Photo != "")
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Photo);
            }
            else
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/images/11.jpeg");
            }
        }
    }


    public virtual Category? Category { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<ProductInOrder> ProductInOrders { get; set; } = new List<ProductInOrder>();

    public virtual Supplier? Supplier { get; set; }

    public virtual TovarType? TovarType { get; set; }

    public virtual Unit? Unit { get; set; }
}
