using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DemoTest.Models;
using MsBox.Avalonia;

namespace DemoTest;

public partial class AddEditTovar : Window
{
    User localUser;
    private Tovar updatetovar;
    private string ImageName;
    private string _currentPhotoPath;


    public AddEditTovar() //add
    {
        InitializeComponent();
        LoadManu();
        LoadSup();
        LoadCat();
        LoadUnit(); 
        LoadTovarType();
        DataContext = new Tovar();
        TovarArtTextBox.IsVisible = false;
        EditBut.IsVisible = false;
        DeleteBut.IsVisible = false;
        AddBut.IsVisible = true;
    }


    public AddEditTovar(User user)
    {
        localUser = user; 
    }

    public AddEditTovar(User user, Tovar tovar) //edit
    {
        InitializeComponent();
        using var context = new TrenirovkaContext();
        updatetovar = tovar;
        localUser = user;
        LoadManu();
        LoadSup();
        LoadCat();
        LoadUnit(); 
        LoadTovarType();
        DataContext = updatetovar;
        TovarArtTextBox.IsVisible = true;
        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;
        AddBut.IsVisible = false;
        ImageBox.Source = updatetovar.GetPhoto;

        Manufacturer.SelectedItem = updatetovar?.Manufacturer?.ManufacturerName;
        TovarType.SelectedItem = updatetovar?.TovarType?.TovarTypeName;
        Supplier.SelectedItem = updatetovar?.Supplier?.SupplierName;
        Category.SelectedItem = updatetovar?.Category?.CategoryName;
        Unit.SelectedItem = updatetovar?.Unit?.UnitName;
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }
        else
        {
            var catalogWindow = new CatalogWindow();
            catalogWindow.Show();
            this.Close();
        }
    }

    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new TrenirovkaContext();
            var newTovar = DataContext as Tovar;

            if (newTovar?.Price < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена не может быть отрицательной", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (newTovar?.Quantity < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество не может быть отрицательной", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }


            if (Manufacturer.SelectedItem != null && Supplier.SelectedItem != null && Category.SelectedItem != null && Unit.SelectedItem != null && TovarType.SelectedItem != null)
            {
                newTovar.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem!.ToString())!;
                newTovar.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem!.ToString())!;
                newTovar.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem!.ToString())!;
                newTovar.Unit = context.Units.FirstOrDefault(x => x.UnitName == Unit.SelectedItem!.ToString())!;
                newTovar.TovarType = context.TovarTypes.FirstOrDefault(x => x.TovarTypeName == TovarType.SelectedItem!.ToString())!;
                newTovar.TovarArt = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 6).Select(s => s[new Random().Next(s.Length)]).ToArray());
                newTovar.Photo = "images/" + ImageName;


                context.Tovars.Add(newTovar);
                await context.SaveChangesAsync();

                var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар создан", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await message.ShowAsync();

                if (Class1.isAdmin == true)
                {
                    var catalogWindow = new CatalogWindow(Class1._user);
                    catalogWindow.Show();
                    this.Close();
                }
                else
                {
                    var catalogWindow = new CatalogWindow();
                    catalogWindow.Show();
                    this.Close();
                }
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пожалуйста, заполните все поля", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }
    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Добавить изображение",
            FileTypeChoices = new[]
            {
            FilePickerFileTypes.All
        }
        });

        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            ImageName = Guid.NewGuid().ToString() + ".png";
            var targetPath = AppDomain.CurrentDomain.BaseDirectory + "/images/" + ImageName;
            File.Copy(file.Path.LocalPath, targetPath);

        }
    }

    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();

        var tovarId = updatetovar.TovarArt;

        var tovarToDelete = context.Tovars.Where(x => x.TovarArt == tovarId).FirstOrDefault();

        context.Tovars.Remove(tovarToDelete!);
        context.SaveChanges();

        var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар удален", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await message.ShowAsync();

        if (Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }
        else
        {
            var catalogWindow = new CatalogWindow();
            catalogWindow.Show();
            this.Close();
        }
    }


    private void LoadManu()
    {
        using var context = new TrenirovkaContext();
        Manufacturer.ItemsSource = context.Manufacturers.Select(x => x.ManufacturerName).ToList();
    }
    private void LoadSup()
    {
        using var context = new TrenirovkaContext();
        Supplier.ItemsSource = context.Suppliers.Select(x => x.SupplierName).ToList();

    }
    private void LoadCat()
    {
        using var context = new TrenirovkaContext();
        var cat = context.Categories.Select(x => x.CategoryName).ToList();
        Category.ItemsSource = cat;
    }

    private void LoadUnit()
    {
        using var context = new TrenirovkaContext();
        var unit = context.Units.Select(x => x.UnitName).ToList();
        Unit.ItemsSource = unit;
    }


    private void LoadTovarType()
    {
        using var context = new TrenirovkaContext();
        var tovartype = context.TovarTypes.Select(x => x.TovarTypeName).ToList();
        TovarType.ItemsSource = tovartype;
    }


    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();
        var updatetovar = DataContext as Tovar;

        try
        {

            updatetovar?.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem!.ToString())!;
            updatetovar?.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem!.ToString())!;
            updatetovar?.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem!.ToString())!;
            updatetovar?.Unit = context.Units.FirstOrDefault(x => x.UnitName == Unit.SelectedItem!.ToString())!;
            updatetovar?.TovarType = context.TovarTypes.FirstOrDefault(x => x.TovarTypeName == TovarType.SelectedItem!.ToString())!;

            if (!string.IsNullOrEmpty(ImageName))
            {
                updatetovar?.Photo = "images/" + ImageName; 
            }
            else if (!string.IsNullOrEmpty(_currentPhotoPath))
            {
                updatetovar?.Photo = _currentPhotoPath;
            }

            if (updatetovar?.Price < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена не может быть отрицательной", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (updatetovar?.Quantity < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество не может быть отрицательной", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }


            context.Tovars.Update(updatetovar);
            context.SaveChanges();



            var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар изменен", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await message.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var catalogWindow = new CatalogWindow(Class1._user);
                catalogWindow.Show();
                this.Close();
            }
            else
            {
                var catalogWindow = new CatalogWindow();
                catalogWindow.Show();
                this.Close();
            }

        }
        catch (Exception ex)
        {
            var exec = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", exec, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }

    }
}