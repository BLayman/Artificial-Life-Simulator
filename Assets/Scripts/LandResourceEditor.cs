// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Used as interface for generating ResourceStore objects.
/// </summary>
public class LandResourceEditor
{
    public ResourceStore resourceStore;

    public LandResourceEditor(ResourceStore _resourceStore)
    {
        resourceStore = _resourceStore;
    }

    /* set functions */

    /// <summary>
    /// Amount of the resource consumped per time unit.
    /// </summary>
    public void setAmtConsumedPerTime(float amountConsumed)
    {
        resourceStore.amountConsumedPerTimeUnit = amountConsumed;
        //Debug.Log("amount consumed per time set to " + amountConsumed);
    }

    /// <summary>
    /// Set how much the resource increases at the end of each interval number of turns (default interval is 10 turns)
    /// </summary>
    public void setRenewalAmt(float amountRenewed)
    {
        resourceStore.renewalAmt = amountRenewed;
        //Debug.Log("renewal amount per turn set to " + amountRenewed);
    }

    /// <summary>
    /// Set amount being stored
    /// </summary>
    public void setAmountOfResource(float amount)
    {
        resourceStore.amountStored = amount;
        //Debug.Log("amount of resource set to " + amount);
    }

    /// <summary>
    /// Sets fraction of resource consumed per time unit that the creature actually recieves if creature does not have an ability for this.
    /// </summary>
    /// <param name="proportion">Must be between 0 and 1.</param>
    public void setProportionExtracted(float proportion)
    {
        resourceStore.proportionExtracted = proportion;
        //Debug.Log("proportion extracted set to " + proportion);
    }

    /// <summary>
    /// Sets maximum amount of the resource that can be stored.
    /// </summary>
    public void setMaxAmt(float maxAmount)
    {
        resourceStore.maxAmount = maxAmount;
        //Debug.Log("maximum amount set to " + maxAmount);
    }
}