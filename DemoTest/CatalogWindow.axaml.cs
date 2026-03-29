using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoTest.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace DemoTest;

public partial class CatalogWindow : Window
{
    User localUser;
    public CatalogWindow()
    {
        InitializeComponent();
        Visibility(3);
        Get();
        FioTextBlock.Text = "Гость";
        using var context = new TrenirovkaContext();
        LoadBox();

    }

    public CatalogWindow(User user)
    {
        InitializeComponent();
        localUser = user;
        Visibility(user.RoleId);
        FioTextBlock.Text = user.UserName;
        using var context = new TrenirovkaContext();
        Get();
        LoadBox();
    }

    public void Visibility(int roleId)
    {
        switch (roleId)
        {
            case 1: SearchBox.IsVisible = true; Sort.IsVisible = true; Filter.IsVisible = true; AddButton.IsVisible = true; OrderButton.IsVisible = true; break;
            case 2: SearchBox.IsVisible = true; Sort.IsVisible = true; Filter.IsVisible = true; OrderButton.IsVisible = true; break;
        }
    }

    private void Get()
    {
        using var context = new TrenirovkaContext();

        var allProducts = context.Tovars
                                .Include(x => x.Category)
                                .Include(x => x.Manufacturer)
                                .Include(x => x.TovarType)
                                .Include(x => x.Supplier)
                                .Include(x => x.Unit)
                                .ToList();


        switch (Sort.SelectedIndex)
        {
            case 0: // Сортировка по возрастанию количества
                allProducts = allProducts.OrderBy(x => x.Quantity).ToList();
                break;
            case 1: // Сортировка по убыванию количества
                allProducts = allProducts.OrderByDescending(x => x.Quantity).ToList();
                break;
            default: // Если не выбрано, сортируем по возрастанию
                allProducts = allProducts.OrderBy(x => x.Quantity).ToList();
                break;
        }

        if (Filter.SelectedItem != null && Filter.SelectedItem.ToString() != "Все поставщики")
        {
            allProducts = allProducts.Where(x => x.Supplier.SupplierName == Filter.SelectedItem.ToString()).ToList();
        }


        if (SearchBox != null && !string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            var searchTerm = SearchBox.Text.ToLower();
            allProducts = allProducts.Where(x =>
                // поиск по типу товара
                (x.TovarType != null && !string.IsNullOrWhiteSpace(x.TovarType.TovarTypeName) && x.TovarType.TovarTypeName.ToLower().Contains(searchTerm)) ||
                // поиск по категории
                (x.Category != null && !string.IsNullOrWhiteSpace(x.Category.CategoryName) && x.Category.CategoryName.ToLower().Contains(searchTerm)) ||
                // поиск по наименовании производителя
                (x.Manufacturer != null && !string.IsNullOrWhiteSpace(x.Manufacturer.ManufacturerName) && x.Manufacturer.ManufacturerName.ToLower().Contains(searchTerm)) ||

                (x.Supplier != null && !string.IsNullOrWhiteSpace(x.Supplier.SupplierName) && x.Supplier.SupplierName.ToLower().Contains(searchTerm)) ||
                //поиск по описанию
                (x.Description != null && !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(searchTerm))

            ).ToList();
        }


        TovarsBox.ItemsSource = allProducts;
    }

    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        Get(); // Обновляем каталог при вводе текста в SearchBox
    }

    private void Sort_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении сортировки
    }

    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении фильтра

    }


    private void LoadBox() //для комбобокса
    {
        using var context = new TrenirovkaContext();

        var sup = context.Suppliers.Select(x => x.SupplierName).ToList();

        sup.Add("Все поставщики");

        Filter.ItemsSource = sup.OrderByDescending(x => x == "Все поставщики");

        Filter.SelectedIndex = 0;


    }
    private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Предполагается, что у вас есть переменная User localUser; в этом окне
      
            var add = new AddEditTovar(); // Передаем текущего пользователя
            add.Show();
            this.Close(); // Закрываем текущее окно (CatalogWindow)


    }


    private void OrderButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        var ord = new OrderWindow(); 
        ord.Show();
        this.Close(); 

    }


    private void TovarsBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TovarsBox.SelectedItem is Tovar tovar)
        {
            if (localUser != null) // Проверяем, что пользователь существует
            {
                var addedit = new AddEditTovar(localUser, tovar); // Передаем пользователя и выбранный товар
                addedit.Show();
                this.Close(); // Закрываем текущее окно (CatalogWindow)
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пожалуйста, войдите в систему, чтобы редактировать товар.", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                error.ShowAsync();
            }
        }
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var main = new MainWindow();
        main.Show();
        this.Close();
    }




}