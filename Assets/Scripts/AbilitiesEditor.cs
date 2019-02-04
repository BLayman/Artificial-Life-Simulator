using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum abilityType {comsumption, defense, attack}

/// <summary>
/// API for Abilities objects.
/// </summary>
public class AbilitiesEditor
{
    public List<Ability> abilities;
    public int abilityPoints;

    public AbilitiesEditor(List<Ability> _abilities, int _abilityPoints)
    {
        abilities = _abilities;
        abilityPoints = _abilityPoints;
    }

    /// <summary>
    /// Adds ability of specific type and target with level 0 and no boost options.
    /// </summary>
    /// <param name="target">The resource/species the ability is applied to.</param>
    public void addAbility(string target, abilityType type)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Adds one to ability level.
    /// </summary>
    /// <param name="ability">Ability to be incremented.</param>
    public void incrementAbility(string ability)
    {
        throw new System.NotImplementedException();
    }

    public int getAbilityPoints()
    {
        return abilityPoints;
    }
}