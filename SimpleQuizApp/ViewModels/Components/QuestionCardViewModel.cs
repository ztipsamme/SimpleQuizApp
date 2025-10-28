using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuestionCardViewModel : ViewModelBase
{
    [ObservableProperty] private string _statement;
    [ObservableProperty] private string _correctOption;
    [ObservableProperty] private string _option1;
    [ObservableProperty] private string _option2;
    [ObservableProperty] private string _option3;

    [ObservableProperty] private bool _isExpanded;

    public ICommand? EditCommand { get; set; }
    public ICommand? RemoveCommand { get; set; }

    public QuestionCardViewModel(ICommand editCommand, ICommand removeCommand)
    {
        IsExpanded = true;
        EditCommand = editCommand;
        RemoveCommand = removeCommand;
    }
}