using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;

namespace Triarch.BusinessLogic.Models.Definitions;

public class Progression
{
    public string ProgressionType { get; set; } = null!;

    public List<string> ProgressionsList { get; set; } = new();

    public bool CustomProgression { get; set; } = false;  

    public Progression()
    {
        //Default Constructor for loading
    }

    public Progression(string progressionType, string[] progressionArray)
    {
        ProgressionType = progressionType;
        ProgressionsList = progressionArray.ToList<string>();
    }

    public ProgressionDto Serialize()
    {
        return new ProgressionDto { ProgressionType = this.ProgressionType ?? "", Progressions = this.ProgressionsList?.Select(x => new ProgressionEntryDto { Text = x }).ToList() ?? new List<ProgressionEntryDto>() };
    }

    public string GetProgressionValue(int rank)
    {
        if (rank > ProgressionsList.Count - 1)
        {
            //error: above maximum rank for calculations
            return "ERROR";
        }
        if (rank < 0)
        {
            //error: below minimum rank for calculations
            return "ERROR";
        }        

        return ProgressionsList[rank];
    }
}
