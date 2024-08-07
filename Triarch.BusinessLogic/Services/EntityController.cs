using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Definitions;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.BusinessLogic.Services;
public class EntityController
{
    public void AddElement(RPGElement parent, RPGElementDefinition definitionToAdd)
    {
        RPGElement elementToAdd = definitionToAdd.CreateNode(parent.Entity, "", false);
        parent.Children.Add(elementToAdd);

        foreach (Freebie freebieDefinition in definitionToAdd.Freebies)
        {
            if (freebieDefinition.FreebieElementDefinition is LevelableDefinition levelableFreebieDefinition)
            {
                elementToAdd.Children.Add(levelableFreebieDefinition.CreateNode(elementToAdd.Entity, "", freebieDefinition.FreeLevels + freebieDefinition.RequiredLevels, true, freebieDefinition.FreeLevels, freebieDefinition.RequiredLevels));
            }
            else
            {
                elementToAdd.Children.Add(freebieDefinition.FreebieElementDefinition.CreateNode(elementToAdd.Entity, "", true));
            }
        }
    }

    public void DeleteElement(RPGElement elementToDelete)
    {
        if (elementToDelete.Parent != null)
        {
            Queue<RPGElement> tempQueue = new Queue<RPGElement>([elementToDelete]);
            Stack<RPGElement> deletionStack = new Stack<RPGElement>();
            while (tempQueue.Count > 0)
            {
                RPGElement current = tempQueue.Dequeue();
                foreach (RPGElement child in current.Children)
                {
                    tempQueue.Enqueue(child);
                }
                deletionStack.Push(current);
            }

            while (deletionStack.Count > 0)
            {
                RPGElement current = deletionStack.Pop();
                if(current.Parent != null)
                {
                    current.Parent.Children.Remove(current);
                    current.Parent = null;
                }                
            }
        }
    }

    public void UpdateLevel(Levelable levelable, int newLevel)
    {
        if (levelable.Level != newLevel)
        {
            levelable.Level = newLevel;
        }
    }
}
