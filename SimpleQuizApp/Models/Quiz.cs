using System.Collections.ObjectModel;

namespace SimpleQuizApp.Models;

public class Quiz
{
    public string Title { get; set; }

    public ObservableCollection<Question> Questions { get; set; }

    public Quiz(string title, ObservableCollection<Question> questions)
    {
        Title = title;
        Questions = questions;
    }
}