// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Resource stored by a creature, and how that resource effects the creature.
/// </summary>


public class CreatureResource
{
    /// <summary>
    /// Threshold below which resource causes health damage.
    /// </summary>
    public float deficiencyThreshold;
    /// <summary>
    /// Amount of health drain from deficiency in one time step.
    /// </summary>
    public float deficiencyHealthDrain;
    /// <summary>
    /// Amount of the resource currently stored.
    /// </summary>
    public float currentLevel;
    /// <summary>
    /// Amount of health gained when resource is adequate.
    /// </summary>
    public float healthGain;
    /// <summary>
    /// Threshold above which health is gained.
    /// </summary>
    public float healthGainThreshold;
    public float maxLevel;
    public float baseUsage;
    public string name;

    /// <summary>
    /// Update creature health according to resource level.
    /// </summary>
    public void healthUpdate(Creature creature)
    {
        if (currentLevel < deficiencyThreshold)
        {
            creature.health -= deficiencyHealthDrain;
        }
        if (currentLevel > healthGainThreshold)
        {
            if (creature.health + healthGain > creature.maxHealth)
            {
                creature.health = creature.maxHealth;
            }
            else
            {
                creature.health += healthGain;
            }
        }
    }

    public CreatureResource getShallowCopy()
    {
        return (CreatureResource)this.MemberwiseClone();
    }

    public void baseUseUpdate()
    {
        currentLevel -= baseUsage;
        if (currentLevel < 0)
        {
            currentLevel = 0;
        }
    }
}