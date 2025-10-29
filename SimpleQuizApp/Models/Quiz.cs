using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleQuizApp.Models;

public class Quiz
{
    public string Title { get; set; }
    public List<Question> Questions { get; set; }

    public Quiz(string title, List<Question> questions)
    {
        Title = title;
        Questions = questions;
    }
}