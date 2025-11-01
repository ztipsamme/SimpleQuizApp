using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizOptionButtonViewModel : ViewModelBase
{
    public int Number { get; }
    public string Text { get; }
    private PlayQuizViewModel _parent;
    [ObservableProperty] private string _bgColor = "Transparent";

    public QuizOptionButtonViewModel(int number, string text,
        PlayQuizViewModel parent, MainWindowViewModel main) : base(main)
    {
        Number = number;
        Text = text;
        _parent = parent;
    }

    [RelayCommand]
    public async Task SelectedAnswer()
    {
        _parent.SelectedOption = Text;

        if (_parent.CurrentQuestion.IsRightAnswer(Text))
            BgColor = "hsla(61, 78%, 46%, 1)";
        else if (Text == _parent.SelectedOption)
        {
            BgColor = "hsla(0, 88%, 66%, 1)";
        }

        await _parent.SelectedAnswer();
    }
}