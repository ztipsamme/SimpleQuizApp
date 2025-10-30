using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleQuizApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ViewModelBase _currentView;


    public MainWindowViewModel()
    {
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