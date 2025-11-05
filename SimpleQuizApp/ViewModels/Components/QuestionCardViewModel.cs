using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuestionCardViewModel : ObservableObject
{
    [ObservableProperty]
    private ImageUploadViewModel _questionImageUpload = new();

    public string Statement { get; }
    public string CorrectOption { get; }
    public string Option1 { get; }
    public string Option2 { get; }
    public string Option3 { get; }

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