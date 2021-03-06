﻿// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Currently not used. Boost requirements allow for modifications to abilities, thus giving creatures with certain resources an advantage over other creatures. If all creatures are capable of this ability, and have the same boost requirements, they all have the opportunity to gain this advantage. However, it should be noted that the relationship between resources and abilities established here does give certain resources more inherent value than others.
/// </summary>


public class BoostRequirement
{
    /// <summary>
    /// List of resources required for boost, and their thresholds.
    /// </summary>
    public Dictionary<string, int> requiredResources;
    /// <summary>
    /// The amount of increase in ability if the boost is active.
    /// </summary>
    public int boostAmount;

    public BoostRequirement getShallowCopy()
    {
        return (BoostRequirement)this.MemberwiseClone();
    }
}