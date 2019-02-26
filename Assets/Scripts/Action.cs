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
    public float timeCost;

    public int age; // not currently in use

    
    public abstract void perform(Creature creature);

    /// <summary>
    /// Performs the action, and spends creatures time and resources
    /// </summary>
    public void performWrapper(Creature c)
    {
        spendTimeAndResources(c);
        perform(c);
    }

    public Action()
    {
        age = 0;
    }

    public bool enoughResources(Creature creature)
    {
        bool enoughToSpend = true;

        foreach (string resource in resourceCosts.Keys)
        {
            if (creature.storedResources[resource].currentLevel <= resourceCosts[resource])
            {
                enoughToSpend = false;
            }
        }
        return enoughToSpend;
    }

    public void spendTimeAndResources(Creature creature)
    {
        creature.remainingTurnTime -= timeCost;
        foreach (string resource in resourceCosts.Keys)
        {
            if (creature.storedResources[resource].currentLevel > resourceCosts[resource])
            {
                creature.storedResources[resource].currentLevel -= resourceCosts[resource];
            }
        }
    }

    public Action getShallowCopy()
    {
        return (Action)this.MemberwiseClone();
    }
}