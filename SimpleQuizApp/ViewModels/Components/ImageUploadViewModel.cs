using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.ViewModels.Components;

public partial class ImageUploadViewModel : ObservableObject
{
    [ObservableProperty] private string _imageName;
    [ObservableProperty] private string _tempImagePath;
    [ObservableProperty] private Bitmap _imageSrc;
    [ObservableProperty] private bool _hasImage;

    public ImageUploadViewModel(string? imageFileName = null)
    {
        if (imageFileName != null)
        {
            ImageName = imageFileName;
            _ = LoadImageAsync(ImageName);
        }
    }

    [RelayCommand]
    public async Task SelectImage()
    {
        var dlg = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters =
            {
                new FileDialogFilter
                    { Name = "Images", Extensions = { "png", "jpg", "jpeg" } }
            }
        };

        var window =
            (App.Current.ApplicationLifetime as
                IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        var res = await dlg.ShowAsync(window);

        if (res is { Length: > 0 })
        {
            TempImagePath = res[0];
            ImageName = Path.GetFileName(res[0]);
            await LoadImageAsync(TempImagePath);
        }
    }

    [RelayCommand]
    public void RemoveImage()
    {
        TempImagePath = null;
        ImageName = null;
        ImageSrc = null;
        HasImage = false;
    }

    public void SaveIfPresent()
    {
        if (!string.IsNullOrWhiteSpace(ImageName) &&
            !string.IsNullOrWhiteSpace(TempImagePath))
        {
            FileService.SaveImage(ImageName, TempImagePath);
        }
    }

    private async Task LoadImageAsync(string imageName)
    {
        ImageSrc = await ImageService.LoadAsync(imageName);
        HasImage = ImageSrc != null;
    }}