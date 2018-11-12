using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ResourceStore
{
    /// <summary>
    /// Amount of resource stored.
    /// </summary>
    public int amountStored;
    /// <summary>
    /// Possible values: [0,1]. Proportion of the attempted amount that will be successfully extracted on one attempt. Modified by creature's extraction ability.
    /// </summary>
    public float proportionExtracted;
    /// <summary>
    /// Amount of resource that can be taken by one action.
    /// </summary>
    public int amountConsumedPerTimeUnit;
    /// <summary>
    /// Amount renewed on a renewal step.
    /// </summary>
    public int renewalAmt;

    public string name;
    /// <summary>
    /// Maximum amount of resource that can be stored in this land location.
    /// </summary>
    public int maxAmount;

    public ResourceStore(string _name)
    {
        name = _name;
    }

    /// <summary>
    /// this returns a new instance of a resource store with shallow copies of all of the instance variables
    /// <summary>
    public ResourceStore shallowCopy()
    {
        return (ResourceStore) this.MemberwiseClone();
    }

    /// <summary>
    /// Resolves amount of resource consumed in given amount of time.
    /// </summary>
    public int attemptConsumption(int timeDedicated, int creatureAbility)
    {
        // Max(proportion * 2 ^ creatureAbility, 1) 
        float actualProportion = Math.Max(proportionExtracted * (float)Math.Pow((double)2, (double)creatureAbility), 1f);
        int amountToTake = (int) Math.Round(timeDedicated * amountConsumedPerTimeUnit * actualProportion);
        // if amount taken would be less than total amount
        if (amountToTake < amountStored)
        {
            amountStored -= amountToTake;
        }
        // if amount taken is totalAmount
        else
        {
            amountToTake = amountStored;
            amountStored = 0;
        }
        return amountToTake;
    }

    /// <summary>
    /// Renews resource according to renewalAmt (called each time step).
    /// </summary>
    public void renewResource()
    {
        throw new System.NotImplementedException();
    }

    public float getProportionStored()
    {
        return (float)amountStored / (float)maxAmount;
    }
}