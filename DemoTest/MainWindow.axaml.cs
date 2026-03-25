using System.Linq;
using Avalonia.Controls;
using DemoTest.Models;
using MsBox.Avalonia;

namespace DemoTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Guest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CatalogWindow productsWindw = new CatalogWindow();
        productsWindw.Show();
        Close();
    }

    private async void Auth_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();
        var login = LoginTextBox.Text;
        var password = PasswordTextBox.Text;

        var user = context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user != null)
            {
                if (user.RoleId == 1)
                {
                    Class1.isAdmin = true;
                }

                CatalogWindow productsWindw = new CatalogWindow(user);
                productsWindw.Show();
                Close();
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректные данные", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await message.ShowAsync();
            }
    }
}