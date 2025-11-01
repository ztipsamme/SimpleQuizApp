using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleQuizApp.Models;

public class Question
{
    public string Statement { get; set; }

    public List<string> Options { get; set; } = new();

    public string CorrectOption { get; set; }

    public string ImagePath { get; set; }

    [JsonConstructor]
    public Question(string statement, string correctOption,
        List<string> options)
    {
        Statement = statement;
        CorrectOption = correctOption;
        Options.AddRange(options);
        Options.Add(CorrectOption);
    }

    public bool IsRightAnswer(string answer) => answer.Equals(CorrectOption,
        StringComparison.OrdinalIgnoreCase);
}