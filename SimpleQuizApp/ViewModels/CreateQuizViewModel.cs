using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class CreateQuizViewModel : ViewModelBase
{
    public QuizFormViewModel QuizForm { get; }

    public CreateQuizViewModel(MainWindowViewModel main) : base(main)
    {
        QuizForm = new QuizFormViewModel(main);
    }
}