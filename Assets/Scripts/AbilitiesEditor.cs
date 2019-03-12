// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum abilityType {comsumption, defense, attack}

/// <summary>
/// API for Abilities objects. Modifies tentativeAbilities variable from CreatureEditor
/// </summary>
public class AbilitiesEditor
{
    public Dictionary<string, Ability> abilities;
    public int remainingAbilityPoints;

    public AbilitiesEditor(Dictionary<string, Ability> _abilities, int _abilityPoints)
    {
        abilities = _abilities;
        remainingAbilityPoints = _abilityPoints;
    }

    /// <summary>
    /// Adds ability of specific type and target with level 0 and no boost options.
    /// </summary>
    /// <param name="target">The resource/species the ability is applied to.</param>
    public void addAbility(string target, abilityType type)
    {
        Ability toAdd = new Ability();
        toAdd.target = target;
        toAdd.type = type;
        toAdd.level = 0;
        abilities[target] = toAdd;
    }

    /// <summary>
    /// Adds one to ability level.
    /// </summary>
    /// <param name="ability">Ability to be incremented.</param>
    public void incrementAbility(string target)
    {
        if(remainingAbilityPoints > 0)
        {
            abilities[target].level += 1;
        }
        else
        {
            Debug.Log("no remaining ability points");
        }
        
    }

    public int getAbilityPoints()
    {
        return remainingAbilityPoints;
    }
}