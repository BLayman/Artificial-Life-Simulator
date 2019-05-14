// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Modifies creature's ability to consume certain resources or attack certain species. Each creature has a specific number of ability points to assign to different abilities.
/// </summary>
public class Ability
{
    /// <summary>
    /// Name of resource to consume, or species to attack or defend against.
    /// </summary>
    public string target;

    /// <summary>
    /// Groups ability into a category for the user to see
    /// </summary>
    public abilityType type;

    /// <summary>
    /// Strength of ability. (multiplier to increase chances)
    /// </summary>
    public int level;

    /// <summary>
    /// NOT IN USE. List of BoostRequirements that could give this ability a boost if the resource needs are met.
    /// </summary>
    public List<BoostRequirement> boostOptions = new List<BoostRequirement>();

    public Ability getShallowCopy()
    {
        return (Ability)this.MemberwiseClone();
    }
}