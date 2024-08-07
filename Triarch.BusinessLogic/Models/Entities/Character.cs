using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.BusinessLogic.Models.Entities;
public class Character : RPGElement
{
    public string CharacterName { get; set; } = string.Empty;

    public int Body { get; set; } = 1;
    public int Mind { get; set; } = 1;
    public int Soul { get; set; } = 1;

    public override int BaseCost
    {
        get
        {
            return (Body + Mind + Soul) * 10;
        }
    }

    public int BaseHealth
    {
        get
        {
            return (Body + Soul) * 5;
        }
    }

    public int BaseEnergy
    {
        get
        {
            return (Mind + Soul) * 5;
        }
    }

    public int BaseCV
    {
        get
        {
            return (Body + Mind + Soul) / 3;
        }
    }

    public int Health
    {
        get
        {
            int totalAdj = Children.Sum(x => x.HealthAdj);
            return BaseHealth + totalAdj;
        }
    }

    public int Energy
    {
        get
        {
            int totalAdj = Children.Sum(x => x.EnergyAdj);
            return BaseEnergy + totalAdj;
        }
    }

    public int ACV
    {
        get
        {
            int totalAdj = Children.Sum(x => x.ACVAdj);
            return BaseCV + totalAdj;
        }
    }

    public int DCV
    {
        get
        {
            int totalAdj = Children.Sum(x => x.DCVAdj);
            return BaseCV + totalAdj;
        }
    }

    public override int Points
    {
        get
        {
            return BaseCost + ChildPoints;
        }        
    }
}
