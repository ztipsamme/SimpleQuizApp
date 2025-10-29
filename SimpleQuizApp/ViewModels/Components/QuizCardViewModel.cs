using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizCardViewModel : ViewModelBase
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private string? _coverImageName;
    [ObservableProperty] private Bitmap _coverImageSrc;
    [ObservableProperty] private ObservableCollection<Question> _questions;

    [ObservableProperty] private bool _hasImage;

    public QuizCardViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        Title = q.Title;
        Questions = new ObservableCollection<Question>(q.Questions);
        CoverImageName = q.CoverImageName;

        if (!string.IsNullOrEmpty(CoverImageName))
        {
            string? path = FileService.GetImageSrc(CoverImageName);

            if (path != null && File.Exists(path))
            {
                _ = Task.Run(() =>
                {
                    try
                    {
                        var bitmap = new Bitmap(path);
                        Dispatcher.UIThread.Post(() =>
                        {
                            CoverImageSrc = bitmap;
                            HasImage = true;
                        });
                    }
                    catch
                    {
                        Dispatcher.UIThread.Post(() => HasImage = false);
                    }
                });
            }
        }
    }

    [RelayCommand]
    public void OpenQuizView()
    {
        Main.NavigateTo(new PlayQuizViewModel(Main));
    }
}