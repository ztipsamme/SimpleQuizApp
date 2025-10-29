using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleQuizApp.Models;

public class Quiz
{
    public string Title { get; set; }
    public string Category { get; set; }
    public string CoverImageName { get; set; }
    public List<Question> Questions { get; set; }

    public Quiz(string title, string category, string coverImageName, List<Question> questions)
    {
        Title = title;
        Category = category;
        CoverImageName = coverImageName;
        Questions = questions;
    }
}