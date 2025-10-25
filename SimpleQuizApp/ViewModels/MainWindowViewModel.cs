using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleQuizApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object _currentView;
    
    public MainWindowViewModel()
    {
        CurrentView = new HomeViewModel();
    }
    
    [RelayCommand]
    private void ShowCreateQuizView() => CurrentView = new CreateQuizViewModel();
    [RelayCommand]
    private void ShowEditQuizView() => CurrentView = new EditQuizViewModel();
    [RelayCommand]
    private void ShowPlayQuizView() => CurrentView = new PlayQuizViewModel();
}