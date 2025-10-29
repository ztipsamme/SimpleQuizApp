using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleQuizApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ViewModelBase _currentView;

    public HomeViewModel HomeVm { get; }
    public CreateQuizViewModel CreateQuizVm { get; }

    public MainWindowViewModel()
    {
        HomeVm = new HomeViewModel(this);
        CreateQuizVm = new CreateQuizViewModel(this);

        CurrentView = HomeVm;
    }

    [RelayCommand]
    private void HomeView() => CurrentView = HomeVm;

    [RelayCommand]
    private void ShowCreateQuizView() =>
        CurrentView = CreateQuizVm;

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentView = viewModel;
    }
}