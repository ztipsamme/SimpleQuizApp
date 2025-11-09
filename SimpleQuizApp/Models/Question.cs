using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleQuizApp.Models;

public class Question
{
    public string Statement { get; set; }
    public string Category { get; set; }

    public List<string> Options { get; set; } = new();

    public string CorrectOption { get; set; }

    public string ImageName { get; set; }

    [JsonConstructor]
    public Question(string statement, string category, string correctOption,
        List<string> options, string imageName)
    {
        Statement = statement;
        Category = category;
        CorrectOption = correctOption;
        Options.AddRange(options);
        ImageName = imageName;
    }

    public bool IsRightAnswer(string answer) => answer.Equals(CorrectOption,
        StringComparison.OrdinalIgnoreCase);
}