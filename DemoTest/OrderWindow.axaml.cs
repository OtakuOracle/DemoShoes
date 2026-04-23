using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoTest.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace DemoTest;

public partial class OrderWindow : Window
{
    User localUser;  

    public OrderWindow()
    {
        InitializeComponent();
        Get();
    }

    public OrderWindow(User user)
    {
        InitializeComponent();
        using var context = new TrenirovkaContext();
        //Visibility(user.RoleId);
        Get();
    }



    private void Get()
    {
        using var context = new TrenirovkaContext();

        var allOrders = context.Orders
                                .Include(x => x.OrderStatusNavigation)
                                .Include(x => x.PickUpPoint)
                                .Include(x => x.ProductInOrders)
                                .ToList();

        OrdersBox.ItemsSource = allOrders;
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
            var catalog = new CatalogWindow(); 
            catalog.Show();
         this.Close();
    }

}