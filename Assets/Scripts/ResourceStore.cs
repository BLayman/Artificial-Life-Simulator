// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Basic unit of a resource to be stored in a Land object. Creature can attempt to consume this resource from the land.
/// </summary>
public class ResourceStore
{
    public float clusterFactor = 0;
    /// <summary>
    /// Amount of resource stored.
    /// </summary>
    public float amountStored;
    /// <summary>
    /// Possible values: [0,1]. Proportion of the attempted amount that will be successfully extracted on one attempt. Modified by creature's extraction ability.
    /// </summary>
    public float proportionExtracted;
    /// <summary>
    /// Amount of resource that can be taken by one action.
    /// </summary>
    public float amountConsumedPerTimeUnit;
    /// <summary>
    /// Amount renewed on a renewal step.
    /// </summary>
    public float renewalAmt;

    public string name;
    /// <summary>
    /// Maximum amount of resource that can be stored in this land location.
    /// </summary>
    public float maxAmount;

    public ResourceStore(string _name)
    {
        name = _name;
    }

    /// <summary>
    /// this returns a new instance of a resource store with shallow copies of all of the instance variables
    /// <summary>
    public ResourceStore shallowCopy()
    {
        return (ResourceStore)this.MemberwiseClone();
    }

    /// <summary>
    /// Resolves amount of resource consumed in given amount of time.
    /// </summary>

    // TODO: make sure creature can't eat more than amount they can store
    public float attemptConsumption(float timeDedicated, float creatureAbility, float cStorageSpace)
    {
        // proportion * (2 ^ creatureAbility) 
        // if creatureAbility = 0, actualProportion = proportion
        float actualProportion = proportionExtracted * (float)Math.Pow((double)2, (double)creatureAbility);
        // amount = time * amount consumed per time * proportion consumed
        float amountToTake = timeDedicated * amountConsumedPerTimeUnit * actualProportion;
        // don't take more of the resource than the creature can store
        if(amountToTake > cStorageSpace)
        {
            amountToTake = cStorageSpace;
        }

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
        if (amountStored + renewalAmt > maxAmount)
        {
            amountStored = maxAmount;
        }
        else
        {
            amountStored += renewalAmt;
        }
    }

    public void renewClusterResource()
    {
        float weightedAmt = renewalAmt * clusterFactor;
        if (amountStored + weightedAmt > maxAmount)
        {
            amountStored = maxAmount;
        }
        else
        {
            amountStored += weightedAmt;
        }
    }

    public float getProportionStored()
    {
        return (float)amountStored / (float)maxAmount;
    }
}