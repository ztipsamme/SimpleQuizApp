using System.Collections.Generic;

namespace SimpleQuizApp.Models;

public class Question
{
    public string Statement { get; set; }

    public List<string> Options { get; set; } = new();

    public string CorrectOption { get; set; }
    
    public string ImagePath { get; set; }

    public Question(string statement, string correctOption,
        params string[] options)
    {
        Statement = statement;
        CorrectOption = correctOption;
        Options.AddRange(options);
    }
}