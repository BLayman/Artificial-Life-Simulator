// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InternalResourceInputNode : Node
{
    /// <summary>
    /// string designating resource to be found in neighbor dictionary
    /// </summary>
    public string sensedResource;
    /// <summary>
    /// stores a reference to the creature it belongs to (for getting neighbors)
    /// </summary>
    public Creature creature;


    public InternalResourceInputNode() { }

    public InternalResourceInputNode(Creature creature)
    {
        this.creature = creature;
    }



    /// <summary>
    /// Uses senses resource from creature's stored resources
    /// </summary>
    public float senseInternalResourceLevel()
    {
        if (!creature.storedResources.ContainsKey(sensedResource))
        {
            Debug.LogError("Creature trying to sense resource that it cannot store");
            return 0;
        }
        else
        {
            // return level relative to max level (normalized)
            return creature.storedResources[sensedResource].currentLevel / creature.storedResources[sensedResource].maxLevel;
        }

    }

    /// <summary>
    /// calls senseInternalResourceLevel to updateValue
    /// </summary>
    public override void updateValue()
    {
        float amount = senseInternalResourceLevel();
        value = amount;
    }
}