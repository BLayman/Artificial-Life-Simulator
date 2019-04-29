// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Action
{
    public string name;
    public int priority;
    /// <summary>
    /// Stores which resources are spent, and the amount spent
    /// </summary>
    public Dictionary<string, float> resourceCosts = new Dictionary<string, float>();
    public float timeCost;

    public int age; // not currently in use

    
    public abstract void perform(Creature creature, Ecosystem eco);

    /// <summary>
    /// Performs the action, and spends creatures time and resources
    /// </summary>
    public void performWrapper(Creature c, Ecosystem e)
    {
        spendTimeAndResources(c);
        perform(c, e);
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
            if (!creature.storedResources.ContainsKey(resource))
            {
                Debug.LogError("creature doesn't store resource: " + resource);
            }
            if (creature.storedResources[resource].currentLevel <= resourceCosts[resource])
            {
                enoughToSpend = false;
            }
        }
        return enoughToSpend;
    }

    public virtual bool isPossible(Creature c)
    {
        return true;
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