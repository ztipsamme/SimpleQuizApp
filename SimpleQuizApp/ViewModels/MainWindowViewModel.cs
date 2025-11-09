using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ViewModelBase _currentView;


    public MainWindowViewModel()
    {
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await FileService.InitialiseQuizzes();
        CurrentView = new HomeViewModel(this);
    }

    [RelayCommand]
    private void HomeView() => CurrentView = new HomeViewModel(this);

    [RelayCommand]
    private void ShowCreateQuizView() =>
        CurrentView = new CreateQuizViewModel(this);

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentView = viewModel;
    }
}