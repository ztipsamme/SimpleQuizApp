using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels.Components;

public partial class ImageUploadViewModel : ObservableObject
{
    [ObservableProperty] private string _coverImageFileName;
    [ObservableProperty] private string _tempCoverImagePath;
    [ObservableProperty] private Bitmap _imageSrc;
    [ObservableProperty] private bool _hasImage;

    public ImageUploadViewModel(string? coverImageFile = null)
    {
        if (coverImageFile != null)
        {
            CoverImageFileName = coverImageFile;
            _ = LoadImageAsync(CoverImageFileName);
        }
    }

    [RelayCommand]
    public async Task SelectCoverImage()
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
            TempCoverImagePath = res[0];
            CoverImageFileName = Path.GetFileName(res[0]);
            HasImage = true;
            await LoadImageAsync(TempCoverImagePath);
        }
    }

    [RelayCommand]
    public void RemoveCoverImage()
    {
        TempCoverImagePath = null;
        CoverImageFileName = null;
        ImageSrc = null;
        HasImage = false;
    }

    public void SaveIfPresent()
    {
        if (!string.IsNullOrWhiteSpace(CoverImageFileName) &&
            !string.IsNullOrWhiteSpace(TempCoverImagePath))
        {
            FileService.SaveImage(CoverImageFileName, TempCoverImagePath);
        }
    }

    private async Task LoadImageAsync(string imgName)
    {
        var (src, hasImage) = await FileService.GetImageAsync(imgName);
        ImageSrc = src;
        HasImage = hasImage;
    }
}