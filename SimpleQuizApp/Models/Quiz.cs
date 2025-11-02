using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleQuizApp.Models;

public class Quiz
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string CoverImageName { get; set; }
    public List<Question> Questions { get; set; }

    public Quiz(string title, string category, string description, string coverImageName,
        List<Question> questions)
    {
        Title = title;
        Category = category;
        Description = description;
        CoverImageName = coverImageName;
        Questions = questions;
    }
}