using Triarch.BusinessLogic.Models.Definitions;
using org.mariuszgromada.math.mxparser;

namespace Triarch.BusinessLogic.Models.Entities;
public class Levelable : RPGElement
{
    public int Level { get; set; } = 1;

    public int RequiredLevels { get; set; } = 0;

    public int FreeLevels { get; set; } = 0;

    public VariantDefinition? Variant { get; set; } = null;
    
    public override int BaseCost
    {
        get
        {
            return PointsPerLevel * (Level - FreeLevels);
        }
    }

    public virtual int PointsPerLevel 
    { 
        get
        {
            if(AssociatedDefinition is LevelableDefinition levelableDefinition)
            {
                if(levelableDefinition.Variants!=null && Variant != null)
                {
                    return Variant.CostPerLevel;
                }
                else
                {
                    return levelableDefinition.CostPerLevel ?? 0;
                }
            }
            else
            {
                return 0;
            }
        }  
    }

    public override int Points
    {
        get
        {
            return (PointsPerLevel * Level) + VariablesOrRestrictions;
        }        
    }

    protected virtual string ProcessDescriptionValue(string valueToParse)
    {
        if(AssociatedDefinition is LevelableDefinition levelableDefinition)
        {
            if (levelableDefinition.Progression != null)
            {                                
                if (levelableDefinition.Progression.CustomProgression)
                {
                    return levelableDefinition.Progression.Progressions[Level].Text;
                }
                else if (!levelableDefinition.Progression.Linear)
                {
                    string[] replacers = ["fn", "mn", "sn", "tn", "trn", "an", "rn", "tgn", "grn"];
                    foreach (var item in replacers)
                    {
                        if (int.TryParse(valueToParse.Replace(item, ""), out int i))
                        {
                            if (levelableDefinition.ProgressionReversed ?? false)
                            {
                                return levelableDefinition.Progression.Progressions[i - (Level - 1)].Text;
                            }
                            else
                            {
                                return levelableDefinition.Progression.Progressions[i - 1 + Level].Text;
                            }
                        }
                    }
                }

                valueToParse = valueToParse.Replace("n", Level.ToString());
                Expression e = new Expression(valueToParse);

                return e.calculate().ToString();
            }
        }
        return valueToParse;
    }

    public override int HealthAdj
    {
        get
        {            
            if (!Children.Any(x => x.AssociatedDefinition.ElementType.TypeName == "Restriction") && AssociatedDefinition.ElementName == "Tough")
            {
                return Level * 5;
            }            
            return base.HealthAdj;
        }
    }

    public override int EnergyAdj    
    {
        get
        {
            if (!Children.Any(x => x.AssociatedDefinition.ElementType.TypeName == "Restriction") && AssociatedDefinition.ElementName == "Energy Bonus")
            {
                return Level * 5;
            }
            return base.HealthAdj;
        }
    }

    public override int ACVAdj
    {
        get
        {
            if (!Children.Any(x => x.AssociatedDefinition.ElementType.TypeName == "Restriction") && AssociatedDefinition.ElementName == "Attack Combat Mastery")
            {
                return Level;
            }
            return base.HealthAdj;
        }
    }

    public override int DCVAdj
    {
        get
        {
            if (!Children.Any(x => x.AssociatedDefinition.ElementType.TypeName == "Restriction") && AssociatedDefinition.ElementName == "Defence Combat Mastery")
            {
                return Level;
            }
            return base.HealthAdj;
        }
    }
}
