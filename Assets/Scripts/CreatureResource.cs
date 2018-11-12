using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Resources stored by a creature.
/// </summary>

[Serializable]
public class CreatureResource
{
    /// <summary>
    /// Threshold below which resource causes health damage.
    /// </summary>
    public int deficiencyThreshold;
    /// <summary>
    /// Amount of health drain from deficiency in one time step.
    /// </summary>
    public int deficiencyHealthDrain;
    /// <summary>
    /// Amount of the resource currently stored.
    /// </summary>
    public int currentLevel;
    /// <summary>
    /// Amount of health gained when resource is adequate.
    /// </summary>
    public int healthGain;
    /// <summary>
    /// Threshold above which health is gained.
    /// </summary>
    public int healthGainThreshold;
    public int maxLevel;
    public int baseUsage;
    public string name;

    /// <summary>
    /// Update creature health according to resource level.
    /// </summary>
    public void healthUpdate(Creature creature)
    {
        throw new System.NotImplementedException();
    }
}