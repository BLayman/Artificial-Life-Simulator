// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConsumeFromLand : Action
{
    /// <summary>
    /// Index of neighboring land to access.
    /// </summary>
    public int neighborIndex;
    /// <summary>
    /// Name of resource to consume.
    /// </summary>
    public string resourceToConsume;

    public ConsumeFromLand() { }

    public ConsumeFromLand(int neighborIndex, string resourceToConsume)
    {
        this.neighborIndex = neighborIndex;
        this.resourceToConsume = resourceToConsume;
    }

    /// <summary>
    /// Accesses land via neighborIndex, and attemps to consume resource.
    /// </summary>
    public override void perform(Creature creature, Ecosystem eco)
    {
        Land land = creature.neighborLands[neighborIndex];
        if (!land.isDummy)
        {
            CreatureResource creatRes = creature.storedResources[resourceToConsume];
            float storageSpace = creatRes.maxLevel - creatRes.currentLevel;
            float consumed = land.attemptResourceConsumption(resourceToConsume, timeCost, creature.abilities[resourceToConsume].level, storageSpace);

            if (storageSpace < consumed)
            {
                creature.storedResources[resourceToConsume].currentLevel = creature.storedResources[resourceToConsume].maxLevel;
            }
            // Only consume resource if creature has space for it
            else
            {
                creature.storedResources[resourceToConsume].currentLevel += consumed;
            }
        }
    }

}