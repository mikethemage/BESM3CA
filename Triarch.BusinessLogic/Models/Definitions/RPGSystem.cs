using System;
using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;


namespace Triarch.BusinessLogic.Models.Definitions;

public class RPGSystem
{
    //Properties:
    public List<RPGElementDefinition> ElementDefinitions { get; set; } = new();

    public List<RPGElementType> ElementTypes { get; set; } = new();

    public string SystemName { get; set; } = string.Empty;

    public List<Genre> Genres { get; set; } = new();

    public List<Progression> Progressions { get; set; } = new();

    public string GetProgression(string progressionType, int rank)
    {
        Progression? SelectedProgression = Progressions.Find(n => n.ProgressionType == progressionType);
        if (SelectedProgression == null)
        {
            return "";
        }
        else
        {
            return SelectedProgression.GetProgressionValue(rank);
        }
    }   
}
