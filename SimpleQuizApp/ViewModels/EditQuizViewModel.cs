using SimpleQuizApp.Models;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class EditQuizViewModel : ViewModelBase
{
    public QuizFormViewModel QuizForm { get; }

    public EditQuizViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        QuizForm = new QuizFormViewModel(main, q);
    }
}