using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class Action
{
    public string name;
    public int priority;
    /// <summary>
    /// Stores which resources are spent, and the amount spent
    /// </summary>
    public Dictionary<string, int> resourceCosts = new Dictionary<string, int>();
    public int timeCost;

    /// <summary>
    /// Performs the action.
    /// </summary>
    public abstract void perform(Creature creature);


    public bool spendTimeAndResources(Creature creature)
    {
        bool enoughToSpend = true;
        creature.remainingTurnTime -= timeCost;
        foreach (string resource in resourceCosts.Keys)
        {
            if (creature.storedResources[resource].currentLevel > resourceCosts[resource])
            {
                creature.storedResources[resource].currentLevel -= resourceCosts[resource];
            }
            else
            {
                enoughToSpend = false;
            }
        }
        return enoughToSpend;
    }

    public Action getShallowCopy()
    {
        return (Action)this.MemberwiseClone();
    }
}