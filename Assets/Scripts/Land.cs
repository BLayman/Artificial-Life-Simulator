// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Class for storing data about a location on the map. Also facilitates creature's interaction with the environment.
/// </summary>
public class Land
{
    /// <summary>
    /// Stores resource values and other attributes of the land
    /// </summary>
    public System.Collections.Generic.Dictionary<string, ResourceStore> propertyDict = new Dictionary<string, ResourceStore>();
    /// <summary>
    /// Stores whatever creature is currently on this land.
    /// </summary>
    public Creature creatureOn;

    public bool isDummy = false;

    /// <summary>
    /// returns true if creatureOn is not null
    /// </summary>
    public bool creatureIsOn()
    {
        return (creatureOn != null);
    }

    /// <summary>
    /// Try to consume resource of given name.
    /// </summary>
    /// <param name="resourceKey">Resource name.</param>
    /// <param name="timeDedicated">Comes from action timeCost variable.</param>
    public float attemptResourceConsumption(string resourceKey, float timeDedicated, float creatureAbility, float cStorageSpace)
    {
        //Debug.Log("consuming resource: " + resourceKey);
        if (propertyDict.ContainsKey(resourceKey))
        {
            return propertyDict[resourceKey].attemptConsumption(timeDedicated, creatureAbility, cStorageSpace);
        }
        else
        {
            Debug.Log("resource not found");
            return 0;
        }
        
    }

    public void depositResource(string res, float amt)
    {
        // don't excede max amount
        if(propertyDict[res].amountStored + amt > propertyDict[res].maxAmount)
        {
            propertyDict[res].amountStored = propertyDict[res].maxAmount;
        }
        else
        {
            propertyDict[res].amountStored += amt;
        }

    }

    public Land shallowCopy()
    {
        return (Land)this.MemberwiseClone();
    }

}