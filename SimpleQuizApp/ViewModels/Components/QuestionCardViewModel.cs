using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuestionCardViewModel : ObservableObject
{
    [ObservableProperty]
    private ImageUploadViewModel _questionImageUpload = new();

    [ObservableProperty] private string _statement;
    [ObservableProperty] private string _correctOption;
    [ObservableProperty] private string _option1;
    [ObservableProperty] private string _option2;
    [ObservableProperty] private string _option3;

    [ObservableProperty] private bool _isExpanded;

    public ICommand? EditCommand { get; set; }
    public ICommand? RemoveCommand { get; set; }

    public QuestionCardViewModel(Question question, ICommand editCommand,
        ICommand removeCommand)
    {
        Statement = question.Statement;
        CorrectOption = question.CorrectOption;
        var options = question.Options;
        Option1 = options[0];
        Option2 = options[1];
        Option3 = options[2];
        QuestionImageUpload = new(question.ImageFileName);
        IsExpanded = false;
        EditCommand = editCommand;
        RemoveCommand = removeCommand;
    }

    public QuestionCardViewModel(ICommand editCommand, ICommand removeCommand)
    {
        IsExpanded = true;
        EditCommand = editCommand;
        RemoveCommand = removeCommand;
    }
}