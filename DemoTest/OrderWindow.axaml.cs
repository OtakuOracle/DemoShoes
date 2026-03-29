using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoTest;

public partial class OrderWindow : Window
{
    User localUser; // Теперь этот localUser будет инициализирован

    // Пустой конструктор, если он вам абсолютно необходим, но лучше его избегать
    // или сделать так, чтобы он тоже получал пользователя каким-то образом.
    public OrderWindow()
    {
        // Если пустой конструктор вызывается, нужно как-то получить пользователя.
        // Например, загрузить его из какого-то глобального хранилища или файла.
        // Если это невозможно, вызывайте краш или показывайте ошибку.
        // throw new InvalidOperationException("OrderWindow must be created with a User object.");
        InitializeComponent();
        Get();
        // Возможно, здесь тоже нужно что-то сделать с localUser, если он не null.
    }

    // Этот конструктор должен использоваться для создания OrderWindow
    public OrderWindow(User user)
    {
        InitializeComponent(); // Важно вызвать InitializeComponent()
        localUser = user;
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

            var catalog = new CatalogWindow(localUser);
            catalog.Show();
            this.Close();
      
    }
}
